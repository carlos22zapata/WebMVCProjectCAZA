#pragma checksum "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Analitica_Rendimiento), @"mvc.1.0.view", @"/Views/Analitica/Rendimiento.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Analitica/Rendimiento.cshtml", typeof(AspNetCore.Views_Analitica_Rendimiento))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3defb3cb8e5ff4773d0f8c535e33fa8d285c79e7", @"/Views/Analitica/Rendimiento.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3801cde418586957cebeceffc6a060738280b80e", @"/Views/_ViewImports.cshtml")]
    public class Views_Analitica_Rendimiento : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/JSCalendar/datepicker.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/JSCalendar/datepicker.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/JSCalendar/eye.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/JSCalendar/layout.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/Rendimiento.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
  
    ViewData["Title"] = "Rendimiento";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(96, 400, true);
            WriteLiteral(@"
<h1>Rendimiento</h1>

<script src=""https://code.highcharts.com/highcharts.js""></script>
<script src=""https://code.highcharts.com/modules/series-label.js""></script>
<script src=""https://code.highcharts.com/modules/exporting.js""></script>
<script src=""https://code.highcharts.com/modules/export-data.js""></script>
<script src=""https://code.highcharts.com/modules/accessibility.js""></script>

");
            EndContext();
            BeginContext(514, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(516, 64, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e76449", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(580, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(648, 53, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e77701", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(701, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(703, 46, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e78878", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(749, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(751, 49, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e710055", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(800, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(854, 21, true);
            WriteLiteral("\r\n<input id=\"Empresa\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 875, "\"", 901, 1);
#line 24 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
WriteAttributeValue("", 883, ViewBag.IdEmpresa, 883, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(902, 296, true);
            WriteLiteral(@" style=""display:none;""></input>

<div>

    <div class=""alert alert-success"" role=""alert"" id=""alertaInicial"">
        Datos obtenidos de la última semana completa de trabajo, desde: hasta:
    </div>

    <button class=""btn btn-primary agregar"" onclick=""mostrarFiltroMes()"" id=""btnMes"">
");
            EndContext();
            BeginContext(1242, 121, true);
            WriteLiteral("        Mes\r\n    </button>\r\n    <button class=\"btn btn-primary agregar\" onclick=\"mostrarFiltroSemana()\" id=\"btnSemana\">\r\n");
            EndContext();
            BeginContext(1407, 118, true);
            WriteLiteral("        Semana\r\n    </button>\r\n    <button class=\"btn btn-primary agregar\" onclick=\"mostrarFiltroDia()\" id=\"btnDia\">\r\n");
            EndContext();
            BeginContext(1569, 117, true);
            WriteLiteral("        Día\r\n    </button>\r\n    <button class=\"btn btn-primary agregar\" onclick=\"mostrarFiltroHora()\" id=\"btnHora\">\r\n");
            EndContext();
            BeginContext(1730, 123, true);
            WriteLiteral("        Hora\r\n    </button>\r\n\r\n    <div class=\"row\" style=\"float:right;\">\r\n\r\n        &nbsp;\r\n        &nbsp;\r\n\r\n        <div");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1853, "\"", 1861, 0);
            EndWriteAttribute();
            BeginContext(1862, 85, true);
            WriteLiteral(">Filtrado inteligente de datos</div>\r\n\r\n        &nbsp;\r\n\r\n        <div>\r\n            ");
            EndContext();
            BeginContext(1949, 175, false);
#line 59 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
        Write(Html.DevExtreme().Switch()
                    .Value(false)
                    .ID("swich01")
                    .OnValueChanged("switch_valueChanged")
                );

#line default
#line hidden
            EndContext();
            BeginContext(2125, 62, true);
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n\r\n<br />\r\n<br />\r\n\r\n\r\n");
            EndContext();
            BeginContext(2459, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(2483, 233, true);
            WriteLiteral("<div class=\"container-fluid\" id=\"mesFilter\" style=\"cursor:pointer; display:none;\">\r\n    <div class=\"container\">\r\n        <div class=\"row\">\r\n            <span class=\"input-group-text\">Desde</span>\r\n            <div class=\"col-xl-3\">\r\n");
            EndContext();
            BeginContext(2816, 157, true);
            WriteLiteral("                <div id=\"mes-ini\"></div>\r\n            </div>\r\n\r\n            <span class=\"input-group-text\">Hasta</span>\r\n            <div class=\"col-xl-3\">\r\n");
            EndContext();
            BeginContext(3073, 216, true);
            WriteLiteral("                <div id=\"mes-fin\"></div>\r\n            </div>\r\n        </div>\r\n\r\n        <br />\r\n\r\n        <div class=\"row\">\r\n            <span class=\"input-group-text\">Sku</span>\r\n            <div class=\"col-xl-5\">\r\n");
            EndContext();
            BeginContext(3447, 18, true);
            WriteLiteral("\r\n                ");
            EndContext();
            BeginContext(3467, 576, false);
#line 104 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().SelectBox()
                    .ID("SkuMes")
                    .Placeholder("Todos los SKU")
                    .DataSource(d => d.Mvc().LoadAction("Lista_sku")
                                            .Key("Cod_producto")
                                            .Controller("Analitica")
                                            .LoadParams(new { idEmpresa = ViewBag.idEmpresa }))
                    .DisplayExpr("Des_producto")
                    .ValueExpr("Cod_producto")
                    .SearchEnabled(true)
                );

#line default
#line hidden
            EndContext();
            BeginContext(4044, 83, true);
            WriteLiteral("\r\n            </div>\r\n\r\n\r\n            <button class=\"col-xl-1 btn btn-info agregar\"");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 4127, "\"", 4167, 3);
            WriteAttributeValue("", 4137, "obtenerMes(", 4137, 11, true);
#line 118 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
WriteAttributeValue("", 4148, ViewBag.idEmpresa, 4148, 18, false);

#line default
#line hidden
            WriteAttributeValue("", 4166, ")", 4166, 1, true);
            EndWriteAttribute();
            BeginContext(4168, 15, true);
            WriteLiteral(" id=\"prueba\">\r\n");
            EndContext();
            BeginContext(4235, 88, true);
            WriteLiteral("                Aplicar\r\n            </button>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n");
            EndContext();
            BeginContext(4350, 202, true);
            WriteLiteral("<div class=\"container-fluid\" id=\"semanaFilter\" style=\"cursor:pointer; display:none;\">\r\n    <div class=\"container\">\r\n        <div class=\"row\">\r\n\r\n            <span class=\"input-group-text\">Desde</span>\r\n");
            EndContext();
            BeginContext(4582, 435, true);
            WriteLiteral(@"            <div class=""col-xl-3"" id=""divSemanaIni"">
                <input type=""week"" name=""week"" id=""semana-ini"" required class=""datePicker-CAZA"">
            </div>

            <div class=""col-xl-3 row"" id=""divFirefoxSemanaIni"">
                &nbsp;&nbsp;
                <div id=""BoxIni"" class=""col-xl-4""></div>
                &nbsp;
                <div id=""BoxIni_ano"" class=""col-xl-6""></div>
            </div>

");
            EndContext();
            BeginContext(5053, 57, true);
            WriteLiteral("            <span class=\"input-group-text\">Hasta</span>\r\n");
            EndContext();
            BeginContext(5140, 589, true);
            WriteLiteral(@"            <div class=""col-xl-3"" id=""divSemanaFin"">
                <input type=""week"" name=""week"" id=""semana-fin"" required class=""datePicker-CAZA"">
            </div>

            <div class=""col-xl-3 row"" id=""divFirefoxSemanaFin"">
                &nbsp;&nbsp;
                <div id=""BoxFin"" class=""col-xl-4""></div>
                &nbsp;
                <div id=""BoxFin_ano"" class=""col-xl-6""></div>
            </div>

        </div>

        <br />

        <div class=""row"">
            <span class=""input-group-text"">Sku</span>
            <div class=""col-xl-5"">
");
            EndContext();
            BeginContext(5890, 18, true);
            WriteLiteral("\r\n                ");
            EndContext();
            BeginContext(5910, 579, false);
#line 168 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().SelectBox()
                    .ID("SkuSemana")
                    .Placeholder("Todos los SKU")
                    .DataSource(d => d.Mvc().LoadAction("Lista_sku")
                                            .Key("Cod_producto")
                                            .Controller("Analitica")
                                            .LoadParams(new { idEmpresa = ViewBag.idEmpresa }))
                    .DisplayExpr("Des_producto")
                    .ValueExpr("Cod_producto")
                    .SearchEnabled(true)
                );

#line default
#line hidden
            EndContext();
            BeginContext(6490, 81, true);
            WriteLiteral("\r\n            </div>\r\n\r\n            <button class=\"col-xl-1 btn btn-info agregar\"");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 6571, "\"", 6614, 3);
            WriteAttributeValue("", 6581, "obtenerSemana(", 6581, 14, true);
#line 181 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
WriteAttributeValue("", 6595, ViewBag.idEmpresa, 6595, 18, false);

#line default
#line hidden
            WriteAttributeValue("", 6613, ")", 6613, 1, true);
            EndWriteAttribute();
            BeginContext(6615, 103, true);
            WriteLiteral(" id=\"prueba\">\r\n                Aplicar\r\n            </button>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n");
            EndContext();
            BeginContext(6740, 249, true);
            WriteLiteral("<div class=\"container-fluid\" id=\"diaFilter\" style=\"cursor:pointer; display:none;\">\r\n    <div class=\"container\">\r\n        <div class=\"row\">\r\n            <span class=\"input-group-text\">Desde</span>\r\n            <div class=\"col-xl-3\">\r\n                ");
            EndContext();
            BeginContext(6991, 301, false);
#line 195 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().DateBox()
                    .ID("diaDesde")
                    .Type(DateBoxType.Date)
                    .Value(DateTime.Now)
                    .DisplayFormat("dd/MM/yyyy")
                    .DropDownButtonTemplate(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    BeginContext(7239, 35, true);
    WriteLiteral("<i class=\"fas fa-calendar-alt\"></i>");
    EndContext();
    PopWriter();
}
))
                );

#line default
#line hidden
            EndContext();
            BeginContext(7294, 131, true);
            WriteLiteral("\r\n            </div>\r\n            <span class=\"input-group-text\">Hasta</span>\r\n            <div class=\"col-xl-3\">\r\n                ");
            EndContext();
            BeginContext(7427, 301, false);
#line 205 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().DateBox()
                    .ID("diaHasta")
                    .DisplayFormat("dd/MM/yyyy")
                    .Type(DateBoxType.Date)
                    .Value(DateTime.Now)
                    .DropDownButtonTemplate(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    BeginContext(7675, 35, true);
    WriteLiteral("<i class=\"fas fa-calendar-alt\"></i>");
    EndContext();
    PopWriter();
}
))
                );

#line default
#line hidden
            EndContext();
            BeginContext(7730, 131, true);
            WriteLiteral("\r\n            </div>\r\n            <span class=\"input-group-text\">Turno</span>\r\n            <div class=\"col-xl-2\">\r\n                ");
            EndContext();
            BeginContext(7863, 715, false);
#line 215 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().SelectBox()
                    //.ID("select-prefix")
                    .ID("diaTurno")
                    .DataSource(d => d.Mvc().LoadAction("GetTurnos2")
                                            .Key("Cod_turno")
                                            .Controller("Tablas")
                                            .LoadParams(new { idEmpresa = ViewBag.idEmpresa }))
                    .Placeholder("Todos")
                    //.Width(225)
                    .DisplayExpr("Cod_turno")
                    .ValueExpr("Cod_turno")
                    .SearchEnabled(true)
                    .DropDownButtonTemplate(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    BeginContext(8530, 30, true);
    WriteLiteral("<i class=\"far fa-clock-o\"></i>");
    EndContext();
    PopWriter();
}
))
                );

#line default
#line hidden
            EndContext();
            BeginContext(8580, 176, true);
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n\r\n        <br />\r\n\r\n        <div class=\"row\">\r\n            <span class=\"input-group-text\">Sku</span>\r\n            <div class=\"col-xl-5\">\r\n");
            EndContext();
            BeginContext(8914, 18, true);
            WriteLiteral("\r\n                ");
            EndContext();
            BeginContext(8934, 576, false);
#line 239 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().SelectBox()
                    .ID("SkuDia")
                    .Placeholder("Todos los SKU")
                    .DataSource(d => d.Mvc().LoadAction("Lista_sku")
                                            .Key("Cod_producto")
                                            .Controller("Analitica")
                                            .LoadParams(new { idEmpresa = ViewBag.idEmpresa }))
                    .DisplayExpr("Des_producto")
                    .ValueExpr("Cod_producto")
                    .SearchEnabled(true)
                );

#line default
#line hidden
            EndContext();
            BeginContext(9511, 81, true);
            WriteLiteral("\r\n            </div>\r\n\r\n            <button class=\"col-xl-1 btn btn-info agregar\"");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 9592, "\"", 9632, 3);
            WriteAttributeValue("", 9602, "obtenerDia(", 9602, 11, true);
#line 252 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
WriteAttributeValue("", 9613, ViewBag.idEmpresa, 9613, 18, false);

#line default
#line hidden
            WriteAttributeValue("", 9631, ")", 9631, 1, true);
            EndWriteAttribute();
            BeginContext(9633, 15, true);
            WriteLiteral(" id=\"prueba\">\r\n");
            EndContext();
            BeginContext(9700, 88, true);
            WriteLiteral("                Aplicar\r\n            </button>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n");
            EndContext();
            BeginContext(9811, 141, true);
            WriteLiteral("<div class=\"container-fluid\" id=\"horaFilter\" style=\"cursor:pointer; display:none;\">\r\n    <div class=\"container\">\r\n        <div class=\"row\">\r\n");
            EndContext();
            BeginContext(10003, 109, true);
            WriteLiteral("            <span class=\"input-group-text\">Desde</span>\r\n            <div class=\"col-xl-4\">\r\n                ");
            EndContext();
            BeginContext(10114, 312, false);
#line 268 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().DateBox()
                    .ID("horaDesde")
                    .DisplayFormat("dd/MM/yyyy HH:mm")
                    .Type(DateBoxType.DateTime)
                    .Value(DateTime.Now)
                    .DropDownButtonTemplate(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    BeginContext(10373, 35, true);
    WriteLiteral("<i class=\"fas fa-calendar-alt\"></i>");
    EndContext();
    PopWriter();
}
))
                );

#line default
#line hidden
            EndContext();
            BeginContext(10428, 22, true);
            WriteLiteral("\r\n            </div>\r\n");
            EndContext();
            BeginContext(10501, 109, true);
            WriteLiteral("            <span class=\"input-group-text\">Hasta</span>\r\n            <div class=\"col-xl-4\">\r\n                ");
            EndContext();
            BeginContext(10612, 312, false);
#line 279 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().DateBox()
                    .ID("horaHasta")
                    .DisplayFormat("dd/MM/yyyy HH:mm")
                    .Type(DateBoxType.DateTime)
                    .Value(DateTime.Now)
                    .DropDownButtonTemplate(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    BeginContext(10871, 35, true);
    WriteLiteral("<i class=\"fas fa-calendar-alt\"></i>");
    EndContext();
    PopWriter();
}
))
                );

#line default
#line hidden
            EndContext();
            BeginContext(10926, 180, true);
            WriteLiteral("\r\n            </div>\r\n\r\n\r\n        </div>\r\n\r\n        <br />\r\n\r\n        <div class=\"row\">\r\n            <span class=\"input-group-text\">Sku</span>\r\n            <div class=\"col-xl-5\">\r\n");
            EndContext();
            BeginContext(11265, 18, true);
            WriteLiteral("\r\n                ");
            EndContext();
            BeginContext(11285, 577, false);
#line 298 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
            Write(Html.DevExtreme().SelectBox()
                    .ID("SkuHora")
                    .Placeholder("Todos los SKU")
                    .DataSource(d => d.Mvc().LoadAction("Lista_sku")
                                            .Key("Cod_producto")
                                            .Controller("Analitica")
                                            .LoadParams(new { idEmpresa = ViewBag.idEmpresa }))
                    .DisplayExpr("Des_producto")
                    .ValueExpr("Cod_producto")
                    .SearchEnabled(true)
                );

#line default
#line hidden
            EndContext();
            BeginContext(11863, 103, true);
            WriteLiteral("\r\n            </div>\r\n\r\n            <br />\r\n\r\n            <button class=\"col-xl-1 btn btn-info agregar\"");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 11966, "\"", 12007, 3);
            WriteAttributeValue("", 11976, "obtenerHora(", 11976, 12, true);
#line 313 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
WriteAttributeValue("", 11988, ViewBag.idEmpresa, 11988, 18, false);

#line default
#line hidden
            WriteAttributeValue("", 12006, ")", 12006, 1, true);
            EndWriteAttribute();
            BeginContext(12008, 15, true);
            WriteLiteral(" id=\"prueba\">\r\n");
            EndContext();
            BeginContext(12075, 502, true);
            WriteLiteral(@"                Aplicar
            </button>
        </div>

    </div>
</div>

<br />

<div id=""progresBar"" style=""display:none;"">
    <div class=""spinner-border text-secondary"" role=""status"">
        <span class=""sr-only"">Calculando valores...</span>
    </div>
    <a>&nbsp; Calculando valores...</a>
    <br />
</div>

<figure class=""highcharts-figure"">
    <div id=""container""></div>
</figure>

<button class=""btn btn-primary agregar"" onclick=""mostrarTabla()"" id=""MOTabla"">
");
            EndContext();
            BeginContext(12701, 244, true);
            WriteLiteral("    Mostrar tabla\r\n</button>\r\n\r\n<div id=\"divTabla\" style=\"padding: 10px; display:none;\">\r\n    <div id=\"tablaRegistros\"></div>\r\n</div>\r\n\r\n<div id=\"divTabla\" style=\"padding: 10px; display:none;\">\r\n    <div id=\"tablaRegistros\"></div>\r\n</div>\r\n\r\n\r\n");
            EndContext();
            BeginContext(13309, 144, true);
            WriteLiteral("\r\n<script type=\"text/javascript\">\r\n\r\n    var FactoryX = FactoryX || {};\r\n    FactoryX.Urls = FactoryX.Urls || {};\r\n    FactoryX.Urls.baseUrl = \'");
            EndContext();
            BeginContext(13454, 16, false);
#line 372 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
                        Write(Url.Content("~"));

#line default
#line hidden
            EndContext();
            BeginContext(13470, 41, true);
            WriteLiteral("\';\r\n\r\n    FactoryX.Urls.AsignaEmpresa = \'");
            EndContext();
            BeginContext(13512, 92, false);
#line 374 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
                              Write(Html.Raw(Url.Action("AsignaEmpresa","Planificacion", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(13604, 4, true);
            WriteLiteral("\';\r\n");
            EndContext();
            BeginContext(13904, 36, true);
            WriteLiteral("    FactoryX.Urls.GetRendimiento = \'");
            EndContext();
            BeginContext(13941, 90, false);
#line 378 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
                               Write(Html.Raw(Url.Action("GetRendimiento", "Analitica", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(14031, 46, true);
            WriteLiteral("\';\r\n    FactoryX.Urls.IndicadorRendimiento = \'");
            EndContext();
            BeginContext(14078, 96, false);
#line 379 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
                                     Write(Html.Raw(Url.Action("IndicadorRendimiento", "Analitica", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(14174, 57, true);
            WriteLiteral("\';\r\n    //FactoryX.Urls.agrega_grafico_disponibilidad = \'");
            EndContext();
            BeginContext(14232, 104, false);
#line 380 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Analitica\Rendimiento.cshtml"
                                                Write(Html.Raw(Url.Action("agrega_grafico_disponibilidad","Analitica", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(14336, 17, true);
            WriteLiteral("\';\r\n\r\n</script>\r\n");
            EndContext();
            BeginContext(14353, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3defb3cb8e5ff4773d0f8c535e33fa8d285c79e734254", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
