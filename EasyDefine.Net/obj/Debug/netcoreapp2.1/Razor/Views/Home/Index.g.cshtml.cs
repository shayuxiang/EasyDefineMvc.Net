#pragma checksum "E:\主站\框架\EasyDefineMvc.Net.Core\EasyDefine.Net\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "793f788e5581e0c01e0f53bb787e05b480f727f6"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"793f788e5581e0c01e0f53bb787e05b480f727f6", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/download/EasyDefine.Demo.rar"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 25, true);
            WriteLiteral("<!DOCTYPE HTML>\r\n<html>\r\n");
            EndContext();
            BeginContext(25, 223, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "56f3150e50b247608766b67e6bdf58ec", async() => {
                BeginContext(31, 210, true);
                WriteLiteral("\r\n    <title>EasyDefine .Net 动态编译API</title>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />\r\n    <link rel=\"stylesheet\" href=\"assets/css/main.css\" />\r\n");
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
            BeginContext(248, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(250, 3384, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5db5ba63df274de6880fa24a093f4992", async() => {
                BeginContext(256, 203, true);
                WriteLiteral("\r\n\r\n    <!-- Header -->\r\n    <header id=\"header\">\r\n        <div class=\"inner\">\r\n            <a class=\"logo\"><strong>EasyDefine Mvc</strong> for .Net Core</a>\r\n            <nav id=\"nav\">\r\n                ");
                EndContext();
                BeginContext(459, 51, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7cf9c07c261a46daadbc669c4bacc334", async() => {
                    BeginContext(500, 6, true);
                    WriteLiteral("DEMO下载");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(510, 3117, true);
                WriteLiteral(@"
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
                    <span class=""icon fa-camera""></span>
                    <h3>服务</h3>
                    <p>即时的本地类库生成，快速调用</p>
                </div>

                <div>
                    <span class=""icon fa-bug""><");
                WriteLiteral(@"/span>
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
                        <a href=""/Home/Document"" class=""button"">服务层接入</a>
                    </footer>
                </article>
                <article>
                    <div class");
                WriteLiteral(@"=""image round"">
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
    </footer>

    <!-- Scripts -->
    <script src=""assets/js/jquery.min.js""></script>
    <script src=""assets/js/skel.min.js""></script>
    <script src=""assets/js/util.js""></script>
    ");
                WriteLiteral("<script src=\"assets/js/main.js\"></script>\r\n\r\n");
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
            BeginContext(3634, 11, true);
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