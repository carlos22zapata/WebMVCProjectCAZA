using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FactoryX.Data;
using FactoryX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FactoryX.Controllers
{    
    public class HistoricoVariablesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EmpresaDbContext _Econtext;
        public static int idEmpresaX;
        public static string cod_planX;

        public HistoricoVariablesController(ApplicationDbContext context)
        {
            _context = context;
            //CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = new CultureInfo("es-MX");
                        
        }

        public async Task<IActionResult> HistoricoVariables(int idEmpresa)
        {
            @ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            ViewBag.idEmpresa = idEmpresa;
            return View();
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


        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> GetDatosTabla1(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var inc = ctx.Incidencias.Where(w => w.Hasta == null);

                List<Activos_Vista> av = await (from ac in ctx.Activos
                                                join at in ctx.Activos_tablas on ac.Cod_activo equals at.Cod_activo
                                                join rp in ctx.Reng_pedidos on at.id_Reng_pedido equals rp.id into r1
                                                from rp in r1.DefaultIfEmpty()
                                                join ii in inc on ac.Cod_activo equals ii.Cod_activo into i2
                                                from ii in i2.DefaultIfEmpty()
                                                select new Activos_Vista
                                                {
                                                    Id = at.id,
                                                    Cod_grupo = ac.Cod_grupo,
                                                    Cod_activo = ac.Cod_activo, //+ at.Variable,
                                                    Des_activo = (ii.Cod_incidencia == null ? "" : "(En paro) ") + ac.Des_activo,
                                                    Variable = at.Variable,
                                                    Unidad = at.Unidad,
                                                    Valor_variable = 0,
                                                    Estado_activo = at.id_Reng_pedido == null ? "No hay SKU seleccionado" : at.id_Reng_pedido.ToString(), //at.Sku_activo == null ? "No hay SKU seleccionado" : at.Sku_activo,
                                                    Tabla = at.Nombre_tabla,
                                                    Cod_incidencia = (ii.Cod_incidencia == null ? 0 : ii.Cod_incidencia)
                                                }).ToListAsync();

                return DataSourceLoader.Load(av, loadOptions);
            }
        }

        public async Task<List<SeriesItem>> ValoresGrafico(int idEmpresa, string cod_activo, string tabla, string filtro, DateTime desde, DateTime hasta)
        {
            List<IOT_TR> gra;
            List<SeriesItem> series = new List<SeriesItem>();
            string consulta0 = "";

            var variableTabla = tabla.Substring(tabla.Length - 2, 2); //obtengo los dos últimos del nombre de la tabla que es la variable

            string fd = desde.ToString().Substring(6, 4) + desde.ToString().Substring(3, 2) + desde.ToString().Substring(0, 2) + " " +
                        (filtro == "th" ? desde.ToString().Substring(11, 5) : "");

            string fh = hasta.ToString().Substring(6, 4) + hasta.ToString().Substring(3, 2) + hasta.ToString().Substring(0, 2) + " " +
                        (filtro == "th" ? hasta.ToString().Substring(11, 5) : "");

            if (tabla.Substring(tabla.Length - 2, 2) == "CI" || tabla.Substring(tabla.Length - 2, 2) == "CA")
            {
                //consulta0 = "select id, timestamp, Sku_activo, Cod_plan, case when value2< 0 then value else value2 end value " +
                //            "from ( select id, value, timestamp, Sku_activo, Cod_plan, " +
                //            "value - case when(LAG(value) over(order by id)) is null then (select top 1 value from " + tabla + " where id = (select max(id) from " + tabla + " where cast(timestamp as date) = DATEADD(day, -1, cast('" + fd + "' as date)))) " +
                //            "else LAG(cast(value as decimal(18, 4))) over(order by id) end value2 from " + tabla + " where cast(timestamp as date) between '" + fd + "' and '" + fh + "') x ";

                consulta0 = "select IOT_id id," +
                            " cast(valor / " +
                            "case when (select top 1 UnidadesXCiclo from Capacidades_activos where Cod_activo = '" + cod_activo + "' and Cod_producto = Sku_activo) is null or " +
                                    "(select top 1 UnidadesXCiclo from Capacidades_activos where Cod_activo = '" + cod_activo + "' and Cod_producto = Sku_activo) = 0 then 1 else " +
                                    "(select top 1 UnidadesXCiclo from Capacidades_activos where Cod_activo = '" + cod_activo + "' and Cod_producto = Sku_activo)end as decimal(18,2)) value, " +
                            "tiempo timestamp, Sku_activo, Cod_plan from IOT_Conciliado where cod_activo = '" + cod_activo +
                            (filtro == "th" ? "' and tiempo between '" : "' and cast(tiempo as date) between '")
                            + fd + "' and '" + fh + "'";
            }
            else
            {
                consulta0 = "select id, value, timestamp, Sku_activo, Cod_plan from " + tabla +
                            (filtro == "th" ? " where timestamp between '" : " where cast(timestamp as date) between '")
                            + fd + "' and '" + fh + "'";
            }

            if (desde.Hour == 0 && desde.Minute == 0 && hasta.Hour == 0 && hasta.Minute == 0)
            {
                hasta = hasta.AddDays(1).AddMinutes(-1);
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    string Des_activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).First();
                    int UmbralMinimo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMin).FirstOrDefault();
                    int UmbralMaximo = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.UmbralMax).FirstOrDefault();
                    string Variable = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Variable).FirstOrDefault();
                    string Unidad = ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Unidad).FirstOrDefault();

                    gra = await (from iot in ctx.IOT.FromSql(consulta0)
                                 //where iot.timestamp >= desde && iot.timestamp <= hasta
                                 select new IOT_TR
                                 {
                                     id = iot.id,
                                     value = iot.value.ToString(),
                                     timestamp = (iot.timestamp.ToString("MM/dd/yyyy HH:mm:ss")),
                                     nombre_maquina = Des_activo, //tabla.Substring(4, tamañoTabla) + "-" + (ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault()),
                                                                                                                                            //nombre_maquina1 = ctx.Activos.Where(w => w.Cod_activo == tabla.Substring(4, tamañoTabla)).Select(s => s.Des_activo).FirstOrDefault(),
                                     umbralMinimo = UmbralMinimo,
                                     umbralMaximo = UmbralMaximo,
                                     variable = Variable,
                                     unidad = Unidad,
                                     sku = iot.Sku_activo, //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Sku_activo).FirstOrDefault(),
                                     cod_plan = iot.Cod_plan //ctx.Activos_tablas.Where(w => w.Nombre_tabla == tabla).Select(s => s.Cod_plan).FirstOrDefault()
                                 }).OrderBy(o => o.timestamp).ToListAsync();

                    series.Add(new SeriesItem()
                    {
                        id = gra.Max(s => s.id),
                        name = gra.Select(s => s.timestamp.ToString()).ToArray(),
                        data = gra.Select(s => Convert.ToDecimal(Convert.ToDecimal(s.value).ToString("N2"))).ToArray(),
                        activo = gra.Max(m => m.nombre_maquina),
                        activo1 = gra.Max(m => m.nombre_maquina), //m.nombre_maquina1),
                        umbralMinimo = gra.Max(m => m.umbralMinimo),
                        umbralMaximo = gra.Max(m => m.umbralMaximo),
                        variable = gra.Max(m => m.variable),
                        unidad = gra.Max(m => m.unidad),
                        sku = gra.Select(m => m.sku).ToArray(),
                        cod_plan = gra.Select(s => s.cod_plan).ToArray(),
                        desde_hasta = desde.ToString("MM/dd/yyyy") + " - " + hasta.ToString("MM/dd/yyyy")
                    });
                }
                catch (Exception ex)
                { }
            }

            return series;
        }

    }
}