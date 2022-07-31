#pragma checksum "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\ResumenOP.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "00270f3e434ca3746f569a95140b3fef1efb27c7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Analitica_ResumenOP), @"mvc.1.0.view", @"/Views/Analitica/ResumenOP.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Analitica/ResumenOP.cshtml", typeof(AspNetCore.Views_Analitica_ResumenOP))]
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
#line 1 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\_ViewImports.cshtml"
using FactoryX;

#line default
#line hidden
#line 2 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\_ViewImports.cshtml"
using FactoryX.Models;

#line default
#line hidden
#line 4 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\_ViewImports.cshtml"
using DevExtreme.AspNet.Mvc;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"00270f3e434ca3746f569a95140b3fef1efb27c7", @"/Views/Analitica/ResumenOP.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3801cde418586957cebeceffc6a060738280b80e", @"/Views/_ViewImports.cshtml")]
    public class Views_Analitica_ResumenOP : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/ResumenOP.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\ResumenOP.cshtml"
  
    ViewData["Title"] = "Calidad";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(92, 45, true);
            WriteLiteral("\r\n<h1>Resumen de orden de producción</h1>\r\n\r\n");
            EndContext();
            BeginContext(688, 46, true);
            WriteLiteral("\r\n<div class=\"scroller\" id=\"tb_activos\">\r\n    ");
            EndContext();
            BeginContext(736, 1467, false);
#line 28 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\ResumenOP.cshtml"
Write(Html.DevExtreme().DataGrid<FactoryX.Models.ResumenOP>().ID("ListaResumenOP")
    .DataSource(ds => ds.Mvc()
    .LoadParams(new { idEmpresa = ViewBag.idEmpresa })
    .Controller("Analitica")
    .LoadAction("GetResumenOP")
    .Key("id"))
    .Editing(editing =>
    {
        editing.Mode(GridEditMode.Cell);
        editing.AllowUpdating(true);
    })
    .ShowBorders(true)
    .Columns(columns =>
    {        

        columns.AddFor(m => m.Cod_plan).Caption("Código de OP").AllowEditing(false).Width(103);

        columns.AddFor(m => m.Cod_producto).Caption("Código producto").Width(130).AllowEditing(false);

        columns.AddFor(m => m.Des_producto).Caption("Nombre producto").AllowEditing(false).MinWidth(100);
        
        columns.AddFor(m => m.value).Caption("Unidades producidas").AllowEditing(false).Width(130);
        
        columns.AddFor(m => m.fini).Caption("Tiempo inicio").AllowEditing(false).Format("dd-MM-yyyy HH:mm").Width(130);

        columns.AddFor(m => m.ffin).Caption("Tiempo fin").AllowEditing(false).Format("dd-MM-yyyy HH:mm").Width(130);

        columns.AddFor(m => m.HorasT).Caption("Horas transcurridas").AllowEditing(false).Width(140);
                
    })
    .AllowColumnResizing(true)
    .AllowColumnReordering(true)
    .FilterRow(filterRow => filterRow.Visible(true))
    .GroupPanel(groupPanel => groupPanel.Visible(true))
    //.OnSelectionChanged("selection_changed")

    );

#line default
#line hidden
            EndContext();
            BeginContext(2204, 463, true);
            WriteLiteral(@"
</div>

<script src=""https://code.jquery.com/jquery-3.1.1.min.js""></script>
<script src=""https://code.highcharts.com/highcharts.js""></script>
<script src=""https://code.highcharts.com/modules/exporting.js""></script>
<script src=""https://code.highcharts.com/modules/export-data.js""></script>
<script src=""https://code.highcharts.com/modules/accessibility.js""></script>

<figure class=""highcharts-figure"">
    <div id=""container""></div>    
</figure>

");
            EndContext();
            BeginContext(2667, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "00270f3e434ca3746f569a95140b3fef1efb27c76642", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 77 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\ResumenOP.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2734, 4, true);
            WriteLiteral("\r\n\r\n");
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