using Dapper;
using EasyDefine.Configuration;
using EasyDefine.Configuration.Runtime;
using EasyDefine.Dapper.Attributes;
using EasyDefine.Dapper.Interface;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyDefine.Dapper.Core
{
    /// <summary>
    /// 动态编译底层信息查询类的
    /// </summary>
    public class DataQueryRunner<T>
    {
        /// <summary>
        /// Dapper的链接
        /// </summary>
        private DapperLink dapperLink = default(DapperLink);

        /// <summary>
        /// 数据访问层的方法
        /// </summary>
        private List<DapperCommand> dapperMethods = null;


        public DataQueryRunner() {
            var atts = typeof(T).GetCustomAttributes(typeof(DapperLink), true);
            if (atts.Length > 0)
            {
                this.dapperLink = atts[0] as DapperLink;
                this.dapperLink.Name = $@"Dbc_{typeof(T).Name}"; //自动生成类名称
                this.dapperMethods = this.dapperLink.GetDapperCommand(typeof(T));
            }
            //方法内语句
          //  customCodeLoader = new CustomCodeLoader(scriptClass.Language, scriptClass.AssociatedScriptRoot, configHelper);
        }


        /// <summary>
        /// 预配置读取类
        /// </summary>
        private ConfigHelper configHelper = new ConfigHelper();

        /// <summary>
        /// 生成内存中动态代码
        /// </summary>
        public void BuildInMemory()
        {
            //写入文件
            CodeDomProvider provide = new CSharpCodeProvider();
            var tempdir = configHelper.GetTempSourceDir() + "/" + dapperLink.Name + ".cs";
            IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(tempdir, false), "   ");
            provide.GenerateCodeFromCompileUnit(GetInterfaceRefCodeUnit(), tw, new CodeGeneratorOptions());
            tw.Close();
            //编译
            //CompilerClass();
        }

        /// <summary>
        /// 获取接口生成类的源码编译组件
        /// </summary>
        /// <returns></returns>
        private CodeCompileUnit GetInterfaceRefCodeUnit()
        {
            //定义CodeDom预编译单元
            CodeCompileUnit unit = new CodeCompileUnit();
            //设置类的命名空间
            CodeNamespace theNamespace = new CodeNamespace(dapperLink.NameSpace);
            //引用命名空间
            //这里需要注入底层的处理接口
            CodeNamespaceImport SystemImport = new CodeNamespaceImport("System");
            CodeNamespaceImport SystemImportCore = new CodeNamespaceImport("EasyDefine.Dapper.Core");
            CodeNamespaceImport Dapper = new CodeNamespaceImport("Dapper");
            CodeNamespaceImport Data = new CodeNamespaceImport("System.Data");
            theNamespace.Imports.Add(SystemImport);
            theNamespace.Imports.Add(SystemImportCore);
            theNamespace.Imports.Add(Dapper);
            theNamespace.Imports.Add(Data);
            //定义实现类
            CodeTypeDeclaration DynamicClass = new CodeTypeDeclaration(dapperLink.Name);
            //设置为类
            DynamicClass.IsClass = true;
            //设置基接口
            DynamicClass.BaseTypes.Add(typeof(T));
            //设置实现方法
            foreach (var m in this.dapperMethods)
            {
                m.SourcePointEnum = this.dapperLink.PointEnum;
                m.SlaveId = this.dapperLink.SlaveId;
                CodeMemberMethod methods = new CodeMemberMethod();
                methods.Name = m.Name;
                //修饰符
                methods.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                //返回类型
                if (m.ReturnType.GetGenericArguments().Length > 0 && m.ReturnType.BaseType == typeof(Task))
                {
                    //异步返回
                    methods.ReturnType = new CodeTypeReference(" async " + m.ReturnType);
                }
                else
                {
                    //同步返回
                    methods.ReturnType = new CodeTypeReference(m.ReturnType);
                }
                //参数
                foreach (var pi in m.ParamInfos)
                {
                    if (pi.ParameterType.GUID == typeof(IDbTransaction).GUID) {
                        m.IsTrans = true;
                        m.TransVariName = pi.Name;
                    } 
                    var _type = new CodeTypeReference(pi.ParameterType);
                    methods.Parameters.Add(new CodeParameterDeclarationExpression(_type, pi.Name));
                }
                var code = new CodeSnippetStatement(m.GetSQLCode()); //获取执行代码
                methods.Statements.Add(code);
                //加入类中
                DynamicClass.Members.Add(methods);
            }
            //加入类
            theNamespace.Types.Add(DynamicClass);
            //加入命名空间
            unit.Namespaces.Add(theNamespace);
            //返回代码组
            return unit;
        }

        /// <summary>
        /// 编译类
        /// </summary>
        public void CompilerAllClass()
        {
            var sysReferences = new List<MetadataReference>();
            var refAsmFiles = new List<string>();
            //注入系统依赖
            var sysRefLocation = typeof(Enumerable).GetTypeInfo().Assembly.Location;
            refAsmFiles.Add(sysRefLocation);
            //注入原本缓存的程序集依赖
            refAsmFiles.Add(typeof(DiagnosticSource).GetTypeInfo().Assembly.Location+ "/../netstandard.dll");
            refAsmFiles.Add(typeof(DbConnection).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(object).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(DynamicParameters).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(Console).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(PagedListResult<>).GetTypeInfo().Assembly.Location);
            var modelsLocation = $@"{AppContext.BaseDirectory}/{ConfigHelper.Dto_OutputRoot}.dll";
            refAsmFiles.Add(modelsLocation);

            var apiAsm = refAsmFiles.Select(t => MetadataReference.CreateFromFile(t)).ToList();
            sysReferences.AddRange(apiAsm);
            //注入程序集依赖
            var thisAss = Assembly.GetEntryAssembly();
            if (thisAss != null)
            {
                var referenceAss = thisAss.GetReferencedAssemblies();
                foreach (var r in referenceAss)
                {
                    var LoadedAss = Assembly.Load(r);
                    sysReferences.Add(MetadataReference.CreateFromFile(LoadedAss.Location));
                }
            }
            //注入Nuget包的库
            //开始编译
            var tempdir = configHelper.GetTempSourceDir() + "/";// + dapperLink.Name + ".cs";
            DirectoryInfo directory = new DirectoryInfo(tempdir);
            List<SyntaxTree> trees = new List<SyntaxTree>();
            foreach (var f in directory.GetFiles("Dbc_*.cs"))
            {
                trees.Add(SyntaxFactory.ParseSyntaxTree(ReadCode(f.FullName)));
            }
           

            var compilation = CSharpCompilation.Create(ConfigHelper.Dbc_OutputRoot)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(sysReferences).AddSyntaxTrees(trees.ToArray());
            EmitResult emitResult = compilation.Emit(configHelper.GetDbcOutputPath());
            if (emitResult.Success)
            {

            }
            else
            {
                foreach (var m in emitResult.Diagnostics)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($@"DAL动态编译错误:{m.ToString()}");
                }
                // System.Reflection.TypeExtensions
                throw new Exception { Source = "编译异常" };
            }
        }

        /// <summary>
        /// 读取临时代码文件
        /// </summary>
        /// <param name="path"></param>
        private string ReadCode(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 获取引用接口的类类型
        /// </summary>
        /// <returns></returns>
        public Type GetImplementClassType()
        {
            var ass = Assembly.LoadFrom(configHelper.GetDbcOutputPath());
            return ass.GetType($@"{dapperLink.NameSpace}.{dapperLink.Name}", true, true);
        }

    }
}
