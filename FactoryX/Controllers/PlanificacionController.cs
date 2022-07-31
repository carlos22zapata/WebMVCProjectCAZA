using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FactoryX.Data;
using FactoryX.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;
using System.Globalization;
using OfficeOpenXml.Table;
using OfficeOpenXml.Drawing.Chart;
using Nancy.Json;
using System.Net.NetworkInformation;

namespace FactoryX.Controllers
{
    public class PlanificacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EmpresaDbContext _Econtext;
        public static int idEmpresaX;
        public static string cod_planX;

        public PlanificacionController(ApplicationDbContext context)
        {
            _context = context;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        public void AsignaEmpresa(int idEmpresa)
        {
            idEmpresaX = idEmpresa;
        }

        [System.Web.Http.Authorize]
        public EmpresaDbContext ConBD(string bd)
        {            
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<EmpresaDbContext>();
            var connectionString = configuration.GetConnectionString(bd);

            builder.UseSqlServer(connectionString);

            if (ValidaUsuario().Result == false)
            {
                //Response.Redirect(Url.Content("~/Identity/Account/Login?Lon=1"));
            }

            return _Econtext = new EmpresaDbContext(builder.Options); //new EmpresaDbContext(builder.Options);                   
        }

        public async Task<bool> ValidaUsuario()
        {
            //Reviso que sea el mismo usuario y la misma MacAdrress
            var userId = User.getUserId();
            var IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            int existe = await _context.LogOn.Where(w => w.UserId == userId && w.IpAddress == IP).CountAsync();

            if (existe == 1)
            {
                return true;
            }
            else
            {
                //
                return false;
                Response.Redirect(Url.Content("~/Identity/Account/Login"));
            }
        }

        [System.Web.Http.Authorize]
        public async Task<string> DescargarExcel(int idEmpresa, DateTime fini_, DateTime ffin_)
        {
            try
            {
                ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
                ViewBag.idEmpresa = idEmpresa;
                AsignaEmpresa(idEmpresa);
                cod_planX = null;
            }
            catch (Exception ex)
            {
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                var pedidosModel = new ExportModelConsolidado
                {

                    exportar = await (from p in ctx.Pedidos
                                      join c in ctx.Centros on p.Cod_centro equals c.Cod_centro
                                      join rp in ctx.Reng_pedidos on p.Cod_plan equals rp.Cod_plan
                                      join pro in ctx.Productos on rp.Cod_producto equals pro.Cod_producto
                                      select new ExportModel
                                      {
                                          Codigo_plan = p.Cod_plan,
                                          //Estado = (p.Fecha == null && p.Fecha_fin == null) ? "No iniciado" : ((p.Fecha_fin == null) ? "Iniciado" : "Cerrado"),
                                          Codigo_planta = p.Cod_centro,
                                          //Fecha_inicio = p.Fecha.ToString(),
                                          //Fecha_fin = p.Fecha_fin.ToString(),
                                          //Descripcion_planta = c.Des_centro,
                                          Codigo_producto = rp.Cod_producto,
                                          //Descripcion_producto = pro.Des_producto,
                                          Cantidad = rp.Cantidad,
                                          Fecha_despacho = rp.Fecha_desp.ToString()

                                      }).ToListAsync(),
                };
                ///Aqui va lo de excel
                int numRows = pedidosModel.exportar.Count();
                if (numRows > 0)
                {
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Exportacion");
                    workSheet.Cells[4, 1].LoadFromCollection(pedidosModel.exportar, true);
                    workSheet.Cells[4, 1, 4, 9].Style.Font.Bold = true;
                    workSheet.Cells.AutoFitColumns();
                    workSheet.Cells[1, 1].Value = "Ordenes de producción";
                    workSheet.Cells[2, 1].Value = "Fecha actual:" + DateTime.Now;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;

                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers["content-disposition"] = "attachment; filename=Todo_Ordenes_de_produccion.xlsx";
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.Body);
                    }

                    //Aqui va lo de excel fin
                    return "";  //View(pedidosModel);
                                        
                }

                List<string> prr = new List<String>();

                return null; //View(prr.ToList());

            }
        }

        //Prueba
        public ActionResult ExportarExcelX(List<Productos> array)
        {

            var listado = array;

            var ruta = new FileInfo("D:\\archivo_excel\\reporte_horas.xlsx");
            ExcelPackage pkg = new ExcelPackage(ruta);
            //ExcelWorksheet ws = pkg.Workbook.Worksheets.Add("Reportes");
            ExcelWorksheet ws = pkg.Workbook.Worksheets["Gestion_horas"];

            ws.Cells["A1"].Value = "Cod_producto";
            ws.Cells["B1"].Value = "Des_producto";

            int rowstart = 7;

            foreach (Productos obj in listado)
            {
                ws.Cells[string.Format("A{0}", rowstart)].Value = obj.Cod_producto; //obj.Proyecto.IdProyecto; //obj.Proyecto.IdProyecto;
                ws.Cells[string.Format("B{0}", rowstart)].Value = obj.Des_producto;//obj.Proyecto.Descripcion;
                
                rowstart++;

            }

            var myChart = ws.Drawings.AddChart("chart", eChartType.Line);


            //Define las series para el cuadro
            var series = myChart.Series.Add("C7: E7", "C6: E6");
            myChart.Border.Fill.Color = System.Drawing.Color.Green;
            myChart.Title.Text = "My Chart";
            myChart.SetSize(500, 400);

            //Agregar a la 6ta fila y a la 6ta columna
            myChart.SetPosition(6, 0, 10, 0);

            var myChart2 = myChart.PlotArea.ChartTypes.Add(eChartType.ColumnClustered);
            //Define las series para el cuadro
            var series2 = myChart2.Series.Add("C7: E7", "C6: E6");

            //Agregar a la 6ta fila y a la 6ta columna
            //myChart2.SetPosition(6, 0, 10, 0);

            ws.Cells["A:AZ"].AutoFitColumns();

            MemoryStream memoryStream = new MemoryStream();
            pkg.SaveAs(memoryStream);
            memoryStream.Position = 0;

            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "Gestion_horas.xlsx" };

        }
                
        public async Task<string> DescargarExcelFiltro(int idEmpresa, DateTime fini_, string ffin_)
        {
            ExcelPackage ExcelPkg = new ExcelPackage();
            ExcelWorksheet wsSheet1 = ExcelPkg.Workbook.Worksheets.Add("Sheet1");

            using (ExcelRange Rng = wsSheet1.Cells[2, 2, 2, 2])
            {
                Rng.Value = "Welcome to Everyday be coding - tutorials for beginners";
                Rng.Style.Font.Size = 16;
                Rng.Style.Font.Bold = true;
                Rng.Style.Font.Italic = true;
            }
            wsSheet1.Protection.IsProtected = false;
            wsSheet1.Protection.AllowSelectLockedCells = false;

            try
            {
                ExcelPkg.SaveAs(new FileInfo(@"C:\New_Caza.xlsx"));
            }
            catch (Exception ex)
            { 
            
            }
            
            return "";
        }

        [System.Web.Http.Authorize]
        public async Task<IActionResult> DescargarExcelPlantilla(int idEmpresa)
        {
            try
            {
                ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
                ViewBag.idEmpresa = idEmpresa;
                AsignaEmpresa(idEmpresa);
                cod_planX = null;
            }
            catch (Exception ex)
            {
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<Pedidos> pedi = new List<Pedidos>();
                pedi.Add(new Pedidos() { Cod_plan = "", Estado = 0, Cod_centro = "" });

                var pedidosModel = new ExportModelConsolidado
                {

                    exportar = (from p in pedi
                                      select new ExportModel
                                      {
                                          Codigo_plan = "",
                                          //Estado = "No iniciado", //(p.Fecha == null && p.Fecha_fin == null) ? "No iniciado" : ((p.Fecha_fin == null) ? "Iniciado" : "Cerrado"),
                                          Codigo_planta = "", //p.Cod_centro,
                                          //Fecha_inicio = p.Fecha.ToString(),
                                          //Fecha_fin = p.Fecha_fin.ToString(),
                                          //Descripcion_planta = "", //c.Des_centro,
                                          Codigo_producto = "", //rp.Cod_producto,
                                          //Descripcion_producto = "", //pro.Des_producto,
                                          Cantidad = 0, //rp.Cantidad,
                                          Fecha_despacho = "01-01-2020" //rp.Fecha_desp.ToString()

                                      }).Take(1).ToList(),
                };

                ///Aqui va lo de excel

                int numRows = pedidosModel.exportar.Count();
                if (numRows > 0)
                {
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Exportacion");
                    workSheet.Cells[1, 1].LoadFromCollection(pedidosModel.exportar, true);
                    workSheet.Cells[1, 1, 1, 9].Style.Font.Bold = true;                    
                    workSheet.Cells.AutoFitColumns();
                    workSheet.Cells[1, 1].Value = "Codigo_plan";
                    workSheet.Cells["A1:A1000"].Style.Numberformat.Format = "@";
                    workSheet.Cells[1, 2].Value = "Codigo_planta";
                    workSheet.Cells["B1:B1000"].Style.Numberformat.Format = "@";
                    //workSheet.Cells[1, 3].Value = "Estado";
                    //workSheet.Cells[1, 4].Value = "Descripcion_planta";
                    workSheet.Cells[1, 3].Value = "Codigo_producto";
                    workSheet.Cells["E1:E1000"].Style.Numberformat.Format = "dd-mm-yyyy"; //"@";
                    //workSheet.Cells[1, 6].Value = "Descripcion_producto";
                    workSheet.Cells[1, 4].Value = "Cantidad";
                    workSheet.Cells[1, 5].Value = "Fecha_despacho";
                    //workSheet.Cells["H1:H1000"].Style.Numberformat.Format = "dd-mm-yyyy";
                    workSheet.Cells[1, 6].Value = "Unidad";
                    //workSheet.Cells["G1:G1000"].Style.Numberformat.Format = "@";

                    //workSheet.Cells[1, 1].Value = "Ordenes de producción";
                    //workSheet.Cells[2, 1].Value = "Fecha actual:" + DateTime.Now;
                    //workSheet.Cells[1, 1].Style.Font.Bold = true;

                    using (var memoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers["content-disposition"] = "attachment; filename=Plantilla_Ordenes_de_produccion.xlsx";
                        excel.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.Body);
                    }
                }
                ///Aqui va lo de excel fin
                return View(pedidosModel);


            }
        }

        public void AsignaCod_plan(string cod_plan)
        {
            cod_planX = cod_plan;
        }

        public async Task<IActionResult> HistoricoVariables(int idEmpresa)
        {
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        //ValoresGrafico
        public async Task<List<SeriesItem>> ValoresGrafico(int idEmpresa, string cod_activo, string tabla, string filtro, DateTime desde, DateTime hasta)
        {
            List<IOT_TR> gra;
            List<SeriesItem> series = new List<SeriesItem>();

            if (desde.Hour == 0 && desde.Minute == 0 && hasta.Hour == 0 && hasta.Minute == 0)
            {
                hasta = hasta.AddDays(1).AddMinutes(-1);
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {                

                try
                {

                    gra = await (from iot in ctx.IOT.FromSql("select id, value, timestamp, Sku_activo, Cod_plan from " + tabla)
                                 where iot.timestamp >= desde && iot.timestamp <= hasta
                                 select new IOT_TR
                                 {
                                     id = iot.id,
                                     value = iot.value.ToString(),
                                     timestamp = (iot.timestamp.ToString("HH:mm")),
                                     nombre_maquina = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).First(), //tabla.Substring(4, tamañoTabla) + "-" + (ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault()),
                                                                                                                                            //nombre_maquina1 = ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault(),
                                     umbralMinimo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMin).FirstOrDefault(),
                                     umbralMaximo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMax).FirstOrDefault(),
                                     variable = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Variable).FirstOrDefault(),
                                     unidad = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Unidad).FirstOrDefault(),
                                     sku = iot.Sku_activo, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Sku_activo).FirstOrDefault(),
                                     cod_plan = iot.Cod_plan //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Cod_plan).FirstOrDefault()
                                 }).ToListAsync();

                    series.Add(new SeriesItem()
                    {
                        id = gra.Max(s => s.id),
                        name = gra.Select(s => s.timestamp.ToString()).ToArray(),
                        data = gra.Select(s => Convert.ToDecimal(s.value)).ToArray(),
                        activo = gra.Max(m => m.nombre_maquina),
                        activo1 = gra.Max(m => m.nombre_maquina), //m.nombre_maquina1),
                        umbralMinimo = gra.Max(m => m.umbralMinimo),
                        umbralMaximo = gra.Max(m => m.umbralMaximo),
                        variable = gra.Max(m => m.variable),
                        unidad = gra.Max(m => m.unidad),
                        sku = gra.Select(m => m.sku).ToArray(),
                        cod_plan = gra.Select(s => s.cod_plan).ToArray()
                    });
                }
                catch (Exception ex)
                { }
            }

            return series;
        }
                

        #region Planificación, carga de pedido

        [System.Web.Http.Authorize]
        public async Task<IActionResult> Planificacion(int idEmpresa, string cod_plan)
        {
            try
            {
                ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
                ViewBag.idEmpresa = idEmpresa;
                AsignaEmpresa(idEmpresa);
                cod_planX = null;
            }
            catch (Exception ex)
            {             
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (cod_plan == null)
                {
                    var pedidosModel = new PedidosModel
                    {
                        pedidos = await (from p in ctx.Pedidos
                                         select new PedidosVM
                                         {
                                             Cod_plan = p.Cod_plan,
                                             Des_pedido = (p.Fecha == null ? "No iniciado" : 
                                                            p.Fecha_fin == null ? 
                                                                "Iniciado" : "Cerrado")

                                                                //"Iniciado - Fecha de inicio: " + p.Fecha.ToString() : 
                                                                //"Cerrado - Fecha de inicio: " + p.Fecha.ToString() + ", Fecha de cierre: " + p.Fecha_fin.ToString())
                                         }).ToListAsync(),

                        centros = await (from c in ctx.Centros
                                         select new Centros{ 
                                             Cod_centro = c.Cod_centro,
                                             Des_centro = c.Des_centro
                                         }).ToListAsync()
                    };

                    return View(pedidosModel);
                }
                else
                {
                    var pedidosModel = new PedidosModel
                    {
                        pedidos = await (from p in ctx.Pedidos
                                         where p.Cod_plan == cod_plan
                                         select new PedidosVM
                                         {
                                             Cod_plan = p.Cod_plan,
                                             Des_pedido = (p.Fecha == null ? "No iniciado" :
                                                            p.Fecha_fin == null ?
                                                                "Iniciado" : "Cerrado")

                                             //"Iniciado - Fecha de inicio: " + p.Fecha.ToString() : 
                                             //"Cerrado - Fecha de inicio: " + p.Fecha.ToString() + ", Fecha de cierre: " + p.Fecha_fin.ToString())
                                         }).ToListAsync()
                    };

                    return View(pedidosModel);
                }

                
            }            
        }

        [System.Web.Http.HttpGet]
        public async Task<object> InsertPedidosGen(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Ped = new Pedidos();
                JsonConvert.PopulateObject(values, Ped);

                var user = User.getUserId();

                var p = new Pedidos
                {
                    Cod_plan = Ped.Cod_plan,
                    Cod_centro = Ped.Cod_centro,
                    Usuario_incluye = user,
                    Fecha_usuario_incluye = DateTime.Now
                };

                ctx.Pedidos.Add(p);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(p);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> DeletePedidosGen(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Pedidos Ped = await ctx.Pedidos.Where(w => w.Cod_plan == key).FirstOrDefaultAsync();

                try
                {
                    ctx.Pedidos.Remove(Ped);
                    ctx.SaveChanges();

                }
                catch (Exception ex)
                {

                }
                return Ok();
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> InsertPedidosG(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Grup = new Grupos();
                JsonConvert.PopulateObject(values, Grup);

                var g = new Grupos
                {
                    Des_grupo = Grup.Des_grupo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Grupos.Add(g);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(g);
            }
        }

        //public async Task<List<IdentityError>> InsertPedidosT(int idEmpresa, string Cod_plan, string Cod_centro) //, int Estado, string Cod_producto, decimal Cantidad, DateTime FechaDespacho, string Unidad)

        public async Task<List<IdentityError>> InsertPedidosT(int idEmpresa, List<ot> resultado ) //string result1)
        {
            var user = User.getUserId();

            var errorList = new List<IdentityError>();
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    foreach (var r in resultado)
                    {
                        try
                        {
                            InsertPedidosT_Gen(idEmpresa, r.Codigo_plan, r.Codigo_planta);
                            InsertPedidosT_Reng(idEmpresa, r.Codigo_plan, r.Codigo_planta, r.Codigo_producto, decimal.Parse(r.Cantidad), DateTime.Now, r.Unidad);
                        }
                        catch (Exception ex)
                        { 
                        
                        }
                        
                    }

                    errorList.Add(new IdentityError
                    {
                        Code = "Save",
                        Description = "Registro insertado correctamente."
                    });

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    errorList.Add(new IdentityError
                    {
                        Code = "Error al guardar",
                        Description = ex.ToString()
                    });
                }
            }
            return errorList;
        }

        public async Task<List<IdentityError>> InsertPedidosT_Gen(int idEmpresa, string Cod_plan, string Cod_centro)
        {
            var user = User.getUserId();

            var errorList = new List<IdentityError>();
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    var PED = new Pedidos
                    {
                        Cod_plan = Cod_plan,
                        Cod_centro = Cod_centro,
                        Usuario_incluye = user,
                        Fecha_usuario_incluye = DateTime.Now
                    };

                    try
                    {
                        ctx.Add(PED);
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    errorList.Add(new IdentityError
                    {
                        Code = "Error al guardar",
                        Description = ex.ToString()
                    });
                }

                return errorList;
            }
        }

        public async Task<List<IdentityError>> InsertPedidosT_Reng(int idEmpresa, string Cod_plan, string Cod_centro, string Cod_producto, decimal Cantidad, DateTime FechaDespacho, string Unidad)
        {
            var user = User.getUserId();

            Cod_producto = Cod_producto.Trim();

            var errorList = new List<IdentityError>();
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {                    

                    var RPED = new Reng_pedidos
                    {
                        Cod_plan = Cod_plan,
                        Cod_producto = Cod_producto,
                        Cantidad = Cantidad,
                        Fecha_desp = FechaDespacho,
                        Cod_unidad = Unidad
                    };

                    var existe = ctx.Reng_pedidos.Where(w => w.Cod_plan == Cod_plan && w.Cod_producto == Cod_producto).Count();

                    if (existe == 0)
                    {
                        try
                        {
                            ctx.Add(RPED);
                            await ctx.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    

                    errorList.Add(new IdentityError
                    {
                        Code = "Save",
                        Description = "Registro insertado correctamente."
                    });

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    errorList.Add(new IdentityError
                    {
                        Code = "Error al guardar",
                        Description = ex.ToString()
                    });
                }
            }
            return errorList;
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetPedidosG(int idEmpresa, DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var pedidos = await (from p in ctx.Pedidos
                                     join c in ctx.Centros on p.Cod_centro equals c.Cod_centro
                                     select new PedidosVM
                                     {
                                         Cod_plan = p.Cod_plan,
                                         Des_pedido = (p.Fecha == null ? "No iniciado" :
                                                            p.Fecha_fin == null ?
                                                                "Iniciado" : "Cerrado"),
                                         Cod_centro = p.Cod_centro,
                                         Des_centro = c.Des_centro
                                     }).ToListAsync();

                return DataSourceLoader.Load(pedidos, loadOptions);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetUnidad(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Unidades> uni = await (from u in ctx.Unidades
                                      select new Unidades
                                      {
                                          Cod_unidad = u.Cod_unidad,
                                          Des_unidad = u.Des_unidad
                                      }).ToListAsync();

                return DataSourceLoader.Load(uni, loadOptions);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetPedidos(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<ListaPedidos> Ped = new List<ListaPedidos>();

                if (cod_planX == null)
                {
                    Ped = await (from p in ctx.Pedidos   
                                 join r in ctx.Reng_pedidos on p.Cod_plan equals r.Cod_plan                                 
                                 select new ListaPedidos
                                 {
                                     Id = r.id,
                                     Cod_plan = p.Cod_plan,
                                     Cod_centro = p.Cod_centro,
                                     Fecha = p.Fecha,
                                     Cod_producto = r.Cod_producto,
                                     Cantidad = r.Cantidad,
                                     Fecha_desp = r.Fecha_desp,
                                     Cod_unidad = r.Cod_unidad,
                                     Estado = p.Estado == null && p.Fecha == null ? "No iniciado" :
                                              p.Estado == null && p.Fecha != null && p.Fecha_fin == null ? "En proceso" :
                                              "Cerrado"
                                 }).ToListAsync();

                    return DataSourceLoader.Load(Ped, loadOptions);
                }
                else
                {
                    Ped = await (from p in ctx.Pedidos
                                 join r in ctx.Reng_pedidos on p.Cod_plan equals r.Cod_plan
                                 where p.Cod_plan == cod_planX
                                 select new ListaPedidos
                                 {
                                     Id = r.id,
                                     Cod_plan = p.Cod_plan,
                                     Cod_centro = p.Cod_centro,
                                     Fecha = p.Fecha,
                                     Cod_producto = r.Cod_producto,
                                     Cantidad = r.Cantidad,
                                     Fecha_desp = r.Fecha_desp, //p.Fecha_desp.ToString("dd/MM/yyyy"),
                                     Cod_unidad = r.Cod_unidad,
                                     Estado = p.Estado == null && p.Fecha == null ? "No iniciado" :
                                              p.Estado == null && p.Fecha != null && p.Fecha_fin == null ? "En proceso" :
                                              "Cerrado"
                                 }).ToListAsync();

                    return DataSourceLoader.Load(Ped, loadOptions);
                }
                
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertPedidos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Rped = new Reng_pedidos();
                JsonConvert.PopulateObject(values, Rped);

                var rp = new Reng_pedidos
                {
                    Cod_plan = cod_planX,
                    Cod_producto = Rped.Cod_producto,
                    Cantidad = Rped.Cantidad,
                    Fecha_desp = Rped.Fecha_desp,
                    Cod_unidad = Rped.Cod_unidad
                };

                ctx.Reng_pedidos.Add(rp);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                }

                return Ok(rp);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdatePedidos(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Reng_pedidos Rped = await ctx.Reng_pedidos.Where(w => w.id == int.Parse(key)).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, Rped);
                
                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                string cp0 = Rped.Cod_plan;

                Pedidos ped = await ctx.Pedidos.Where(w => w.Cod_plan == Rped.Cod_plan).FirstOrDefaultAsync();

                ped.Usuario_modifica = User.getUserId();
                ped.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(ped);
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeletePedidos(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Reng_pedidos Rped = await ctx.Reng_pedidos.Where(w => w.id == int.Parse(key)).FirstOrDefaultAsync();

                try
                {
                    ctx.Reng_pedidos.Remove(Rped);
                    ctx.SaveChanges();
                    
                }
                catch (Exception ex)
                { 
                
                }
                return Ok();
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetProductos(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Productos> Prod = await (from p in ctx.Productos
                                              select new Productos
                                              {
                                                  Cod_producto = p.Cod_producto,
                                                  Des_producto = p.Cod_producto.Trim() + " - " + p.Des_producto.Trim(),
                                                  Cod_grupo = p.Cod_grupo
                                              }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Prod, loadOptions);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<bool> PlanCerrado(string cod_plan, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (cod_plan != null)
                {
                    var status = await (from s in ctx.Pedidos
                                        where s.Cod_plan == cod_plan
                                        select new
                                        {
                                            Fecha_fin = s.Fecha_fin == null ? false : true
                                        }).FirstOrDefaultAsync();

                    return status.Fecha_fin;
                }
                else
                {
                    return false;
                }

                

            }
                

            return true;
        }

        #endregion Planificación, carga de pedido

        #region Centros/pedidos, carga de pedido

        [System.Web.Http.HttpGet]
        public async Task<object> GetCentros(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Centros> Cent = await (from p in ctx.Centros
                                              select new Centros
                                              {
                                                  Cod_centro = p.Cod_centro,
                                                  Des_centro = p.Des_centro
                                              }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Cent, loadOptions);
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetCentros2(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Centros> Cent = await (from p in ctx.Centros
                                            select new Centros
                                            {
                                                Cod_centro = p.Cod_centro,
                                                Des_centro = p.Des_centro
                                            }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Cent, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateCentros(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Centros Cent = await ctx.Centros.Where(w => w.Cod_centro == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, Cent);

                Cent.Usuario_modifica = User.getUserId();
                Cent.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(Cent);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertCentros(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Cent = new Centros();
                JsonConvert.PopulateObject(values, Cent);

                var c = new Centros
                {
                    Cod_centro = Cent.Cod_centro,
                    Des_centro = Cent.Des_centro,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Centros.Add(c);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(c);
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteCentros(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Centros Cent = await ctx.Centros.Where(w => w.Cod_centro == key).FirstOrDefaultAsync();
                ctx.Centros.Remove(Cent);

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo eliminar el registro, es posible que sea debido a que esta asociado a algún pedido");
                }

                return Ok();
            }
        }

        #endregion Planificación, carga de pedido

        #region Orden de producción

        public async Task<List<IdentityError>> GuardaPlan(string cod_plan, string cod_centro, int idEmpresa)
        {
            var errorList = new List<IdentityError>();

            var user = User.getUserId();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var pedidos = new Pedidos
                {
                    Cod_plan = cod_plan,
                    Cod_centro = cod_centro,
                    Usuario_incluye = user,
                    Fecha_usuario_incluye = DateTime.Now
                };

                ctx.Add(pedidos);

                try
                {
                    await ctx.SaveChangesAsync();

                    errorList.Add(new IdentityError
                    {
                        Code = "OK",
                        Description = "Ok"
                    });

                }

                catch (Exception ex)
                {
                    errorList.Add(new IdentityError
                    {
                        Code = "Error al guardar",
                        Description = ex.ToString()
                    });
                }
            }

            return errorList;
        }

        public async Task<List<IdentityError>> EliminaPlan(string cod_plan, int idEmpresa)
        {
            var errorList = new List<IdentityError>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Pedidos pe = await ctx.Pedidos.Where(w => w.Cod_plan == cod_plan).FirstOrDefaultAsync();

                ctx.Pedidos.Remove(pe);
                await ctx.SaveChangesAsync();

                return errorList;
            }

                
        }

        public async Task CierrePlanes(string valores, int idEmpresa)
        {
            valores = valores.Replace("[", "").Replace("]", "");
            
            string[] subs = valores.Split(',');
      
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var user = User.getUserId();

                foreach (var sub in subs)
                {
                    var planes = new cod_planes();
                    JsonConvert.PopulateObject(sub, planes);

                    string cp = planes.Cod_plan;

                    //Si existe este plan asociado a algun registro en Activos_tablas
                    //var existe = await (from a in ctx.Activos_tablas
                    //                      join r in ctx.Reng_pedidos on a.id_Reng_pedido equals r.id
                    //                      where r.Cod_plan == cp
                    //                      select new { id = a.id }).ToListAsync();

                    int existe = await ctx.Pedidos.Where(w => w.Cod_plan == cp && w.Estado == null && w.Fecha_fin == null && w.Fecha != null).CountAsync();

                    if (existe > 0)
                    {
                        Pedidos p = await ctx.Pedidos.Where(w => w.Cod_plan == cp).FirstOrDefaultAsync();

                        p.Fecha_fin = DateTime.Now;
                        p.Estado = 1;

                        ctx.Update(p);
                        await ctx.SaveChangesAsync();

                        //Actualizo ahora Activos_tablas para poner en Null el codigo del plan y el sku
                        var selecto = await (from r in ctx.Reng_pedidos
                                             join t in ctx.Activos_tablas on r.id equals t.id_Reng_pedido
                                             where r.Cod_plan == cp
                                             select new
                                             {
                                                 id = t.id
                                             }).ToListAsync();

                        if (selecto.Count() > 0)
                        {
                            foreach (var x in selecto)
                            {
                                Activos_tablas act = await ctx.Activos_tablas.Where(w => w.id == x.id).FirstOrDefaultAsync();

                                act.id_Reng_pedido = null;
                                act.Sku_activo = null;

                                ctx.Update(act);
                                await ctx.SaveChangesAsync();
                            }
                        }


                    }

                }
            }
        }

        //public async Task<List<IdentityError>> CerrarOPPlan(int iid, int idEmpresa)
        //{
        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        var errorList = new List<IdentityError>();
        //        var user = User.getUserId();

        //        //string cp = await ctx.Activos_tablas.Where(w => w.id == iid).Select(s => s.id_Reng_pedido).FirstOrDefaultAsync();

        //        var cp = await (from at in ctx.Activos_tablas
        //                        join rp in ctx.Reng_pedidos on at.id_Reng_pedido equals rp.id
        //                        where at.id == iid
        //                        select new
        //                        {
        //                            rp.Cod_plan
        //                        }).FirstOrDefaultAsync();

        //        try
        //        {
        //            //Actuvlizo los pedidos para poner el estado en 1 (Terminado)

        //            Pedidos ped = await ctx.Pedidos.Where(w => w.Cod_plan == cp.Cod_plan).FirstOrDefaultAsync();
        //            ped.Estado = 1;
        //            ped.Fecha_fin = DateTime.Now;

        //            ctx.Update(ped);
        //            await ctx.SaveChangesAsync();

        //            //Actualizo ahora Activos_tablas para poner en Null el codigo del plan y el sku

        //            Activos_tablas at = await ctx.Activos_tablas.Where(w => w.id == iid).FirstOrDefaultAsync();
        //            var registros = await ctx.Activos_tablas.Where(w => w.id_Reng_pedido == (int)at.id_Reng_pedido).ToListAsync();

        //            foreach (var r in registros)
        //            {
        //                Activos_tablas at2 = await ctx.Activos_tablas.Where(w => w.id == r.id).FirstOrDefaultAsync();

        //                at2.Sku_activo = null;
        //                at2.id_Reng_pedido = null;

        //                ctx.Update(at2);
        //                await ctx.SaveChangesAsync();
        //            }

        //            errorList.Add(new IdentityError
        //            {
        //                Code = "Update",
        //                Description = "Registro modificado correctamente."
        //            });
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            errorList.Add(new IdentityError
        //            {
        //                Code = "Error al modificar",
        //                Description = ex.ToString()
        //            });
        //        }

        //        return errorList;
        //    }
        //}

        public async Task<List<IdentityError>> CierraPlan(int iid, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var errorList = new List<IdentityError>();
                var user = User.getUserId();

                //string cp = await ctx.Activos_tablas.Where(w => w.id == iid).Select(s => s.id_Reng_pedido).FirstOrDefaultAsync();

                var cp = await (from at in ctx.Activos_tablas
                                join rp in ctx.Reng_pedidos on at.id_Reng_pedido equals rp.id
                                where at.id == iid
                                select new
                                {
                                    rp.Cod_plan
                                }).FirstOrDefaultAsync();

                try
                {
                    //Actuvlizo los pedidos para poner el estado en 1 (Terminado)

                    Pedidos ped = await ctx.Pedidos.Where(w => w.Cod_plan == cp.Cod_plan).FirstOrDefaultAsync();
                    ped.Estado = 1;
                    ped.Fecha_fin = DateTime.Now;

                    ctx.Update(ped);
                    await ctx.SaveChangesAsync();

                    //Actualizo ahora Activos_tablas para poner en Null el codigo del plan y el sku

                    Activos_tablas at = await ctx.Activos_tablas.Where(w => w.id == iid).FirstOrDefaultAsync();
                    var registros = await ctx.Activos_tablas.Where(w => w.id_Reng_pedido == (int)at.id_Reng_pedido).ToListAsync();

                    foreach (var r in registros)
                    {
                        Activos_tablas at2 = await ctx.Activos_tablas.Where(w => w.id == r.id).FirstOrDefaultAsync();

                        at2.Sku_activo = null;
                        at2.id_Reng_pedido = null;

                        ctx.Update(at2);
                        await ctx.SaveChangesAsync();
                    }

                    errorList.Add(new IdentityError
                    {
                        Code = "Update",
                        Description = "Registro modificado correctamente."
                    });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    errorList.Add(new IdentityError
                    {
                        Code = "Error al modificar",
                        Description = ex.ToString()
                    });
                }

                return errorList;
            }
        }

        #endregion Orden de producción
    }

    public class ot {
        public string Codigo_plan { get; set; }
        public string Codigo_planta { get; set; }
        public string Estado { get; set; }
        public string Descripcion_planta { get; set; }
        public string Codigo_producto { get; set; }
        public string Descripcion_producto { get; set; }
        public string Cantidad { get; set; }
        public string Fecha_despacho { get; set; }
        public string Unidad { get; set; }

    }

    public class Tings
    { 
        public int id { get; set; }
        public string color { get; set; }
    }

    public class cod_planes
    { 
        public string Cod_plan { get; set; }
    }
}

