# EasyDefineMvc.Net
view: http://www.easydefinemvc.cn/
It's a dynamic library to combile MVC program without much code

使用方式如下：

安装EasyDefine全局配置
全局配置组件:请使用Nuget打包 EasyDefine.Configuration.dll，该动态库是EasyDefine抽象MVC业务层和数据访问层之间进行交互的共有抽象。

            
PM> Install-Package EasyDefine.Configuration -Version 1.0.3.1 

            

服务层组件:请使用Nuget打包 EasyDefine.ServiceFramework.dll，该动态库封装EasyDefine服务层的逻辑，其提供的标注组件可以快速地帮助业务层定义逻辑，帮助业务层动态地编译其具体实现类。使得开发人员更加专注于业务交互。

            
PM> Install-Package EasyDefine.ServiceFramework -Version 1.0.3.3 

            

数据访问层组件:请使用Nuget打包 EasyDefine.Dapper.dll，该动态库 EasyDefine提供对Dapper访问底层MySQL数据仓储的抽象，提供标注组件可以快速地实现数据的查询和命令的执行，并且提供动态实体映射的支持，向上层服务提供支持。

            
PM> Install-Package EasyDefine.Dapper -Version 1.0.3.2 

            

在这其中，服务层组件和数据访问层组件是可选的，也就是说，如果我们对项目进行拆分的时候。比如说按照大家的习惯：Controller-Api是一个csproj，BAL和DAL各有一个csproj(BAL，我习惯的命名是SOA，各位见谅)。然后关系是向上引用的。那么， 全局配置组件在每个项目中都需要安装，而服务层组件仅需安装在BAL项目中，而数据层组件仅需安装在DAL项目中. *请不要再Nuget下载使用1.0.3之前的版本，那是我测试的。
配置和StartUp的注入

appsetting.json/appsetting.Development.json配置项 位于.json配置文件的根节点下:

            

                    "EasyDefineSetting":
                    {
                    //主库地址(只能有一个)
                    "MasterDb": "server=localhost;database=test;port=3306;uid=root;pwd=unknow;",
                    //从库地址(可以有多个,从而进行读写分离)
                    "SlaveDb": [
                    "",
                    ""
                    ],
                    "SOASolution": "PPM.Online.SOA",
                    "DALSolution": "PPM.Online.DAL",
                    "DtoSolution": "PPM.Online.DTO"
                    }
                

            

这里需要解释一下:MasterDb代表的是主库的连接池，而SlaveDb是从库的连接池，这是为了实现数据库级别的读写分离而做的，首先必须保证可以连的通。一般我们只需填写MasterDb的连接字串即可。目前第一个版本，仅支持MySql数据库,后续，看情况我们会接入其它的数据库 -_-!!

接着，我们需要对StartUp进行一下修改

            

                    public IServiceProvider ConfigureServices(IServiceCollection services)
                    {
                    //使用EasyDefine注册动态
                    #region 动态编译、注入、待封装代码

                    //注册DAL-DLL,注意DAL一定要放在SOA之前注册
                    services.AddEasyDefineDAL(typeof(ITestQueries).Assembly);
                    //注册SOA-DLL
                    services.AddEasyDefineSOA(typeof(ITest).Assembly);

                    #endregion
                    //添加MVC
                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                    return GetAutofacServiceProvider(services);
                    }

                    private IServiceProvider GetAutofacServiceProvider(IServiceCollection services)
                    {
                    var builder = new ContainerBuilder();
                    builder.Populate(services);
                    var container = builder.Build();
                    return new AutofacServiceProvider(container);
                    }
                

            

如您所见，我这里使用了AutoFac对我们编译好的服务进行了注册,EasyDefine并不强制使用这种方式进行注册。只是本人懒得写了，如果您有兴趣可以试试别的方式

注意1：编译的顺序一定要将DAL放在之前(如果您对服务和数据访问进行了拆分,而对于直接不写Services层，裸操作数据的懒人们，请忽略)，因为SOA服务是依赖DAL的，在下面我们会说到引用的问题。

注意2: 上例中的typeof中的类型，分别是您的服务层和数据访问层的一个接口，EasyDefine默认是必须要有一个向上提供服务的接口，哪怕是个空接口，否则我们的操作没有任何意义。 巧合的是，我们正好可以利用这个空接口，作为我们当前模组的驱动，顺便来为我们做一些辅助性的工作，在“4、接口辅助标记” 中，我会给您详细讲述。 
