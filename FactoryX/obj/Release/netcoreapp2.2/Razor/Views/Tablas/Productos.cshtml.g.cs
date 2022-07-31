#pragma checksum "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5b9988b78f7aacf50b5cfb85671d8fb71d721799"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Tablas_Productos), @"mvc.1.0.view", @"/Views/Tablas/Productos.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Tablas/Productos.cshtml", typeof(AspNetCore.Views_Tablas_Productos))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5b9988b78f7aacf50b5cfb85671d8fb71d721799", @"/Views/Tablas/Productos.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3801cde418586957cebeceffc6a060738280b80e", @"/Views/_ViewImports.cshtml")]
    public class Views_Tablas_Productos : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/TablasPrincipales.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 1 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
  
    ViewData["Title"] = "Productos";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(92, 45, true);
            WriteLiteral("\r\n<h1>Productos</h1>\r\n\r\n<input id=\"idEmpresa\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 137, "\"", 163, 1);
#line 8 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
WriteAttributeValue("", 145, ViewBag.idEmpresa, 145, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(164, 28, true);
            WriteLiteral(" style=\"display:none;\"/>\r\n\r\n");
            EndContext();
            BeginContext(194, 1515, false);
#line 10 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
Write(Html.DevExtreme().DataGrid<FactoryX.Models.Productos>().ID("dg_productos")
    .DataSource(ds => ds.Mvc()
        .LoadParams(new { idEmpresa = ViewBag.idEmpresa })
        .Controller("Tablas")
        .LoadAction("GetProductos")
        .InsertAction("InsertProductos")
        .Key("Cod_producto")
        .UpdateAction("UpdateProductos").OnBeforeSend("asignaId") //.OnBeforeSend("function(actionName, e) { e.data.context = '" + ViewBag.idEmpresa + "'; }")
        .DeleteAction("DeleteProductos")
    )
    .Editing(editing => editing
                        .Mode(GridEditMode.Row)
                        .AllowAdding(true)
                        .AllowUpdating(true)
                        .AllowDeleting(true)
                        .UseIcons(true)
                        .Texts(t => t.ConfirmDeleteMessage("Esta seguro que desea eliminar el registro?"))
                      )
    .Columns(columns =>
    {
        columns.AddFor(m => m.Cod_producto).Width(150);

        columns.AddFor(m => m.Des_producto);
        
        columns.AddFor(m => m.Cod_grupo).Width(150)
        .Lookup(lookup => lookup
            .DataSource(d => d.Mvc().Controller("Tablas").LoadAction("GetAgrupacion").Key("Cod_grupo"))
        .ValueExpr("Cod_grupo")
        .DisplayExpr("Des_grupo")
        );

    })
    .AllowColumnResizing(true)
    .AllowColumnReordering(true)
    .FilterRow(filterRow => filterRow.Visible(true))
    .GroupPanel(groupPanel => groupPanel.Visible(true))
);

#line default
#line hidden
            EndContext();
            BeginContext(1710, 233, true);
            WriteLiteral("\r\n\r\n<br />\r\n<br />\r\n\r\n<hr style=\"color: #0056b2;\" />\r\n\r\n<h1>Grupo de productos</h1>\r\n<a class=\"dx-button\" onclick=\"ver_gruposProductos()\"> &nbsp; Ver grupo de productos &nbsp; </a>\r\n\r\n<div id=\"grid_grupo\" style=\"display:none;\">\r\n    ");
            EndContext();
            BeginContext(1945, 1185, false);
#line 57 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
Write(Html.DevExtreme().DataGrid<FactoryX.Models.Grupos>().ID("dg_grupo_productos")
        .DataSource(ds => ds.Mvc()
            .Controller("Tablas")
            .LoadAction("GetAgrupacion")
            .InsertAction("InsertAgrupacion")
            .Key("Cod_grupo")
            .UpdateAction("UpdateAgrupacion")
            .DeleteAction("DeleteAgrupacion")
        )
        .Editing(editing => editing
                            .Mode(GridEditMode.Row)
                            .AllowAdding(true)
                            .AllowUpdating(true)
                            .AllowDeleting(true)
                            .UseIcons(true)
                            .Texts(t => t.ConfirmDeleteMessage("Esta seguro que desea eliminar el registro?"))
                            //.Texts(t => t.ConfirmDeleteTitle("Eliminar"))
                          )
        .Columns(columns =>
         {
             //columns.AddFor(m => m.Cod_grupo).Width(150);

             columns.AddFor(m => m.Des_grupo);
         })
        .AllowColumnResizing(true)
        .AllowColumnReordering(true)
        .FilterRow(filterRow => filterRow.Visible(true))        
    );

#line default
#line hidden
            EndContext();
            BeginContext(3131, 10, true);
            WriteLiteral("\r\n</div>\r\n");
            EndContext();
            BeginContext(3141, 49, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5b9988b78f7aacf50b5cfb85671d8fb71d7217998052", async() => {
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
            BeginContext(3190, 146, true);
            WriteLiteral("\r\n\r\n<script type=\"text/javascript\">\r\n\r\n    var FactoryX = FactoryX || {};\r\n    FactoryX.Urls = FactoryX.Urls || {};\r\n    FactoryX.Urls.baseUrl = \'");
            EndContext();
            BeginContext(3337, 16, false);
#line 92 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
                        Write(Url.Content("~"));

#line default
#line hidden
            EndContext();
            BeginContext(3353, 41, true);
            WriteLiteral("\';\r\n\r\n    FactoryX.Urls.AsignaEmpresa = \'");
            EndContext();
            BeginContext(3395, 85, false);
#line 94 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
                              Write(Html.Raw(Url.Action("AsignaEmpresa","Tablas", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(3480, 38, true);
            WriteLiteral("\';\r\n    FactoryX.Urls.GetProductos = \'");
            EndContext();
            BeginContext(3519, 84, false);
#line 95 "C:\Users\MyHP\source\repos\factoryx\FactoryX\Views\Tablas\Productos.cshtml"
                             Write(Html.Raw(Url.Action("GetProductos","Tablas", new { @idEmpresa = ViewBag.idEmpresa})));

#line default
#line hidden
            EndContext();
            BeginContext(3603, 17, true);
            WriteLiteral("\';\r\n\r\n\r\n</script>");
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
