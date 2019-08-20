#pragma checksum "E:\主站\框架\EasyDefineMvc.Net.Core\EasyDefine.Net\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ea3854c1236a854d78f0053651c69c40d8cb385f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ea3854c1236a854d78f0053651c69c40d8cb385f", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 25, true);
            WriteLiteral("<!DOCTYPE HTML>\r\n<html>\r\n");
            EndContext();
            BeginContext(25, 215, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1e20d664c9834912aeeb97fe2fdefa6e", async() => {
                BeginContext(31, 202, true);
                WriteLiteral("\r\n    <title>EasyDefine .Net</title>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\r\n    <link rel=\"stylesheet\" href=\"assets/css/main.css\" />\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(240, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(242, 3315, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ace89cb64c9448be8ca268e911d41e47", async() => {
                BeginContext(248, 3302, true);
                WriteLiteral(@"

    <!-- Header -->
    <header id=""header"">
        <div class=""inner"">
            <a class=""logo""><strong>EasyDefine Mvc</strong> for .Net Core</a>
            <nav id=""nav"">
                <a href=""/Home/Document"">文档</a>
                <a href=""https://github.com/shayuxiang/EasyDefineMvc.Net"">Git</a>
                <a href=""/Home/Document#wfw"">微服务</a>
                <a href=""/Home/About"">关于</a>
            </nav>
            <a href=""#navPanel"" class=""navPanelToggle""><span class=""fa fa-bars""></span></a>
        </div>
    </header>

    <!-- Banner -->
    <section id=""banner"">
        <div class=""inner"">
            <header>
                <h1>一款基于.net standard 的动态编译框架</h1>
            </header>

            <div class=""flex "">

                <div>
                    <span class=""icon fa-car""></span>
                    <h3>起步</h3>
                    <p>使用标注，自动生成简洁的编码</p>
                </div>

                <div>
                    <span class=""icon fa-came");
                WriteLiteral(@"ra""></span>
                    <h3>服务</h3>
                    <p>即时的本地类库生成，快速调用</p>
                </div>

                <div>
                    <span class=""icon fa-bug""></span>
                    <h3>下载</h3>
                    <p>轻引用,低耦合,高效率的第三方库</p>
                </div>

            </div>

            <footer>
                <a href=""/Home/Fast"" class=""button"">快速上车</a>
            </footer>
        </div>
    </section>


    <!-- Three -->
    <section id=""three"" class=""wrapper align-center"">
        <div class=""inner"">
            <div class=""flex flex-2"">
                <article>
                    <div class=""image round"">
                        <img src=""images/pic01.jpg"" alt=""Pic 01"" />
                    </div>
                    <header>
                        <h3>几乎为零的编码量<br /></h3>
                    </header>
                    <p>AOP的编码风格，让我们告别那千遍一律调试<br />高效的封装，让我们告别那繁杂的配置和底层细节<br />让我们解放双手,更多地把时间留给家人</p>
                    <footer>
      ");
                WriteLiteral(@"                  <a href=""/Home/Document"" class=""button"">服务层接入</a>
                    </footer>
                </article>
                <article>
                    <div class=""image round"">
                        <img src=""images/pic02.jpg"" alt=""Pic 02"" />
                    </div>
                    <header>
                        <h3>便捷、安全地数据流转</h3>
                    </header>
                    <p>让我们告别复杂的Model、映射，让系统为我们做这些事<br />让我们忘记CodeFirst、以及一切以业务狗为主角的迭代<br />让我们告别各种语法的优化，让996见鬼去吧!!!</p>
                    <footer>
                        <a href=""/Home/Document"" class=""button"">数据层接入</a>
                    </footer>
                </article>
            </div>
        </div>
    </section>

    <!-- Footer -->
    <footer id=""footer"">
        <div class=""inner"">


            <div class=""copyright"">Copyright &copy; 2019.EasyDefine Org All rights reserved.<a target=""_blank"" href=""http://www.easydefine.cn/"">www.easydefine.cn</a></div>

        </div>
    </foo");
                WriteLiteral("ter>\r\n\r\n    <!-- Scripts -->\r\n    <script src=\"assets/js/jquery.min.js\"></script>\r\n    <script src=\"assets/js/skel.min.js\"></script>\r\n    <script src=\"assets/js/util.js\"></script>\r\n    <script src=\"assets/js/main.js\"></script>\r\n\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3557, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
