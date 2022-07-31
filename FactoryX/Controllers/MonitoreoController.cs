using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FactoryX.Data;
using FactoryX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FactoryX.Controllers
{
    public class MonitoreoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EmpresaDbContext _Econtext;
        public static int idEmpresaX;

        public MonitoreoController(ApplicationDbContext context)
        {
            _context = context;
            CultureInfo.CurrentCulture = new CultureInfo("es-MX");
            //CultureInfo.CurrentCulture = new CultureInfo("en-US");
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
                //Response.Redirect(Url.Content("~/Identity/Account/Login"));
            }
        }



        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Monitoreo(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            var user = User.getUserId();

            var verificar = _context.UsuariosEmpresas.Where(u => u.IdUser == user && u.IdEmpresa == idEmpresa).FirstOrDefault();
            if (verificar != null)
            {
                using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
                {
                    //Si estoy en la empresa demo se ejecuta el procedimiento almacenado que lo que hace es insertar datos si no hay nada en las tablas de IOT
                    if (idEmpresa == 1)
                    {
                        var xx = await ctx.Activos.FromSql("exec sp_llena_iot_dia_actual").ToListAsync();
                    }

                    try
                    {
                        ViewBag.idEmpresa = idEmpresa;

                        //var check = await ctx.Monitor.FromSql("exec sp_unidades_producidas").ToListAsync();
                        //var checkSum = await ctx.Monitor.FromSql("select *from dbo.fun_unidades_producidas()").ToListAsync();

                        string fini = DateTime.Now.ToString("yyyyMMdd");
                        string ffin = DateTime.Now.ToString("yyyyMMdd");
                        //var cantidades = decimal.Parse(ctx.IOT.FromSql("select sum(value) from " + at.Nombre_tabla).ToString());
                        List<Lista_tablas> acumulador = new List<Lista_tablas>();
                        string consultaUnida = "";

                        var activ0 = await (from a in ctx.Activos_tablas
                                            select new
                                            {
                                                Cod_activo = a.Cod_activo,
                                                Nombre_tabla = a.Nombre_tabla,
                                                Des_activo = a.Nombre_tabla.Substring(4, a.Nombre_tabla.Length - 7) + "-" + (ctx.Activos.Where(w => w.Cod_activo == a.Nombre_tabla.Substring(4, a.Nombre_tabla.Length - 7)).Select(s => s.Des_activo).FirstOrDefault())
                                            }).ToListAsync();


                        //for (int i = 0; i < activ0.Count(); i++)
                        //{
                        //    consultaUnida = consultaUnida +

                        //                    "select x.id, " +
                        //                    "case when (select count(*) from " + activ0[i].Nombre_tabla + " where cast(timestamp as date) = cast(GETDATE() as date)) = 0 then 0 else (select top 1 cast(value as decimal) from " + activ0[i].Nombre_tabla + " where id = x.id) end value, " +
                        //                    "x.timestamp, x.Sku_activo, x.Cod_plan, x.Minutos, x.dia, x.turno, x.marca, x.planificado, x.ndia, x.semana, x.nhora " +
                        //                    " from ( select case when min(id) is null then 0 else min(id) end id, 0.0 value, getdate() timestamp, '" + activ0[i].Nombre_tabla + "' Sku_activo, '" + activ0[i].Cod_activo + "' Cod_plan, 0 Minutos, GETDATE() dia, '' turno, '' marca, null planificado, 0 ndia, 0 semana, 0 nhora from " + activ0[i].Nombre_tabla +
                        //                    " where cast(timestamp as date) = " + "cast(GETDATE() as date) ) x "  //+ "'20210705') x "

                        //                    + ((i + 1) < activ0.Count() ? " union " : "");
                        //}

                        //List<IOT> cont000 = new List<IOT>();

                        //try
                        //{
                        //    cont000 = await ctx.IOT.FromSql(consultaUnida).ToListAsync();
                        //}
                        //catch (Exception ex)
                        //{ 

                        //}


                        //foreach (var c in cont000)
                        //{
                        //    acumulador.Add(new Lista_tablas { Cod_activo = c.Cod_plan, Nombre_tabla = c.Sku_activo, Cantidad = c.value });
                        //}

                        int contador01 = 0;

                        foreach (var x in activ0)
                        {
                            //string sel = "select id, value, timestamp from " + x.Nombre_tabla + " where cast(timestamp as date) = cast(GETDATE() as date)";
                            string sel = "select top 1 id, cast(value as decimal(18,4)) value, timestamp, Sku_activo, Cod_plan, 0 Minutos, GETDATE() dia, '' turno, '' marca, null planificado, 0 ndia, 0 semana, 0 nhora from " + x.Nombre_tabla + " where cast(timestamp as date) = " +
                                         "cast(GETDATE() as date)"; //" '20210705' "; //"cast(GETDATE() as date")";

                            var cont = new List<IOT>();

                            try
                            {
                                cont = await ctx.IOT.FromSql(sel).ToListAsync();

                                decimal ultimo = 0;
                                var mx = 0;

                                try
                                {
                                    mx = cont.Max(m => m.id);
                                }
                                catch (Exception ex)
                                {
                                    mx = contador01;
                                }
                                contador01++;

                                try
                                {
                                    var maximo = cont.Where(w => w.id == mx).Select(s => (decimal)s.value).FirstOrDefault();
                                    ultimo = Convert.ToDecimal(maximo); //cont.Where(w => w.id == cont.Max(m => m.id)).Select(s => s.value);
                                                                        //ultimo = cont.Where(w => w.id == cont.Max(m => m.id)).Select(s => Convert.ToDecimal(s.value));
                                }
                                catch (Exception ex)
                                {

                                }

                                acumulador.Add(new Lista_tablas { Cod_activo = x.Cod_activo, Nombre_tabla = x.Nombre_tabla, Cantidad = ultimo });
                            }
                            catch (Exception ex)
                            { 
                            
                            }
                            
                        }

                        var viewModel = new MonitoreoModel
                        {

                            //Monitor_TR = await (from a in ctx.IOT
                            //                    orderby a.timestamp ascending
                            //                    select new Monitor_TR
                            //                    {
                            //                        id = a.id,
                            //                        value = Convert.ToDecimal(a.value),
                            //                        timestamp = (a.timestamp) //.TimeOfDay
                            //                    }).ToListAsync(),

                            ActivosV = await (from ac in ctx.Activos
                                              join at in ctx.Activos_tablas on ac.Cod_activo equals at.Cod_activo
                                              select new Activos_Vista
                                              {
                                                  Id = at.id,
                                                  Cod_grupo = ac.Cod_grupo,
                                                  Cod_activo = ac.Cod_activo,
                                                  Des_activo = ac.Des_activo,
                                                  Variable = at.Variable,
                                                  Unidad = at.Unidad,
                                                  Valor_variable = acumulador.Where(w => w.Nombre_tabla == at.Nombre_tabla).Select(s => s.Cantidad).FirstOrDefault(),
                                                  Estado_activo = "Producción",
                                                  Check = false
                                              }).ToListAsync(),

                            Tipos_incidencia = await (from ti in ctx.Tipos_incidencia
                                                      select new Tipos_incidencia
                                                      {
                                                          Cod_tipo = ti.Cod_tipo,
                                                          Des_tipo = ti.Des_tipo
                                                      }).ToListAsync(),

                            ListaSKU_ = await (from p in ctx.Productos
                                                   //join pe in ctx.Pedidos on p.Cod_producto equals pe.Cod_producto
                                                   //where pe.Estado == null
                                               select new Lista_productos_basico
                                               {
                                                   Cod_producto = p.Cod_producto,
                                                   Des_producto = p.Cod_producto + ", " + p.Des_producto
                                                   //Des_producto = p.Cod_producto + ", OP: " + pe.Cod_plan
                                               }).ToListAsync(),

                            ListaPedidos_ = await (from lp in ctx.Pedidos
                                                   select new ListaPedidos
                                                   {
                                                       Cod_plan = lp.Cod_plan
                                                   }).ToListAsync(),

                            total = await ctx.IOT.Where(s => (s.timestamp.Date) <= DateTime.Now.Date).SumAsync(s => Convert.ToDecimal(s.value)),

                        };


                        ViewBag.NivelI = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Nivel).FirstOrDefaultAsync();

                        return View(viewModel);

                    }
                    catch (Exception ex)
                    {
                        //Response.Redirect(Url.Content("~/Monitoreo/Monitoreo?idEmpresa=" + idEmpresa.ToString()));
                        //return View();

                        ex.ToString();
                        return NotFound();
                    }
                }
            }

            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]

        public async Task<object> GraficoTR(int idEmpresa, string cod_activo, string variable)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var tabla = await (from t in ctx.Activos_tablas
                                   where t.Cod_activo == cod_activo && t.Variable == variable
                                   select new Activos_tablas
                                   {
                                       Nombre_tabla = t.Nombre_tabla,
                                       Cod_activo = t.Cod_activo,
                                       Variable = t.Variable
                                   }).ToListAsync();

                var j = JsonConvert.SerializeObject(tabla);

                return (j);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<List<SeriesItem>> GraficoTR2(int idEmpresa, string tabla, int? actualizado)
        {
            List<IOT_TR> gra;
            List<SeriesItem> series = new List<SeriesItem>();
            string consulta0 = "";

            var variableTabla = tabla.Substring(tabla.Length - 2, 2); //obtengo los dos últimos del nombre de la tabla que es la variable

            if (tabla.Substring(tabla.Length - 2, 2) == "CI" || tabla.Substring(tabla.Length - 2, 2) == "CA")
            {
                //consulta0 = "select id, timestamp, Sku_activo, Cod_plan, case when value2< 0 then value else value2 end value " +
                //            "from ( select id, value, timestamp, Sku_activo, Cod_plan, " +
                //            "value - case when(LAG(value) over(order by id)) is null then (select top 1 value from " + tabla +
                //            " where id = (select max(id) from " + tabla + " where cast(timestamp as date) = DATEADD(day, -1, cast(getdate() as date)))) " +
                //            "else LAG(cast(value as decimal(18, 4))) over(order by id) end value2 from " + tabla +
                //            " where cast(timestamp as date) = cast(getdate() as date)) x";

                consulta0 = "select id, timestamp, sku_activo, Cod_plan, value from  " + tabla +
                            " where cast(timestamp as date) = cast(getdate() as date)";
            }
            else
            {
                consulta0 = "select id, value, timestamp, Sku_activo, Cod_plan from " + tabla +
                            " where cast(timestamp as date) = cast(getdate() as date)";
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var tamañoTabla = tabla.Length - 7;

                if (actualizado == null) //cuando actualiza en dato despues de un minuto
                {
                    try
                    {
                        string Des_activo = tabla.Substring(4, tamañoTabla) + "-" + (ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault());
                        int UmbralMinimo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMin).FirstOrDefault();
                        int UmbralMaximo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMax).FirstOrDefault();
                        string Variable = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Variable).FirstOrDefault();
                        string Unidad = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Unidad).FirstOrDefault();

                        gra = await (from iot in ctx.IOT.FromSql(consulta0)
                                     where (iot.timestamp).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")
                                     select new IOT_TR
                                     {
                                         id = iot.id,
                                         value = iot.value.ToString(),
                                         timestamp = (iot.timestamp.ToString("HH:mm:ss")),
                                         nombre_maquina = Des_activo, //tabla.Substring(4, tamañoTabla) + "-" + (ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault()),
                                         nombre_maquina1 = Des_activo, //ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault(),
                                         umbralMinimo = UmbralMinimo, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMin).FirstOrDefault(),
                                         umbralMaximo = UmbralMaximo, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMax).FirstOrDefault(),
                                         variable = Variable, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Variable).FirstOrDefault(),
                                         unidad = Unidad, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Unidad).FirstOrDefault(),
                                         sku = iot.Sku_activo, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Sku_activo).FirstOrDefault(),
                                         cod_plan = iot.Cod_plan //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Cod_plan).FirstOrDefault()
                                     }).OrderBy(o => o.timestamp).ToListAsync();


                        series.Add(new SeriesItem()
                        {
                            id = gra.Max(s => s.id),
                            name = gra.Select(s => s.timestamp.ToString()).ToArray(),
                            data = gra.Select(s => Convert.ToDecimal(Convert.ToDecimal(s.value).ToString("N2"))).ToArray(),
                            activo = gra.Max(m => m.nombre_maquina),
                            activo1 = gra.Max(m => m.nombre_maquina1),
                            umbralMinimo = gra.Max(m => m.umbralMinimo),
                            umbralMaximo = gra.Max(m => m.umbralMaximo),
                            variable = gra.Max(m => m.variable),
                            unidad = gra.Max(m => m.unidad),
                            sku = gra.Select(m => m.sku).ToArray(),
                            cod_plan = gra.Select(s => s.cod_plan).ToArray()
                        });
                    }
                    catch (Exception ex)
                    {
                        return series;
                    }


                }
                else
                {
                    gra = await (from iot in ctx.IOT.FromSql(consulta0)
                                 where (iot.timestamp).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") && iot.id > actualizado
                                 select new IOT_TR
                                 {
                                     id = iot.id,
                                     value = iot.value.ToString(),
                                     timestamp = (iot.timestamp.ToString("HH:mm:ss"))
                                 }).OrderBy(o => o.timestamp).ToListAsync();

                    try
                    {
                        series.Add(new SeriesItem()
                        {
                            id = gra.Max(s => s.id),
                            name = gra.Select(s => s.timestamp.ToString()).ToArray(),
                            data = gra.Select(s => Convert.ToDecimal(s.value)).ToArray(),
                            sku = gra.Select(s => s.sku).ToArray(),
                            cod_plan = gra.Select(s => s.cod_plan).ToArray()
                        });
                    }
                    catch (Exception ex)
                    { }
                }

                //var g1 = JsonConvert.SerializeObject(g);                                               
            }

            return series;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public void AsignaEmpresa(int idEmpresa)
        {
            idEmpresaX = idEmpresa;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> CargaPlan(DataSourceLoadOptions loadOptions, string cod_activo, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Pedidos> plan = (from p in ctx.Pedidos
                                      join r in ctx.Reng_pedidos on p.Cod_plan equals r.Cod_plan
                                      join c in ctx.Capacidades_activos on r.Cod_producto equals c.Cod_producto
                                      where p.Fecha_fin == null && p.Estado == null &&
                                            c.Cod_activo == cod_activo
                                      select new Pedidos
                                      {
                                          Cod_plan = p.Cod_plan,
                                          Cod_centro = "OP: " + p.Cod_plan + ", SKU: " + r.Cod_producto
                                      }).OrderByDescending(d => d.Cod_plan).ToList();

                return plan.ToList();
            }


            return null;
        }


        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> ListaSKU(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Lista_productos_basico> sku = await (from pe in ctx.Pedidos
                                                          join rp in ctx.Reng_pedidos on pe.Cod_plan equals rp.Cod_plan
                                                          orderby rp.Cod_plan, rp.Cod_producto
                                                          where pe.Fecha_fin == null
                                                          select new Lista_productos_basico
                                                          {
                                                              Cod_producto = rp.id.ToString(), //p.Cod_producto,
                                                              Des_producto = "OP: " + pe.Cod_plan + ", SKU: " + rp.Cod_producto
                                                          }).ToListAsync();

                //List<Lista_productos_basico> sku = await (from p in ctx.Productos
                //                                          join r in ctx.Reng_pedidos on p.Cod_producto equals r.Cod_producto
                //                                          join pe in ctx.Pedidos on r.Cod_plan equals pe.Cod_plan
                //                                          where pe.Estado == null
                //                                          select new Lista_productos_basico
                //                                          {
                //                                              Cod_producto = r.id.ToString(), //p.Cod_producto,
                //                                              Des_producto = "OP: " + pe.Cod_plan + ", SKU: " + p.Cod_producto
                //                                          }).ToListAsync();                

                return DataSourceLoader.Load(sku, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> GetDatosTabla1(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var inc = (from i in ctx.Incidencias
                           where i.Hasta == null
                           group i by new { i.Cod_activo } into ii
                           select new
                           {
                               Cod_activo = ii.Key.Cod_activo,
                               Cod_incidencia = ii.Max(m => m.Cod_incidencia)
                           }).ToList();  //ctx.Incidencias.Where(w => w.Hasta == null).GroupBy(g => g.Cod_activo);

                List<Activos_Vista> av;

                try
                {
                    av = await (from ac in ctx.Activos
                                join at in ctx.Activos_tablas on ac.Cod_activo equals at.Cod_activo
                                join rp in ctx.Reng_pedidos on at.id_Reng_pedido equals rp.id into r1
                                from rp in r1.DefaultIfEmpty()
                                join ii in inc on ac.Cod_activo equals ii.Cod_activo into i2
                                from ii in i2.DefaultIfEmpty()
                                select new Activos_Vista
                                {
                                    Id = rp.id == null ? 0 : rp.id,
                                    Iid = at.id,
                                    Cod_grupo = ac.Cod_grupo,
                                    Cod_activo = ac.Cod_activo,
                                    Des_activo = (ii.Cod_incidencia == null ? "" : "(En paro) ") + ac.Des_activo,
                                    Variable = at.Variable,
                                    Unidad = at.Unidad,
                                    Valor_variable = 0,
                                    Estado_activo = (at.id_Reng_pedido == null ? "No hay SKU seleccionado" : "OP: " + rp.Cod_plan + ", SKU: " + rp.Cod_producto),
                                    //at.id_Reng_pedido.ToString(), //at.Sku_activo == null ? "No hay SKU seleccionado" : at.Sku_activo,
                                    Tabla = at.Nombre_tabla,
                                    Cod_incidencia = (ii.Cod_incidencia == null ? 0 : ii.Cod_incidencia),
                                    Cod_plan = rp.Cod_plan,
                                    SKU = rp.Cod_producto
                                }).ToListAsync();

                    return DataSourceLoader.Load(av, loadOptions);
                }
                catch (Exception ex)
                {

                }

                return null;

            }
        }

        //InformacionOP
        public async Task<object> InformacionOP(int nreng, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var ord = await (from ro in ctx.Reng_pedidos
                                 where ro.id == nreng
                                 select new Reng_pedidos
                                 {
                                     Cod_plan = ro.Cod_plan,
                                     Cod_producto = ro.Cod_producto
                                 }).FirstOrDefaultAsync();

                return ord;
            }


            return View();
        }

        public async Task DesacoplaOrden(string cod_activo, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Activos_tablas> at = await ctx.Activos_tablas.Where(w => w.Cod_activo == cod_activo).ToListAsync();

                foreach (var att in at)
                {
                    Activos_tablas actt = await ctx.Activos_tablas.Where(w => w.id == att.id).FirstOrDefaultAsync();

                    actt.Sku_activo = null;
                    actt.id_Reng_pedido = null;

                    ctx.Update(actt);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public async Task ActualizaSKU(string cod_activo, string cod_plan, string sku_activo, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Activos_tablas> at = await ctx.Activos_tablas.Where(w => w.Cod_activo == cod_activo).ToListAsync();

                foreach (var att in at)
                {
                    Activos_tablas actt = await ctx.Activos_tablas.Where(w => w.id == att.id).FirstOrDefaultAsync();
                    var reng_pedido = await (from r in ctx.Reng_pedidos
                                             where r.Cod_plan == cod_plan && r.Cod_producto == sku_activo
                                             select new
                                             {
                                                 id = r.id,
                                                 Cod_plan = r.Cod_plan
                                             }).FirstOrDefaultAsync();

                    actt.Sku_activo = sku_activo;
                    actt.id_Reng_pedido = reng_pedido.id;

                    ctx.Update(actt);
                    await ctx.SaveChangesAsync();

                    //Actualizo el plan en su tabla (pedidos) con estado de iniciado
                    Pedidos p = await ctx.Pedidos.Where(w => w.Cod_plan == reng_pedido.Cod_plan).FirstOrDefaultAsync();
                    p.Fecha = DateTime.Now;

                    ctx.Update(p);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public async Task<object> GetDatosTablaHistoricos(DataSourceLoadOptions loadOptions, int idEmpresa, int activo)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //List<IOT> hist = await (from )
                return DataSourceLoader.Load("", loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task UpdateSKU(int idEmpresa)
        {

            var prueba = "";

            //using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            //{
            //    List<Activos_tablas> at = await ctx.Activos_tablas.Where(w => w.Cod_activo == cod_activo).ToListAsync();

            //    foreach (var att in at)
            //    {
            //        Activos_tablas actt = await ctx.Activos_tablas.Where(w => w.id == att.id).FirstOrDefaultAsync();
            //        var reng_pedido = await (from r in ctx.Reng_pedidos
            //                                 where r.Cod_plan == cod_plan && r.Cod_producto == sku_activo
            //                                 select new
            //                                 {
            //                                     id = r.id
            //                                 }).FirstOrDefaultAsync();

            //        actt.Sku_activo = sku_activo;
            //        actt.id_Reng_pedido = reng_pedido.id;

            //        ctx.Update(actt);
            //        await ctx.SaveChangesAsync();
            //    }
            //}
        }

        //public async Task<IActionResult> UpdateSKU(int key, string values)
        //{
        //    sku Nsku = JsonConvert.DeserializeObject<sku>(values);

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        var errorList = new List<IdentityError>();


        //        string cp = Nsku.Estado_activo; //await ctx.Reng_pedidos.Where(w => w.id == int.Parse(Nsku.Estado_activo)).Select(s => s.Cod_plan).FirstOrDefaultAsync();
        //        string cs = await ctx.Reng_pedidos.Where(w => w.id == int.Parse(Nsku.Estado_activo)).Select(s => s.Cod_producto).FirstOrDefaultAsync();
        //        string cod_plan = await ctx.Reng_pedidos.Where(w => w.id == int.Parse(Nsku.Estado_activo)).Select(s => s.Cod_plan).FirstOrDefaultAsync();

        //        Activos_tablas at = await ctx.Activos_tablas.Where(w => w.id == key).FirstOrDefaultAsync();
        //        string cod_activo = at.Cod_activo;

        //        var renglones = ctx.Activos_tablas.Where(w => w.Cod_activo == cod_activo);

        //        foreach (var r in renglones)
        //        {
        //            Activos_tablas at2 = await ctx.Activos_tablas.Where(w => w.id == r.id).FirstOrDefaultAsync();
        //            at2.Sku_activo = cs;
        //            at2.id_Reng_pedido = int.Parse(cp);

        //            try
        //            {
        //                ctx.Update(at2);
        //                await ctx.SaveChangesAsync();
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }

        //        //Actualizo la tabla de Pedidos fijando la fecha como Datetime.Now
        //        try
        //        {
        //            Pedidos ped = await ctx.Pedidos.Where(w => w.Cod_plan == cod_plan).FirstOrDefaultAsync();
        //            ped.Fecha = DateTime.Now;
        //            ctx.Update(ped);
        //        }
        //        catch (Exception ex)
        //        { 

        //        }

        //        await ctx.SaveChangesAsync();

        //        return Ok(at);
        //    }
        //}

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<List<IdentityError>> enviaCorreo(string asunto, string mensaje, int codigo, string destinatario)
        {
            string origen = "fpb_soporte@cogcs.com";
            string destino = destinatario;
            string cuerpo = mensaje + codigo.ToString();

            try
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress(origen, "Centro de soporte FPB");
                var to = new EmailAddress(destinatario, "");

                var msg = MailHelper.CreateSingleEmail(from, to, asunto, "", cuerpo);

                var response = await client.SendEmailAsync(msg);

            }
            catch (Exception ex)
            {

            }



            var errorList = new List<IdentityError>();
            return errorList;
        }


        //[Authorize(Roles = "Administrador")]

        public async Task<List<IdentityError>> enviaTelegram()
        {
            string token = "718436457:AAFfzBwEdcbO0Qj689mEb_MvtebTZ2y6LGo";
            string id_grupo = "-482792328";
            string mensaje = "Prueba desde la aplicación web";

            var request = WebRequest.Create("https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + id_grupo + "&text=" + mensaje);
            var resp = request.GetResponse();
            var reader = new StreamReader(resp.GetResponseStream());

            reader.Close();
            resp.Dispose();

            var errorList = new List<IdentityError>();
            return errorList;
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public async Task<string> ValidaCorreo(string tabla, int idEmpresa, string correo)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string c1 = await ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Correo_supervisor).FirstOrDefaultAsync();
                string c2 = await ctx.Configuracion.Select(s => s.Correo_supervisor).FirstOrDefaultAsync();

                string correoX = (c1 == null || c1 == "") ? c2 : c1;

                //

                //Activos_tablas at = await ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).FirstOrDefaultAsync();
                //at.Nro_autorizacion = codigoX;
                //ctx.SaveChanges();

                //enviaCorreo(
                //    "Código de verificación Factory Performance Booster",
                //    "El código solicitado por el operador para poder hacer efectivos los cambios es el: ",
                //    codigoX,
                //    correoX
                //);

                return correoX;
            }

        }

        public async Task<string> CreaCodigo(string correo, string fini, string ffin, string cod_plan, string sku, string tabla, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                int codigoX = RandomNumber(5000, 9999);
                Activos_tablas at = await ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).FirstOrDefaultAsync();
                at.Nro_autorizacion = codigoX;
                ctx.SaveChanges();

                sku = (sku == null ? "no seleccionado" : sku);
                cod_plan = (cod_plan == null ? "no seleccionado" : cod_plan);

                try
                {
                    enviaCorreo(
                        "Código de verificación Factory Performance Booster",
                        "Este correo ha sido enviado por que un operador de Factory Performance Booster ha solicitado cambiar los registros entre la fecha: " + fini + " y la fecha: " + ffin +
                        ", asignando el SKU: " + sku + ", y el código de orden de producción: " + cod_plan + ", para el rango seleccionado" +
                        ". Si usted aprueba los cambios envie este código al operador que lo solicita: ",
                        codigoX,
                        correo
                    );
                }
                catch (Exception ex)
                {

                }

            }

            return "";
        }

        public async Task<bool> ValidaCodigo(int idEmpresa, int idActivos_tablas, int codigo)
        {
            bool retorno;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var valida = await ctx.Activos_tablas.Where(w => w.id == idActivos_tablas).MaxAsync(s => s.Nro_autorizacion);

                if ((int)valida == codigo)
                {
                    retorno = true;
                }
                else
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        public async Task<IActionResult> CargaHoras(string tabla, string cod_activo, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                int act = await (ctx.Incidencias.Where(w => w.Hasta == null && w.Cod_activo == cod_activo).CountAsync());
                List<IOT_TR> hora;


                if (act > 0)
                {
                    hora = await (from iot in ctx.Incidencias
                                  where iot.Hasta == null && iot.Cod_activo == cod_activo
                                  select new IOT_TR
                                  {
                                      timestamp = iot.Desde.ToString("dd/MM/yyyy HH:mm")
                                  }).ToListAsync();



                }
                else
                {
                    hora = await (from iot in ctx.IOT.FromSql("select max(id) id, max(value) value, cast(cast(timestamp as varchar(20)) as datetime) timestamp, max(sku_activo) sku_activo from " + tabla + " where cast(timestamp as date) >= cast(DATEADD(DAY,-2,GETDATE()) as date) group by cast(cast(timestamp as varchar(20)) as datetime)")
                                  orderby iot.timestamp descending
                                  select new IOT_TR
                                  {
                                      timestamp = iot.timestamp.ToString("dd/MM/yyyy HH:mm")
                                  }).ToListAsync();
                }

                return new JsonResult(hora);
            }
        }

        public async Task<List<IdentityError>> GuardaParo(string cod_activo, int cod_tipo, string observacion, string fecha, int idEmpresa)
        {
            DateTime fec = DateTime.Parse(fecha);

            var errorList = new List<IdentityError>();

            var user = User.getUserId();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    var Incidencia = new Incidencias
                    {
                        Des_incidencia = "",
                        Cod_activo = cod_activo,
                        Cod_tipo = cod_tipo,
                        Cod_producto = null,
                        Observacion = observacion,
                        Desde = fec,
                        Hasta = null,
                        Usuario_incluye = user,
                        Fecha_usuario_incluye = DateTime.Now
                    };

                    ctx.Add(Incidencia);

                    try
                    {
                        await ctx.SaveChangesAsync();
                    }

                    catch (Exception ex)
                    { }

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

        public async Task<List<IdentityError>> ActualizaHistoricos(string cod_producto, string cod_plan, string fini, string ffin, string tabla, int idEmpresa)
        {
            var errorList = new List<IdentityError>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //string finiT = fini.ToString("yyyy/dd/MM hh:mm");
                //string ffinT = ffin.ToString("yyyy/dd/MM hh:mm");
                List<IOT> seleccion = new List<IOT>();

                try
                {

                    seleccion = await (ctx.IOT.FromSql("select id, value, timestamp, sku_activo, Cod_plan from " + tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'")).ToListAsync();

                    foreach (var s in seleccion)
                    {
                        try
                        {
                            string upd = "update " + tabla + " set sku_activo = '" + cod_producto + "' , Cod_plan = '" + cod_plan + "' where id = " + s.id;
                            ctx.Database.ExecuteSqlCommand(upd);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                foreach (var s in seleccion)
                {

                }
            }



            return errorList;
        }

        public async Task<List<IdentityError>> CierreParo(int idIncidencia, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var errorList = new List<IdentityError>();
                var user = User.getUserId();

                try
                {
                    Incidencias inc = await ctx.Incidencias.Where(w => w.Cod_incidencia == idIncidencia).FirstOrDefaultAsync();
                    inc.Hasta = DateTime.Now;
                    inc.Usuario_modifica = user;
                    inc.Fecha_usuario_modifica = DateTime.Now;


                    ctx.Update(inc);
                    await ctx.SaveChangesAsync();

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

        public async Task<List<IdentityError>> CerrarOP(int iid, int idEmpresa)
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

        public async Task<IActionResult> IncidenciasActivas(string cod_activo, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var act = await (from i in ctx.Incidencias
                                 where i.Hasta == null && i.Cod_activo == cod_activo
                                 select new
                                 {
                                     Cod_incidencia = i.Cod_incidencia,
                                     Cod_activo = i.Cod_activo,
                                     Cod_tipo = i.Cod_tipo,
                                     Observacion = i.Observacion,
                                     Desde = i.Desde.ToString("dd/MM/yyyy HH:mm")
                                 }).ToListAsync();

                return new JsonResult(act);
            }



        }
    }

    public class sku
    {
        public string Estado_activo { get; set; }
    }
}