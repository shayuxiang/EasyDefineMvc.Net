using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyDefine.Configuration;
using EasyDefine.Configuration.Interface;
using EasyDefine.Configuration.Runtime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CSharp;

namespace EasyDefine.ServiceFramework.Runtime
{
    public class SerivceRunner<T> 
    {
        /// <summary>
        /// 标记待生成的类
        /// </summary>
        private ScriptClass scriptClass = default(ScriptClass);

        /// <summary>
        /// 标记待生成的方法
        /// </summary>
        private List<ScriptMethod> scriptMethods = null;

        /// <summary>
        /// 方法内语句  禁用
        /// </summary>
        // private CustomCodeLoader customCodeLoader = null;

        /// <summary>
        /// 预配置读取类
        /// </summary>
        private ConfigHelper configHelper = new ConfigHelper();

        /// <summary>
        /// 注入标记
        /// </summary>
        private List<Inject> InjectAttributes = new List<Inject>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_scriptClass"></param>
        public SerivceRunner()
        {
            var atts = typeof(T).GetCustomAttributes(typeof(ScriptClass), true);
            if (atts.Length > 0)
            {
                this.scriptClass = atts[0] as ScriptClass;
                this.scriptClass.Name = $@"Srv_{typeof(T).Name}"; //自动生成类名称
                this.scriptMethods = this.scriptClass.GetScriptMethods(typeof(T));
                //获取注入的标记
                var inject_attr = typeof(T).GetCustomAttributes(typeof(Inject), true);
                if (inject_attr.Length > 0) {
                    InjectAttributes.AddRange((IEnumerable<Inject>)inject_attr);
                }
            }
            //方法内语句
            //customCodeLoader = new CustomCodeLoader(scriptClass.Language, scriptClass.AssociatedScriptRoot, configHelper);
        }

        /// <summary>
        /// 生成内存中动态对象
        /// </summary>
        public void BuildInMemory() {

            //写入文件
            CodeDomProvider provide = new CSharpCodeProvider();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(scriptClass.Name);
            var tempdir = configHelper.GetTempSourceDir() + "/" + scriptClass.Name + ".cs";
            IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(tempdir,false), "   ");
            provide.GenerateCodeFromCompileUnit(GetInterfaceRefCodeUnit(), tw, new CodeGeneratorOptions());
            tw.Close();
            //编译
            //CompilerClass();
        }
        
        /// <summary>
        /// 获取接口生成类的源码编译组件
        /// </summary>
        /// <returns></returns>
        private CodeCompileUnit GetInterfaceRefCodeUnit() {
            //定义CodeDom预编译单元
            CodeCompileUnit unit = new CodeCompileUnit();
            //设置类的命名空间
            CodeNamespace theNamespace = new CodeNamespace(scriptClass.NameSpace);
            //引用命名空间
            //这里需要注入底层的处理接口
            CodeNamespaceImport SystemImport = new CodeNamespaceImport("System");
            theNamespace.Imports.Add(SystemImport);
            //定义实现类
            CodeTypeDeclaration DynamicClass = new CodeTypeDeclaration(scriptClass.Name);
            //设置为类
            DynamicClass.IsClass = true;
            //设置基接口
            DynamicClass.BaseTypes.Add(typeof(T));
            //设置构造器，这里利用MVC注入
            if (InjectAttributes.Count > 0) {
                var constructor = new CodeConstructor();
                constructor.Attributes = MemberAttributes.Public; //公共的
                foreach (var inj in InjectAttributes)
                {
                    //DynamicClass.TypeParameters.Add(new CodeParameterDeclarationExpression());
                    // 属性
                    CodeMemberField prty = new CodeMemberField();
                    prty.Name = inj.VariableName;
                    // 属性值的类型
                    prty.Type = new CodeTypeReference(" readonly "  + inj.Ref);
                    // 公共属性
                    prty.Attributes = MemberAttributes.Private | MemberAttributes.Final;
                    //注入的变量
                    DynamicClass.Members.Add(prty);
                    //添加构造器
                    constructor.Parameters.Add(new CodeParameterDeclarationExpression(inj.Ref, inj.VariableName));
                    //添加注入赋值代码
                    constructor.Statements.Add(new CodeSnippetStatement($@"this.{inj.VariableName} = {inj.VariableName};"));
                }
                DynamicClass.Members.Add(constructor);
            }
            //设置实现方法
            foreach (var m in this.scriptMethods)
            {
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
                    var _type = new CodeTypeReference(pi.ParameterType);
                    methods.Parameters.Add(new CodeParameterDeclarationExpression(_type, pi.Name));
                }
                #region CodeExecute和Return未设置 检索注入接口中是否有同名、同参、同返回的方法可供直接调用
                //添加动态注入的代码段
                if (m.CodeExecutes == null && m.Return == null)
                {
                    //判断同名方法并注入
                    foreach (var ina in this.InjectAttributes)
                    {
                        //判断是否有同名同返回的方法
                        if (ina.Ref.GetMethods().Where(mm => mm.Name == m.Name && mm.ReturnType == m.ReturnType).Count() > 0)
                        {
                            //判断是否同参
                            var mm = ina.Ref.GetMethods().Where(xx => xx.Name == m.Name && xx.ReturnType == m.ReturnType);
                            var isSame = true;
                            //mm:所有同名同返的方法，mp:其中的某一个重载
                            foreach (var mp in mm)
                            {
                                //参数必须完全相同
                                foreach (var pi in m.ParamInfos)
                                {
                                    //名称相同，但是类型不相同
                                    if (mp.GetParameters().Where(p => p.Name == pi.Name).Count() > 0)
                                    {
                                        var first = mp.GetParameters().Where(p => p.Name == pi.Name).FirstOrDefault();
                                        if (first.ParameterType != pi.ParameterType)
                                        {
                                            isSame = false;
                                        }
                                    }
                                    else
                                    {
                                        isSame = false;
                                    }
                                }
                                if (isSame)
                                {
                                    var _params = m.ParamInfos;
                                    var _paramsStr = "";
                                    _params.ToList().ForEach(p =>
                                    {
                                        _paramsStr += p.Name + ",";
                                    });
                                    if (!string.IsNullOrEmpty(_paramsStr))
                                    {
                                        _paramsStr = _paramsStr.Substring(0, _paramsStr.LastIndexOf(","));
                                    }
                                    //是完全相同的方法
                                    m.CodeExecutes = new List<CodeExecute>();
                                    m.CodeExecutes.Add(new CodeExecute
                                    {
                                        Code = $@"{ina.VariableName}.{mp.Name}({_paramsStr})",
                                        Var = "_ret"
                                    });
                                    m.Return = new Attributes.Return()
                                    {
                                        Var = "_ret"
                                    };
                                }
                                else {
                                    //参数有所不同，但参数只有一个且是一个RequestDTO
                                    //并且认证所需要的所有参数在DTO中都有映射
                                    if (m.ParamInfos.Count() == 1) {
                                        var param = m.ParamInfos.FirstOrDefault();
                                        if (param.ParameterType.BaseType == typeof(DynamicRequestEntity))
                                        {
                                            //是请求DTO
                                            var Fields = param.ParameterType.GetMembers().Where(g => g.MemberType == MemberTypes.Property); //取到运行时的属性
                                            var _paramsStr = "";
                                            Fields.ToList().ForEach(p =>
                                            {
                                                if (mp.GetParameters().Where(pp => pp.Name == p.Name).Count() > 0)
                                                {
                                                    _paramsStr += $@"{param.Name}.{p.Name},";
                                                }
                                            });
                                            _paramsStr = _paramsStr.Substring(0, _paramsStr.LastIndexOf(","));
                                            //编译
                                            m.CodeExecutes = new List<CodeExecute>();
                                            m.CodeExecutes.Add(new CodeExecute
                                            {
                                                Code = $@"{ina.VariableName}.{mp.Name}({_paramsStr})",
                                                Var = "_ret"
                                            });
                                            m.Return = new Attributes.Return()
                                            {
                                                Var = "_ret"
                                            };
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //判断Code中是否存在引用的注入变量
                else
                {
                    m.CodeExecutes.ForEach(e => {
                        this.InjectAttributes.ForEach(p => {
                            if (e.Code.IndexOf(p.VariableName.Replace("_",string.Empty)) >= 0)
                            {
                                e.Code = e.Code.Replace(p.VariableName.Replace("_", string.Empty),p.VariableName);
                            }
                        });
                    });
                }
                #endregion
                //将CodeExecute和Return转为可执行的代码
                var express = "";//customCodeLoader.GetMethodCode(m.Name);
                if (m.CodeExecutes != null)
                {
                    foreach (var c in m.CodeExecutes)
                    {
                        if (!string.IsNullOrEmpty(c.Var))
                        {
                            //Var不为空 调用有返回的代码
                            var varx = c.Var.StartsWith("_") ? c.Var : "_" + c.Var;
                            if (m.ReturnType.BaseType == typeof(Task))
                            {
                                //异步返回
                                express += $@"var {varx} = await {c.Code};";
                            }
                            else
                            {
                                //同步返回
                                express += $@"var {varx} = {c.Code};";
                            }
                        }
                        else
                        {
                            //Var为空，调用无返回的代码
                            express += $@"{c.Code};";
                        }
                    }
                }
                if (m.Return != null)
                {
                    //有返回值
                    if (methods.ReturnType.BaseType != "System.Void")
                    {
                        var varx = m.Return.Var.StartsWith("_") ? m.Return.Var : "_" + m.Return.Var;
                        express += $@"return {varx};";
                    }
                }
                //methods.Statements.Add(express.UserCode);
                methods.Statements.Add(new CodeSnippetStatement(express));
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
        /// 获取引用接口的类类型
        /// </summary>
        /// <returns></returns>
        public Type GetImplementClassType() {
            var ass = Assembly.LoadFrom(configHelper.GetSrvOutputPath());
            return ass.GetType($@"{scriptClass.NameSpace}.{scriptClass.Name}", true, true);
        }

        /// <summary>
        /// 编译类
        /// </summary>
        public void CompilerAllClass()
        {
            Console.WriteLine("begining of Complier SOA...");
            var sysReferences = new List<MetadataReference>();
            var refAsmFiles = new List<string>();
            //注入系统依赖
            var sysRefLocation = typeof(Enumerable).GetTypeInfo().Assembly.Location;
            refAsmFiles.Add(sysRefLocation);
            //注入原本缓存的程序集依赖
            refAsmFiles.Add(typeof(DiagnosticSource).GetTypeInfo().Assembly.Location + "/../netstandard.dll");
            refAsmFiles.Add(typeof(DbConnection).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(object).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(Console).GetTypeInfo().Assembly.Location);
            refAsmFiles.Add(typeof(PagedListResult<>).GetTypeInfo().Assembly.Location);
            var modelsLocation = $@"{AppContext.BaseDirectory}/{ConfigHelper.Dto_OutputRoot}.dll";
            refAsmFiles.Add(modelsLocation);

            var apiAsm = refAsmFiles.Select(t => MetadataReference.CreateFromFile(t)).ToList();
            sysReferences.AddRange(apiAsm);
            //注入程序集依赖
            var thisAss = Assembly.GetEntryAssembly();
            if (thisAss != null) {
                var referenceAss = thisAss.GetReferencedAssemblies();
                foreach (var r in referenceAss) {
                    var LoadedAss = Assembly.Load(r);
                    sysReferences.Add(MetadataReference.CreateFromFile(LoadedAss.Location));
                }
            }
            //开始编译
            var tempdir = configHelper.GetTempSourceDir() + "/";// + dapperLink.Name + ".cs";
            Console.WriteLine(tempdir);
            DirectoryInfo directory = new DirectoryInfo(tempdir);
            List<SyntaxTree> trees = new List<SyntaxTree>();
            foreach (var f in directory.GetFiles("Srv_*.cs"))
            {
                trees.Add(SyntaxFactory.ParseSyntaxTree(ReadCode(f.FullName)));
            }

            var compilation = CSharpCompilation.Create(ConfigHelper.Srv_OutputRoot)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(sysReferences).AddSyntaxTrees(trees.ToArray());
            EmitResult emitResult = compilation.Emit(configHelper.GetSrvOutputPath());
            if (emitResult.Success)
            {
            }
            else
            {
                foreach (var m in emitResult.Diagnostics)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($@"SOA动态编译错误:{m.ToString()}");
                }
                Console.Read();
                throw new Exception {  Source = "编译异常" };
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
    }
}
