using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using FactoryX.Data;
using FactoryX.Models;
//using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryX.Controllers
{
    public class AnaliticaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EmpresaDbContext _Econtext;
        public static int idEmpresaX;

        public AnaliticaController(ApplicationDbContext context)
        {
            _context = context;
            //CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = new CultureInfo("es-MX");
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
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

        public string ConvierteFecha(DateTime fecha, string tipo)
        {
            string valor = "";

            if (tipo == "mes")
            {
                if (fecha.Month == 1)
                {
                    valor = "1-Ene-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 2)
                {
                    valor = "2-Feb-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 3)
                {
                    valor = "3-Mar-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 4)
                {
                    valor = "4-Abr-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 5)
                {
                    valor = "5-May-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 6)
                {
                    valor = "6-Jun-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 7)
                {
                    valor = "7-Jul-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 8)
                {
                    valor = "8-Ago-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 9)
                {
                    valor = "9-Sep-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 10)
                {
                    valor = "10-Oct-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 11)
                {
                    valor = "11-Nov-" + fecha.ToString("yyyy");
                }
                else if (fecha.Month == 12)
                {
                    valor = "12-Dic-" + fecha.ToString("yyyy");
                }

            }

            return valor;
        }

        public string FormatoSemana(int semana, int anno)
        {
            string valor = semana.ToString() + " - " + anno.ToString();

            return valor;
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
        public void AsignaEmpresa(int idEmpresa)
        {
            idEmpresaX = idEmpresa;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Calidad(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        //Historicos Inicio
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GestionKPI(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GestionKPI_tiempos(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GestionKPI_indicadores(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> Lista_activos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            //idEmpresa = 1;
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<ActivosVariables> av = await (from a in ctx.Activos
                                                   join t in ctx.Activos_tablas on a.Cod_activo equals t.Cod_activo
                                                   where t.Variable != "PR"
                                                   select new ActivosVariables
                                                   {
                                                       Cod_activo = a.Cod_activo,
                                                       Des_activo = a.Cod_activo + " - " + a.Des_activo,
                                                       Variable = t.Variable,
                                                       Unidad = t.Unidad
                                                   }).ToListAsync();

                return DataSourceLoader.Load(av, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<List<ActivosVariables>> Lista_activos2(int idEmpresa)
        {
            //idEmpresa = 1;
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<ActivosVariables> av = await (from a in ctx.Activos
                                            join t in ctx.Activos_tablas on a.Cod_activo equals t.Cod_activo
                                            where t.Variable != "PR"
                                            select new ActivosVariables
                                            {
                                                Cod_activo = a.Cod_activo,
                                                Des_activo = a.Cod_activo + " -- " + a.Des_activo,
                                                Variable = t.Variable,
                                                Unidad = t.Unidad
                                            }).ToListAsync();

                return av.ToList();
            }
        }

        public async Task<object> Lista_sku(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            //idEmpresa = 1;
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<Productos> sk = await (from a in ctx.Productos
                                          select new Productos
                                          {
                                              Cod_producto = a.Cod_producto,
                                              Des_producto = a.Cod_producto + " - " + a.Des_producto
                                          }).ToListAsync();

                return DataSourceLoader.Load(sk, loadOptions);
            }
        }

        public async Task<object> ConsolidadoKpiTiempos(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string variable, string unidad, string tiempo, string verTiempo, bool sw01)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //List<JsonResult> res = new List<JsonResult>();
                List<ActivosVariables> av = Lista_activos2(idEmpresa).Result;
                List<Consolidado_tiempos> Consol = new List<Consolidado_tiempos>();

                try
                {
                    //List<Consolidado_tiempos> hv = Historicos_variables2_consolidado(idEmpresa, fini_, ffin_, filtro, turno, a.Cod_activo, a.Variable, false).Result;

                    var at = IndicadorAnalisisTiempos(idEmpresa, fini_, ffin_, filtro, turno, null, variable, verTiempo, false, sw01).Result;
                    

                    foreach (var c in at)
                    {
                        Consol.Add(new Consolidado_tiempos()
                        {
                            fecha = c.Leyenda,
                            turno = turno,
                            nombreActivo = c.Activo,
                            to = c.To,
                            tpp = c.Tpp,
                            tpnp = c.Tpnp,
                            noRegistrado = c.NoRegistrados, //- c.To - c.Tpp - c.Tpnp,
                            unidades = c.Filtro,
                            tiempo = tiempo,
                            hora = c.Hora
                        });
                    }
                }
                catch (Exception ex)
                {

                }
               
                return Consol;
            }

        }
        
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<List<Consolidado_tiempos>> Historicos_variables2_consolidado(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string maquina, string variable, bool individual)
        {
            List<SeriesUnidades> series = new List<SeriesUnidades>();
            List<Consolidado_tiempos> totales0 = new List<Consolidado_tiempos>();

            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var at = await ctx.Activos_tablas.ToListAsync();
                //var pro = await ctx.Productos.ToListAsync();
                List<IOT> iot = new List<IOT>();
                List<IOT> seleccion = new List<IOT>();
                
                int vueltas = 0;

                if (filtro == "mes")
                {

                    //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                    fini = fini.Substring(0, 8);
                    ffin = ffin.Substring(0, 8);

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        string activoX = await ctx.Activos.Where(w => w.Cod_activo == maquina).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefaultAsync();

                        var ps = seleccion.Where(w => w.turno != null).ToList();

                        var MinutosNoRegistradosDeCadaTurno = (from s in seleccion
                                                               where s.turno != null
                                                               group s by new { s.turno, s.dia, s.dia.Month } into t
                                                               select new
                                                               {
                                                                   minutos = t.Max(m => m.Minutos),
                                                                   mes = t.Key.Month
                                                               }).ToList();

                        totales0 = (from sel in seleccion
                                    join act in ctx.Activos on maquina equals act.Cod_activo
                                    where sel.Minutos > 0 && sel.turno != null && sel.planificado == null
                                    group sel by new { sel.dia.Year, sel.dia.Month } into ss
                                    select new Consolidado_tiempos
                                    {
                                        nombreActivo = activoX,
                                        to    = seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == ss.Key.Year && w.dia.Month == ss.Key.Month)).Count(),
                                        tpp   = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.dia.Month == ss.Key.Month)).Count(),
                                        tpnp  = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.dia.Month == ss.Key.Month)).Count(),
                                        noRegistrado = MinutosNoRegistradosDeCadaTurno.Where(w => w.mes == ss.Key.Month).Sum(s => s.minutos), //seleccion.Where(w => w.turno != null && w.dia.Year == ss.Key.Year && w.dia.Month == ss.Key.Month).Count(),
                                        fecha = ConvierteFecha(ss.Max(m => m.dia), "mes")
                                    }).ToList();

                    }
                    catch (Exception ex)
                    { }
                }
                else if (filtro == "semana")
                {
                    //Calculo anterior
                    //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
                    //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


                    //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
                    fini = fini.Substring(0, 8);
                    ffin = ffin.Substring(0, 8);

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        string activoX = await ctx.Activos.Where(w => w.Cod_activo == maquina).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefaultAsync();

                        double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        string anno1; 
                        string anno2; 

                        try
                        {
                            anno1 = turno.Substring(0, 4);
                            anno2 = turno.Substring(separador + 1, 4);
                        }
                        catch
                        {
                            anno1 = "";
                            anno2 = "";
                        }

                        int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

                        var MinutosNoRegistradosDeCadaTurno = (from s in seleccion
                                                               where s.turno != null
                                                               group s by new { s.turno, s.dia, s.semana } into t
                                                               select new
                                                               {
                                                                   minutos = t.Max(m => m.Minutos),
                                                                   semana = t.Key.semana
                                                               }).ToList();

                        totales0 = (from sel in seleccion
                                    join act in ctx.Activos on maquina equals act.Cod_activo
                                    where sel.Minutos > 0 && sel.turno != null && sel.planificado == null
                                    group sel by new { sel.dia.Year, sel.semana } into ss
                                    select new Consolidado_tiempos
                                    {
                                        nombreActivo = activoX,
                                        to = seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == ss.Key.Year && w.semana == ss.Key.semana)).Count(),
                                        tpp = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.semana == ss.Key.semana)).Count(),
                                        tpnp = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.semana == ss.Key.semana)).Count(),
                                        noRegistrado = MinutosNoRegistradosDeCadaTurno.Where(w => w.semana == ss.Key.semana).Sum(s => s.minutos), //seleccion.Where(w => w.turno != null && w.dia.Year == ss.Key.Year && w.semana == ss.Key.semana).Count(),
                                        fecha = FormatoSemana(ss.Key.semana, ss.Key.Year) //ss.Key.semana.ToString() + " - " + ss.Key.Year.ToString()
                                    }).ToList();

                        //return totales0;

                    }
                    catch (Exception ex)
                    { }

                }
                else if (filtro == "dia")
                {
                    try
                    {
                        //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
                        DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";
                            string activoX = await ctx.Activos.Where(w => w.Cod_activo == maquina).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefaultAsync();


                            if (turno == "Todos" || turno == null)
                            {
                                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                                turno = null;
                            }
                            else
                            {
                                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                                seleccion = seleccion.Where(w => w.turno == turno).ToList();
                            }

                            var MinutosNoRegistradosDeCadaTurno = (from s in seleccion
                                                                   where s.turno != null
                                                                   group s by new { s.turno, s.dia } into t
                                                                   select new
                                                                   {
                                                                       minutos = t.Max(m => m.Minutos),
                                                                       dia = t.Key.dia
                                                                   }).ToList();                                                                  

                            totales0 = (from sel in seleccion
                                        join act in ctx.Activos on maquina equals act.Cod_activo
                                        where sel.Minutos > 0 && sel.turno != null && sel.planificado == null
                                        group sel by new { sel.dia.Year, sel.dia } into ss
                                        select new Consolidado_tiempos
                                        {
                                            nombreActivo = activoX,
                                            to = seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == ss.Key.Year && w.dia == ss.Key.dia)).Count(),
                                            tpp = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.dia == ss.Key.dia)).Count(),
                                            tpnp = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 && (w.dia.Year == ss.Key.Year && w.dia == ss.Key.dia)).Count(),
                                            noRegistrado = MinutosNoRegistradosDeCadaTurno.Where(w => w.dia == ss.Key.dia).Sum(s => s.minutos), //seleccion.Where(w => w.turno != null && w.dia == ss.Key.dia).Max(m => m.Minutos),
                                            fecha = ss.Key.dia.ToString("dd/MM/yyyy")
                                        }).ToList();

                            //return totales0;


                        }
                        catch (Exception ex)
                        { }
                    }
                    catch (Exception ex)
                    { }
                }
                else if (filtro == "hora")
                {

                    //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                    //fini = fini; //.Substring(0, 8);
                    //ffin = ffin.Substring(0, 8);

                    fini = fini_.ToString("yyyyMMdd HH:mm");
                    ffin = ffin_.ToString("yyyyMMdd HH:mm");

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";


                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        string activoX = await ctx.Activos.Where(w => w.Cod_activo == maquina).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefaultAsync();

                        var MinutosNoRegistradosDeCadaTurno = (from s in seleccion
                                                               where s.turno != null
                                                               group s by new { s.turno, s.timestamp.Hour } into t
                                                               select new
                                                               {
                                                                   minutos = t.Max(m => m.Minutos),
                                                                   hora = t.Key.Hour
                                                               }).ToList();

                        totales0 = (from sel in seleccion
                                    join act in ctx.Activos on maquina equals act.Cod_activo
                                    where sel.Minutos > 0 && sel.turno != null && sel.planificado == null
                                    group sel by new { sel.timestamp.Hour } into ss
                                    select new Consolidado_tiempos
                                    {
                                        nombreActivo = activoX,
                                        to = seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia == ss.Max(m => m.dia) && w.timestamp.Hour == ss.Key.Hour)).Count(),
                                        tpp = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia == ss.Max(m => m.dia) && w.timestamp.Hour == ss.Key.Hour)).Count(),
                                        tpnp = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 && (w.dia == ss.Max(m => m.dia) && w.timestamp.Hour == ss.Key.Hour)).Count(),
                                        noRegistrado = MinutosNoRegistradosDeCadaTurno.Where(w => w.hora == ss.Key.Hour).Sum(s => s.minutos), //seleccion.Where(w => w.turno != null && w.dia == ss.Max(m => m.dia) && w.timestamp.Hour == ss.Key.Hour).Count(),
                                        fecha = ss.Min(m => m.timestamp.ToString("dd/MM/yyyy hh:mm"))
                                    }).ToList();

                        //return totales0;

                    }
                    catch (Exception ex)
                    { }
                }   
            }
            return totales0;
        }
        
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<List<SeriesUnidades>> Historicos_variables3(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string maquina, string variable)
        {
            List<SeriesUnidades> series = new List<SeriesUnidades>();

            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //var prueba = (ctx.IOT.FromSql("Exec sp_prueba")).ToListAsync();
                string Activo_ = await ctx.Activos.Where(w => w.Cod_activo == maquina).Select(f => f.Cod_activo + " - " + f.Des_activo).FirstOrDefaultAsync();
                var at = await ctx.Activos_tablas.ToListAsync();
                //var pro = await ctx.Productos.ToListAsync();
                List<IOT> iot = new List<IOT>();
                List<IOT> seleccion = new List<IOT>();
                List<IOT> seleccion2 = new List<IOT>();
                List<IOT> seleccion3 = new List<IOT>();
                List<IOT> seleccion4 = new List<IOT>();
                List<IOT> seleccion5 = new List<IOT>();

                int vueltas = 0;

                if (filtro == "mes")
                {

                    //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                    fini = fini.Substring(0, 8);
                    ffin = ffin.Substring(0, 8);

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        seleccion2 = seleccion;
                        seleccion3 = seleccion;
                        seleccion4 = seleccion;
                        seleccion5 = seleccion;
                        //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                        //seleccion2 = await ctx.IOT.FromSql(p1).ToListAsync();

                        var diasTrabajados = (from dt in seleccion
                                              join te in ctx.Turnos_activos_extras on maquina equals te.Cod_activo
                                              where dt.dia >= DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4))
                                              && dt.dia <= DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4))
                                              group dt by new { dt.dia, dt.turno, dt.semana } into dtt
                                              select new
                                              {
                                                  dia = dtt.Key.dia,
                                                  valor = dtt.Sum(s => s.value),
                                                  turno = dtt.Max(m => m.turno),
                                                  semana = dtt.Key.semana
                                              }).Where(w => w.turno != null).ToList();

                        #region código comentado viejo

                        //##############################################
                        //Calculo el total de los minutos del mes inicio
                        //##############################################
                        //var dXmes = (from dia in seleccion
                        //             group dia by new { dia.ndia, dia.timestamp.Month } into m
                        //             select new {
                        //                 mes = m.Key
                        //             }).ToList();


                        //lun = lun * ((seleccion.Where(w => w.ndia == 2).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 2).Max(m => m.Minutos)));
                        //mar = mar * ((seleccion.Where(w => w.ndia == 3).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 3).Max(m => m.Minutos)));
                        //mie = mie * ((seleccion.Where(w => w.ndia == 4).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 4).Max(m => m.Minutos)));
                        //jue = jue * ((seleccion.Where(w => w.ndia == 5).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 5).Max(m => m.Minutos)));
                        //vie = vie * ((seleccion.Where(w => w.ndia == 6).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 6).Max(m => m.Minutos)));
                        //sab = sab * ((seleccion.Where(w => w.ndia == 7).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 7).Max(m => m.Minutos)));
                        //dom = dom * ((seleccion.Where(w => w.ndia == 1).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 1).Max(m => m.Minutos)));
                        //decimal totalMes = lun + mar + mie + jue + vie + sab + dom;

                        //##############################################
                        //Calculo el total de los minutos del mes fin
                        //##############################################

                        //##########################################################################
                        //Busco el total de minutos de paradas planificadas y no planificadas inicio
                        //##########################################################################
                        //var paradas = await (from i in ctx.Incidencias
                        //                     join t in ctx.Tipos_incidencia on i.Cod_tipo equals t.Cod_tipo
                        //                     where i.Desde >= fini_ && i.Hasta <= ffin_ && 
                        //                     i.Hasta != null && t.Planificado == true
                        //                     select new
                        //                     {
                        //                         minutos = ((i.Hasta ?? DateTime.Parse("01-01-1900")) - i.Desde).TotalMinutes
                        //                     }).SumAsync(s => s.minutos);                            
                        //##########################################################################
                        //Busco el total de minutos de paradas planificadas y no planificadas fin
                        //##########################################################################



                        //int Planificados = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0).Count();
                        //int NoPlanificados = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0).Count();
                        //int TO = seleccion.Where(w => w.planificado != false && w.turno != null && w.Minutos > 0).Count();

                        #endregion
                        seleccion = (from iotX in seleccion               //unidades
                                     where iotX.value > 0 && iotX.planificado == null && iotX.turno != null
                                     group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                     select new IOT
                                     {
                                         id = mm.Max(m => m.id),
                                         value = mm.Sum(m => m.value),
                                         timestamp = mm.Max(m => m.timestamp),
                                         Sku_activo = mm.Max(m => m.Sku_activo),
                                         Cod_plan = mm.Max(m => m.Cod_plan),
                                         dia = mm.Max(m => m.dia),
                                         marca = maquina,
                                         turno = turno
                                     }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion2 = (from iotX in seleccion2               //tpp
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                      select new IOT
                                      {
                                          value = //Planificado
                                          seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
                                          dia = mm.Max(m => m.dia)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion3 = (from iotX in seleccion3                   //total horario
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                      select new IOT
                                      {
                                          value = mm.Max(m => m.Minutos),
                                          dia = mm.Max(m => m.dia)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion3 = (from iotX in seleccion3                   //total horario
                                      group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                      select new IOT
                                      {
                                          value = mm.Sum(m => m.value),
                                          dia = mm.Max(m => m.dia)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion4 = (from iotX in seleccion4               //to
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                      select new IOT
                                      {
                                          id = mm.Max(m => m.id),
                                          value =
                                          seleccion4.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
                                          timestamp = mm.Max(m => m.timestamp),
                                          Sku_activo = mm.Max(m => m.Sku_activo),
                                          Cod_plan = mm.Max(m => m.Cod_plan),
                                          dia = mm.Max(m => m.dia),
                                          marca = maquina,
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion5 = (from iotX in seleccion5
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                      select new IOT
                                      {
                                          value =
                                          //TiempoXmes(DateTime.Parse("01-" + (mm.Key.Month < 10 ? "0" : "") + mm.Key.Month.ToString() + "-" + mm.Key.Year.ToString()),
                                          //                //Según lo conversado con Dina, si el mes es igual al mes actual solo en este caso se toma para el día en el que estamos pero solo para el mes actual
                                          //                mm.Key.Month == DateTime.Now.Month ? DateTime.Now :
                                          //                DateTime.Parse("01-" + (mm.Key.Month < 10 ? "0" : "") + mm.Key.Month.ToString() + "-" + mm.Key.Year.ToString()).AddMonths(1).AddDays(-1)
                                          //                //seleccion.Where(w => w.dia.Month == mm.Key.Month).Max(m => m.dia)
                                          //                , maquina, null, idEmpresa),

                                          ((seleccion5.Where(w => w.dia.Month == mm.Key.Month).Max(m => m.Minutos) * (diasTrabajados.Where(w => w.dia.Month == mm.Key.Month).Count()))

                                            //Planificados
                                            - (seleccion5.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())),

                                          dia = mm.Max(m => m.dia)
                                      }).OrderBy(o => o.timestamp.Month).ToList();                        

                        try
                        {
                            series.Add(new SeriesUnidades()
                            {
                                id = seleccion.Max(s => s.id),
                                fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                tiempo = seleccion.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                tiempo2 = seleccion2.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                tiempo3 = seleccion3.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                tiempo4 = seleccion4.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                tiempo5 = seleccion.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                data = seleccion.Select(s => s.value).ToArray(),   //unidades
                                data2 = seleccion2.Select(s => s.value).ToArray(),   //tpp
                                data3 = seleccion3.Select(s => s.value).ToArray(),   //horario real
                                data4 = seleccion4.Select(s => s.value).ToArray(),   //to
                                data5 = seleccion5.Select(s => s.value).ToArray(),   //horario teorico
                                activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                filtro = filtro,
                                nombreActivo = Activo_, //maquina,
                                sku = seleccion.Select(s => "Mes" + s.turno).ToArray()
                            });
                        }
                        catch (Exception ex)
                        { }

                    }
                    catch (Exception ex)
                    { }
                }
                else if (filtro == "semana")
                {
                    //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
                    fini = fini.Substring(0, 8);
                    ffin = ffin.Substring(0, 8);

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        seleccion2 = seleccion;
                        seleccion3 = seleccion;
                        seleccion4 = seleccion;
                        seleccion5 = seleccion;

                        double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        string anno1; // = turno.Substring(0, 4);
                        string anno2; // = turno.Substring(separador+1, 4);

                        try
                        {
                            anno1 = turno.Substring(0, 4);
                            anno2 = turno.Substring(separador + 1, 4);
                        }
                        catch
                        {
                            anno1 = "";
                            anno2 = "";
                        }

                        int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2)); 
                                                

                        if (sfin < sini)
                        {
                            var diasTrabajados = (from dt in seleccion
                                                  join te in ctx.Turnos_activos_extras on maquina equals te.Cod_activo
                                                  where dt.semana >= sfin && dt.semana <= sini && turno != null && dt.dia == te.Fecha_ini.Date
                                                  group dt by new { dt.dia, dt.turno, dt.semana } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.dia,
                                                      valor = dtt.Sum(s => s.value),
                                                      turno = dtt.Max(m => m.turno),
                                                      semana = dtt.Key.semana
                                                  }).Where(w => w.turno != null).ToList();

                            seleccion = (from iotX in seleccion            //unidades producidas en turno variables1
                                         where iotX.value > 0 && iotX.planificado == null && iotX.turno != null && (iotX.semana >= sfin && iotX.semana <= sini) //|| (iotX.semana >= sfin)
                                         group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value = mm.Sum(m => m.value),
                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)), //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year),//mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             marca = maquina,
                                             turno = turno
                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion5 = (from iotX in seleccion5
                                          where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sfin && iotX.semana <= sini)
                                          group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                          select new IOT
                                          {
                                              value =
                                               (
                                                     //Total minutos del mes 
                                                     (seleccion5.Where(w => w.semana == mm.Key.semana).Max(m => m.Minutos) *
                                                     (diasTrabajados.Where(w => w.semana == mm.Key.semana).Count()))
                                                     -
                                                     //Planificados
                                                     (seleccion5.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count())
                                                 ),

                                              Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)) //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year)
                                          }).OrderBy(o => o.timestamp.Month).ToList();
                        }
                        else
                        {
                            var diasTrabajados = (from dt in seleccion
                                                  join te in ctx.Turnos_activos_extras on maquina equals te.Cod_activo
                                                  where dt.semana >= sini && dt.semana <= sfin && turno != null && dt.dia == te.Fecha_ini.Date
                                                  group dt by new { dt.dia, dt.turno, dt.semana } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.dia,
                                                      valor = dtt.Sum(s => s.value),
                                                      turno = dtt.Max(m => m.turno),
                                                      semana = dtt.Key.semana
                                                  }).Where(w => w.turno != null).ToList();

                            seleccion = (from iotX in seleccion            //unidades producidas en turno variables1
                                         where iotX.value > 0 && iotX.planificado == null && iotX.turno != null && (iotX.semana >= sini && iotX.semana <= sfin) //|| (iotX.semana >= sfin)
                                         group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value = mm.Sum(m => m.value),
                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)), //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year),//mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             marca = maquina,
                                             turno = turno
                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion5 = (from iotX in seleccion5
                                          where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sini && iotX.semana <= sfin)
                                          group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                          select new IOT
                                          {
                                              value =
                                               (
                                                     //Total minutos del mes    
                                                     (seleccion5.Where(w => w.semana == mm.Key.semana).Max(m => m.Minutos) *
                                                     (diasTrabajados.Where(w => w.semana == mm.Key.semana).Count()))
                                                     -
                                                     //Planificados
                                                     (seleccion5.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count())
                                                 ),

                                              Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)) //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year)
                                          }).OrderBy(o => o.timestamp.Month).ToList();
                        }
                        

                        seleccion2 = (from iotX in seleccion2    //tpp variables 2
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                      select new IOT
                                      {
                                          value = //Planificado
                                          seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
                                          Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)) //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year)
                                      }).OrderBy(o => o.timestamp.Month).ToList();


                        seleccion3 = (from iotX in seleccion3                   //total horario
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                      select new IOT
                                      {
                                          value = mm.Max(m => m.Minutos),
                                          semana = mm.Max(m => m.semana),
                                          dia = mm.Max(m => m.dia)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion3 = (from iotX in seleccion3                   //total horario
                                      group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                      select new IOT
                                      {
                                          value = mm.Sum(m => m.value),
                                          Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)) //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion4 = (from iotX in seleccion4                    //to
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                      select new IOT
                                      {
                                          id = mm.Max(m => m.id),
                                          value =
                                          seleccion4.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
                                          timestamp = mm.Max(m => m.timestamp),
                                          Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)), //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year),//mm.Max(m => m.Sku_activo),
                                          Cod_plan = mm.Max(m => m.Cod_plan),
                                          marca = maquina
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        

                        series.Add(new SeriesUnidades()
                        {
                            id = seleccion.Max(s => s.id),
                            fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                            //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                            tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
                            tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
                            tiempo3 = seleccion3.Select(s => s.Sku_activo).ToArray(),
                            tiempo4 = seleccion4.Select(s => s.Sku_activo).ToArray(),
                            tiempo5 = seleccion5.Select(s => s.Sku_activo).ToArray(),
                            data = seleccion.Select(s => s.value).ToArray(),
                            data2 = seleccion2.Select(s => s.value).ToArray(),
                            data3 = seleccion3.Select(s => s.value).ToArray(),
                            data4 = seleccion4.Select(s => s.value).ToArray(),
                            data5 = seleccion5.Select(s => s.value).ToArray(), //################################## Aqui esta el problema con el tiempo CAZA ##################################
                            activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                            cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                            filtro = filtro,
                            nombreActivo = Activo_,
                            sku = seleccion.Select(s => s.turno).ToArray()
                        });

                    }
                    catch (Exception ex)
                    { }

                }
                else if (filtro == "dia")
                {
                    try
                    {
                        //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
                        DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";



                            if (turno == "Todos" || turno == null)
                            {
                                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                                seleccion2 = seleccion;
                                seleccion3 = seleccion;
                                seleccion4 = seleccion;
                                seleccion5 = seleccion;
                                turno = null;
                            }
                            else
                            {
                                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                                seleccion = seleccion.Where(w => w.turno == turno).ToList();
                                seleccion2 = seleccion;
                                seleccion3 = seleccion;
                                seleccion4 = seleccion;
                                seleccion5 = seleccion;
                            }

                            //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);


                            //vueltas++;

                            seleccion = (from iotX in seleccion
                                         where (iotX.value > 0 && iotX.planificado == null && iotX.turno != null /*&& iotX.Minutos > 0*/) && (iotX.dia >= Dfini && iotX.dia <= Dffin)

                                         group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value = mm.Sum(m => m.value),
                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             marca = maquina,
                                             turno = turno
                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion2 = (from iotX in seleccion2    //tpp
                                          where iotX.turno != null && iotX.Minutos > 0 && (iotX.dia >= Dfini && iotX.dia <= Dffin)
                                          group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                          select new IOT
                                          {
                                              value = //Planificado
                                              seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
                                              Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),                                            
                                          }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion3 = (from iotX in seleccion3                   //total horario
                                          where iotX.turno != null && iotX.Minutos > 0
                                          group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                          select new IOT
                                          {
                                              value = mm.Max(m => m.Minutos),
                                              //semana = mm.Max(m => m.semana)
                                              Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),
                                          }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion4 = (from iotX in seleccion4    //to
                                          where iotX.turno != null && iotX.Minutos > 0 && (iotX.dia >= Dfini && iotX.dia <= Dffin)
                                          group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                          select new IOT
                                          {
                                              id = mm.Max(m => m.id),
                                              value =
                                              seleccion4.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
                                              timestamp = mm.Max(m => m.timestamp),
                                              Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),
                                              Cod_plan = mm.Max(m => m.Cod_plan),
                                              marca = maquina
                                          }).OrderBy(o => o.timestamp.Month).ToList();

                            seleccion5 = (from iotX in seleccion5
                                          where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
                                          group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                          select new IOT
                                          {
                                              value =
                                              TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), maquina, turno, idEmpresa),
                                              Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10)
                                          }).OrderBy(o => o.timestamp.Month).ToList();

                            series.Add(new SeriesUnidades()
                            {
                                id = seleccion.Max(s => s.id),
                                fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
                                tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
                                tiempo3 = seleccion3.Select(s => s.Sku_activo).ToArray(),
                                tiempo4 = seleccion4.Select(s => s.Sku_activo).ToArray(),
                                tiempo5 = seleccion5.Select(s => s.Sku_activo).ToArray(),
                                data = seleccion.Select(s => s.value).ToArray(),
                                data2 = seleccion2.Select(s => s.value).ToArray(),
                                data3 = seleccion3.Select(s => s.value).ToArray(),
                                data4 = seleccion4.Select(s => s.value).ToArray(),
                                data5 = seleccion5.Select(s => s.value).ToArray(),
                                activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                filtro = filtro,
                                nombreActivo = Activo_,
                                sku = seleccion.Select(s => s.turno).ToArray()
                            });

                        }
                        catch (Exception ex)
                        { }
                    }
                    catch (Exception ex)
                    { }
                }
                else if (filtro == "hora")
                {

                    fini = fini_.ToString("yyyyMMdd HH:mm");
                    ffin = ffin_.ToString("yyyyMMdd HH:mm");

                    try
                    {
                        string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";


                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
                        seleccion2 = seleccion;
                        seleccion3 = seleccion;
                        seleccion4 = seleccion;
                        seleccion5 = seleccion;

                        //vueltas++;
                        seleccion = (from iotX in seleccion
                                     where iotX.value > 0 && iotX.planificado == null && iotX.turno != null /*&& iotX.Minutos > 0*/
                                     group iotX by new { iotX.timestamp.Hour } into mm
                                     select new IOT
                                     {
                                         id = mm.Max(m => m.id),
                                         value = mm.Sum(m => m.value),
                                         timestamp = mm.Max(m => m.timestamp),
                                         Sku_activo = mm.Max(m => m.Sku_activo),
                                         Cod_plan = mm.Max(m => m.Cod_plan),
                                         dia = mm.Max(m => m.dia),
                                         marca = maquina,
                                         turno = turno
                                     }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion2 = (from iotX in seleccion2           //tpp
                                      where iotX.turno != null && iotX.Minutos > 0 /*&& iotX.Minutos > 0*/
                                      group iotX by new { iotX.timestamp.Hour } into mm
                                      select new IOT
                                      {
                                          value =
                                          seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
                                          timestamp = mm.Max(m => m.timestamp)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        seleccion4 = (from iotX in seleccion4    //to
                                      where iotX.turno != null && iotX.Minutos > 0 /*&& iotX.Minutos > 0*/
                                      group iotX by new { iotX.timestamp.Hour } into mm
                                      select new IOT
                                      {
                                          id = mm.Max(m => m.id),
                                          value =
                                          seleccion4.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
                                          timestamp = mm.Max(m => m.timestamp),
                                          Sku_activo = mm.Max(m => m.Sku_activo),
                                          Cod_plan = mm.Max(m => m.Cod_plan),
                                          dia = mm.Max(m => m.dia),
                                          marca = maquina
                                      }).OrderBy(o => o.timestamp.Month).ToList();
                        seleccion5 = (from iotX in seleccion5
                                      where iotX.turno != null && iotX.Minutos > 0
                                      group iotX by new { iotX.timestamp.Hour } into mm
                                      select new IOT
                                      {
                                          value =
                                          TiempoXmes(fini_, ffin_, maquina, "HH", idEmpresa),
                                          timestamp = mm.Max(m => m.timestamp)
                                      }).OrderBy(o => o.timestamp.Month).ToList();

                        try
                        {
                            series.Add(new SeriesUnidades()
                            {
                                id = seleccion.Max(s => s.id),
                                fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                tiempo  = seleccion.Select(s  => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
                                tiempo2 = seleccion2.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
                                tiempo4 = seleccion4.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
                                tiempo5 = seleccion5.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
                                data = seleccion.Select(s => s.value).ToArray(),
                                data2 = seleccion2.Select(s => s.value).ToArray(),
                                data4 = seleccion4.Select(s => s.value).ToArray(),
                                data5 = seleccion5.Select(s => s.value).ToArray(),
                                activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                filtro = filtro,
                                nombreActivo = Activo_,
                                sku = seleccion.Select(s => s.turno).ToArray()
                            });
                        }
                        catch (Exception ex)
                        { }

                    }
                    catch (Exception ex)
                    { }
                }                
            }
            return series;
        }

        //Historicos Fin3
        [Microsoft.AspNetCore.Authorization.Authorize]
        //Metodo para la vista inicial de OEE
        public async Task<IActionResult> OEE(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var vr = new viewOEE
                {
                    Sku = await (from s in ctx.Productos
                                 select new Productos
                                 {
                                     Cod_producto = s.Cod_producto,
                                     Des_producto = s.Des_producto
                                 }).ToListAsync(),

                    Act = await (from a in ctx.Activos
                                 select new Activos
                                 {
                                     Cod_activo = a.Cod_activo,
                                     Des_activo = a.Des_activo
                                 }).ToListAsync()
                };

                return View(vr);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        //Metodo usado para inciar la vista de Disponibilidad
        public async Task<IActionResult> Disponibilidad(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var vr = new viewDisponibilidad
                {
                    Sku = await (from s in ctx.Productos
                                 select new Productos
                                 {
                                     Cod_producto = s.Cod_producto,
                                     Des_producto = s.Des_producto
                                 }).ToListAsync()

                };

                return View(vr);
            }
        }

        static DateTime primerDíaSemana(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7).AddDays(1);
        }

        //public List<string> SemanasAsignadas(int sini, int annoIni, int sfin, int annoFin, int idEmpresa)
        //{
        //    List<string> r = new List<string>();

        //    CultureInfo myCIintl = new CultureInfo("es-MX", false);

        //    DateTime PD = primerDíaSemana(annoIni, sini, myCIintl);
        //    DateTime SD = primerDíaSemana(annoFin, sfin, myCIintl);

        //    #region

        //    //using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    //{
        //    //    int semanas = 0;

        //    //    for (int i = annoIni; i <= annoFin; i++) //recorro los años
        //    //    {
        //    //        //Comprobar si el año es bisiesto
        //    //        if (i % 4 == 0 && i % 100 != 0 || i % 400 == 0)
        //    //        {
        //    //            semanas = 53;
        //    //        }
        //    //        else
        //    //        {
        //    //            semanas = 52;
        //    //        }                        

        //    //        //Si el año de inicio es igual al año final
        //    //        if (annoIni == annoFin)
        //    //        {
        //    //            for (int y = sini; y <= sfin; y++)
        //    //            {
        //    //                r.Add((y < 10 ? "0" + y.ToString() : y.ToString()) + i.ToString());
        //    //            }
        //    //        }
        //    //        else //Si el año inicial es menor que el actual 
        //    //        {
        //    //            if (i == annoIni)
        //    //            {
        //    //                for (int y = sini; y <= semanas; y++)
        //    //                {
        //    //                    r.Add((y < 10 ? "0" + y.ToString() : y.ToString()) + i.ToString());
        //    //                }
        //    //            }
        //    //            else if (i == annoFin)
        //    //            {
        //    //                if (semanas == 52)
        //    //                {
        //    //                    for (int y = 1; y < sfin; y++)
        //    //                    {

        //    //                    }
        //    //                }
        //    //            }
        //    //            else if (i > annoIni && i < annoFin)
        //    //            {

        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    #endregion

        //    return r;
        //}


        #region Calculos nuevos optimizados inicio ###########################################################################################################

        public async Task<List<DatosRetorno>> ConsultaDisponibilidadX(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, string[] act, bool sw01)
        {            
            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");
            List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPO = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado> ConsultaPSku = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPOSku = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();
            List<IOT_Conciliado_Semanas> ConsultaPSemanaSku = new List<IOT_Conciliado_Semanas>();
            List<DatosRetorno> seleccion = new List<DatosRetorno>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (sw01 == false) //Si va a buscar los datos en la tabla IOT_Conciliado
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPSku = ConsultaP.ToList();
                            }
                            else
                            {
                                ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        var diasTrabajadosMes = (from dt in ConsultaP
                                                 join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                 where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                 group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Dia.Month } into dtt
                                                 select new
                                                 {
                                                     dia = dtt.Key.Dia,
                                                     valor = dtt.Sum(s => s.Valor),
                                                     turno = dtt.Max(m => m.Cod_turno),
                                                     cod_activo = dtt.Key.Cod_activo,
                                                     mes = dtt.Key.Month,
                                                     minutos = dtt.Max(s => s.Minutos)
                                                 }).Where(w => w.turno != null).ToList();

                        //var minutos = (from dt in ConsultaP
                        //               join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //               where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                        //               group dt by new { dt.Cod_activo, dt.Dia } into dtt
                        //               select new
                        //               {
                        //                   cod_activo = dtt.Key.Cod_activo,
                        //                   minutos = dtt.Max(s => s.Minutos) *
                        //                   (diasTrabajadosMes.Where(w => w.cod_activo == dtt.Key.Cod_activo && w.dia == dtt.Key.Dia).Count()),
                        //                   mes = dtt.Key.Dia.Month
                        //               }).ToList();

                        var minutosTurnos = (from mt in ConsultaP
                                             group mt by new { mt.Cod_activo, mt.Cod_turno, mt.Minutos, mt.Dia, mt.Dia.Month } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 minutos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia,
                                                 mes = mtt.Key.Month
                                             }).ToList();

                        seleccion = (from iotX in ConsultaP
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                     orderby mm.Key.Cod_activo, mm.Key.Year, mm.Key.Month
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count(),

                                         Top = minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.mes == mm.Key.Month).Sum(s => s.minutos)
                                               -
                                               //Planificados
                                               (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                               w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia.Month == mm.Key.Month)).Count())
                                               ,

                                         Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")

                                     }).ToList();

                        return seleccion.ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (act.Count() > 0)
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where ((c.Dia >= f1 && c.Dia <= f2)) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                                                     //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                                //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();

                            #region Código antiguo
                            //if (sku == null)
                            //{
                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                              where ((c.Dia >= f1 && c.Dia <= f2)) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                            //                             //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }).ToListAsync();
                            //}

                            //else 
                            //{
                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                        //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}
                            #endregion

                        }
                        else
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                         //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     })
                                                         .Union(
                                                                from c in ctx.IOT_Conciliado
                                                                    //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).OrderBy(o => o.Cod_activo)
                                                        .ToListAsync();


                            #region Código anterior
                            //if (sku == null)
                            //    //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) 
                            //    //&& w.Cod_turno != null && w.Minutos > 0).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             })
                            //                             .Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                        //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }).OrderBy(o => o.Cod_activo)
                            //                            .ToListAsync();

                            //else

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             //(where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                      from c in ctx.IOT_Conciliado
                            //                                      //(where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                                      where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                                      select new IOT_Conciliado_Semanas
                            //                                      {
                            //                                          Cod_activo = c.Cod_activo,
                            //                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                          Dia = c.Dia,
                            //                                          Cod_plan = c.Cod_plan,
                            //                                          Cod_turno = c.Cod_turno,
                            //                                          IOT_id = c.IOT_id,
                            //                                          Marca = c.Marca,
                            //                                          Minutos = c.Minutos,
                            //                                          Ndia = c.Ndia,
                            //                                          Nhora = c.Nhora,
                            //                                          Planificado = c.Planificado,
                            //                                          Semana = c.Semana,
                            //                                          Sku_activo = c.Sku_activo,
                            //                                          Tiempo = c.Tiempo,
                            //                                          Valor = c.Valor,
                            //                                          Variable = c.Variable,
                            //                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                      }
                            //                                   ).ToListAsync();
                            #endregion
                        }

                        if (sku == null)
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.ToList();
                        }
                        else
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.Where(w => w.Sku_activo == sku).ToList();
                        }

                        var diasTrabajadosSemana = (from dt in ConsultaPSemana
                                                    join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                    //where dt.Cod_turno != null && dt.Dia == te.Fecha_ini && dt.Cod_turno == te.Cod_turno
                                                    where dt.Cod_turno != null && dt.Dia >= f1 && dt.Dia <= f2 && dt.Cod_turno == te.Cod_turno
                                                    group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Semana } into dtt
                                                    select new
                                                    {
                                                        dia = dtt.Key.Dia,
                                                        valor = dtt.Sum(s => s.Valor),
                                                        turno = dtt.Max(m => m.Cod_turno),
                                                        cod_activo = dtt.Key.Cod_activo,
                                                        semana = dtt.Key.Semana
                                                    }).Where(w => w.turno != null
                                                    ).ToList();

                        var minutosTurnos = (from mt in ConsultaPSemana
                                             group mt by new { mt.Cod_activo, mt.Cod_turno, mt.Minutos, mt.Dia, mt.Semana } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 semana = mtt.Key.Semana,
                                                 dia = mtt.Key.Dia
                                             }).ToList();

                        try
                        {
                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPSemanaSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                  && (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count(),

                                             Top = //ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Max(m => m.Minutos) *
                                                   (minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && (w.dia >= f1 && w.dia <= f2) && w.semana == mm.Key.Semana).Sum(s => s.munitos)) 

                                                   //* (diasTrabajadosSemana.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Count())
                                                    -
                                             //Planificados
                                             (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && (w.Dia >= f1 && w.Dia <= f2) && w.Planificado == true && w.Cod_turno != null &&
                                             w.Minutos > 0 && (w.Semana == mm.Key.Semana)).Count()),

                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno), //"Semana: " + mm.Key.Semana.ToString()

                                             Dia = mm.Max(m => m.Dia)

                                         }).ToList();
                        }
                        catch (Exception EX)
                        {

                        }

                        return seleccion.ToList();

                    }
                    else if (filtro == "dia") //Solo aquí aplica el turno
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));
                        int tiempo_turno = 0;

                        if (turno != "Todos")
                        {
                            if (turno != null)
                            {
                                var t1 = (from t in ctx.Turnos
                                          where t.Cod_turno == turno
                                          select new
                                          {
                                              fini = DateTime.Parse("01-01-1900" + " " + t.Hora_ini1.ToString("HH:mm")),
                                              ffin = DateTime.Parse("01-01-1900" + " " + t.Hora_fin1.ToString("HH:mm"))
                                          }).FirstOrDefault();

                                DateTime fecha1 = t1.ffin;
                                DateTime fecha2 = t1.fini;
                                TimeSpan ttt;

                                if (fecha2 < fecha1)
                                    ttt = t1.ffin.Subtract(t1.fini);
                                else
                                {
                                    ttt = t1.ffin.AddDays(1).Subtract(t1.fini);
                                }

                                tiempo_turno = ttt.Hours * 60;
                            }
                        }



                        if (act.Count() > 0)
                        {
                            //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"

                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Minutos > 0 && w.Cod_turno == turno && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                        }
                        else //Si no se establecieron activos
                        {
                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Minutos > 0 && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        if (sku == null)
                        {
                            ConsultaPSku = ConsultaP.ToList();
                        }
                        else
                        {
                            ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                        }

                        var diasTrabajadosDia = (from dt in ConsultaP
                                                 join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                 where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                 group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno } into dtt
                                                 select new
                                                 {
                                                     dia = dtt.Key.Dia,
                                                     valor = dtt.Sum(s => s.Valor),
                                                     turno = dtt.Max(m => m.Cod_turno),
                                                     cod_activo = dtt.Key.Cod_activo
                                                     //timestamp = dt.timestamp,
                                                     //turno = te.Cod_turno
                                                 }).Where(w => w.turno != null).ToList();

                        var minutosTurnos = (from mt in ConsultaP
                                             group mt by new { mt.Cod_activo, mt.Cod_turno, mt.Minutos, mt.Dia } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia
                                             }).ToList();

                        var prueba = minutosTurnos;

                        seleccion = (from iotX in ConsultaP
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count(),

                                         Top = minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Sum(s => s.munitos) //ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia == mm.Key.Dia).Max(m => m.Minutos)

                                               //* (diasTrabajadosDia.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Count())
                                               -
                                               //Planificados
                                               (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia == mm.Key.Dia)).Count())
                                               ,

                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy") + " " + (turno != null ? "Turno: " + turno : ""),

                                         Dia = mm.Key.Dia

                                     }).ToList();

                        return seleccion.ToList(); //.Where(w => w.To > 0 && w.Top > 0).ToList();
                    }
                    else if (filtro == "hora")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPSku = ConsultaP.ToList();
                            }
                            else
                            {
                                ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                            }


                        }
                        catch (Exception ex)
                        {

                        }

                        var diasTrabajadosHora = (from dt in ConsultaP
                                                  join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                  where ArmaFechaHora(dt.Dia,dt.Nhora) >= fini_ && ArmaFechaHora(dt.Dia, dt.Nhora) <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                  group dt by new { dt.Cod_activo, dt.Dia, dt.Nhora, dt.Cod_turno } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.Dia,
                                                      hora = dtt.Key.Nhora,
                                                      valor = dtt.Sum(s => s.Valor),
                                                      turno = dtt.Max(m => m.Cod_turno),
                                                      cod_activo = dtt.Key.Cod_activo,
                                                      minutos = dtt.Max(s => s.Minutos)
                                                  }).Where(w => w.turno != null).ToList();

                        var minutosTurnos = (from mt in ConsultaP
                                             group mt by new { mt.Cod_activo, mt.Minutos, mt.Dia, mt.Nhora } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia,
                                                 hora = mtt.Key.Nhora
                                             }).ToList();

                        seleccion = (from iotX in ConsultaP
                                     join text in ctx.Turnos_activos_extras on iotX.Cod_turno equals text.Cod_turno
                                     where iotX.Cod_activo == text.Cod_activo
                                     orderby iotX.Cod_activo, iotX.Tiempo
                                     group iotX by new { iotX.Cod_activo, iotX.Dia, iotX.Nhora } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count(),

                                         Top = //minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Sum(s => s.munitos) 

                                               ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora).Count()
                                               //* (diasTrabajadosHora.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Count())
                                               -
                                               //Planificados
                                               (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true &&
                                               (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count())
                                               ,

                                         Leyenda = mm.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")), //mm.Key.Tiempo.ToString("dd/MM/yyyy hh:mm")

                                         Dia = mm.Max(m => m.Dia),
                                         Hora = mm.Key.Nhora //mm.Min(m => int.Parse(m.Tiempo.ToString("HH")))

                                     }).ToList();

                        return seleccion.ToList();
                    }
                }
                else ////Si va a buscar los datos en la tabla IOT_Conciliado_optimizado
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPOSku = ConsultaPO.ToList();
                            }
                            else
                            {
                                ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        var diasTrabajadosMes = (from dt in ConsultaPO
                                                 join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                 where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                 group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Dia.Month } into dtt
                                                 select new
                                                 {
                                                     dia = dtt.Key.Dia,
                                                     valor = dtt.Sum(s => s.Valor),
                                                     turno = dtt.Max(m => m.Cod_turno),
                                                     cod_activo = dtt.Key.Cod_activo,
                                                     mes = dtt.Key.Month,
                                                     minutos = dtt.Max(s => s.Minutos)
                                                 }).Where(w => w.turno != null).ToList();

                        //var minutos = (from dt in ConsultaPO
                        //               join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //               where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                        //               group dt by new { dt.Cod_activo, dt.Dia } into dtt
                        //               select new
                        //               {
                        //                   cod_activo = dtt.Key.Cod_activo,
                        //                   minutos = dtt.Max(s => s.Minutos) *
                        //                   (diasTrabajadosMes.Where(w => w.cod_activo == dtt.Key.Cod_activo && w.dia == dtt.Key.Dia).Count()),
                        //                   mes = dtt.Key.Dia.Month
                        //               }).ToList();

                        var minutosTurnos = (from mt in ConsultaPO
                                             group mt by new { mt.Cod_activo, mt.Minutos, mt.Dia, mt.Dia.Month } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 minutos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia,
                                                 mes = mtt.Key.Month
                                             }).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                     orderby mm.Key.Cod_activo, mm.Key.Year, mm.Key.Month
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPOSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count(),

                                         Top = minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.mes == mm.Key.Month).Sum(s => s.minutos)
                                               -
                                               //Planificados
                                               (ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                               w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia.Month == mm.Key.Month)).Count())
                                               ,

                                         Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")

                                     }).ToList();

                        return seleccion.ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (act.Count() > 0)
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                     where ((c.Dia >= f1 && c.Dia <= f2)) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                                                     //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                                //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();

                            #region Código antiguo
                            //if (sku == null)
                            //{
                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                              where ((c.Dia >= f1 && c.Dia <= f2)) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                            //                             //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }).ToListAsync();
                            //}

                            //else 
                            //{
                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                        //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Sku_activo == sku
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}
                            #endregion

                        }
                        else
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                         //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     })
                                                         .Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                    //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).OrderBy(o => o.Cod_activo)
                                                        .ToListAsync();


                            #region Código anterior
                            //if (sku == null)
                            //    //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) 
                            //    //&& w.Cod_turno != null && w.Minutos > 0).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             })
                            //                             .Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                        //where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }).OrderBy(o => o.Cod_activo)
                            //                            .ToListAsync();

                            //else

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             //(where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                      from c in ctx.IOT_Conciliado_optimizado
                            //                                      //(where c.Dia >= f1 && c.Dia <= f2 && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                                      where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Sku_activo == sku
                            //                                      select new IOT_Conciliado_Semanas
                            //                                      {
                            //                                          Cod_activo = c.Cod_activo,
                            //                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                          Dia = c.Dia,
                            //                                          Cod_plan = c.Cod_plan,
                            //                                          Cod_turno = c.Cod_turno,
                            //                                          IOT_id = c.IOT_id,
                            //                                          Marca = c.Marca,
                            //                                          Minutos = c.Minutos,
                            //                                          Ndia = c.Ndia,
                            //                                          Nhora = c.Nhora,
                            //                                          Planificado = c.Planificado,
                            //                                          Semana = c.Semana,
                            //                                          Sku_activo = c.Sku_activo,
                            //                                          Tiempo = c.Tiempo,
                            //                                          Valor = c.Valor,
                            //                                          Variable = c.Variable,
                            //                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                      }
                            //                                   ).ToListAsync();
                            #endregion
                        }

                        if (sku == null)
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.ToList();
                        }
                        else
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.Where(w => w.Sku_activo == sku).ToList();
                        }

                        var diasTrabajadosSemana = (from dt in ConsultaPSemana
                                                    join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                    //where dt.Cod_turno != null && dt.Dia == te.Fecha_ini && dt.Cod_turno == te.Cod_turno
                                                    where dt.Cod_turno != null && dt.Dia >= f1 && dt.Dia <= f2 && dt.Cod_turno == te.Cod_turno
                                                    group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Semana } into dtt
                                                    select new
                                                    {
                                                        dia = dtt.Key.Dia,
                                                        valor = dtt.Sum(s => s.Valor),
                                                        turno = dtt.Max(m => m.Cod_turno),
                                                        cod_activo = dtt.Key.Cod_activo,
                                                        semana = dtt.Key.Semana
                                                    }).Where(w => w.turno != null
                                                    ).ToList();

                        var minutosTurnos = (from mt in ConsultaPSemana
                                             group mt by new { mt.Cod_activo, mt.Minutos, mt.Dia, mt.Semana } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 semana = mtt.Key.Semana,
                                                 dia = mtt.Key.Dia
                                             }).ToList();

                        try
                        {
                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPSemanaSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                  && (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count(),

                                             Top =
                                                    //ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Max(m => m.Minutos) *
                                                   (minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && (w.dia >= f1 && w.dia <= f2) && w.semana == mm.Key.Semana).Sum(s => s.munitos))

                                                    //* (diasTrabajadosSemana.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Count())
                                                    -
                                             //Planificados
                                             (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && (w.Dia >= f1 && w.Dia <= f2) && w.Planificado == true && w.Cod_turno != null &&
                                             w.Minutos > 0 && (w.Semana == mm.Key.Semana)).Count()),

                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno), //"Semana: " + mm.Key.Semana.ToString()

                                             Dia = mm.Max(m => m.Dia)

                                         }).ToList();
                        }
                        catch (Exception EX)
                        {

                        }

                        return seleccion.ToList();

                    }
                    else if (filtro == "dia") //Solo aquí aplica el turno
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));
                        int tiempo_turno = 0;

                        if (turno != "Todos")
                        {
                            if (turno != null)
                            {
                                var t1 = (from t in ctx.Turnos
                                          where t.Cod_turno == turno
                                          select new
                                          {
                                              fini = DateTime.Parse("01-01-1900" + " " + t.Hora_ini1.ToString("HH:mm")),
                                              ffin = DateTime.Parse("01-01-1900" + " " + t.Hora_fin1.ToString("HH:mm"))
                                          }).FirstOrDefault();

                                DateTime fecha1 = t1.ffin;
                                DateTime fecha2 = t1.fini;
                                TimeSpan ttt;

                                if (fecha2 < fecha1)
                                    ttt = t1.ffin.Subtract(t1.fini);
                                else
                                {
                                    ttt = t1.ffin.AddDays(1).Subtract(t1.fini);
                                }

                                tiempo_turno = ttt.Hours * 60;
                            }
                        }



                        if (act.Count() > 0)
                        {
                            //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"

                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Minutos > 0 && w.Cod_turno == turno && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                        }
                        else //Si no se establecieron activos
                        {
                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Minutos > 0 && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        if (sku == null)
                        {
                            ConsultaPOSku = ConsultaPO.ToList();
                        }
                        else
                        {
                            ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                        }

                        var diasTrabajadosDia = (from dt in ConsultaPO
                                                 join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                 where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                 group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno } into dtt
                                                 select new
                                                 {
                                                     dia = dtt.Key.Dia,
                                                     valor = dtt.Sum(s => s.Valor),
                                                     turno = dtt.Max(m => m.Cod_turno),
                                                     cod_activo = dtt.Key.Cod_activo
                                                     //timestamp = dt.timestamp,
                                                     //turno = te.Cod_turno
                                                 }).Where(w => w.turno != null).ToList();

                        var minutosTurnos = (from mt in ConsultaPO
                                             group mt by new { mt.Cod_activo, mt.Minutos, mt.Dia } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia
                                             }).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPOSku.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count(),

                                         Top = minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Sum(s => s.munitos) //ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia == mm.Key.Dia).Max(m => m.Minutos) * (diasTrabajadosDia.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Count())
                                               -
                                               //Planificados
                                               (ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia == mm.Key.Dia)).Count())
                                               ,

                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy") + " " + (turno != null ? "Turno: " + turno : ""),

                                         Dia = mm.Key.Dia

                                     }).ToList();

                        return seleccion.ToList(); //.Where(w => w.To > 0 && w.Top > 0).ToList();
                    }
                    else if (filtro == "hora")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPOSku = ConsultaPO.ToList();
                            }
                            else
                            {
                                ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                            }


                        }
                        catch (Exception ex)
                        {

                        }

                        var diasTrabajadosHora = (from dt in ConsultaPO
                                                  join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                                                  where dt.Nhora >= fini_.Hour && dt.Nhora <= ffin_.Hour && dt.Cod_turno != null && dt.Dia == te.Fecha_ini.Date && dt.Cod_turno == te.Cod_turno
                                                  group dt by new { dt.Cod_activo, dt.Dia, dt.Nhora, dt.Cod_turno } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.Dia,
                                                      hora = dtt.Key.Nhora,
                                                      valor = dtt.Sum(s => s.Valor),
                                                      turno = dtt.Max(m => m.Cod_turno),
                                                      cod_activo = dtt.Key.Cod_activo,
                                                      minutos = dtt.Max(s => s.Minutos)
                                                  }).Where(w => w.turno != null).ToList();

                        var minutosTurnos = (from mt in ConsultaPO
                                             group mt by new { mt.Cod_activo, mt.Minutos, mt.Dia, mt.Nhora } into mtt
                                             select new
                                             {
                                                 cod_activo = mtt.Key.Cod_activo,
                                                 munitos = mtt.Key.Minutos,
                                                 dia = mtt.Key.Dia,
                                                 hora = mtt.Key.Nhora
                                             }).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     join text in ctx.Turnos_activos_extras on iotX.Cod_turno equals text.Cod_turno
                                     orderby iotX.Cod_activo, iotX.Tiempo
                                     group iotX by new { iotX.Cod_activo, iotX.Dia, iotX.Nhora } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count(),

                                         Top = //minutosTurnos.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Sum(s => s.munitos) 
                                                 ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora).Count()
                                                 * (diasTrabajadosHora.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Count())
                                               -
                                               //Planificados
                                               (ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true &&
                                               (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count())
                                               ,

                                         Leyenda = mm.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")), //mm.Key.Tiempo.ToString("dd/MM/yyyy hh:mm")

                                         Dia = mm.Max(m => m.Dia),
                                         Hora = mm.Key.Nhora //mm.Min(m => int.Parse(m.Tiempo.ToString("HH")))

                                     }).ToList();

                        return seleccion.ToList();
                    }
                }
                
            }
            return null;
        }

        public DateTime ArmaFechaHora(DateTime Fecha, int Hora)
        {
            string fecha_ = Fecha.ToString("dd/MM/yyyy");
            string hora_ = (Hora < 10 ? ("0" + Hora.ToString()) : Hora.ToString()) + ":00";

            DateTime res = DateTime.Now; 

            try
            { 
                res = DateTime.Parse(fecha_ + " " + hora_);
            }
            catch (Exception ex)
            { }
                

            return res.AddMinutes(1);
        }

        public async Task<List<DatosRetorno>> ConsultaRendimientoX(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, string[] act, bool sw01)
        {
            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");
            List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPO = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado> ConsultaPSku = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPOSku = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();
            List<IOT_Conciliado_Semanas> ConsultaPSemanaSku = new List<IOT_Conciliado_Semanas>();
            List<DatosRetorno> seleccion = new List<DatosRetorno>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (sw01 == false) //Si la consulta es a IOT_Conciliado
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPSku = ConsultaP.ToList();
                            }
                            else
                            {
                                ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                            }

                            var capacidadesX = (from i in ConsultaPSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Dia.Year, i.Dia } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    semana = ppp.Max(m => m.Semana),
                                                    #region Campos de prueba
                                                    //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                    //              (                                                              
                                                    //                  (
                                                    //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                    //                                ?

                                                    //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                    //                                :

                                                    //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                    //                   )
                                                    //               ),

                                                    //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                    #endregion
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();


                            seleccion = (from iotX in ConsultaP
                                         group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month).Sum(s => s.valores),

                                             Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (act.Count() > 0)
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                                                     //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(from c in ctx.IOT_Conciliado
                                                              where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                              //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                              select new IOT_Conciliado_Semanas
                                                              {
                                                                  Cod_activo = c.Cod_activo,
                                                                  Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                  Dia = c.Dia,
                                                                  Cod_plan = c.Cod_plan,
                                                                  Cod_turno = c.Cod_turno,
                                                                  IOT_id = c.IOT_id,
                                                                  Marca = c.Marca,
                                                                  Minutos = c.Minutos,
                                                                  Ndia = c.Ndia,
                                                                  Nhora = c.Nhora,
                                                                  Planificado = c.Planificado,
                                                                  Semana = c.Semana,
                                                                  Sku_activo = c.Sku_activo,
                                                                  Tiempo = c.Tiempo,
                                                                  Valor = c.Valor,
                                                                  Variable = c.Variable,
                                                                  Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                              }).ToListAsync();

                            #region Código anterior
                            //if (sku == null)

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                            //                             //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(from c in ctx.IOT_Conciliado
                            //                                      where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                      //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                      select new IOT_Conciliado_Semanas
                            //                                      {
                            //                                          Cod_activo = c.Cod_activo,
                            //                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                          Dia = c.Dia,
                            //                                          Cod_plan = c.Cod_plan,
                            //                                          Cod_turno = c.Cod_turno,
                            //                                          IOT_id = c.IOT_id,
                            //                                          Marca = c.Marca,
                            //                                          Minutos = c.Minutos,
                            //                                          Ndia = c.Ndia,
                            //                                          Nhora = c.Nhora,
                            //                                          Planificado = c.Planificado,
                            //                                          Semana = c.Semana,
                            //                                          Sku_activo = c.Sku_activo,
                            //                                          Tiempo = c.Tiempo,
                            //                                          Valor = c.Valor,
                            //                                          Variable = c.Variable,
                            //                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                      }).ToListAsync();

                            //else
                            //    //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && w.Minutos > 0 && 
                            //    //w.Sku_activo == sku && act.Contains(w.Cod_activo)).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) &&
                            //                             c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) &&
                            //                                    c.Sku_activo == sku && c.Tiempo > Inicio
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();

                            #endregion
                        }
                        else
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                     //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                                //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }
                                                            ).ToListAsync();

                            #region Código anterior

                            //if (sku == null)
                            //{
                            //    //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && 
                            //    //w.Minutos > 0).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                             //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                                    //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}
                            //else
                            //{
                            //    //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && w.Minutos > 0 &&
                            //    //w.Sku_activo == sku).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 &&
                            //                             c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 &&
                            //                                    c.Sku_activo == sku && c.Tiempo > Inicio
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}

                            #endregion
                        }

                        if (sku == null)
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.ToList();
                        }
                        else
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.Where(w => w.Sku_activo == sku).ToList();
                        }

                        try
                        {

                            var capacidadesX = (from i in ConsultaPSemanaSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Anno, i.Dia } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    semana = ppp.Max(m => m.Semana),
                                                    #region Campos de prueba
                                                    //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                    //              (                                                              
                                                    //                  (
                                                    //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                    //                                ?

                                                    //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                    //                                :

                                                    //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                    //                   )
                                                    //               ),

                                                    //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                    #endregion
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();

                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                  && w.Semana == mm.Key.Semana).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Sum(s => s.valores),

                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno) //"Semana: " + mm.Key.Semana.ToString()

                                         }).ToList();

                            return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    else if (filtro == "dia")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (act.Count() > 0)
                        {
                            //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"

                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();

                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_turno == turno && act.Contains(w.Cod_activo)).ToListAsync();

                            }
                        }
                        else //Si no se establecieron activos
                        {
                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        if (sku == null)
                        {
                            ConsultaPSku = ConsultaP.ToList();
                        }
                        else
                        {
                            ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                        }


                        //saco la suma de las capacidades de los activos                     
                        var capacidadesX = (from i in ConsultaPSku
                                            group i by new { i.Cod_activo, i.Sku_activo, i.Dia.Year, i.Dia } into ppp
                                            select new
                                            {
                                                cod_activo = ppp.Key.Cod_activo,
                                                sku = ppp.Key.Sku_activo,
                                                dia = ppp.Key.Dia,
                                                #region Campos de prueba
                                                //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                //              (                                                              
                                                //                  (
                                                //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                //                                ?

                                                //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                //                                :

                                                //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                //                   )
                                                //               ),

                                                //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                #endregion
                                                valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                          (
                                                              ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                              (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                        && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                        ?

                                                               ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                        :

                                                                ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                        && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                           )

                                            }).ToList();

                        seleccion = (from iotX in ConsultaP
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count(),

                                         Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Sum(s => s.valores),

                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy") + " " + (turno != null ? "Turno: " + turno : "")
                                     }).ToList();

                        return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();
                    }
                    else if (filtro == "hora")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 &&
                                act.Contains(w.Cod_activo)
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPSku = ConsultaP.ToList();
                            }
                            else
                            {
                                ConsultaPSku = ConsultaP.Where(w => w.Sku_activo == sku).ToList();
                            }

                            var capacidadesX = (from i in ConsultaPSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Dia, i.Nhora } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    hora = ppp.Key.Nhora,
                                                    semana = ppp.Max(m => m.Semana),
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();

                            seleccion = (from iotX in ConsultaP
                                         orderby iotX.Cod_activo, iotX.Tiempo
                                         group iotX by new { iotX.Cod_activo, iotX.Dia, iotX.Nhora } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Sum(s => s.valores),

                                             Leyenda = mm.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),

                                             Hora = mm.Key.Nhora //mm.Min(m => int.Parse(m.Tiempo.ToString("HH")))
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        //return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();
                        return seleccion.ToList();

                    }
                }
                else //Si la consulta es a IOT_Conciliado_optimizado
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPOSku = ConsultaPO.ToList();
                            }
                            else
                            {
                                ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                            }

                            var capacidadesX = (from i in ConsultaPOSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Dia.Year, i.Dia } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    semana = ppp.Max(m => m.Semana),
                                                    #region Campos de prueba
                                                    //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                    //              (                                                              
                                                    //                  (
                                                    //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                    //                                ?

                                                    //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                    //                                :

                                                    //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                    //                   )
                                                    //               ),

                                                    //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                    #endregion
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();


                            seleccion = (from iotX in ConsultaPO
                                         group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month).Sum(s => s.valores),

                                             Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (act.Count() > 0)
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                                                     //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(from c in ctx.IOT_Conciliado_optimizado
                                                              where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                              //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                              select new IOT_Conciliado_Semanas
                                                              {
                                                                  Cod_activo = c.Cod_activo,
                                                                  Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                  Dia = c.Dia,
                                                                  Cod_plan = c.Cod_plan,
                                                                  Cod_turno = c.Cod_turno,
                                                                  IOT_id = c.IOT_id,
                                                                  Marca = c.Marca,
                                                                  Minutos = c.Minutos,
                                                                  Ndia = c.Ndia,
                                                                  Nhora = c.Nhora,
                                                                  Planificado = c.Planificado,
                                                                  Semana = c.Semana,
                                                                  Sku_activo = c.Sku_activo,
                                                                  Tiempo = c.Tiempo,
                                                                  Valor = c.Valor,
                                                                  Variable = c.Variable,
                                                                  Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                              }).ToListAsync();

                            #region Código anterior
                            //if (sku == null)

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) && c.Tiempo > Inicio
                            //                             //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(from c in ctx.IOT_Conciliado_optimizado
                            //                                      where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                      //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                            //                                      select new IOT_Conciliado_Semanas
                            //                                      {
                            //                                          Cod_activo = c.Cod_activo,
                            //                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                          Dia = c.Dia,
                            //                                          Cod_plan = c.Cod_plan,
                            //                                          Cod_turno = c.Cod_turno,
                            //                                          IOT_id = c.IOT_id,
                            //                                          Marca = c.Marca,
                            //                                          Minutos = c.Minutos,
                            //                                          Ndia = c.Ndia,
                            //                                          Nhora = c.Nhora,
                            //                                          Planificado = c.Planificado,
                            //                                          Semana = c.Semana,
                            //                                          Sku_activo = c.Sku_activo,
                            //                                          Tiempo = c.Tiempo,
                            //                                          Valor = c.Valor,
                            //                                          Variable = c.Variable,
                            //                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                      }).ToListAsync();

                            //else
                            //    //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && w.Minutos > 0 && 
                            //    //w.Sku_activo == sku && act.Contains(w.Cod_activo)).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) &&
                            //                             c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo) &&
                            //                                    c.Sku_activo == sku && c.Tiempo > Inicio
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();

                            #endregion
                        }
                        else
                        {
                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                     //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                                                                //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }
                                                            ).ToListAsync();

                            #region Código anterior

                            //if (sku == null)
                            //{
                            //    //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && 
                            //    //w.Minutos > 0).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                             //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Tiempo > Inicio
                            //                                    //where c.Semana >= sini && c.Semana <= sfin && c.Cod_turno != null && c.Minutos > 0
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}
                            //else
                            //{
                            //    //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_turno != null && w.Minutos > 0 &&
                            //    //w.Sku_activo == sku).ToListAsync();

                            //    ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                            //                             where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 &&
                            //                             c.Sku_activo == sku && c.Tiempo > Inicio
                            //                             select new IOT_Conciliado_Semanas
                            //                             {
                            //                                 Cod_activo = c.Cod_activo,
                            //                                 Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                 Dia = c.Dia,
                            //                                 Cod_plan = c.Cod_plan,
                            //                                 Cod_turno = c.Cod_turno,
                            //                                 IOT_id = c.IOT_id,
                            //                                 Marca = c.Marca,
                            //                                 Minutos = c.Minutos,
                            //                                 Ndia = c.Ndia,
                            //                                 Nhora = c.Nhora,
                            //                                 Planificado = c.Planificado,
                            //                                 Semana = c.Semana,
                            //                                 Sku_activo = c.Sku_activo,
                            //                                 Tiempo = c.Tiempo,
                            //                                 Valor = c.Valor,
                            //                                 Variable = c.Variable,
                            //                                 Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                            //                             }).Union(
                            //                                    from c in ctx.IOT_Conciliado_optimizado
                            //                                    where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 &&
                            //                                    c.Sku_activo == sku && c.Tiempo > Inicio
                            //                                    select new IOT_Conciliado_Semanas
                            //                                    {
                            //                                        Cod_activo = c.Cod_activo,
                            //                                        Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                            //                                        Dia = c.Dia,
                            //                                        Cod_plan = c.Cod_plan,
                            //                                        Cod_turno = c.Cod_turno,
                            //                                        IOT_id = c.IOT_id,
                            //                                        Marca = c.Marca,
                            //                                        Minutos = c.Minutos,
                            //                                        Ndia = c.Ndia,
                            //                                        Nhora = c.Nhora,
                            //                                        Planificado = c.Planificado,
                            //                                        Semana = c.Semana,
                            //                                        Sku_activo = c.Sku_activo,
                            //                                        Tiempo = c.Tiempo,
                            //                                        Valor = c.Valor,
                            //                                        Variable = c.Variable,
                            //                                        Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                            //                                    }
                            //                                ).ToListAsync();
                            //}

                            #endregion
                        }

                        if (sku == null)
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.ToList();
                        }
                        else
                        {
                            ConsultaPSemanaSku = ConsultaPSemana.Where(w => w.Sku_activo == sku).ToList();
                        }

                        try
                        {

                            var capacidadesX = (from i in ConsultaPSemanaSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Anno, i.Dia } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    semana = ppp.Max(m => m.Semana),
                                                    #region Campos de prueba
                                                    //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                    //              (                                                              
                                                    //                  (
                                                    //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                    //                                ?

                                                    //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                    //                                :

                                                    //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                    //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                    //                   )
                                                    //               ),

                                                    //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                    #endregion
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();

                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                  && w.Semana == mm.Key.Semana).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Sum(s => s.valores),

                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno) //"Semana: " + mm.Key.Semana.ToString()

                                         }).ToList();

                            return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    else if (filtro == "dia")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (act.Count() > 0)
                        {
                            //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"

                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && act.Contains(w.Cod_activo)).ToListAsync();

                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_turno == turno && act.Contains(w.Cod_activo)).ToListAsync();

                            }
                        }
                        else //Si no se establecieron activos
                        {
                            if (turno == "Todos" || turno == null)
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        if (sku == null)
                        {
                            ConsultaPOSku = ConsultaPO.ToList();
                        }
                        else
                        {
                            ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                        }


                        //saco la suma de las capacidades de los activos                     
                        var capacidadesX = (from i in ConsultaPOSku
                                            group i by new { i.Cod_activo, i.Sku_activo, i.Dia.Year, i.Dia } into ppp
                                            select new
                                            {
                                                cod_activo = ppp.Key.Cod_activo,
                                                sku = ppp.Key.Sku_activo,
                                                dia = ppp.Key.Dia,
                                                #region Campos de prueba
                                                //capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                //              (                                                              
                                                //                  (
                                                //                      ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                //                                && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                //                                ?

                                                //                       ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                //                                :

                                                //                        ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                //                                && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima)
                                                //                   )
                                                //               ),

                                                //registros = ppp.Where(w => w.Planificado == null).Sum(s => s.Valor),
                                                #endregion
                                                valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                          (
                                                              ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                              (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                        && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                        ?

                                                               ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                        :

                                                                ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                        && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                           )

                                            }).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetorno
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         To = ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count(),

                                         Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia).Sum(s => s.valores),

                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy") + " " + (turno != null ? "Turno: " + turno : "")
                                     }).ToList();

                        return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();
                    }
                    else if (filtro == "hora")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy hh:mm tt"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy hh:mm tt"));

                            if (act.Count() > 0) //Si recibo la lista de activos le agrego la parte del filtro "act.Contains(w.Cod_activo)"
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 &&
                                act.Contains(w.Cod_activo)
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }
                            else //Si no se establecieron activos
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                            }

                            if (sku == null)
                            {
                                ConsultaPOSku = ConsultaPO.ToList();
                            }
                            else
                            {
                                ConsultaPOSku = ConsultaPO.Where(w => w.Sku_activo == sku).ToList();
                            }

                            var capacidadesX = (from i in ConsultaPOSku
                                                group i by new { i.Cod_activo, i.Sku_activo, i.Dia, i.Nhora } into ppp
                                                select new
                                                {
                                                    cod_activo = ppp.Key.Cod_activo,
                                                    sku = ppp.Key.Sku_activo,
                                                    dia = ppp.Key.Dia,
                                                    hora = ppp.Key.Nhora,
                                                    semana = ppp.Max(m => m.Semana),
                                                    valores = ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Count() == 0 ? 0 :
                                                              (
                                                                  ppp.Where(w => w.Planificado == null).Sum(s => s.Valor) /
                                                                  (ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Count() == 0

                                                                            ?

                                                                   ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo).Max(m => m.Capacidad_maxima)

                                                                            :

                                                                    ctx.Capacidades_activos.Where(w => w.Cod_activo == ppp.Key.Cod_activo
                                                                            && w.Cod_producto == ppp.Key.Sku_activo).Max(m => m.Capacidad_maxima))
                                                               )

                                                }).ToList();

                            seleccion = (from iotX in ConsultaPO
                                         orderby iotX.Cod_activo, iotX.Tiempo
                                         group iotX by new { iotX.Cod_activo, iotX.Dia, iotX.Nhora } into mm
                                         select new DatosRetorno
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),

                                             To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count(),

                                             Top = capacidadesX.Where(w => w.cod_activo == mm.Key.Cod_activo && w.dia == mm.Key.Dia && w.hora == mm.Key.Nhora).Sum(s => s.valores),

                                             Leyenda = mm.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),

                                             Hora = mm.Key.Nhora //mm.Min(m => int.Parse(m.Tiempo.ToString("HH")))
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                        return seleccion.Where(w => w.To > 0 && w.Top > 0).ToList();

                    }
                }
                
            }
            return null;
        }

        //Consolidado de unidades producidas
        public async Task<object> KpiUnidadesProducidasX(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string unidad, string cod_activo, bool porSku, bool sw01)
        {
            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Consolidado> Consol = new List<Consolidado>();
                List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
                List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();
                List<IOT_Conciliado_optimizado> ConsultaPO = new List<IOT_Conciliado_optimizado>();
                List<IOT_Conciliado_optimizado> ConsultaPOSku = new List<IOT_Conciliado_optimizado>();

                if (sw01 == false)
                {
                    if (filtro != "semana") //Si el filtro es diferente a la semana
                    {
                        if (turno == null)
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaP = ctx.IOT_Conciliado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0).ToList();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            }
                        }
                        else
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaP = ctx.IOT_Conciliado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_turno == turno).ToList();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                            }
                        }
                    }

                    if (filtro == "mes")
                    {
                        var totales0 = (from sel in ConsultaP //ctx.IOT_Conciliado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null
                                                && (sel.Dia >= fini_ && sel.Dia <= ffin_)
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo
                                        }).ToList();

                        if (porSku == false) //Consulta no agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia.Year, t.dia.Month } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = ConvierteFecha(tt.Max(m => m.dia), "mes"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              fecha = ConvierteFecha(tt.Max(m => m.dia), "mes"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }




                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (cod_activo == null)
                        {
                            //ConsultaP = ctx.IOT_Conciliado.Where(w => (w.Dia.Year == fini_.Year && w.Semana >= sini) || (w.Dia.Year == ffin_.Year && w.Semana <= sfin) && w.Valor > 0).ToList();
                            //ConsultaP = ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Valor > 0).ToList();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Valor > 0 //&& c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Valor > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        }
                        else
                        {
                            //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia.Year == fini_.Year && w.Semana >= sini) || (w.Dia.Year == ffin_.Year && w.Semana <= sfin) && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Valor > 0 && c.Cod_activo == cod_activo //&& c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_activo == cod_activo && c.Valor > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        }

                        var totales0 = (from sel in ConsultaPSemana //ctx.IOT_Conciliado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null 
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            semana = sel.Semana,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo,
                                            anno = sel.Semana == 53 ? f1.Year : sel.Dia.Year
                                        }).ToList();

                        if (porSku == false)
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.anno, t.semana } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = FormatoSemana(tt.Key.semana, tt.Key.anno),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              turno = turno,
                                                              unidades = ff,
                                                              hora = 0,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }


                    } 
                    else if (filtro == "dia")
                    {
                        var totales0 = (from sel in ConsultaP //ctx.IOT_Conciliado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null 
                                        && (sel.Dia >= fini_ && sel.Dia <= ffin_)
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo
                                        }).ToList();

                        if (porSku == false) //Consulta no agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia.Year, t.dia } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = tt.Key.dia.ToString("dd/MM/yyyy"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else //Consulta agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              turno = turno,
                                                              unidades = ff,
                                                              hora = 0,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }

                    }
                    else if (filtro == "hora")
                    {
                        //fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm tt"));
                        //ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm tt"));

                        //Caza01
                        if (turno == null)
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaP = ctx.IOT_Conciliado.Where(w => w.Tiempo > fini_ && w.Tiempo <= ffin_ && w.Valor > 0).ToList();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => w.Tiempo > fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            }
                        }
                        else
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaP = ctx.IOT_Conciliado.Where(w => w.Tiempo > fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_turno == turno).ToList();
                            }
                            else
                            {
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => w.Tiempo > fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        var totales0 = (from sel in ConsultaP //ctx.IOT_Conciliado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        //where sel.Nhora > fini_.Hour
                                        //where sel.Valor > 0 //&& //sel.Planificado == null 
                                        //&& (sel.Tiempo >= fini_ && sel.Tiempo <= ffin_) &&
                                        //DateTime.Parse(sel.Dia.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00")) >= fini_ &&
                                        //DateTime.Parse(sel.Dia.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00")) <= ffin_
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Tiempo.Hour,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            tiempo = sel.Tiempo,
                                            cod_producto = sel.Sku_activo,
                                            nhora = sel.Nhora,
                                            ttiempo = DateTime.Parse(sel.Tiempo.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00"))
                                        }).OrderBy(o => o.tiempo).ToList();

                        //totales0 = totales0.Where(w => w.ttiempo >= fini_ && w.ttiempo <= ffin_).ToList();
                        //DateTime fec01 = DateTime.Parse("2021-08-17 00:00");
                        //DateTime fec02 = DateTime.Parse("2021-08-17 23:00");                        
                        //DateTime f1 = DateTime.Parse("2021-08-17");

                        if (porSku == false)
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia, t.nhora } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = tt.Min(m => m.tiempo).ToString("dd/MM/yyyy"),
                                                              hora = tt.Key.nhora, //tt.Min(m => m.tiempo.Hour),
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).ToList();

                            return totalesX;
                        }
                        else
                        {
                            //Por aquí
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              fecha = tt.Min(m => m.tiempo).ToString("dd/MM/yyyy"),
                                                              hora = tt.Min(m => m.tiempo.Hour),
                                                              turno = turno,
                                                              unidades = ff,
                                                              dia = tt.Max(m => m.dia),
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();



                            return totalesX;
                        }


                    }
                }
                else
                {
                    if (filtro != "semana") //Si el filtro es diferente a la semana
                    {
                        if (turno == null)
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0).ToList();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            }
                        }
                        else
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_turno == turno).ToList();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => w.Dia >= fini_ && w.Dia <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                            }
                        }
                    }

                    if (filtro == "mes")
                    {
                        var totales0 = (from sel in ConsultaPO //ctx.IOT_Conciliado_optimizado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null
                                                && (sel.Dia >= fini_ && sel.Dia <= ffin_)
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo
                                        }).ToList();

                        if (porSku == false) //Consulta no agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia.Year, t.dia.Month } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = ConvierteFecha(tt.Max(m => m.dia), "mes"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              fecha = ConvierteFecha(tt.Max(m => m.dia), "mes"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }




                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (cod_activo == null)
                        {
                            //ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia.Year == fini_.Year && w.Semana >= sini) || (w.Dia.Year == ffin_.Year && w.Semana <= sfin) && w.Valor > 0).ToList();
                            //ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Valor > 0).ToList();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                      where (c.Dia >= f1 && c.Dia <= f2) && c.Valor > 0 //&& c.Tiempo > Inicio
                                                      select new IOT_Conciliado_Semanas
                                                      {
                                                          Cod_activo = c.Cod_activo,
                                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                          Dia = c.Dia,
                                                          Cod_plan = c.Cod_plan,
                                                          Cod_turno = c.Cod_turno,
                                                          IOT_id = c.IOT_id,
                                                          Marca = c.Marca,
                                                          Minutos = c.Minutos,
                                                          Ndia = c.Ndia,
                                                          Nhora = c.Nhora,
                                                          Planificado = c.Planificado,
                                                          Semana = c.Semana,
                                                          Sku_activo = c.Sku_activo,
                                                          Tiempo = c.Tiempo,
                                                          Valor = c.Valor,
                                                          Variable = c.Variable,
                                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                      }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Valor > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        }
                        else
                        {
                            //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia.Year == fini_.Year && w.Semana >= sini) || (w.Dia.Year == ffin_.Year && w.Semana <= sfin) && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                      where (c.Dia >= f1 && c.Dia <= f2) && c.Valor > 0 && c.Cod_activo == cod_activo //&& c.Tiempo > Inicio
                                                      select new IOT_Conciliado_Semanas
                                                      {
                                                          Cod_activo = c.Cod_activo,
                                                          Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                          Dia = c.Dia,
                                                          Cod_plan = c.Cod_plan,
                                                          Cod_turno = c.Cod_turno,
                                                          IOT_id = c.IOT_id,
                                                          Marca = c.Marca,
                                                          Minutos = c.Minutos,
                                                          Ndia = c.Ndia,
                                                          Nhora = c.Nhora,
                                                          Planificado = c.Planificado,
                                                          Semana = c.Semana,
                                                          Sku_activo = c.Sku_activo,
                                                          Tiempo = c.Tiempo,
                                                          Valor = c.Valor,
                                                          Variable = c.Variable,
                                                          Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                      }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_activo == cod_activo && c.Valor > 0
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        }

                        var totales0 = (from sel in ConsultaPSemana //ctx.IOT_Conciliado_optimizado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null 
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            semana = sel.Semana,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo,
                                            anno = sel.Semana == 53 ? f1.Year : sel.Dia.Year
                                        }).ToList();

                        if (porSku == false)
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.anno, t.semana } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = FormatoSemana(tt.Key.semana, tt.Key.anno),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              turno = turno,
                                                              unidades = ff,
                                                              hora = 0,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }


                    }
                    else if (filtro == "dia")
                    {
                        var totales0 = (from sel in ConsultaPO //ctx.IOT_Conciliado_optimizado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        where sel.Valor > 0 //&& sel.Planificado == null 
                                        && (sel.Dia >= fini_ && sel.Dia <= ffin_)
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Dia.Month,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            cod_producto = sel.Sku_activo
                                        }).ToList();

                        if (porSku == false) //Consulta no agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia.Year, t.dia } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = tt.Key.dia.ToString("dd/MM/yyyy"),
                                                              hora = 0,
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).OrderBy(o => o.cod_activo).ToList();

                            return totalesX;
                        }
                        else //Consulta agrupada por SKU
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              turno = turno,
                                                              unidades = ff,
                                                              hora = 0,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }

                    }
                    else if (filtro == "hora")
                    {
                        if (turno == null)
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => w.Tiempo >= fini_ && w.Tiempo <= ffin_ && w.Valor > 0).ToList();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => w.Tiempo >= fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo).ToListAsync();
                            }
                        }
                        else
                        {
                            //Se hace esta consulta para diferenciar cuando esta filtrado por producto y cuando no
                            if (cod_activo == null)
                            {
                                ConsultaPO = ctx.IOT_Conciliado_optimizado.Where(w => w.Tiempo >= fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_turno == turno).ToList();
                            }
                            else
                            {
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => w.Tiempo >= fini_ && w.Tiempo <= ffin_ && w.Valor > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                            }
                        }

                        var totales0 = (from sel in ConsultaPO //ctx.IOT_Conciliado_optimizado
                                        join act in ctx.Activos on sel.Cod_activo equals act.Cod_activo
                                        //where sel.Valor > 0 //&& //sel.Planificado == null 
                                        //&& (sel.Tiempo >= fini_ && sel.Tiempo <= ffin_) &&
                                        //DateTime.Parse(sel.Dia.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00")) >= fini_ &&
                                        //DateTime.Parse(sel.Dia.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00")) <= ffin_
                                        select new
                                        {
                                            enTurno = sel.Cod_turno != null ? sel.Valor : 0,
                                            sinTurno = sel.Cod_turno == null ? sel.Valor : 0,
                                            nombreActivo = act.Cod_activo + " - " + act.Des_activo,
                                            dia = sel.Dia,
                                            fecha = sel.Tiempo.Hour,
                                            total = sel.Valor,
                                            cod_activo = sel.Cod_activo + " - " + act.Des_activo,
                                            tiempo = sel.Tiempo,
                                            cod_producto = sel.Sku_activo,
                                            nhora = sel.Nhora,
                                            ttiempo = DateTime.Parse(sel.Tiempo.ToString("dd/MM/yyyy") + ((sel.Nhora < 10 ? " 0" + sel.Nhora.ToString() : " " + sel.Nhora.ToString()) + ":00"))
                                        }).ToList();

                        totales0 = totales0.Where(w => w.ttiempo >= fini_ && w.ttiempo <= ffin_).ToList();

                        if (porSku == false)
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_activo, t.dia, t.nhora } into tt
                                                          select new Consolidado
                                                          {
                                                              cod_activo = tt.Key.cod_activo,
                                                              enTurnox = tt.Sum(s => s.enTurno),
                                                              sinTurno = tt.Sum(s => s.sinTurno),
                                                              nombreActivo = tt.Max(m => m.nombreActivo),
                                                              fecha = tt.Min(m => m.tiempo).ToString("dd/MM/yyyy"),
                                                              hora = tt.Min(m => m.tiempo.Hour),
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total)
                                                          }).ToList();

                            return totalesX;
                        }
                        else
                        {
                            List<Consolidado> totalesX = (from t in totales0
                                                          group t by new { t.cod_producto } into tt
                                                          select new Consolidado
                                                          {
                                                              fecha = tt.Min(m => m.tiempo).ToString("dd/MM/yyyy"),
                                                              hora = tt.Min(m => m.tiempo.Hour),
                                                              turno = turno,
                                                              unidades = ff,
                                                              total = tt.Sum(s => s.total),
                                                              cod_producto = (tt.Key.cod_producto == null || tt.Key.cod_producto == "") ?
                                                                                "Sin registro: " : tt.Key.cod_producto
                                                          }).OrderBy(o => o.cod_producto).ToList();

                            return totalesX;
                        }


                    }
                }

                return Consol;
            }
        }

        #endregion Calculos nuevos optimizados fin ##############################################################################################################

        public List<fechasIniciales> fechas_iniciales(DateTime Dfini)
        {
            List<fechasIniciales> fi = new List<fechasIniciales>();

            fi.Add(new fechasIniciales { 
                fini = PrimerDía(Dfini).AddDays(-7),
                ffin = PrimerDía(Dfini).AddDays(-1)
            });

            return fi;
        }

        public async Task<List<SeriesIndicadoresDR>> IndicadorDisponibilidad(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, bool inicio, bool sw01)
        {
            if (inicio == true)
            {
                fini_ = PrimerDía(fini_).AddDays(-7);
                ffin_ = fini_.AddDays(6);
            }

            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            string[] act = new string[0];

            List<SeriesIndicadoresDR> series = new List<SeriesIndicadoresDR>();
            List<DatosRetorno> Disponibilidad = await ConsultaDisponibilidadX(idEmpresa, fini_, ffin_, filtro, turno, sku, act, sw01);
            Disponibilidad = Disponibilidad.OrderBy(o => o.Dia).ToList();

            var fechas = (from d in Disponibilidad
                          group d by new { d.Leyenda } into dd
                          select new
                          {
                              Leyenda = dd.Key.Leyenda,
                              Dia = dd.Max(m => m.Dia),
                              Tiempo = dd.Max(m => m.Tiempo),
                              Hora = dd.Max(m => m.Hora)
                          }).ToList();
            
            int conteo = 1;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                foreach (var f in fechas)
                {
                    var act0 = ctx.Activos.ToList();

                    foreach (var ac in act0)
                    {
                        var Existe = Disponibilidad.Where(w => w.Cod_activo == ac.Cod_activo && w.Leyenda == f.Leyenda).Count();

                        if (Existe == 0)
                        {
                            //Si no existe se agrega el registro a Disponibilidad
                            Disponibilidad.Add(new DatosRetorno()
                            {
                                id = 0,
                                Agrupacion = null,
                                Cod_activo = ac.Cod_activo,
                                Dia = f.Dia,
                                Tiempo = f.Tiempo,
                                Leyenda = f.Leyenda,
                                To = 0,
                                Top = 0,
                                UnidadesP = 0,
                                Hora = f.Hora
                            });
                        }
                        
                    }
                }



                var activos = (from d in Disponibilidad
                               join a in ctx.Activos on d.Cod_activo equals a.Cod_activo
                               group d by new { d.Cod_activo, a.Des_activo } into dd
                               orderby dd.Max(m => m.Tiempo)
                               select new
                               {
                                   cod_activo = dd.Key.Cod_activo,
                                   des_activo = dd.Key.Des_activo,
                                   tiempo = dd.Max(m => m.Tiempo)
                               }).OrderBy(o => o.tiempo).ToList();                          

                foreach (var d in activos)
                {
                    series.Add(new SeriesIndicadoresDR()
                    {
                        id = conteo,
                        fecha = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Tiempo.ToString()).ToArray(),
                        tiempo = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Leyenda).ToArray(),
                        data = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Top == 0 ? 0 : decimal.Parse(((s.To / s.Top) * 100).ToString("N2"))).ToArray(),
                        activo = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                        cod_plan = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                        sku = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                        nombreActivo = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Cod_activo + " - " + d.des_activo).FirstOrDefault(),
                        hora = Disponibilidad.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Hora).ToArray(),
                        filtro = ff
                    });

                    conteo++;
                }

                var total_planta = (from d in Disponibilidad
                                    //where (d.To > 0 && d.Top >0)
                                    group d by new { d.Leyenda } into dd
                                    //orderby dd.Max(m => m.Tiempo.Year), dd.Key.Leyenda
                                    orderby dd.Max(m => m.Tiempo)
                                    select new { 
                                        Tiempo = dd.Max(m => m.Tiempo),
                                        Cod_activo = dd.Max(m => m.Cod_activo),
                                        Leyenda = dd.Max(m => m.Leyenda),
                                        To = dd.Sum(s => s.To),
                                        Top = dd.Sum(s => s.Top),
                                        Dia = dd.Max(m => m.Dia),
                                        Hora = dd.Max(m => m.Hora)
                                    }).OrderBy(o => o.Tiempo).ToList();

                //Aquí agrego el total de la planta
                series.Add(new SeriesIndicadoresDR()
                {
                    id = conteo,
                    fecha = total_planta.OrderBy(o => o.Dia).Select(s => s.Tiempo.ToString()).ToArray(),
                    tiempo = total_planta.OrderBy(o => o.Dia).Select(s => s.Leyenda).ToArray(),
                    data = total_planta.OrderBy(o => o.Dia).Select(s => s.Top == 0 ? 0 : decimal.Parse(((s.To / s.Top) * 100).ToString("N2"))).ToArray(),
                    activo = total_planta.OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                    cod_plan = total_planta.OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                    sku = total_planta.OrderBy(o => o.Dia).Select(s => s.Cod_activo).ToArray(),
                    hora = total_planta.OrderBy(o => o.Dia).Select(s => s.Hora).ToArray(),
                    nombreActivo = "Planta",
                    filtro = ff
                });

            }

           return series;
        }

        public async Task<List<SeriesIndicadoresDR>> IndicadorRendimiento(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, bool inicio, bool sw01)
        {
            if (inicio == true)
            {
                fini_ = PrimerDía(fini_).AddDays(-7);
                ffin_ = fini_.AddDays(6);
            }

            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            string[] act = new string[0];

            List<SeriesIndicadoresDR> series = new List<SeriesIndicadoresDR>();
            List<DatosRetorno> Rendimiento = await ConsultaRendimientoX(idEmpresa, fini_, ffin_, filtro, turno, sku, act, sw01);
            
            int conteo = 1;

            var fechas = (from d in Rendimiento
                          group d by new { d.Leyenda } into dd
                          select new
                          {
                              Leyenda = dd.Key.Leyenda,
                              Dia = dd.Max(m => m.Dia),
                              Tiempo = dd.Max(m => m.Tiempo),
                              Hora = dd.Max(m => m.Hora)
                          }).ToList();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                foreach (var f in fechas)
                {
                    var act0 = ctx.Activos.ToList();

                    foreach (var ac in act0)
                    {
                        var Existe = Rendimiento.Where(w => w.Cod_activo == ac.Cod_activo && w.Leyenda == f.Leyenda).Count();

                        if (Existe == 0)
                        {
                            //Si no existe se agrega el registro a Disponibilidad
                            Rendimiento.Add(new DatosRetorno()
                            {
                                id = 0,
                                Agrupacion = null,
                                Cod_activo = ac.Cod_activo,
                                Dia = f.Dia,
                                Tiempo = f.Tiempo,
                                Leyenda = f.Leyenda,
                                To = 0,
                                Top = 0,
                                UnidadesP = 0,
                                Hora = f.Hora
                            });

                        }

                    }
                }

                Rendimiento = Rendimiento.OrderBy(o => o.Tiempo.Year).ThenBy(t => t.Leyenda).ToList();

                var activos = (from d in Rendimiento
                               join a in ctx.Activos on d.Cod_activo equals a.Cod_activo
                               group d by new { d.Cod_activo, a.Des_activo } into dd
                               select new
                               {
                                   cod_activo = dd.Key.Cod_activo,
                                   des_activo = dd.Key.Des_activo,
                                   tiempo = dd.Max(m => m.Tiempo)
                               }).OrderBy(o => o.tiempo).ToList();


                foreach (var d in activos)
                {
                    series.Add(new SeriesIndicadoresDR()
                    {
                        id = conteo,
                        fecha = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Tiempo.ToString()).ToArray(),
                        tiempo = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Leyenda).ToArray(),
                        data = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Top == 0 ? 0 : decimal.Parse(((s.Top / s.To) * 100).ToString("N2"))).ToArray(),
                        activo = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                        cod_plan = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                        sku = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                        nombreActivo = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Tiempo).Select(s => s.Cod_activo + " - " + d.des_activo).FirstOrDefault(),
                        hora = Rendimiento.Where(w => w.Cod_activo == d.cod_activo).OrderBy(o => o.Dia).Select(s => s.Hora).ToArray(),
                        filtro = ff
                    });

                    conteo++;
                }

                var total_planta = (from d in Rendimiento
                                    //where (d.To > 0 && d.Top > 0)
                                    group d by new { d.Leyenda } into dd
                                    orderby dd.Max(m => m.Tiempo.Year), dd.Key.Leyenda
                                    select new
                                    {
                                        Tiempo = dd.Max(m => m.Tiempo),
                                        Cod_activo = dd.Max(m => m.Cod_activo),
                                        Leyenda = dd.Max(m => m.Leyenda),
                                        To = dd.Sum(s => s.To),
                                        Top = dd.Sum(s => s.Top),
                                        Dia = dd.Max(m => m.Dia),
                                        Hora = dd.Max(m => m.Hora)
                                    }).OrderBy(o => o.Tiempo).ToList();

                //Aquí agrego el total de la planta
                series.Add(new SeriesIndicadoresDR()
                {
                    id = conteo,
                    fecha = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Tiempo.ToString()).ToArray(),
                    tiempo = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Leyenda).ToArray(),
                    data = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Top == 0 ? 0 : decimal.Parse(((s.Top / s.To) * 100).ToString("N2"))).ToArray(),
                    activo = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                    cod_plan = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                    sku = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Cod_activo).ToArray(),
                    hora = total_planta.OrderBy(o => o.Tiempo).Select(s => s.Hora).ToArray(),
                    nombreActivo = "Planta",
                    filtro = ff
                });

            }

            return series;
        }

        //Metodo usado para cuando se inicia la vista de OEE para los indicadores que reflejan la disponibilidad y rendimiento generales 
        public async Task<Object> IndicadorInicio(int idEmpresa, DateTime Dfini, DateTime Dffin, bool inicio, string turno, string sku, string filtro, string[] act, bool sw01)
        {
            decimal toD = 0, toR = 0;
            decimal topD = 0, topR = 0;

            if (inicio == true)
            {
                Dfini = PrimerDía(Dfini).AddDays(-7);
                Dffin = Dfini.AddDays(6);
            }

            List<DatosRetorno> Disponibilidad = await ConsultaDisponibilidadX(idEmpresa, Dfini, Dffin, filtro, turno, sku, act, sw01);
            List<DatosRetorno> Rendimiento    = await ConsultaRendimientoX(idEmpresa, Dfini, Dffin, filtro, turno, sku, act, sw01);

            
            toD  = Disponibilidad.Sum(s => s.To);
            topD = Disponibilidad.Sum(s => s.Top) == 0 ? 1 : Disponibilidad.Sum(s => s.Top);
            toR  = Rendimiento.Sum(s => s.To) == 0 ? 1 : Rendimiento.Sum(s => s.To);
            topR = Rendimiento.Sum(s => s.Top);

            decimal disponibilidadTotal = decimal.Parse(((toD / topD) * 100).ToString("N2"));

            decimal rendimientoTotal = decimal.Parse(((topR / toR) * 100).ToString("N2"));
            decimal oee = decimal.Parse((((toD / topD) * (topR / toR)) * 100).ToString("N2"));

            List<IndicadoresTot> it = new List<IndicadoresTot> { new IndicadoresTot { Disp = disponibilidadTotal, Rend = rendimientoTotal, Oee = oee, Fecha = Dfini, Agrupacion = "Todo" } };

            return it.ToArray(); //GetDisponibilidad(idEmpresa, DateTime.Now, DateTime.Now, "", "", null); 
        }

        //Metodo usado para cuando se inicia la vista de OEE o se aplican algunos de los filtros pero para el gáfico de barras 
        public async Task<Object> IndicadorAgrupadoOEE(int idEmpresa, DateTime Dfini, DateTime Dffin, bool inicio, string turno, string sku, string filtro, string[] act, bool sw01)
        {
            List<DatosRetorno> d = new List<DatosRetorno>();
            List<IndicadoresTot> iAgrup = new List<IndicadoresTot>();

            if (inicio == true)
            {
                Dfini = PrimerDía(Dfini).AddDays(-7);
                Dffin = Dfini.AddDays(6);
            }

            List<DatosRetorno> Disponibilidad = await ConsultaDisponibilidadX(idEmpresa, Dfini, Dffin, filtro, turno, sku, act, sw01);
            List<DatosRetorno> Rendimiento    = await ConsultaRendimientoX(idEmpresa, Dfini, Dffin, filtro, turno, sku, act, sw01);

            //Agrego los registros de disponibilidad o rendimiento según lo que falte para cada uno           
            
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //Disponibilidad

                var fechas0 = (from dis in Disponibilidad
                              group dis by new { dis.Leyenda } into dd
                              select new
                              {
                                  Leyenda = dd.Key.Leyenda,
                                  Dia = dd.Max(m => m.Dia),
                                  Tiempo = dd.Max(m => m.Tiempo)
                              }).ToList();

                foreach (var f in fechas0)
                {
                    var act0 = ctx.Activos.ToList();

                    foreach (var ac in act0)
                    {
                        var Existe = Disponibilidad.Where(w => w.Cod_activo == ac.Cod_activo && w.Leyenda == f.Leyenda).Count();

                        if (Existe == 0)
                        {
                            //Si no existe se agrega el registro a Disponibilidad
                            Disponibilidad.Add(new DatosRetorno()
                            {
                                id = 0,
                                Agrupacion = null,
                                Cod_activo = ac.Cod_activo,
                                Dia = f.Dia,
                                Tiempo = f.Tiempo,
                                Leyenda = f.Leyenda,
                                To = 0,
                                Top = 0,
                                UnidadesP = 0
                            });
                        }

                    }
                }

                //Rendimiento

                var fechas1 = (from ren in Rendimiento
                              group ren by new { ren.Leyenda } into dd
                              select new
                              {
                                  Leyenda = dd.Key.Leyenda,
                                  Dia = dd.Max(m => m.Dia),
                                  Tiempo = dd.Max(m => m.Tiempo)
                              }).ToList();

                foreach (var f in fechas1)
                {
                    var act0 = ctx.Activos.ToList();

                    foreach (var ac in act0)
                    {
                        var Existe = Rendimiento.Where(w => w.Cod_activo == ac.Cod_activo && w.Leyenda == f.Leyenda).Count();

                        if (Existe == 0)
                        {
                            //Si no existe se agrega el registro a Disponibilidad
                            Rendimiento.Add(new DatosRetorno()
                            {
                                id = 0,
                                Agrupacion = null,
                                Cod_activo = ac.Cod_activo,
                                Dia = f.Dia,
                                Tiempo = f.Tiempo,
                                Leyenda = f.Leyenda,
                                To = 0,
                                Top = 0,
                                UnidadesP = 0
                            });

                        }

                    }
                }

            }

            d = (from dis in Disponibilidad
                 where dis.Top > 0
                 group dis by new { dis.Leyenda } into dd
                 select new DatosRetorno
                 {
                     To = (dd.Sum(s => s.To) / dd.Sum(s => s.Top)),
                     Leyenda = dd.Key.Leyenda,
                     Agrupacion = "D",
                     Tiempo = dd.Max(m => m.Tiempo)
                 }).Union(from ren in Rendimiento
                          where ren.To > 0
                          group ren by new { ren.Leyenda } into rr
                          select new DatosRetorno
                          {
                              To = (rr.Sum(s => s.Top) / rr.Sum(s => s.To)),
                              Leyenda = rr.Key.Leyenda,
                              Agrupacion = "R",
                              Tiempo = rr.Max(m => m.Tiempo)
                          }).ToList();

            //d = d.OrderBy(o => o.Agrupacion).ThenBy(t => t.Tiempo);

            iAgrup = (from tot in d
                      group tot by new { tot.Leyenda } into tt
                      select new IndicadoresTot
                      {
                          Disp = decimal.Parse((tt.Where(w => w.Agrupacion == "D").Sum(s => s.To) * 100).ToString("N2")),
                          Rend = decimal.Parse((tt.Where(w => w.Agrupacion == "R").Sum(s => s.To) * 100).ToString("N2")),
                          Oee = decimal.Parse(((tt.Where(w => w.Agrupacion == "D").Sum(s => s.To) * tt.Where(w => w.Agrupacion == "R").Sum(s => s.To)) * 100).ToString("N2")),
                          Agrupacion = tt.Max(m => m.Leyenda),
                          Fecha = tt.Max(m => m.Tiempo)
                      }).OrderBy(o => o.Fecha).ToList();

            return iAgrup.ToArray();
        }

        public async Task<List<DatosRetornoVelocidadProductividad>> IndicadorProductividadVelocidad(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string variable, string sku, string cod_activo, string verTiempo, bool sw01)
        {
            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");
            List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPO = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();
            List<DatosRetornoVelocidadProductividad> seleccion = new List<DatosRetornoVelocidadProductividad>();

            string Medida = verTiempo;
            verTiempo = (verTiempo == "Min" ? "Minutos" : "Horas");

            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (sw01 == false)
                {
                    if (filtro == "mes")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();

                        //var diasTrabajadosMes = (from dt in ConsultaP
                        //                         join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //                         where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini && dt.Cod_turno == te.Cod_turno
                        //                         group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Dia.Month } into dtt
                        //                         select new
                        //                         {
                        //                             dia = dtt.Key.Dia,
                        //                             valor = dtt.Sum(s => s.Valor),
                        //                             turno = dtt.Max(m => m.Cod_turno),
                        //                             cod_activo = dtt.Key.Cod_activo,
                        //                             mes = dtt.Key.Month,
                        //                             minutos = dtt.Max(s => s.Minutos)
                        //                         }).Where(w => w.turno != null).ToList();

                        var TTT = (from dt in ConsultaP
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Dia.Month, dt.Cod_turno } into dd
                                   orderby dd.Key.Dia
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Mes = dd.Key.Month,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Mes = h.Mes,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        TTT = (from h in TTT
                               group h by new { h.Mes } into d
                               select new
                               {
                                   Dia = DateTime.Now,
                                   Mes = d.Key.Mes,
                                   Cod_turno = d.Max(m => m.Cod_turno),
                                   Valor = d.Sum(s => s.Valor) * 60
                               }).ToList();

                        seleccion = (from iotX in ConsultaP
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                     select new DatosRetornoVelocidadProductividad
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         Velocidad = (TTT.Where(w => w.Mes == mm.Key.Month).Sum(s => s.Valor))

                                                         == 0 ? 0 :

                                                         //Unidades producidas en el tiempo
                                                         (
                                                         ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Sum(s => s.Valor)
                                                         /
                                                         (decimal)TTT.Where(w => w.Mes == mm.Key.Month).Sum(s => s.Valor)

                                                         ) * 60,

                                         #region Antigua velocidad
                                         //Velocidad = (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia.Month == mm.Key.Month).Max(m => m.Minutos)
                                         //            * (diasTrabajadosMes.Where(w => w.cod_activo == mm.Key.Cod_activo && w.mes == mm.Key.Month).Count())
                                         //            -
                                         //            //Planificados
                                         //            (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                         //            w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia.Month == mm.Key.Month)).Count()))

                                         //            == 0 ? 0 :

                                         //            //Unidades producidas en el tiempo
                                         //            (ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                                         //            (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Sum(s => s.Valor)
                                         //            /
                                         //            //tiempo,
                                         //            (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Dia.Month == mm.Key.Month).Max(m => m.Minutos)
                                         //            * (diasTrabajadosMes.Where(w => w.cod_activo == mm.Key.Cod_activo && w.mes == mm.Key.Month).Count())
                                         //            -
                                         //            //Planificados
                                         //            (ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                         //            w.Minutos > 0 && (w.Dia.Year == mm.Key.Year && w.Dia.Month == mm.Key.Month)).Count()))) * 60,
                                         #endregion

                                         Productividad = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count()

                                                         == 0 ? 0 :

                                                         (ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Sum(s => s.Valor) /
                                                         ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count()) * 60,

                                         Filtro = ff,
                                         Turno = turno,
                                         Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                         VerTiempo = verTiempo,
                                         Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")
                                     }).ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();
                        ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                 where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Cod_activo == cod_activo && c.Tiempo > Inicio
                                                 select new IOT_Conciliado_Semanas
                                                 {
                                                     Cod_activo = c.Cod_activo,
                                                     Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                     Dia = c.Dia,
                                                     Cod_plan = c.Cod_plan,
                                                     Cod_turno = c.Cod_turno,
                                                     IOT_id = c.IOT_id,
                                                     Marca = c.Marca,
                                                     Minutos = c.Minutos,
                                                     Ndia = c.Ndia,
                                                     Nhora = c.Nhora,
                                                     Planificado = c.Planificado,
                                                     Semana = c.Semana,
                                                     Sku_activo = c.Sku_activo,
                                                     Tiempo = c.Tiempo,
                                                     Valor = c.Valor,
                                                     Variable = c.Variable,
                                                     Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                 }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Cod_activo == cod_activo
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();


                        //var diasTrabajadosSemana = (from dt in ConsultaPSemana
                        //                            join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //                            where dt.Cod_turno != null && dt.Dia == te.Fecha_ini && dt.Cod_turno == te.Cod_turno
                        //                            //&& (dt.Semana >= sini && dt.Semana <= sfin)
                        //                            group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno, dt.Semana } into dtt
                        //                            select new
                        //                            {
                        //                                dia = dtt.Key.Dia,
                        //                                valor = dtt.Sum(s => s.Valor),
                        //                                turno = dtt.Max(m => m.Cod_turno),
                        //                                cod_activo = dtt.Key.Cod_activo,
                        //                                semana = dtt.Key.Semana
                        //                            }).Where(w => w.turno != null ///&& w.valor > 0
                        //                            ).ToList();

                        var TTT = (from dt in ConsultaPSemana
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Semana, dt.Cod_turno } into dd
                                   orderby dd.Key.Semana
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Semana = dd.Key.Semana,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Semana = h.Semana,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        try
                        {
                            TTT = (from h in TTT
                                   group h by new { h.Semana } into d
                                   select new
                                   {
                                       Dia = DateTime.Now,
                                       Semana = d.Key.Semana,
                                       Cod_turno = d.Max(m => m.Cod_turno),
                                       Valor = d.Sum(s => s.Valor) * 60
                                   }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }


                        try
                        {
                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetornoVelocidadProductividad
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),
                                             Velocidad = (TTT.Where(w => w.Semana == mm.Key.Semana).Sum(s => s.Valor))

                                                         == 0 ? 0 :

                                                         //Unidades producidas en el tiempo
                                                         (
                                                         ConsultaPSemana.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Sum(s => s.Valor)
                                                         /
                                                         (decimal)TTT.Where(w => w.Semana == mm.Key.Semana).Sum(s => s.Valor)

                                                         ) * 60,

                                             #region Velocidad Antigua
                                             //Velocidad = (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Semana == mm.Key.Semana).Max(m => m.Minutos)
                                             //            * (diasTrabajadosSemana.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Count())
                                             //            -
                                             //            //Planificados
                                             //            (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                             //            w.Minutos > 0 && (w.Semana == mm.Key.Semana)).Count())) 

                                             //            == 0 ? 0 :

                                             //            //Unidades producidas en el tiempo
                                             //            (ConsultaPSemana.Where(w => w.Planificado == null && w.Valor > 0 &&
                                             //            (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Sum(s => s.Valor)
                                             //            /
                                             //            //tiempo,
                                             //            (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Semana == mm.Key.Semana).Max(m => m.Minutos)
                                             //            * (diasTrabajadosSemana.Where(w => w.cod_activo == mm.Key.Cod_activo && w.semana == mm.Key.Semana).Count())
                                             //            -
                                             //            //Planificados
                                             //            (ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == true && w.Cod_turno != null &&
                                             //            w.Minutos > 0 && (w.Semana == mm.Key.Semana)).Count()))) * 60,
                                             #endregion

                                             Productividad = ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count()

                                                             == 0 ? 0 :

                                                             (ConsultaPSemana.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Sum(s => s.Valor) /
                                                             ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count()) * 60,

                                             Filtro = ff,
                                             Turno = turno,
                                             Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                             VerTiempo = verTiempo,
                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno)
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                    else if (filtro == "dia") //Solo aquí aplica el turno
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (turno == "Todos" || turno == null)
                        {
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();
                        }
                        else
                        {
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                        }

                        #region Calculos viejos
                        ////Calculo del tiempo total de trabajo en el día TTT

                        var TTT = (from dt in ConsultaP
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Cod_turno } into dd
                                   orderby dd.Key.Dia
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        TTT = (from h in TTT
                               group h by new { h.Dia } into d
                               select new
                               {
                                   Dia = d.Key.Dia,
                                   Cod_turno = d.Max(m => m.Cod_turno),
                                   Valor = d.Sum(s => s.Valor) * 60
                               }).ToList();
                        #endregion

                        //var diasTrabajadosDia = (from dt in ConsultaP
                        //                         join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //                         where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini &&
                        //                         dt.Cod_turno == te.Cod_turno && dt.Cod_activo == cod_activo
                        //                         group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno } into dtt
                        //                         select new
                        //                         {
                        //                             dia = dtt.Key.Dia,
                        //                             valor = dtt.Sum(s => s.Valor),
                        //                             turno = dtt.Max(m => m.Cod_turno),
                        //                             cod_activo = dtt.Key.Cod_activo
                        //                         }).Where(w => w.turno != null).ToList();

                        seleccion = (from iotX in ConsultaP
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetornoVelocidadProductividad
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         Velocidad = (TTT.Where(w => w.Dia == mm.Key.Dia).Sum(s => s.Valor))

                                                     == 0 ? 0 :

                                                     //Unidades producidas en el tiempo
                                                     (
                                                     ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                     (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Sum(s => s.Valor)
                                                     /
                                                     (decimal)TTT.Where(w => w.Dia == mm.Key.Dia).Sum(s => s.Valor)

                                                     ) * 60,


                                         Productividad = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count()

                                                         == 0 ? 0 :

                                                         (
                                                             ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                             (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Sum(s => s.Valor) /
                                                             ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count()
                                                         ) * 60
                                                         
                                                         ,

                                         Filtro = ff,
                                         Turno = turno,
                                         Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                         VerTiempo = verTiempo,
                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy")
                                     }).ToList();


                    }
                    else if (filtro == "hora")
                    {
                        fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();

                        var s0 = (from c in ConsultaP
                                  select new
                                  {
                                      Id = c.IOT_id,
                                      Tiempo = c.Tiempo,
                                      Cod_activo = c.Cod_activo,
                                      Dia = c.Dia,
                                      Nhora = c.Nhora,
                                      Cod_turno = c.Cod_turno,
                                      Leyenda = c.Tiempo.ToString("dd/MM/yyyy") + " " + (c.Nhora < 10 ? "0" + c.Nhora.ToString() : c.Nhora.ToString()) + ":00"
                                  }).ToList();

                        //var prueba0 = (from s1 in s0
                        //               where s1.Nhora == 23
                        //               select new
                        //               {
                        //                   Le = s1.Leyenda,
                        //                   Nh = s1.Nhora,
                        //                   Ti = s1.Tiempo
                        //               }).ToList();

                        //s0.Where(w => w.Nhora == 7).Select(s => s.Nhora).ToList();

                        var TTT = (from dt in s0
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   orderby dt.Cod_activo, dt.Tiempo
                                   group dt by new { dt.Leyenda, dt.Cod_activo, dt.Cod_turno } into dd
                                   orderby dd.Key.Leyenda
                                   select new
                                   {
                                       Hora = int.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("HH")),
                                       Dia = DateTime.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("dd/MM/yyyy")),
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = ConsultaP.Where(w => w.Nhora == int.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("HH")) && w.Cod_turno == dd.Key.Cod_turno &&
                                                               w.Dia.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == dd.Key.Leyenda).Count(),
                                       Leyenda = dd.Key.Leyenda
                                   }).ToList();

                        //var prueba1 = TTT.Where(w => w.Hora == 5).ToList();

                        try
                        {
                            seleccion = (from p in s0
                                         orderby DateTime.Parse(p.Leyenda)
                                         group p by new { p.Cod_activo, p.Leyenda } into mm
                                         select new DatosRetornoVelocidadProductividad
                                         {
                                             Cod_activo = mm.Key.Cod_activo,
                                             Velocidad =

                                                       //Unidades producidas en el tiempo
                                                       (decimal)TTT.Where(w => w.Hora == mm.Max(m => m.Nhora) && w.Leyenda == mm.Key.Leyenda).Sum(s => s.Valor) == 0 ? 0 :
                                                       ((

                                                        ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 //
                                                                                                                  //&& (w.Tiempo >= fini_ && w.Tiempo <= ffin_) //&& w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                       && w.Dia.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                       ).Sum(s => s.Valor)

                                                       /

                                                       (decimal)TTT.Where(w => w.Hora == mm.Max(m => m.Nhora) && w.Leyenda == mm.Key.Leyenda).Sum(s => s.Valor)

                                                       ) * 60
                                                       )
                                                       ,


                                             Productividad = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                             //&&  (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                             && w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                             ).Count()

                                                               == 0 ? 0 :

                                                               (
                                                               ConsultaP.Where(w => w.Planificado == null && w.Valor > 0
                                                               //&& (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                               && w.Dia.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                               ).Sum(s => s.Valor)

                                                               /

                                                               ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                               w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora))
                                                               //&& w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                               ).Count()
                                                           )
                                                           * 60
                                                           ,

                                             Filtro = ff,
                                             Turno = turno,
                                             Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                             VerTiempo = verTiempo,
                                             Leyenda = mm.Key.Leyenda,
                                             Tiempo = DateTime.Parse(mm.Key.Leyenda),
                                             Hora = int.Parse(DateTime.Parse(mm.Key.Leyenda).ToString("HH"))

                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }



                        //seleccion = (from iotX in ConsultaP
                        //             group iotX by new { iotX.Cod_activo, iotX.Dia, iotX.Nhora } into mm
                        //             select new DatosRetornoVelocidadProductividad
                        //             {
                        //                 id = mm.Max(m => m.IOT_id),
                        //                 Cod_activo = mm.Key.Cod_activo,
                        //                 Tiempo = mm.Max(m => m.Tiempo),

                        //                 Velocidad = (TTT.Where(w => w.Dia == mm.Key.Dia && w.Hora == mm.Key.Nhora).Sum(s => s.Valor))

                        //                             == 0 ? 0 :

                        //                             //Unidades producidas en el tiempo
                        //                             (ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                        //                             (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora).Sum(s => s.Valor)
                        //                             /(decimal)TTT.Where(w => w.Dia == mm.Key.Dia && w.Hora == mm.Key.Nhora).Sum(s => s.Valor)) * 60
                        //                             ,


                        //                 Productividad = ConsultaP.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                        //                                 (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora).Count()

                        //                                     == 0 ? 0 :

                        //                                     (
                        //                                     ConsultaP.Where(w => w.Planificado == null && w.Valor > 0 &&
                        //                                     (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora).Sum(s => s.Valor) /
                        //                                     ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                        //                                     w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Key.Dia && w.Nhora == mm.Key.Nhora)).Count()
                        //                                 ) *60
                        //                                 ,

                        //                 Filtro = ff,
                        //                 Turno = turno,
                        //                 Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                        //                 VerTiempo = verTiempo,
                        //                 Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy") + " " + mm.Min(m => m.Tiempo.ToString("HH:mm"))
                        //             }).ToList();


                    }
                }
                else
                {
                    if (filtro == "mes")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();

                        var TTT = (from dt in ConsultaPO
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Dia.Month, dt.Cod_turno } into dd
                                   orderby dd.Key.Dia
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Mes = dd.Key.Month,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Mes = h.Mes,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        TTT = (from h in TTT
                               group h by new { h.Mes } into d
                               select new
                               {
                                   Dia = DateTime.Now,
                                   Mes = d.Key.Mes,
                                   Cod_turno = d.Max(m => m.Cod_turno),
                                   Valor = d.Sum(s => s.Valor) * 60
                               }).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia.Month } into mm
                                     select new DatosRetornoVelocidadProductividad
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         Velocidad = (TTT.Where(w => w.Mes == mm.Key.Month).Sum(s => s.Valor))

                                                         == 0 ? 0 :

                                                         //Unidades producidas en el tiempo
                                                         (
                                                         ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Sum(s => s.Valor)
                                                         /
                                                         (decimal)TTT.Where(w => w.Mes == mm.Key.Month).Sum(s => s.Valor)

                                                         ) * 60,

                                         #region Antigua velocidad
                                         #endregion

                                         Productividad = ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count()

                                                         == 0 ? 0 :

                                                         (ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Sum(s => s.Valor) /
                                                         ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == mm.Key.Month).Count()) * 60,

                                         Filtro = ff,
                                         Turno = turno,
                                         Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                         VerTiempo = verTiempo,
                                         Leyenda = ConvierteFecha(mm.Max(m => m.Dia), "mes")
                                     }).ToList();

                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();
                        ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                 where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_turno != null && c.Minutos > 0 && c.Cod_activo == cod_activo && c.Tiempo > Inicio
                                                 select new IOT_Conciliado_Semanas
                                                 {
                                                     Cod_activo = c.Cod_activo,
                                                     Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                     Dia = c.Dia,
                                                     Cod_plan = c.Cod_plan,
                                                     Cod_turno = c.Cod_turno,
                                                     IOT_id = c.IOT_id,
                                                     Marca = c.Marca,
                                                     Minutos = c.Minutos,
                                                     Ndia = c.Ndia,
                                                     Nhora = c.Nhora,
                                                     Planificado = c.Planificado,
                                                     Semana = c.Semana,
                                                     Sku_activo = c.Sku_activo,
                                                     Tiempo = c.Tiempo,
                                                     Valor = c.Valor,
                                                     Variable = c.Variable,
                                                     Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                 }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_turno != null && c.Minutos > 0 && c.Cod_activo == cod_activo
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();

                        var TTT = (from dt in ConsultaPSemana
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Semana, dt.Cod_turno } into dd
                                   orderby dd.Key.Semana
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Semana = dd.Key.Semana,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Semana = h.Semana,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        try
                        {
                            TTT = (from h in TTT
                                   group h by new { h.Semana } into d
                                   select new
                                   {
                                       Dia = DateTime.Now,
                                       Semana = d.Key.Semana,
                                       Cod_turno = d.Max(m => m.Cod_turno),
                                       Valor = d.Sum(s => s.Valor) * 60
                                   }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }


                        try
                        {
                            seleccion = (from iotX in ConsultaPSemana
                                         group iotX by new { iotX.Cod_activo, iotX.Anno, iotX.Semana } into mm
                                         select new DatosRetornoVelocidadProductividad
                                         {
                                             id = mm.Max(m => m.IOT_id),
                                             Cod_activo = mm.Key.Cod_activo,
                                             Tiempo = mm.Max(m => m.Tiempo),
                                             Velocidad = (TTT.Where(w => w.Semana == mm.Key.Semana).Sum(s => s.Valor))

                                                         == 0 ? 0 :

                                                         //Unidades producidas en el tiempo
                                                         (
                                                         ConsultaPSemana.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                         (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Sum(s => s.Valor)
                                                         /
                                                         (decimal)TTT.Where(w => w.Semana == mm.Key.Semana).Sum(s => s.Valor)

                                                         ) * 60,

                                             #region Velocidad Antigua
                                             #endregion

                                             Productividad = ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count()

                                                             == 0 ? 0 :

                                                             (ConsultaPSemana.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Sum(s => s.Valor) /
                                                             ConsultaPSemana.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= f1 && w.Dia <= f2) && w.Semana == mm.Key.Semana).Count()) * 60,

                                             Filtro = ff,
                                             Turno = turno,
                                             Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                             VerTiempo = verTiempo,
                                             Leyenda = FormatoSemana(mm.Key.Semana, mm.Key.Anno)
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                    else if (filtro == "dia") //Solo aquí aplica el turno
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (turno == "Todos" || turno == null)
                        {
                            ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();
                        }
                        else
                        {
                            ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                        }

                        #region Calculos viejos
                        ////Calculo del tiempo total de trabajo en el día TTT

                        var TTT = (from dt in ConsultaPO
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Dia, dt.Cod_turno } into dd
                                   orderby dd.Key.Dia
                                   select new
                                   {
                                       Dia = dd.Key.Dia,
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = 0.00
                                   }).ToList();

                        TTT = (from h in TTT
                               join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                               select new
                               {
                                   Dia = h.Dia,
                                   Cod_turno = h.Cod_turno,

                                   Valor = DateTime.Parse((t.Hora_fin1.Hour < t.Hora_ini1.Hour ? DateTime.Now.AddDays(1) : DateTime.Now)
                                                       .ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                                                       .Subtract(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm"))).TotalHours
                               }).ToList();

                        TTT = (from h in TTT
                               group h by new { h.Dia } into d
                               select new
                               {
                                   Dia = d.Key.Dia,
                                   Cod_turno = d.Max(m => m.Cod_turno),
                                   Valor = d.Sum(s => s.Valor) * 60
                               }).ToList();
                        #endregion

                        //var diasTrabajadosDia = (from dt in ConsultaPO
                        //                         join te in ctx.Turnos_activos_extras on dt.Cod_activo equals te.Cod_activo
                        //                         where dt.Dia >= fini_ && dt.Dia <= ffin_ && dt.Cod_turno != null && dt.Dia == te.Fecha_ini &&
                        //                         dt.Cod_turno == te.Cod_turno && dt.Cod_activo == cod_activo
                        //                         group dt by new { dt.Cod_activo, dt.Dia, dt.Cod_turno } into dtt
                        //                         select new
                        //                         {
                        //                             dia = dtt.Key.Dia,
                        //                             valor = dtt.Sum(s => s.Valor),
                        //                             turno = dtt.Max(m => m.Cod_turno),
                        //                             cod_activo = dtt.Key.Cod_activo
                        //                         }).Where(w => w.turno != null).ToList();

                        seleccion = (from iotX in ConsultaPO
                                     group iotX by new { iotX.Cod_activo, iotX.Dia.Year, iotX.Dia } into mm
                                     select new DatosRetornoVelocidadProductividad
                                     {
                                         id = mm.Max(m => m.IOT_id),
                                         Cod_activo = mm.Key.Cod_activo,
                                         Tiempo = mm.Max(m => m.Tiempo),

                                         Velocidad = (TTT.Where(w => w.Dia == mm.Key.Dia).Sum(s => s.Valor))

                                                     == 0 ? 0 :

                                                     //Unidades producidas en el tiempo
                                                     (
                                                     ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                     (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Sum(s => s.Valor)
                                                     /
                                                     (decimal)TTT.Where(w => w.Dia == mm.Key.Dia).Sum(s => s.Valor)

                                                     ) * 60,


                                         Productividad = ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                         (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count()

                                                         == 0 ? 0 :

                                                         (
                                                             ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0 &&
                                                             (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Sum(s => s.Valor) /
                                                             ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                             (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == mm.Key.Dia).Count()
                                                         ) * 60,

                                         Filtro = ff,
                                         Turno = turno,
                                         Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                         VerTiempo = verTiempo,
                                         Leyenda = mm.Key.Dia.ToString("dd/MM/yyyy")
                                     }).ToList();


                    }
                    else if (filtro == "hora")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy HH:mm"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                        ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();

                        var s0 = (from c in ConsultaPO
                                  select new
                                  {
                                      Id = c.IOT_id,
                                      Tiempo = c.Tiempo,
                                      Cod_activo = c.Cod_activo,
                                      Dia = c.Dia,
                                      Nhora = c.Nhora,
                                      Cod_turno = c.Cod_turno,
                                      Leyenda = c.Tiempo.ToString("dd/MM/yyyy") + " " + (c.Nhora < 10 ? "0" + c.Nhora.ToString() : c.Nhora.ToString()) + ":00"
                                  }).ToList();

                        var prueba0 = (from s1 in s0
                                       where s1.Nhora == 23
                                       select new
                                       {
                                           Le = s1.Leyenda,
                                           Nh = s1.Nhora,
                                           Ti = s1.Tiempo
                                       }).ToList();

                        s0.Where(w => w.Nhora == 7).Select(s => s.Nhora).ToList();

                        var TTT = (from dt in s0
                                   join tu in ctx.Turnos on dt.Cod_turno equals tu.Cod_turno
                                   group dt by new { dt.Leyenda, dt.Cod_activo, dt.Cod_turno } into dd
                                   orderby dd.Key.Leyenda
                                   select new
                                   {
                                       Hora = int.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("HH")),
                                       Dia = DateTime.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("dd/MM/yyyy")),
                                       Cod_turno = dd.Key.Cod_turno,
                                       Valor = ConsultaPO.Where(w => w.Nhora == int.Parse(DateTime.Parse(dd.Key.Leyenda).ToString("HH")) && w.Cod_turno == dd.Key.Cod_turno &&
                                                               w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == dd.Key.Leyenda).Count(),
                                       Leyenda = dd.Key.Leyenda
                                   }).ToList();

                        var prueba1 = TTT.Where(w => w.Hora == 5).ToList();

                        try
                        {
                            seleccion = (from p in s0
                                         group p by new { p.Cod_activo, p.Leyenda } into mm
                                         select new DatosRetornoVelocidadProductividad
                                         {
                                             Cod_activo = mm.Key.Cod_activo,
                                             Velocidad =

                                                       //Unidades producidas en el tiempo
                                                       (decimal)TTT.Where(w => w.Hora == mm.Max(m => m.Nhora) && w.Leyenda == mm.Key.Leyenda).Sum(s => s.Valor) == 0 ? 0 :
                                                       ((

                                                        ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0 //
                                                                                                                   //&& (w.Tiempo >= fini_ && w.Tiempo <= ffin_) //&& w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                       && w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                       ).Sum(s => s.Valor)

                                                       /

                                                       (decimal)TTT.Where(w => w.Hora == mm.Max(m => m.Nhora) && w.Leyenda == mm.Key.Leyenda).Sum(s => s.Valor)

                                                       ) * 60
                                                       )
                                                       ,


                                             Productividad = ConsultaPO.Where(w => w.Cod_activo == mm.Key.Cod_activo && w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0
                                                             //&&  (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                             && w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                             ).Count()

                                                               == 0 ? 0 :

                                                               (
                                                               ConsultaPO.Where(w => w.Planificado == null && w.Valor > 0
                                                               //&& (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora)
                                                               && w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                               ).Sum(s => s.Valor) /

                                                               ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                               w.Cod_activo == mm.Key.Cod_activo && (w.Dia == mm.Max(m => m.Dia) && w.Nhora == mm.Max(m => m.Nhora))
                                                               //&& w.Tiempo.ToString("dd/MM/yyyy") + " " + (w.Nhora < 10 ? "0" + w.Nhora.ToString() : w.Nhora.ToString()) + ":00" == mm.Key.Leyenda
                                                               ).Count()
                                                           ) * 60
                                                           ,

                                             Filtro = ff,
                                             Turno = turno,
                                             Activo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault(),
                                             VerTiempo = verTiempo,
                                             Leyenda = mm.Key.Leyenda,
                                             Hora = int.Parse(DateTime.Parse(mm.Key.Leyenda).ToString("HH"))
                                         }).ToList();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
                

                int convierte = (Medida.Trim() == "Unidades por día" ? 24 : 1);

                seleccion = (from s in seleccion
                             select new DatosRetornoVelocidadProductividad
                             {
                                 id = s.id,
                                 Cod_activo = s.Cod_activo,
                                 Tiempo = s.Tiempo,
                                 Velocidad = decimal.Parse((s.Velocidad * convierte).ToString("N2")),
                                 Productividad = decimal.Parse((s.Productividad * convierte).ToString("N2")),
                                 Filtro = s.Filtro,
                                 Turno = s.Turno,
                                 Activo = s.Activo,
                                 VerTiempo = s.VerTiempo,
                                 Leyenda = s.Leyenda,
                                 Hora = s.Hora
                             }).ToList();

                return seleccion.ToList();
            }
            //return null;
        }

        public async Task<List<AnalisisTiempos>> IndicadorAnalisisTiempos(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string cod_activo, string variable, string verTiempo, bool inicio, bool sw01)
        {
            if (inicio == true)
            {
                fini_ = PrimerDía(fini_).AddDays(-7);
                ffin_ = fini_.AddDays(6);
            }

            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            string UnidadesX = verTiempo == "Hor" ? "Horas" : "Minutos";

            List<SeriesIndicadoresDR> series = new List<SeriesIndicadoresDR>();
            List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
            List<IOT_Conciliado_optimizado> ConsultaPO = new List<IOT_Conciliado_optimizado>();
            List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();
            List<AnalisisTiempos> at = new List<AnalisisTiempos>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (sw01 == false)
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));


                            if (cod_activo == null)
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();
                            else
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                            if (cod_activo != null)
                            {
                                at = (from c in ConsultaP
                                      where c.Cod_turno != null && c.Minutos > 0
                                      group c by new { c.Dia.Year, c.Dia.Month } into cc
                                      select new AnalisisTiempos
                                      {
                                          To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month).Count(),

                                          Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month)).Count(),

                                          Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month)).Count(),

                                          NoRegistrados = ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_turno != null).Count(),

                                          Filtro = ff,
                                          Unidades = UnidadesX,
                                          Cod_turno = turno,
                                          Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                          Leyenda = ConvierteFecha(cc.Max(m => m.Dia), "mes")

                                      }).ToList();
                            }
                            else
                            {
                                at = (from c in ConsultaP
                                      where c.Cod_turno != null && c.Minutos > 0
                                      group c by new { c.Cod_activo, c.Dia.Year, c.Dia.Month } into cc
                                      select new AnalisisTiempos
                                      {
                                          To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          NoRegistrados = ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Filtro = ff,
                                          Unidades = UnidadesX,
                                          Cod_turno = turno,
                                          Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                          Leyenda = ConvierteFecha(cc.Max(m => m.Dia), "mes")

                                      }).ToList();
                            }



                            //return at;

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (cod_activo == null)
                            //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2)).
                            //                    Union(ctx.IOT_Conciliado.Where(w => (w.Dia == f2.AddDays(1) && w.Semana == sfin))).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin)
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        else
                            //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_activo == cod_activo).
                            //                    Union(ctx.IOT_Conciliado.Where(w => (w.Dia == f2.AddDays(1) && w.Semana == sfin) && w.Cod_activo == cod_activo)).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_activo == cod_activo && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_activo == cod_activo
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                                                                }).ToListAsync();

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaPSemana
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Anno, c.Semana } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPSemana.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana).Count(),

                                      Tpp = ConsultaPSemana.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana)).Count(),

                                      Tpnp = ConsultaPSemana.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana)).Count(),

                                      NoRegistrados = ConsultaPSemana.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_turno != null).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = FormatoSemana(cc.Key.Semana, cc.Key.Anno)

                                  }).ToList();
                        }
                        else
                        {
                            at = (from c in ConsultaPSemana
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Cod_activo, c.Anno, c.Semana } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPSemana.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaPSemana.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaPSemana.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaPSemana.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = FormatoSemana(cc.Key.Semana, cc.Key.Anno)

                                  }).ToList();
                        }



                        //return at;

                    }
                    else if (filtro == "dia")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (turno == null)
                        {
                            try
                            {
                                if (cod_activo == null)
                                    ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();
                                else
                                    ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();
                            }
                            catch (Exception ex)
                            { 
                            
                            }

                            
                        }
                        else
                        {
                            if (cod_activo == null)
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno == turno).ToListAsync();
                            else
                                ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                        }

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaP
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Dia.Year, c.Dia } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = (ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia).Count()),

                                      Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia)).Count(),

                                      Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia)).Count(),

                                      NoRegistrados = (ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_turno != null).Count()),

                                      Tiempo = cc.Key.Dia,
                                      Leyenda = cc.Key.Dia.ToString("dd/MM/yyyy"),
                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault()
                                  }).OrderBy(o => o.Tiempo).ToList();
                        }
                        else
                        {
                            at = (from c in ConsultaP
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Cod_activo, c.Dia.Year, c.Dia } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tiempo = cc.Key.Dia,
                                      Leyenda = cc.Key.Dia.ToString("dd/MM/yyyy"),
                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault()
                                  }).OrderBy(o => o.Tiempo).ToList();
                        }

                        //return at;
                    }
                    else if (filtro == "hora")
                    {

                        //CAZA02

                        fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm tt"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm tt"));
                        int hini = int.Parse(fini_.ToString("HH"));
                        int hfin = int.Parse(ffin_.ToString("HH")) - 1;

                        if (cod_activo == null)
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0).ToListAsync();
                        //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                        //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                        //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)

                        //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_)).ToListAsync();
                        else
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo).ToListAsync();
                        //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                        //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                        //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)

                        //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Tiempo <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaP
                                  orderby c.Tiempo
                                  group c by new { c.Dia, c.Nhora } into cc                                  
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      NoRegistrados = ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_turno != null).Count(),

                                      Filtro    = ff,
                                      Unidades  = UnidadesX,
                                      Cod_turno = turno,
                                      Activo    = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda   = DateTime.Parse(cc.Min(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //cc.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),
                                      Tiempo    = DateTime.Parse(cc.Min(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Ttiempo   = DateTime.Parse(cc.Min(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Hora      = int.Parse(cc.Min(m => m.Tiempo).ToString("HH"))

                                  }).ToList();

                        }
                        else
                        {
                            at = (from c in ConsultaP
                                  orderby c.Cod_activo, c.Tiempo
                                  group c by new { c.Cod_activo, c.Dia, c.Nhora } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaP.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaP.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaP.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaP.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = DateTime.Parse(cc.Min(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //cc.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),
                                      Tiempo = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Ttiempo = DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Hora = int.Parse(cc.Min(m => m.Tiempo).ToString("HH"))

                                  }).ToList();

                            
                        }

                        //return at;
                    }
                }
                else
                {
                    if (filtro == "mes")
                    {
                        try
                        {
                            fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                            ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));


                            if (cod_activo == null)
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();
                            else
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                            if (cod_activo != null)
                            {
                                at = (from c in ConsultaPO
                                      where c.Cod_turno != null && c.Minutos > 0
                                      group c by new { c.Dia.Year, c.Dia.Month } into cc
                                      select new AnalisisTiempos
                                      {
                                          To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month).Count(),

                                          Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month)).Count(),

                                          Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month)).Count(),

                                          NoRegistrados = ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_turno != null).Count(),

                                          Filtro = ff,
                                          Unidades = UnidadesX,
                                          Cod_turno = turno,
                                          Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                          Leyenda = ConvierteFecha(cc.Max(m => m.Dia), "mes")

                                      }).ToList();
                            }
                            else
                            {
                                at = (from c in ConsultaPO
                                      where c.Cod_turno != null && c.Minutos > 0
                                      group c by new { c.Cod_activo, c.Dia.Year, c.Dia.Month } into cc
                                      select new AnalisisTiempos
                                      {
                                          To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                                  (w.Dia.Year == cc.Key.Year && w.Dia.Month == cc.Key.Month) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          NoRegistrados = ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                                  (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia.Month == cc.Key.Month && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                          Filtro = ff,
                                          Unidades = UnidadesX,
                                          Cod_turno = turno,
                                          Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                          Leyenda = ConvierteFecha(cc.Max(m => m.Dia), "mes")

                                      }).ToList();
                            }



                            //return at;

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (filtro == "semana")
                    {
                        int separador = turno.IndexOf("|", 0, turno.Length);

                        int sini = int.Parse(turno.Substring(6, 2));
                        int annoIni = int.Parse(turno.Substring(0, 4));
                        int sfin = int.Parse(turno.Substring(separador + 7, 2));
                        int annoFin = int.Parse(turno.Substring(9, 4));

                        CultureInfo myCIintl = new CultureInfo("es-MX", false);

                        DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                        DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                        var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                        if (cod_activo == null)
                            //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2)).
                            //                    Union(ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia == f2.AddDays(1) && w.Semana == sfin))).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin)
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                                }).ToListAsync();
                        else
                            //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= f1 && w.Dia <= f2) && w.Cod_activo == cod_activo).
                            //                    Union(ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia == f2.AddDays(1) && w.Semana == sfin) && w.Cod_activo == cod_activo)).ToListAsync();

                            ConsultaPSemana = await (from c in ctx.IOT_Conciliado_optimizado
                                                     where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_activo == cod_activo && c.Tiempo > Inicio
                                                     select new IOT_Conciliado_Semanas
                                                     {
                                                         Cod_activo = c.Cod_activo,
                                                         Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                         Dia = c.Dia,
                                                         Cod_plan = c.Cod_plan,
                                                         Cod_turno = c.Cod_turno,
                                                         IOT_id = c.IOT_id,
                                                         Marca = c.Marca,
                                                         Minutos = c.Minutos,
                                                         Ndia = c.Ndia,
                                                         Nhora = c.Nhora,
                                                         Planificado = c.Planificado,
                                                         Semana = c.Semana,
                                                         Sku_activo = c.Sku_activo,
                                                         Tiempo = c.Tiempo,
                                                         Valor = c.Valor,
                                                         Variable = c.Variable,
                                                         Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                                                     }).Union(
                                                                from c in ctx.IOT_Conciliado_optimizado
                                                                where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_activo == cod_activo
                                                                select new IOT_Conciliado_Semanas
                                                                {
                                                                    Cod_activo = c.Cod_activo,
                                                                    Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                    Dia = c.Dia,
                                                                    Cod_plan = c.Cod_plan,
                                                                    Cod_turno = c.Cod_turno,
                                                                    IOT_id = c.IOT_id,
                                                                    Marca = c.Marca,
                                                                    Minutos = c.Minutos,
                                                                    Ndia = c.Ndia,
                                                                    Nhora = c.Nhora,
                                                                    Planificado = c.Planificado,
                                                                    Semana = c.Semana,
                                                                    Sku_activo = c.Sku_activo,
                                                                    Tiempo = c.Tiempo,
                                                                    Valor = c.Valor,
                                                                    Variable = c.Variable,
                                                                    Anno = c.Semana == 53 ? fini_.Year : c.Dia.Year
                                                                }).ToListAsync();

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaPSemana
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Anno, c.Semana } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPSemana.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana).Count(),

                                      Tpp = ConsultaPSemana.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana)).Count(),

                                      Tpnp = ConsultaPSemana.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana)).Count(),

                                      NoRegistrados = ConsultaPSemana.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_turno != null).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = FormatoSemana(cc.Key.Semana, cc.Key.Anno)

                                  }).ToList();
                        }
                        else
                        {
                            at = (from c in ConsultaPSemana
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Cod_activo, c.Anno, c.Semana } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPSemana.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaPSemana.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaPSemana.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Semana == cc.Key.Semana) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaPSemana.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= f1 && w.Dia <= f2) && w.Semana == cc.Key.Semana && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = FormatoSemana(cc.Key.Semana, cc.Key.Anno)

                                  }).ToList();
                        }



                        //return at;

                    }
                    else if (filtro == "dia")
                    {
                        fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                        if (turno == null)
                        {
                            if (cod_activo == null)
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();
                            else
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();
                        }
                        else
                        {
                            if (cod_activo == null)
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno == turno).ToListAsync();
                            else
                                ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                        }

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaPO
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Dia.Year, c.Dia } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = (ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia).Count()),

                                      Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia)).Count(),

                                      Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia)).Count(),

                                      NoRegistrados = (ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_turno != null).Count()),

                                      Tiempo = cc.Key.Dia,
                                      Leyenda = cc.Key.Dia.ToString("dd/MM/yyyy"),
                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault()
                                  }).OrderBy(o => o.Tiempo).ToList();
                        }
                        else
                        {
                            at = (from c in ConsultaPO
                                  where c.Cod_turno != null && c.Minutos > 0
                                  group c by new { c.Cod_activo, c.Dia.Year, c.Dia } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia.Year == cc.Key.Year && w.Dia == cc.Key.Dia) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia >= fini_ && w.Dia <= ffin_) && w.Dia == cc.Key.Dia && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tiempo = cc.Key.Dia,
                                      Leyenda = cc.Key.Dia.ToString("dd/MM/yyyy"),
                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault()
                                  }).OrderBy(o => o.Tiempo).ToList();
                        }

                        //return at;
                    }
                    else if (filtro == "hora")
                    {
                        //CAZA t1 2

                        fini_ = DateTime.Parse(fini_.AddMinutes(1).ToString("dd-MM-yyyy HH:mm tt"));
                        ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm tt"));
                        int hini = int.Parse(fini_.ToString("HH"));
                        int hfin = int.Parse(ffin_.ToString("HH")) - 1;

                        if (cod_activo == null)
                        {
                            ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                            //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                            ).ToListAsync();
                        }
                        //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_)).ToListAsync();
                        else
                        {
                            ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                                //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                                //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                                ).ToListAsync();
                        }
                            
                        //ConsultaPO = await ctx.IOT_Conciliado_optimizado.Where(w => (w.Dia >= fini_ && w.Tiempo <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                        if (cod_activo != null)
                        {
                            at = (from c in ConsultaPO
                                  orderby c.Cod_activo, c.Tiempo
                                  group c by new { c.Dia, c.Nhora } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora)).Count(),

                                      NoRegistrados = ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_turno != null).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //cc.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),
                                      Tiempo = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Ttiempo = DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Hora = int.Parse(cc.Min(m => m.Tiempo).ToString("HH"))

                                  }).ToList();

                        }
                        else
                        {
                            at = (from c in ConsultaPO
                                  orderby c.Cod_activo, c.Tiempo
                                  group c by new { c.Cod_activo, c.Dia, c.Nhora } into cc
                                  select new AnalisisTiempos
                                  {
                                      To = ConsultaPO.Where(w => w.Planificado == null && w.Cod_turno != null && w.Minutos > 0 && w.Valor > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpp = ConsultaPO.Where(w => w.Planificado == true && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Tpnp = ConsultaPO.Where(w => w.Planificado == false && w.Cod_turno != null && w.Minutos > 0 &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      NoRegistrados = ConsultaPO.Where(w => (w.Planificado == null && w.Valor == 0) &&
                                              (w.Dia == cc.Key.Dia && w.Nhora == cc.Key.Nhora) && w.Cod_turno != null && w.Cod_activo == cc.Key.Cod_activo).Count(),

                                      Filtro = ff,
                                      Unidades = UnidadesX,
                                      Cod_turno = turno,
                                      Activo = cc.Key.Cod_activo + " - " + ctx.Activos.Where(w => w.Cod_activo == cc.Key.Cod_activo).Select(s => s.Des_activo).FirstOrDefault(),
                                      Leyenda = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")).ToString("dd/MM/yyyy HH:00"), //cc.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")),
                                      Tiempo = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Ttiempo = DateTime.Parse(cc.Max(m => m.Tiempo).ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00")),
                                      Hora = int.Parse(cc.Min(m => m.Tiempo).ToString("HH"))

                                  }).ToList();
                        }

                        //return at;
                    }
                }

                at = (from a in at
                      //where a.Tiempo >= fini_ //&& a.Tiempo <= ffin_ 
                      select new AnalisisTiempos
                      {
                          To = decimal.Parse((a.To / (verTiempo == "Hor" ? 60 : 1)).ToString("N2")),
                          Tpp = decimal.Parse((a.Tpp / (verTiempo == "Hor" ? 60 : 1)).ToString("N2")),
                          Tpnp = decimal.Parse((a.Tpnp / (verTiempo == "Hor" ? 60 : 1)).ToString("N2")),
                          NoRegistrados = decimal.Parse((a.NoRegistrados / (verTiempo == "Hor" ? 60 : 1)).ToString("N2")),
                          Filtro = a.Filtro,
                          Unidades = a.Unidades,
                          Cod_turno = a.Cod_turno,
                          Activo = a.Activo,
                          Leyenda = a.Leyenda,
                          Hora = a.Hora
                      }).ToList();

              return at;

            }

            return null;
        }

        //IndicadorTipoAnalisisTiempos
        public async Task<List<AnalisisTipoTiempos2>> IndicadorTipoAnalisisTiempos(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string cod_activo, string variable, string verTiempo, bool inicio, bool sw01)
        {
            List<IOT_Conciliado> ConsultaP = new List<IOT_Conciliado>();
            List<AnalisisTipoTiempos> att = new List<AnalisisTipoTiempos>();
            List<IOT_Conciliado_Semanas> ConsultaPSemana = new List<IOT_Conciliado_Semanas>();

            string ff = "";

            switch (filtro)
            {
                case "dia":
                    if (turno != null)
                    {
                        ff = "Día - Turno: " + turno;
                    }
                    else
                        ff = "Día";
                    break;
                case "mes":
                    ff = "Mes";
                    break;
                case "semana":
                    ff = "Semana";
                    break;
                case "hora":
                    ff = "Fecha y hora";
                    break;
                default:
                    break;
            }

            string UnidadesX = verTiempo == "Hor" ? "Horas" : "Minutos";

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                if (filtro == "mes")
                {
                    fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                    ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));


                    if (cod_activo == null)
                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();
                    else
                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                    att = (from c in ConsultaP
                           join t in ctx.Tipos_incidencia on c.Cod_tipo_incidencia equals t.Cod_tipo
                           where c.Cod_turno != null && c.Minutos > 0
                           group c by new { t.Cod_tipo, t.Des_tipo, t.Planificado, c.Dia.Year, c.Dia.Month } into cc
                           select new AnalisisTipoTiempos
                           {
                               Des_tipo = cc.Key.Des_tipo,
                               Valor = cc.Count(),
                               Tipo = (cc.Key.Planificado == true ? "TPP" : "TPNP"),
                               Filtro = ff,
                               Unidades = UnidadesX,
                               Leyenda = ConvierteFecha(cc.Max(m => m.Dia), "mes"),
                               Cod_tipo_incidencia = cc.Key.Cod_tipo
                           }).ToList();


                    //return att;

                }
                else if (filtro == "semana")
                {
                    int separador = turno.IndexOf("|", 0, turno.Length);

                    int sini = int.Parse(turno.Substring(6, 2));
                    int annoIni = int.Parse(turno.Substring(0, 4));
                    int sfin = int.Parse(turno.Substring(separador + 7, 2));
                    int annoFin = int.Parse(turno.Substring(9, 4));

                    CultureInfo myCIintl = new CultureInfo("es-MX", false);

                    DateTime f1 = primerDíaSemana(annoIni, sini, myCIintl);
                    DateTime f2 = primerDíaSemana(annoFin, sfin, myCIintl).AddDays(6); //Agrego un día mas por que hay registros del siguiente día que aún pertenecen a la última semana 
                    var Inicio = DateTime.Parse(ctx.Configuracion.Select(s => f1.ToString("dd/MM/yyyy") + " " + s.Hora_inicio_actividades.ToString()).FirstOrDefault());

                    if (cod_activo == null)
                    {
                        //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_)).ToListAsync();

                        ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                 where (c.Dia >= f1 && c.Dia <= f2) && c.Tiempo > Inicio
                                                 //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                 select new IOT_Conciliado_Semanas
                                                 {
                                                     Cod_activo = c.Cod_activo,
                                                     Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                     Dia = c.Dia,
                                                     Cod_plan = c.Cod_plan,
                                                     Cod_turno = c.Cod_turno,
                                                     IOT_id = c.IOT_id,
                                                     Marca = c.Marca,
                                                     Minutos = c.Minutos,
                                                     Ndia = c.Ndia,
                                                     Nhora = c.Nhora,
                                                     Planificado = c.Planificado,
                                                     Semana = c.Semana,
                                                     Sku_activo = c.Sku_activo,
                                                     Tiempo = c.Tiempo,
                                                     Valor = c.Valor,
                                                     Variable = c.Variable,
                                                     Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                 }).Union(
                                                            from c in ctx.IOT_Conciliado
                                                            where (c.Dia == f2.AddDays(1) && c.Semana == sfin)
                                                            select new IOT_Conciliado_Semanas
                                                            {
                                                                Cod_activo = c.Cod_activo,
                                                                Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                Dia = c.Dia,
                                                                Cod_plan = c.Cod_plan,
                                                                Cod_turno = c.Cod_turno,
                                                                IOT_id = c.IOT_id,
                                                                Marca = c.Marca,
                                                                Minutos = c.Minutos,
                                                                Ndia = c.Ndia,
                                                                Nhora = c.Nhora,
                                                                Planificado = c.Planificado,
                                                                Semana = c.Semana,
                                                                Sku_activo = c.Sku_activo,
                                                                Tiempo = c.Tiempo,
                                                                Valor = c.Valor,
                                                                Variable = c.Variable,
                                                                Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                            }).ToListAsync();
                    }


                    else
                    {
                        //ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo).ToListAsync();

                        ConsultaPSemana = await (from c in ctx.IOT_Conciliado
                                                 where (c.Dia >= f1 && c.Dia <= f2) && c.Cod_activo == cod_activo && c.Tiempo > Inicio
                                                 //where (c.Semana >= sini && c.Semana <= sfin) && c.Cod_turno != null && c.Minutos > 0 && act.Contains(c.Cod_activo)
                                                 select new IOT_Conciliado_Semanas
                                                 {
                                                     Cod_activo = c.Cod_activo,
                                                     Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                     Dia = c.Dia,
                                                     Cod_plan = c.Cod_plan,
                                                     Cod_turno = c.Cod_turno,
                                                     IOT_id = c.IOT_id,
                                                     Marca = c.Marca,
                                                     Minutos = c.Minutos,
                                                     Ndia = c.Ndia,
                                                     Nhora = c.Nhora,
                                                     Planificado = c.Planificado,
                                                     Semana = c.Semana,
                                                     Sku_activo = c.Sku_activo,
                                                     Tiempo = c.Tiempo,
                                                     Valor = c.Valor,
                                                     Variable = c.Variable,
                                                     Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                 }).Union(
                                                            from c in ctx.IOT_Conciliado
                                                            where (c.Dia == f2.AddDays(1) && c.Semana == sfin) && c.Cod_activo == cod_activo
                                                            select new IOT_Conciliado_Semanas
                                                            {
                                                                Cod_activo = c.Cod_activo,
                                                                Cod_tipo_incidencia = c.Cod_tipo_incidencia,
                                                                Dia = c.Dia,
                                                                Cod_plan = c.Cod_plan,
                                                                Cod_turno = c.Cod_turno,
                                                                IOT_id = c.IOT_id,
                                                                Marca = c.Marca,
                                                                Minutos = c.Minutos,
                                                                Ndia = c.Ndia,
                                                                Nhora = c.Nhora,
                                                                Planificado = c.Planificado,
                                                                Semana = c.Semana,
                                                                Sku_activo = c.Sku_activo,
                                                                Tiempo = c.Tiempo,
                                                                Valor = c.Valor,
                                                                Variable = c.Variable,
                                                                Anno = c.Semana == 53 ? f1.Year : c.Dia.Year
                                                            }).ToListAsync();
                    }
                        

                    att = (from c in ConsultaPSemana
                           join t in ctx.Tipos_incidencia on c.Cod_tipo_incidencia equals t.Cod_tipo
                           where c.Cod_turno != null && c.Minutos > 0
                           group c by new { t.Cod_tipo, t.Des_tipo, t.Planificado, c.Dia.Year, c.Semana } into cc
                           select new AnalisisTipoTiempos
                           {
                               Des_tipo = cc.Key.Des_tipo,
                               Valor = cc.Count(),
                               Tipo = (cc.Key.Planificado == true ? "TPP" : "TPNP"),
                               Filtro = ff,
                               Unidades = UnidadesX,
                               Leyenda = FormatoSemana(cc.Key.Semana, cc.Key.Year),
                               Cod_tipo_incidencia = cc.Key.Cod_tipo
                           }).OrderBy(o => o.Des_tipo).ToList();

                }
                else if (filtro == "dia")
                {
                    fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy"));
                    ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy"));

                    if (turno == null)
                    {
                        if (cod_activo == null)
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno != null).ToListAsync();
                        else
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo && w.Cod_turno != null).ToListAsync();
                    }
                    else
                    {
                        if (cod_activo == null)
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_turno == turno).ToListAsync();
                        else
                            ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Dia >= fini_ && w.Dia <= ffin_) && w.Cod_activo == cod_activo && w.Cod_turno == turno).ToListAsync();
                    }

                    
                    att = (from c in ConsultaP
                            join t in ctx.Tipos_incidencia on c.Cod_tipo_incidencia equals t.Cod_tipo
                            where c.Cod_turno != null && c.Minutos > 0
                            group c by new { t.Cod_tipo, t.Des_tipo, t.Planificado, c.Dia.Year, c.Dia } into cc
                            select new AnalisisTipoTiempos
                            {
                                Des_tipo = cc.Key.Des_tipo,
                                Valor = cc.Count(),
                                Tipo = (cc.Key.Planificado == true ? "TPP" : "TPNP"),
                                Filtro = ff,
                                Unidades = UnidadesX,
                                Leyenda = cc.Key.Dia.ToString("dd/MM/yyyy"),
                                Cod_tipo_incidencia = cc.Key.Cod_tipo
                            }).ToList();
                    

                    //return att;
                }
                else if (filtro == "hora")  ////aqui 1
                {
                    fini_ = DateTime.Parse(fini_.ToString("dd-MM-yyyy HH:mm"));
                    ffin_ = DateTime.Parse(ffin_.ToString("dd-MM-yyyy HH:mm"));

                    if (cod_activo == null)
                    {
                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                            //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                            ).ToListAsync();
                    }
                    else
                    {
                        ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_) && w.Cod_turno != null && w.Minutos > 0 && w.Cod_activo == cod_activo
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_
                            //&& DateTime.Parse(w.Tiempo.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) <= ffin_
                            //&& (DateTime.Parse(w.Dia.ToString("dd/MM/yyyy") + ((w.Nhora < 10 ? " 0" + w.Nhora.ToString() : " " + w.Nhora.ToString()) + ":00")) >= fini_)
                            ).ToListAsync();
                    }
                        
                    

                    //if (cod_activo == null)
                    //    ConsultaP = await ctx.IOT_Conciliado.Where(w => w.Tiempo >= fini_ && w.Tiempo <= ffin_ && (w.Cod_turno != null && w.Minutos > 0)).ToListAsync();
                    //else
                    //    ConsultaP = await ctx.IOT_Conciliado.Where(w => (w.Tiempo >= fini_ && w.Tiempo <= ffin_ && (w.Cod_turno != null && w.Minutos > 0)) && w.Cod_activo == cod_activo).ToListAsync();

                    att = (from c in ConsultaP
                           join t in ctx.Tipos_incidencia on c.Cod_tipo_incidencia equals t.Cod_tipo
                           group c by new { t.Cod_tipo, t.Des_tipo, t.Planificado, c.Dia, c.Nhora } into cc
                           select new AnalisisTipoTiempos
                           {
                               Des_tipo = cc.Key.Des_tipo,
                               Valor = cc.Count(),
                               Tipo = (cc.Key.Planificado == true ? "TPP" : "TPNP"),
                               Filtro = ff,
                               Unidades = UnidadesX,
                               Leyenda = cc.Min(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")), //cc.Max(m => m.Tiempo.ToString("dd/MM/yyyy HH:00")), //ConvierteFecha(cc.Max(m => m.Dia), "mes"),
                               Cod_tipo_incidencia = cc.Key.Cod_tipo,
                               Tiempo = DateTime.Parse(cc.Key.Dia.ToString("dd/MM/yyyy") + ((cc.Key.Nhora < 10 ? " 0" + cc.Key.Nhora.ToString() : " " + cc.Key.Nhora.ToString()) + ":00"))
                           }).ToList();
                }

                var attAgrupado1 = (from a in att

                                    group a by new { a.Leyenda } into aa
                                    select new
                                    {
                                        Leyenda = aa.Key.Leyenda
                                    }).ToList();


                var tinc = ctx.Tipos_incidencia.ToList();
                List<CompletaIncidencias> att1 = new List<CompletaIncidencias>();
                List<AnalisisTipoTiempos2> att2 = new List<AnalisisTipoTiempos2>();
                decimal[] ValorR = new decimal[attAgrupado1.Count()];

                //Hago esto para rellenar con el uso de todas las incidencias que existen en cada caso de cada agrupacion por Leyenda
                foreach (var l in attAgrupado1)
                {
                    foreach (var t in tinc)
                    {
                        var existe = att.Where(w => w.Leyenda == l.Leyenda && w.Cod_tipo_incidencia == t.Cod_tipo).ToList();

                        if (existe.Count() > 0)
                        {
                            att1.Add(new CompletaIncidencias
                            {
                                Agrupacion = l.Leyenda,
                                DesTipoIncidencia = t.Des_tipo,
                                Valor = existe.Select(s => s.Valor).FirstOrDefault()
                            });
                        }
                        else
                        {
                            att1.Add(new CompletaIncidencias
                            {
                                Agrupacion = l.Leyenda,
                                DesTipoIncidencia = t.Des_tipo,
                                Valor = 0
                            });
                        }
                    }
                }

                var at2 = (from a1 in att1
                           join t in ctx.Tipos_incidencia on a1.DesTipoIncidencia equals t.Des_tipo
                           group a1 by new { a1.DesTipoIncidencia, t.Planificado } into aa
                           select new AnalisisTipoTiempos2
                           {
                               Des_tipo = aa.Key.DesTipoIncidencia,
                               Valor = att1.Where(w => w.DesTipoIncidencia == aa.Key.DesTipoIncidencia).Select(s => Decimal.Parse((s.Valor / (verTiempo == "Hor" ? 60 : 1)).ToString("N2"))).ToArray(),
                               Tipo = (aa.Key.Planificado == true ? "TPP" : "TPNP"),
                               Filtro = ff,
                               Leyenda = att1.Where(w => w.DesTipoIncidencia == aa.Key.DesTipoIncidencia).Select(s => s.Agrupacion).ToArray(),
                               Hora = att1.Where(w => w.DesTipoIncidencia == aa.Key.DesTipoIncidencia).Select(s => int.Parse(DateTime.Parse(s.Agrupacion).ToString("HH"))).ToArray()
                           }).ToList();

                 return at2;
            }
                                        
            return null;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        //Metodo para ejecutar la vista principal de rendimiento
        public async Task<IActionResult> Rendimiento(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var vr = new viewRendimiento
                {
                    Sku = await (from s in ctx.Productos
                                 select new Productos
                                 {
                                     Cod_producto = s.Cod_producto,
                                     Des_producto = s.Des_producto
                                 }).ToListAsync()

                };

                return View(vr);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        //Metodo para ejecutar la vista principal de resumen de operaciones (Resumen OP)
        public async Task<IActionResult> ResumenOP(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        public DateTime? fini, ffin;

        public void AsignaFechas(DateTime fi, DateTime ff)
        {
            fini = fi;
            ffin = ff;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<object> GetResumenOP(DataSourceLoadOptions loadOptions, int idEmpresa)
        {           

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string tabla = await ctx.Activos_tablas.Select(s => s.Nombre_tabla).FirstOrDefaultAsync(); //Optengo el nombre de la tabla del primer registro OJO hay que cambiarlo luego

                fini = (fini == null ? DateTime.Now : fini);
                ffin = (ffin == null ? DateTime.Now : ffin);

                try
                {
                    var IOTX = ctx.IOT.FromSql("select id, value, timestamp, Sku_activo, Cod_plan, '0' Tiempo from " + tabla);

                    //var prueba = ctx.IOT.FromSql("select id, value, timestamp, Sku_activo, Cod_plan, '0' Tiempo from " + tabla).ToList();

                    List<ResumenOP> op = await (from iot in IOTX
                                                join pro in ctx.Productos on iot.Sku_activo equals pro.Cod_producto
                                                join ped in ctx.Pedidos on iot.Cod_plan equals ped.Cod_plan
                                                where ped.Fecha != null && ped.Fecha_fin != null
                                                group iot by iot.Cod_plan into ggr
                                                select new ResumenOP
                                                {
                                                    value = ggr.Sum(s => (int)s.value), //int.Parse(s.value)),
                                                    fini = ctx.Pedidos.Where(w => w.Cod_plan == ggr.Key).Select(s => s.Fecha).FirstOrDefault(),
                                                    ffin = ctx.Pedidos.Where(w => w.Cod_plan == ggr.Key).Select(s => s.Fecha_fin).FirstOrDefault(),
                                                    Cod_producto = ggr.Max(m => m.Sku_activo),
                                                    Des_producto = ctx.Productos.Where(w => w.Cod_producto == ggr.Max(m => m.Sku_activo)).Select(s => s.Des_producto).FirstOrDefault(),
                                                    Cod_plan = ggr.Key
                                                }).ToListAsync();

                    op = (from o in op
                          select new ResumenOP
                          {
                              value = o.value,
                              fini = o.fini,
                              ffin = o.ffin,
                              Cod_producto = o.Cod_producto,
                              Des_producto = o.Des_producto,
                              Cod_plan = o.Cod_plan,
                              HorasT = double.Parse((DateTime.Parse(o.ffin.ToString()).Subtract(DateTime.Parse(o.fini.ToString())).TotalHours).ToString("N2"))
                          }).ToList();

                    return DataSourceLoader.Load(op, loadOptions);
                }
                catch (Exception ex)
                {
                    return DataSourceLoader.Load("", loadOptions);
                }
            }
        }

        public List<IOT> ListadoIOT(string fini, string ffin, string cod_activo, string variable, int idEmpresa) {

            List<IOT> consulta0 = new List<IOT>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string t1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";
                try
                {
                    consulta0 = ctx.IOT.FromSql(t1).ToList();
                }
                catch (Exception ex)
                { }
                
                return consulta0;
            }
        }

        public List<IOT2> ListadoIOT2(string fini, string ffin, string cod_activo, string variable, int idEmpresa)
        {

            List<IOT2> consulta0 = new List<IOT2>();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string t1 = "exec sp_Registros_IOT2 '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";
                consulta0 = ctx.IOT2.FromSql(t1).ToList();
                return consulta0;
            }
        }

        //Calcula el tiempo basado en la tabla de los horarios de los equipos y los turnos en los que trabaja cada día
        public decimal TiempoXmes(DateTime fini, DateTime ffin, string cod_activo, string turno, int idEmpresa)
        {

            //DateTime fini = DateTime.Parse("01" + "-" + (mes < 10 ? "0" : "") + mes.ToString() + "-" + anno.ToString());
            //DateTime ffin = fini.AddMonths(1).AddDays(-1);

            TimeSpan difference = ffin - fini;
            int differenceInDays = difference.Days;

            DateTime finix = fini;
            int lun = 0, mar = 0, mie = 0, jue = 0, vie = 0, sab = 0, dom = 0;

            for (int i = 0; i < differenceInDays + 1; i++)
            {

                switch ((int)finix.DayOfWeek)
                {
                    case 0:
                        dom++;
                        break;
                    case 1:
                        lun++;
                        break;
                    case 2:
                        mar++;
                        break;
                    case 3:
                        mie++;
                        break;
                    case 4:
                        jue++;
                        break;
                    case 5:
                        vie++;
                        break;
                    case 6:
                        sab++;
                        break;

                    default:
                        break;
                }

                finix = finix.AddDays(1);
            }

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                decimal mint2 = 0;

                if (turno == null)
                {
                    try
                    {
                        var mint = (from tt in ctx.Turnos_activos_extras
                                    join t0 in ctx.Turnos on tt.Cod_turno equals t0.Cod_turno
                                    where tt.Cod_activo == cod_activo && tt.Fecha_ini == fini
                                    select new
                                    {
                                        cod_turno = tt.Cod_turno,
                                        dia = tt.Fecha_ini.DayOfWeek.ToString(),
                                        minutos = ((DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm")) < DateTime.Parse("01-01-1900 " + " " + t0.Hora_ini1.ToString("HH:mm"))
                                            ? DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm")).AddDays(1) : DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm"))) -
                                            (DateTime.Parse("01-01-1900 " + " " + t0.Hora_ini1.ToString("HH:mm")))).TotalMinutes
                                    }).ToList();

                        mint2 = (decimal)(from m in mint
                                          group m by m.dia into d
                                          select new
                                          {
                                              //cod_turno = d.Max(m => m.cod_turno),
                                              //dia = d.Key,
                                              minuto = d.Sum(s => s.minutos) *
                                                       (d.Key == "Monday" ? lun :
                                                        d.Key == "Tuesday" ? mar :
                                                        d.Key == "Wednesday" ? mie :
                                                        d.Key == "Thursday" ? jue :
                                                        d.Key == "Friday" ? vie :
                                                        d.Key == "Saturday" ? sab :
                                                        dom)
                                          }).Sum(s => s.minuto);
                    }
                    catch (Exception ex)
                    { }
                    
                }
                else if (turno == "HH")
                {
                    mint2 = 60;
                }
                else
                {
                    try
                    {
                        mint2 = (decimal)(from tt in ctx.Turnos_activos_extras
                                          join t0 in ctx.Turnos on tt.Cod_turno equals t0.Cod_turno
                                          where tt.Cod_activo == cod_activo && tt.Cod_turno == turno
                                          select new
                                          {
                                              minutos = ((DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm")) < DateTime.Parse("01-01-1900 " + " " + t0.Hora_ini1.ToString("HH:mm"))
                                                 ? DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm")).AddDays(1) : DateTime.Parse("01-01-1900 " + " " + t0.Hora_fin1.ToString("HH:mm"))) -
                                                 (DateTime.Parse("01-01-1900 " + " " + t0.Hora_ini1.ToString("HH:mm")))).TotalMinutes
                                          }).Max(s => s.minutos);
                    }
                    catch (Exception ex)
                    { }
                    
                }

                

                //decimal prueba = decimal.Parse(mint2.ToString());


                return decimal.Parse(mint2.ToString());
            }
            
        }

        //Calcula el primer día de la semana recibiendo un parametro de fecha sin importanr el día que sea de esa semana
        public DateTime PrimerDía(DateTime dt)
        {
            while (dt.DayOfWeek != DayOfWeek.Monday) dt = dt.AddDays(-1);
            return dt;
        }   

        public int ObtenerNumeroSemana(DateTime dt)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;

            // Usa la fecha formateada y calcula el número de la semana
            int NumeroSemana = cul.Calendar.GetWeekOfYear(
                 dt,
                 CalendarWeekRule.FirstDay,
                 DayOfWeek.Monday);

            return NumeroSemana;
        }

        //Metodo para calcular la disponibilidad para la vista disponibilidad, recibiendo cualquier filtro 
        public async Task<List<SeriesDisponibilidad>> GetDisponibilidad(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, bool inicio)
        {
            List<SeriesDisponibilidad> series = new List<SeriesDisponibilidad>();

            if (inicio == true)
            {
                fini_ = PrimerDía(fini_).AddDays(-7);
                ffin_ = fini_.AddDays(6);
            }

            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");

            //if (filtro == "hora")
            //{
            //    fini = fini_.ToString("yyyyMMdd HH:00");
            //    ffin = ffin_.ToString("yyyyMMdd HH:59");
            //}

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Activos_tablas> at = new List<Activos_tablas>();
                var at_aux = await ctx.Activos_tablas.ToListAsync();
                List<IOT> iot = new List<IOT>();
                List<IOT> seleccion = new List<IOT>();
                int vueltas = 0;
                decimal toD = 0, toR = 0;
                decimal topD = 0, topR = 0;
                List<IOT> seleccionD = new List<IOT>();
                List<IOT> seleccionT = new List<IOT>();
                string Tipo_Filtro = "";

                //at.RemoveRange(0, at.Count);
                foreach (var a in at_aux)
                {
                    if (a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CA" || a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CI")
                    {
                        at.Add(a);                       
                    }                    
                }

                foreach (var a in at)
                {                    
                        if (filtro == "mes")
                        {
                        Tipo_Filtro = "Mes";

                        //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            if (vueltas == 0)
                            {
                                if (sku == null)
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                }
                                else
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
                                }

                            }
                            else
                            {
                                if (sku == null)
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                }
                                else
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
                                }

                            }

                            vueltas++;


                            #region código comentado viejo

                            //##############################################
                            //Calculo el total de los minutos del mes inicio
                            //##############################################
                            //var dXmes = (from dia in seleccion
                            //             group dia by new { dia.ndia, dia.timestamp.Month } into m
                            //             select new {
                            //                 mes = m.Key
                            //             }).ToList();


                            //lun = lun * ((seleccion.Where(w => w.ndia == 2).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 2).Max(m => m.Minutos)));
                            //mar = mar * ((seleccion.Where(w => w.ndia == 3).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 3).Max(m => m.Minutos)));
                            //mie = mie * ((seleccion.Where(w => w.ndia == 4).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 4).Max(m => m.Minutos)));
                            //jue = jue * ((seleccion.Where(w => w.ndia == 5).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 5).Max(m => m.Minutos)));
                            //vie = vie * ((seleccion.Where(w => w.ndia == 6).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 6).Max(m => m.Minutos)));
                            //sab = sab * ((seleccion.Where(w => w.ndia == 7).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 7).Max(m => m.Minutos)));
                            //dom = dom * ((seleccion.Where(w => w.ndia == 1).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 1).Max(m => m.Minutos)));
                            //decimal totalMes = lun + mar + mie + jue + vie + sab + dom;

                            //##############################################
                            //Calculo el total de los minutos del mes fin
                            //##############################################

                            //##########################################################################
                            //Busco el total de minutos de paradas planificadas y no planificadas inicio
                            //##########################################################################
                            //var paradas = await (from i in ctx.Incidencias
                            //                     join t in ctx.Tipos_incidencia on i.Cod_tipo equals t.Cod_tipo
                            //                     where i.Desde >= fini_ && i.Hasta <= ffin_ && 
                            //                     i.Hasta != null && t.Planificado == true
                            //                     select new
                            //                     {
                            //                         minutos = ((i.Hasta ?? DateTime.Parse("01-01-1900")) - i.Desde).TotalMinutes
                            //                     }).SumAsync(s => s.minutos);                            
                            //##########################################################################
                            //Busco el total de minutos de paradas planificadas y no planificadas fin
                            //##########################################################################



                            //int Planificados = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0).Count();
                            //int NoPlanificados = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0).Count();
                            //int TO = seleccion.Where(w => w.planificado != false && w.turno != null && w.Minutos > 0).Count();

                            #endregion

                            //debo buscar los días cuyos values sean mayores acero para considerarlos como dias laborados en la semana
                            var diasTrabajados = (from dt in seleccion
                                                  join te in ctx.Turnos_activos_extras on a.Cod_activo equals te.Cod_activo
                                                  where dt.dia >= DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4)) 
                                                  &&    dt.dia <= DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4))
                                                  group dt by new { dt.dia, dt.turno, dt.dia.Month } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.dia,
                                                      valor = dtt.Sum(s => s.value),
                                                      turno = dtt.Max(m => m.turno),
                                                      mes = dtt.Key.Month
                                                  }).Where(w => w.turno != null).ToList();

                            seleccion = (from iotX in seleccion
                                         where iotX.turno != null && iotX.Minutos > 0
                                         group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value =
                                             decimal.Parse(
                                             //TO
                                             (((
                                             
                                             seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count()
                                                                                          
                                             )) 
                                             
                                             ).ToString("N2")),

                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             dia = mm.Max(m => m.dia),
                                             marca = a.Cod_activo,
                                             
                                             turno =((seleccion.Where(w => w.dia.Month == mm.Key.Month).Max(m => m.Minutos) * 
                                             (diasTrabajados.Where(w => w.dia.Month == mm.Key.Month).Count()))
                                                         
                                                         //Planificados
                                                         - (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())).ToString("N2")

                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            if (seleccion.Count == 0)
                            {
                                seleccion.Add(new IOT { id = 0, value = 0, turno = "0.01", dia = fini_, timestamp = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", marca = "" });
                            }

                            try
                            {
                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes
                                    tiempo = seleccion.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                    //seleccion.Select(s => s.dia.Month == 1 ? "1 Enero" :
                                    //                                s.dia.Month == 2 ? "2 Febrero" :
                                    //                                s.dia.Month == 3 ? "3 Marzo" :
                                    //                                s.dia.Month == 4 ? "4 Abril" :
                                    //                                s.dia.Month == 5 ? "5 Mayo" :
                                    //                                s.dia.Month == 6 ? "6 Junio" :
                                    //                                s.dia.Month == 7 ? "7 Julio" :
                                    //                                s.dia.Month == 8 ? "8 Agosto" :
                                    //                                s.dia.Month == 9 ? "9 Septiembre" :
                                    //                                s.dia.Month == 10 ? "10 Octubre" :
                                    //                                s.dia.Month == 11 ? "11 Noviembre" : 
                                    //                                "12 Diciembre").ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                });

                                //seleccionD = await ConsultaDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }

                                //if (ctx.Activos_tablas.Count() == vueltas)
                                if (at.Count() == vueltas)
                                    {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.dia.Year, st.dia.Month } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = stt.Sum(s => s.value),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Max(m => m.Sku_activo),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Sum(s => decimal.Parse(s.turno)).ToString("N2"),
                                                      dia = stt.Max(m => m.dia),
                                                      //marca = stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();


                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                        //seleccionT.Select(s => s.dia.Month == 1 ? "1 Enero" :
                                        //                                s.dia.Month == 2 ? "2 Febrero" :
                                        //                                s.dia.Month == 3 ? "3 Marzo" :
                                        //                                s.dia.Month == 4 ? "4 Abril" :
                                        //                                s.dia.Month == 5 ? "5 Mayo" :
                                        //                                s.dia.Month == 6 ? "6 Junio" :
                                        //                                s.dia.Month == 7 ? "7 Julio" :
                                        //                                s.dia.Month == 8 ? "8 Agosto" :
                                        //                                s.dia.Month == 9 ? "9 Septiembre" :
                                        //                                s.dia.Month == 10 ? "10 Octubre" :
                                        //                                s.dia.Month == 11 ? "11 Noviembre" : "12 Diciembre").ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2")))).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro
                                    });
                                }
                            }
                            catch (Exception ex)
                            { }

                        }
                        catch (Exception ex)
                        { }
                    }
                    else if (filtro == "semana")
                    {
                        Tipo_Filtro = "Semana";

                        //Calculo anterior
                        //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
                        //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


                        //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            if (vueltas == 0)
                            {
                                if (sku == null)
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                }
                                else
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
                                }

                            }
                            else
                            {
                                if (sku == null)
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                }
                                else
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
                                }


                            }

                            vueltas++;


                            double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
                            int separador = turno.IndexOf("|", 0, turno.Length);

                            string anno1; // = turno.Substring(0, 4);
                            string anno2; // = turno.Substring(separador+1, 4);

                            try
                            {
                                anno1 = turno.Substring(0, 4);
                                anno2 = turno.Substring(separador + 1, 4);
                            }
                            catch
                            {
                                anno1 = "";
                                anno2 = "";
                            }

                            //int p1xxxx = turno.Length - (separador + 5);
                            //string sem1 = turno.Substring(6, 2);
                            //string sem2 = turno.Substring(separador+7, 2);

                            int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
                            int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

                            //int sini = int.Parse(turno.Substring(0, separador));
                            //int sfin = int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

                            //debo buscar los días cuyos values sean mayores acero para considerarlos como dias laborados en la semana
                            var diasTrabajados = (from dt in seleccion
                                                  join te in ctx.Turnos_activos_extras on a.Cod_activo equals te.Cod_activo
                                                  where dt.semana >= sini && dt.semana <= sfin && turno != null && dt.dia == te.Fecha_ini.Date
                                                  group dt by new { dt.dia, dt.turno, dt.semana } into dtt
                                                  select new
                                                  {
                                                      dia = dtt.Key.dia,
                                                      valor = dtt.Sum(s => s.value),
                                                      turno = dtt.Max(m => m.turno),
                                                      semana = dtt.Key.semana
                                                  }).Where(w => w.turno != null).ToList();


                            seleccion = (from iotX in seleccion
                                         where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sini && iotX.semana <= sfin)
                                         group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value =
                                             decimal.Parse(//TO
                                                seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count().ToString("N2"))
                                             ,

                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = FormatoSemana(mm.Key.semana, mm.Key.Year), //mm.Key.semana.ToString(),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             marca = a.Cod_activo,
                                             semana = mm.Key.semana,
                                             turno = 
                                             (
                                                 //Total minutos del mes                                             
                                                 //TiempoXmes(PrimerDía(mm.Max(m => m.dia)), PrimerDía(mm.Max(m => m.dia)) == PrimerDía(DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"))) ?
                                                 //DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")) : PrimerDía(mm.Max(m => m.dia)).AddDays(6), a.Cod_activo, null, idEmpresa)
                                                 (seleccion.Where(w => w.semana == mm.Key.semana).Max(m => m.Minutos) * 
                                                 (diasTrabajados.Where(w => w.semana == mm.Key.semana).Count()))
                                                 -
                                                 //Planificados
                                                 (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count())
                                             ).ToString("N2")
                                         }).OrderBy(o => o.timestamp.Month).ToList();


                            //CAZA

                            if (seleccion.Count == 0)
                            {
                                seleccion.Add(new IOT { id = 0, value = 0, timestamp = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", marca = "" });
                            }

                            series.Add(new SeriesDisponibilidad()
                            {
                                id = seleccion.Max(s => s.id),
                                fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                data = seleccion.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2"))).ToArray(),
                                activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                filtro = Tipo_Filtro
                            });

                            //seleccionD = await ConsultaDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                            foreach (var d in seleccion)
                            {

                                try
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }
                                catch (Exception ex)
                                { }

                            }

                            //if (ctx.Activos_tablas.Count() == vueltas)
                            if (at.Count() == vueltas)
                                {
                                
                                seleccionT = (from st in seleccionT
                                              where st.semana > 0
                                              group st by new { st.dia.Year, st.semana } into stt
                                              select new IOT
                                              {
                                                  id = stt.Max(m => m.id),
                                                  value = decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")),
                                                  timestamp = stt.Max(m => m.timestamp),
                                                  Sku_activo = stt.Max(m => m.Sku_activo), //FormatoSemana(stt.Key.semana, stt.Key.Year), //stt.Key.semana.ToString(),
                                                  Cod_plan = stt.Max(m => m.Cod_plan),
                                                  turno = stt.Max(m => m.turno),
                                                  dia = stt.Max(m => m.dia),
                                                  marca = "" //stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                  //planificado = d.planificado,
                                                  //ndia = d.ndia,
                                                  //Minutos = d.Minutos,
                                                  //semana = d.semana
                                              }).ToList();


                                series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro    
                                });
                            }

                        }
                        catch (Exception ex)
                        { }

                    }
                    else if (filtro == "dia")
                    {
                        Tipo_Filtro = "Día";

                        try
                        {
                            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id ")).ToListAsync();
                            //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(case when total = 0 then 0 else cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) end as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(dia_fecha) dia_fecha from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, day(timestamp) dia_fecha, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select dia_fecha, SUM(tot_min) total from(select z.dia, dia_fecha, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, day(timestamp) dia_fecha	from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin group by day(timestamp),DATEPART(dw,timestamp), day(timestamp))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, dia_fecha)t group by dia_fecha) w on t.dia_fecha = w.dia_fecha")).ToListAsync();

                            //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas

                            fini = fini.Substring(0, 8);
                            ffin = ffin.Substring(0, 8);
                            string dia = fini.Substring(6, 2);
                            string mes = fini.Substring(4, 2);
                            string ano = fini.Substring(0, 4);

                            DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
                            DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


                            try
                            {
                                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";
                                //##########################################################################################################################
                                //Si en base de datos hay menos de una semana de datos no muestra nada por que por defecto muestra datos de una semana atras
                                //##########################################################################################################################

                                if (vueltas == 0)
                                {
                                    if (turno == "Todos" || turno == null)
                                    {
                                        if (sku == null)
                                        {
                                            seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                        }
                                        else
                                        {
                                            seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
                                        }

                                        turno = null;
                                    }
                                    else
                                    {
                                        Tipo_Filtro = "Día - Turno: " + turno;

                                        if (sku == null)
                                        {
                                            seleccion = await ctx.IOT.FromSql(p1).Where(w => w.turno == turno).ToListAsync();
                                        }
                                        else
                                        {
                                            seleccion = await ctx.IOT.FromSql(p1).Where(w => w.turno == turno).Where(w => w.Sku_activo == sku).ToListAsync();
                                        }

                                    }
                                }
                                else
                                {
                                    if (turno == "Todos" || turno == null)
                                    {
                                        if (sku == null)
                                        {
                                            seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                        }
                                        else
                                        {
                                            seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
                                        }

                                        turno = null;
                                    }
                                    else
                                    {
                                        Tipo_Filtro = "Día - Turno: " + turno;

                                        if (sku == null)
                                        {
                                            seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                        }
                                        else
                                        {
                                            seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
                                        }

                                        seleccion = seleccion.Where(w => w.turno == turno).ToList();
                                    }

                                    //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                }

                                vueltas++;

                                var diasTrabajados = (from dt in seleccion
                                                      join te in ctx.Turnos_activos_extras on a.Cod_activo equals te.Cod_activo
                                                      where dt.dia >= Dfini && dt.dia <= Dffin && dt.turno != null && dt.dia == te.Fecha_ini.Date && dt.turno == te.Cod_turno
                                                      group dt by new { dt.dia, dt.turno } into dtt
                                                      select new
                                                      {
                                                          dia = dtt.Key.dia,
                                                          valor = dtt.Sum(s => s.value),
                                                          turno = dtt.Max(m => m.turno)
                                                          //timestamp = dt.timestamp,
                                                          //turno = te.Cod_turno
                                                      }).Where(w => w.turno != null).ToList();


                                //PROBLEMA CON EL ACTIVO PH2 EN EL DÍA 12/12/2020
                                seleccion = (from iotX in seleccion
                                             where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
                                             group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                             select new IOT
                                             {
                                                 id = mm.Max(m => m.id),
                                                 dia = mm.Key.dia,
                                                 value = //TO
                                                 decimal.Parse((seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2")),

                                                 timestamp = mm.Max(m => m.timestamp),
                                                 Sku_activo = mm.Key.dia.ToString("dd/MM/yyyy"),//mm.Max(m => m.Sku_activo),
                                                 Cod_plan = mm.Max(m => m.Cod_plan),
                                                 marca = a.Cod_activo,
                                                 turno = //Total minutos del mes                                                 
                                                 (
                                                     (seleccion.Where(w => w.dia == mm.Key.dia).Max(m => m.Minutos)
                                                     *
                                                     (diasTrabajados.Where(w => w.dia == mm.Key.dia).Count()))
                                                     -
                                                     //Planificados
                                                     (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count())
                                                 ).ToString("N2")


                                                 //(
                                                 //    (TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), a.Cod_activo, turno, idEmpresa) -
                                                 //    //Planificados
                                                 //    (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count())) == 0 ? 1 : //Si es cero queda igualado a 1
                                                 //                                                                                                                                                                //Si no es cero pasa esto 
                                                 //        (TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), a.Cod_activo, turno, idEmpresa) -
                                                 //        //Planificados
                                                 //        (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()))
                                                 //).ToString("N2")
                                             }).OrderBy(o => o.timestamp.Month).ToList();


                                if (seleccion.Count == 0)
                                { 
                                    seleccion.Add(new IOT { id= 0, value = 0, timestamp = fini_, dia = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", turno = "0.00", marca = a.Cod_activo });
                                }

                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(s.turno) == 0 ? 0 : decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                });

                                //seleccionD = await ConsultaDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                //en seleccionT se van agregando los registros a medida que se van recorriendo para luego agruparlos
                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = DateTime.Parse(d.dia.ToString("dd/MM/yyyy")),
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }

                                //if (ctx.Activos_tablas.Count() == vueltas)
                               if (at.Count() == vueltas)
                                    {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.dia.Year, st.dia } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = stt.Sum(s => decimal.Parse(s.turno)) == 0 ? 0 : decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")), //decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")), //stt.Sum(s => s.value),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Key.dia.ToString("dd/MM/yyyy"),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Sum(s => decimal.Parse(s.turno)).ToString("N2"),
                                                      dia = stt.Max(m => m.dia),
                                                      marca = "1"//stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();

                                    seleccionT = seleccionT.OrderBy(o => o.dia).ToList();

                                    //var id = seleccionT.Max(s => s.id);
                                    //var fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray();
                                    //var tiempo = seleccionT.Select(s => "Día " + s.timestamp.ToString("dd/MM/yyyy")).ToArray();
                                    //var data = (seleccionT.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) == 0 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2")))).ToArray();
                                    //var activo = seleccionT.Select(m => "Total PLanta").ToArray();
                                    //var cod_plan = seleccionT.Select(m => "Total").ToArray();
                                    //var sku0 = seleccionT.Select(m => "Sku").ToArray();
                                    //var nombreActivo = "Planta";

                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro
                                    });
                                }

                            }
                            catch (Exception ex)
                            { }
                        }
                        catch (Exception ex)
                        { }
                    }  
                    else if(filtro == "hora")
                    {
                        Tipo_Filtro = "Fecha y hora";

                        //##############################################################################################
                        //##############################################################################################
                        //##############################################################################################

                        //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        //fini = fini; //.Substring(0, 8);
                        //ffin = ffin.Substring(0, 8);

                        fini = fini_.ToString("yyyyMMdd HH:mm");
                        ffin = ffin_.ToString("yyyyMMdd HH:mm");

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            if (vueltas == 0)
                            {
                                if (sku == null)
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                }
                                else
                                {
                                    seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
                                }

                            }
                            else
                            {
                                if (sku == null)
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                }
                                else
                                {
                                    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
                                }

                            }

                            vueltas++;

                           
                            seleccion = (from iotX in seleccion
                                         where iotX.turno != null && iotX.Minutos > 0
                                         group iotX by new { iotX.dia, iotX.nhora } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             
                                             value =
                                             decimal.Parse( //TO
                                                 (seleccion.Where(w => w.planificado == null && 
                                                                       w.turno != null && w.Minutos > 0 && w.value > 0 && 
                                                                       (w.dia == mm.Max(m => m.dia) && w.nhora == mm.Max(m => m.nhora))).Count()).ToString("N2")                                             
                                             ),
                                             
                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             dia = mm.Max(m => m.dia),
                                             marca = a.Cod_activo,
                                             
                                             nhora = mm.Key.nhora,

                                             turno =
                                             ((int.Parse(mm.Max(m => m.timestamp).ToString("mm")) > 0 && int.Parse(mm.Min(m => m.timestamp).ToString("mm")) <= 1 ?           
                                                    int.Parse(mm.Max(m => m.timestamp).ToString("mm")) : //Por aqui va el dos
                                               int.Parse(mm.Max(m => m.timestamp).ToString("mm")) == 0 && int.Parse(mm.Min(m => m.timestamp).ToString("mm")) > 1 ?
                                                    int.Parse(mm.Min(m => m.timestamp).ToString("mm")) :
                                               60) 
                                                    
                                                    
                                                    -
                                             //Planificados
                                                     (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia == mm.Max(m => m.dia) &&
                                                     w.timestamp.Hour == mm.Key.nhora)).Count())).ToString("N2")
                                                     
                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            if (seleccion.Count == 0)
                            {
                                seleccion.Add(new IOT { id = 0, value = 0, turno = "0.01", timestamp = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", marca = "" });
                            }

                            try
                            {
                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccion.Select(s => s.timestamp.ToString("dd/MM/yyyy HH:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2"))).ToArray(), //seleccion.Select(s => s.value).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                });

                                //seleccionD = await ConsultaDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana,
                                        nhora = d.nhora
                                    });
                                }

                                //if (ctx.Activos_tablas.Count() == vueltas)
                                if (at.Count() == vueltas)
                                {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.nhora } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = stt.Sum(s => s.value),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Max(m => m.timestamp).ToString("dd/MM/yyyy HH:mm"),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Sum(s => decimal.Parse(s.turno)).ToString("N2"),
                                                      dia = stt.Max(m => m.dia),
                                                      //marca = stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();


                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => s.timestamp.ToString("dd/MM/yyyy HH:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) == 0 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2")))).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro
                                    });
                                }
                            }
                            catch (Exception ex)
                            { }

                        }
                        catch (Exception ex)
                        { }
                    }

                }

            }
            return series;
        }

        //Metodo para calcular la disponibilidad para la vista rendimiento, recibiendo cualquier filtro
        public async Task<List<SeriesDisponibilidad>> GetRendimiento(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, bool inicio)
        {
            List<SeriesDisponibilidad> series = new List<SeriesDisponibilidad>();

            if (inicio == true)
            {
                fini_ = PrimerDía(fini_).AddDays(-7);
                ffin_ = fini_.AddDays(6);
            }

            string fini = fini_.ToString("yyyyMMdd HH:mm");
            string ffin = ffin_.ToString("yyyyMMdd 23:59");

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Activos_tablas> at = new List<Activos_tablas>();
                var at_aux = await ctx.Activos_tablas.ToListAsync();
                List<IOT> iot = new List<IOT>();
                List<IOT> seleccion = new List<IOT>();
                List<IOT> seleccionTodos = new List<IOT>();
                List<IOT> seleccionR = new List<IOT>();
                List<IOT> seleccionT = new List<IOT>();
                int vueltas = 0;
                int capA = ctx.Capacidades_activos.Max(m => Decimal.ToInt32(m.Capacidad_maxima));
                string Tipo_Filtro = "";

                //at.RemoveRange(0, at.Count);
                foreach (var a in at_aux)
                {
                    if (a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CA" || a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CI")
                    {
                        at.Add(a);
                    }
                }

                foreach (var a in at)
                {
                    if (filtro == "mes")
                    {
                        Tipo_Filtro = "Mes";

                        //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            if (vueltas == 0)
                            {
                                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

                                if (sku == null)
                                {

                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {

                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }
                            else
                            {
                                //var ca = ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo);  
                                //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);

                                seleccionTodos = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).ToList();

                                if (sku == null)
                                {
                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = //0,
                                                     (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {
                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = //0,
                                                     (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }

                            vueltas++;

                            seleccion = (from iotX in seleccion
                                         where iotX.turno != null && iotX.Minutos > 0
                                         group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value = 
                                                                                          
                                             mm.Where(w => w.planificado == null).Sum(s => s.value /
                                             (ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo).Max(m => m.Capacidad_maxima))),

                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             dia = mm.Max(m => m.dia),
                                             marca = a.Cod_activo,

                                             turno = //TO

                                                ((seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
                                                 (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())).ToString("N2")

                                         }).OrderBy(o => o.timestamp.Month).ToList();
                            try
                            {
                                if (seleccion.Count == 0)
                                {
                                    seleccion.Add(new IOT { id = 0, value = 0, timestamp = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", marca = "" });
                                }

                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccion.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
                                    //seleccion.Select(s => s.dia.Month == 1 ? "1 Enero" :
                                    //                                s.dia.Month == 2 ? "2 Febrero" :
                                    //                                s.dia.Month == 3 ? "3 Marzo" :
                                    //                                s.dia.Month == 4 ? "4 Abril" :
                                    //                                s.dia.Month == 5 ? "5 Mayo" :
                                    //                                s.dia.Month == 6 ? "6 Junio" :
                                    //                                s.dia.Month == 7 ? "7 Julio" :
                                    //                                s.dia.Month == 8 ? "8 Agosto" :
                                    //                                s.dia.Month == 9 ? "9 Septiembre" :
                                    //                                s.dia.Month == 10 ? "10 Octubre" :
                                    //                                s.dia.Month == 11 ? "11 Noviembre" : "12 Diciembre").ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) < 1 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                }); ;

                                //seleccionR = await ConsultaRendimiento(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }

                                if (at.Count() == vueltas)
                                {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.dia.Year, st.dia.Month } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = stt.Sum(s => s.value),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Max(m => m.Sku_activo),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Sum(s => decimal.Parse(s.turno)).ToString("N2"),
                                                      dia = stt.Max(m => m.dia),
                                                      //marca = stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();


                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => ConvierteFecha(s.dia,"mes")).ToArray(),
                                        //seleccionT.Select(s => s.dia.Month == 1 ? "1 Enero" :
                                        //                                s.dia.Month == 2 ? "2 Febrero" :
                                        //                                s.dia.Month == 3 ? "3 Marzo" :
                                        //                                s.dia.Month == 4 ? "4 Abril" :
                                        //                                s.dia.Month == 5 ? "5 Mayo" :
                                        //                                s.dia.Month == 6 ? "6 Junio" :
                                        //                                s.dia.Month == 7 ? "7 Julio" :
                                        //                                s.dia.Month == 8 ? "8 Agosto" :
                                        //                                s.dia.Month == 9 ? "9 Septiembre" :
                                        //                                s.dia.Month == 10 ? "10 Octubre" :
                                        //                                s.dia.Month == 11 ? "11 Noviembre" : "12 Diciembre").ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2")))).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro
                                    });
                                }

                            }
                            catch (Exception ex)
                            { }

                        }
                        catch (Exception ex)
                        { }
                    }
                    else if (filtro == "semana")
                    {
                        Tipo_Filtro = "Semana";

                        //Calculo anterior
                        //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
                        //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


                        //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        fini = fini.Substring(0, 8);
                        ffin = ffin.Substring(0, 8);

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            //if (vueltas == 0)
                            //{
                            //    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                            //}
                            //else
                            //{
                            //    seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                            //}

                            if (vueltas == 0)
                            {
                                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

                                if (sku == null)
                                {
                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {
                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }
                            else
                            {
                                //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                seleccionTodos = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).ToList();

                                if (sku == null)
                                {
                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {
                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }

                            vueltas++;


                            double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
                            int separador = turno.IndexOf("|", 0, turno.Length);

                            string anno1; // = turno.Substring(0, 4);
                            string anno2; // = turno.Substring(separador+1, 4);

                            try
                            {
                                anno1 = turno.Substring(0, 4);
                                anno2 = turno.Substring(separador + 1, 4);
                            }
                            catch
                            {
                                anno1 = "";
                                anno2 = "";
                            }

                            //int p1xxxx = turno.Length - (separador + 5);
                            //string sem1 = turno.Substring(6, 2);
                            //string sem2 = turno.Substring(separador+7, 2);

                            int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
                            int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

                            //int sini = int.Parse(turno.Substring(0, separador));
                            //int sfin = int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

                            seleccion = (from iotX in seleccion
                                         where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sini && iotX.semana <= sfin)
                                         group iotX by new { iotX.dia.Year, iotX.semana } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),

                                             value =

                                             mm.Where(w => w.planificado == null).Sum(s => s.value /
                                             ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo).Max(m => m.Capacidad_maxima)),

                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = FormatoSemana(mm.Key.semana, mm.Key.Year), //mm.Key.semana.ToString(),//mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             marca = a.Cod_activo,
                                             semana = mm.Key.semana,

                                             turno = //TO
                                             (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
                                             (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()).ToString("N2")

                                         }).OrderBy(o => o.timestamp.Month).ToList();

                            if (seleccion.Count == 0)
                            {
                                seleccion.Add(new IOT { id = 0, value = 0, timestamp = fini_, dia = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", turno = "0.00", marca = a.Cod_activo });
                            }

                            series.Add(new SeriesDisponibilidad()
                            {
                                id = seleccion.Max(s => s.id),
                                fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                //data = seleccion.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) < 1 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2"))).ToArray(),
                                data = seleccion.Select(s => decimal.Parse(((s.value / decimal.Parse(s.turno)) * 100).ToString("N2"))).ToArray(),
                                activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                filtro = Tipo_Filtro
                            });

                            //seleccionR = await ConsultaRendimiento(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                            foreach (var d in seleccion)
                            {
                                seleccionT.Add(new IOT
                                {
                                    id = d.id,
                                    value = d.value,
                                    timestamp = d.timestamp,
                                    Sku_activo = d.Sku_activo,
                                    Cod_plan = d.Cod_plan,
                                    turno = d.turno,
                                    dia = d.dia,
                                    marca = d.marca,
                                    planificado = d.planificado,
                                    ndia = d.ndia,
                                    Minutos = d.Minutos,
                                    semana = d.semana
                                });
                            }

                            if (at.Count() == vueltas)
                            {

                                seleccionT = (from st in seleccionT
                                              where st.semana > 0
                                              group st by new { st.dia.Year, st.semana } into stt
                                              select new IOT
                                              {
                                                  id = stt.Max(m => m.id),
                                                  value = decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")),
                                                  timestamp = stt.Max(m => m.timestamp),
                                                  Sku_activo = stt.Max(m => m.Sku_activo), //stt.Key.semana.ToString(),
                                                  Cod_plan = stt.Max(m => m.Cod_plan),
                                                  turno = stt.Max(m => m.turno),
                                                  dia = stt.Max(m => m.dia),
                                                  marca = "" //stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                  //planificado = d.planificado,
                                                  //ndia = d.ndia,
                                                  //Minutos = d.Minutos,
                                                  //semana = d.semana
                                              }).ToList();


                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccionT.Max(s => s.id),
                                    fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccionT.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                    cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                    sku = seleccionT.Select(m => "Sku").ToArray(),
                                    nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                    filtro = Tipo_Filtro
                                });
                            }

                        }
                        catch (Exception ex)
                        { }

                    }
                    else if (filtro == "dia")
                    {
                        Tipo_Filtro = "Día";

                        try
                        {
                            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id ")).ToListAsync();
                            //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(case when total = 0 then 0 else cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) end as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(dia_fecha) dia_fecha from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, day(timestamp) dia_fecha, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select dia_fecha, SUM(tot_min) total from(select z.dia, dia_fecha, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, day(timestamp) dia_fecha	from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin group by day(timestamp),DATEPART(dw,timestamp), day(timestamp))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, dia_fecha)t group by dia_fecha) w on t.dia_fecha = w.dia_fecha")).ToListAsync();

                            //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
                            fini = fini.Substring(0, 8);
                            ffin = ffin.Substring(0, 8);

                            DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
                            DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


                            try
                            {
                                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";
                                
                                if (vueltas == 0)
                                {
                                    if (turno == "Todos" || turno == null)
                                    {
                                        //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                        try
                                        {
                                            seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();
                                        }
                                        catch (Exception ex)
                                        { 
                                        
                                        }
                                        

                                        if (sku == null)
                                        {
                                            try
                                            {

                                                seleccion = (from i in seleccionTodos
                                                             join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                             from d in ag.DefaultIfEmpty()
                                                             select new IOT
                                                             {
                                                                 value = i.value,
                                                                 timestamp = i.timestamp,
                                                                 Sku_activo = i.Sku_activo,
                                                                 Cod_plan = i.Cod_plan,
                                                                 turno = i.turno,
                                                                 dia = i.dia,
                                                                 marca = i.marca,
                                                                 planificado = i.planificado,
                                                                 ndia = (d == null ? capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                                 //ctx.Capacidades_activos.Max(m => Decimal.ToInt32(m.Capacidad_maxima)) : Decimal.ToInt32(d.Capacidad_maxima)),
                                                                 Minutos = i.Minutos,
                                                                 semana = i.semana
                                                             }).ToList();
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                        }
                                        else
                                        {                                           

                                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }

                                        turno = null;
                                    }
                                    else
                                    {
                                        //seleccion = await ctx.IOT.FromSql(p1).Where(w => w.turno == turno).ToListAsync();
                                        seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();
                                        Tipo_Filtro = "Día - Turno: " + turno;

                                        if (sku == null)
                                        {
                                            seleccion = (from i in seleccionTodos
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         where i.turno == turno
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }
                                        else
                                        {
                                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         where i.turno == turno
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }

                                    }
                                }
                                else
                                {
                                    if (turno == "Todos" || turno == null)
                                    {
                                        //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                        seleccionTodos = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).ToList();

                                        if (sku == null)
                                        {
                                            seleccion = (from i in seleccionTodos
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }
                                        else
                                        {
                                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }

                                        turno = null;
                                    }
                                    else
                                    {
                                        //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).Where(w => w.turno == turno).ToList();
                                        seleccionTodos = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).ToList();
                                        Tipo_Filtro = "Día - Turno: " + turno;

                                        if (sku == null)
                                        {
                                            seleccion = (from i in seleccionTodos
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         where i.turno == turno
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }
                                        else
                                        {
                                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                         join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                         from d in ag.DefaultIfEmpty()
                                                         where i.turno == turno
                                                         select new IOT
                                                         {
                                                             value = i.value,
                                                             timestamp = i.timestamp,
                                                             Sku_activo = i.Sku_activo,
                                                             Cod_plan = i.Cod_plan,
                                                             turno = i.turno,
                                                             dia = i.dia,
                                                             marca = i.marca,
                                                             planificado = i.planificado,
                                                             ndia = (d == null ?
                                                             capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                             Minutos = i.Minutos,
                                                             semana = i.semana
                                                         }).ToList();
                                        }

                                    }

                                    //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                }

                                vueltas++;

                                decimal Capacidad = 0;

                                Capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo).Max(m => m.Capacidad_maxima);

                                //var Capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo).ToList();

                                //Falta asociar la capacidad al sku que tiene en la tabla de los datos del SQL

                                //registro 131 del día 03/04
                                seleccion = (from iotX in seleccion
                                             where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
                                             group iotX by new { iotX.dia.Year, iotX.dia } into mm
                                             select new IOT
                                             {
                                                 id = mm.Max(m => m.id),
                                                 value =

                                                 mm.Where(w => w.planificado == null).Sum(s => s.value / (Capacidad == 0 ? 1 : Capacidad)
                                                 ),

                                                 timestamp = mm.Max(m => m.timestamp),
                                                 Sku_activo = mm.Key.dia.ToString("dd/MM/yyyy"), //mm.Max(m => m.Sku_activo),
                                                 Cod_plan = mm.Max(m => m.Cod_plan),
                                                 marca = a.Cod_activo,
                                                 dia = mm.Key.dia,
                                                 turno = //TO

                                                 (seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
                                                 (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2")

                                             }).OrderBy(o => o.timestamp.Month).ToList();


                                //if (seleccion.Count == 0)
                                //{
                                //    seleccion.Add(new IOT { id = 0, value = 0, turno = "0.01", timestamp = fini_, Sku_activo = "Día " + fini_.ToString("dd/MM/yyyy"), Cod_plan = "", marca = "" });
                                //}

                                if (seleccion.Count == 0)
                                {
                                    seleccion.Add(new IOT { id = 0, value = 0, timestamp = fini_, dia = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", turno = "0.00", marca = a.Cod_activo });
                                }

                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) < 1 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                });

                                //seleccionR = await ConsultaRendimiento(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }

                                if (at.Count() == vueltas)
                                {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.dia.Year, st.dia } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = stt.Sum(s => decimal.Parse(s.turno)) == 0 ? 0 : decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")), //value = decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Key.dia.ToString("dd/MM/yyyy"),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Sum(s => decimal.Parse(s.turno)).ToString(),
                                                      dia = stt.Max(m => m.dia),
                                                      //marca = stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();

                                    seleccionT = seleccionT.OrderBy(o => o.dia).ToList();



                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta",
                                        filtro = Tipo_Filtro

                                        //id = seleccionT.Max(s => s.id),
                                        //fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        ////TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        //tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        //data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        //activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        //cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        //sku = seleccionT.Select(m => "Sku").ToArray(),
                                        //nombreActivo = "Planta" //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                    });
                                }

                            }
                            catch (Exception ex)
                            { }
                        }
                        catch (Exception ex)
                        { }
                    }
                    else if (filtro == "hora")
                    {
                        Tipo_Filtro = "Fecha y hora";

                        //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
                        //fini = fini; //.Substring(0, 8);
                        //ffin = ffin.Substring(0, 8);

                        fini = fini_.ToString("yyyyMMdd HH:mm");
                        ffin = ffin_.ToString("yyyyMMdd HH:mm");

                        try
                        {
                            string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + a.Cod_activo + "','" + a.Variable + "'";

                            if (vueltas == 0)
                            {
                                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
                                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

                                if (sku == null)
                                {
                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {
                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }
                            else
                            {
                                //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
                                seleccionTodos = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa).ToList();

                                if (sku == null)
                                {
                                    seleccion = (from i in seleccionTodos
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }
                                else
                                {
                                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
                                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo) on i.Sku_activo equals c.Cod_producto into ag
                                                 from d in ag.DefaultIfEmpty()
                                                 select new IOT
                                                 {
                                                     value = i.value,
                                                     timestamp = i.timestamp,
                                                     Sku_activo = i.Sku_activo,
                                                     Cod_plan = i.Cod_plan,
                                                     turno = i.turno,
                                                     dia = i.dia,
                                                     marca = i.marca,
                                                     planificado = i.planificado,
                                                     ndia = (d == null ?
                                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
                                                     Minutos = i.Minutos,
                                                     semana = i.semana
                                                 }).ToList();
                                }

                            }

                            vueltas++;

                            decimal Capacidad = 0;
                            Capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == a.Cod_activo).Max(m => m.Capacidad_maxima);

                            seleccion = (from iotX in seleccion
                                         where iotX.turno != null && iotX.Minutos > 0
                                         group iotX by new { iotX.dia, iotX.nhora } into mm
                                         select new IOT
                                         {
                                             id = mm.Max(m => m.id),
                                             value =

                                                mm.Where(w => w.planificado == null).Sum(s => s.value / (Capacidad == 0 ? 1 : Capacidad))

                                               ////condición para el denominador, que no sea cero
                                               //(seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
                                               //(w.dia == mm.Max(m => m.dia) && w.nhora == mm.Max(m => m.nhora))).Count()) 

                                               ,

                                             timestamp = mm.Max(m => m.timestamp),
                                             Sku_activo = mm.Max(m => m.Sku_activo),
                                             Cod_plan = mm.Max(m => m.Cod_plan),
                                             dia = mm.Max(m => m.dia),
                                             marca = a.Cod_activo,

                                             turno = //TO                                           
                                                 (seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
                                                 (w.dia == mm.Max(m => m.dia) && w.nhora == mm.Max(m => m.nhora))).Count()).ToString("N2")

                                         }).OrderBy(o => o.timestamp.Month).ToList();
                            try
                            {

                                if (seleccion.Count == 0)
                                {
                                    seleccion.Add(new IOT { id = 0, value = 0, timestamp = fini_, dia = fini_, Sku_activo = fini_.ToString("dd/MM/yyyy"), Cod_plan = "", turno = "0.00", marca = a.Cod_activo });
                                }

                                series.Add(new SeriesDisponibilidad()
                                {
                                    id = seleccion.Max(s => s.id),
                                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

                                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                    tiempo = seleccion.Select(s => s.timestamp.ToString("dd/MM/yyyy HH:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                    data = seleccion.Select(s => decimal.Parse(((s.value / (decimal.Parse(s.turno) < 1 ? 1 : decimal.Parse(s.turno))) * 100).ToString("N2"))).ToArray(),
                                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
                                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
                                    sku = seleccion.Select(m => m.Sku_activo.ToString()).ToArray(),
                                    nombreActivo = ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo).FirstOrDefault().ToString(),
                                    filtro = Tipo_Filtro
                                });

                                //seleccionR = await ConsultaRendimiento(idEmpresa, fini_, ffin_, filtro, turno, sku, a.Cod_activo, a.Variable);

                                foreach (var d in seleccion)
                                {
                                    seleccionT.Add(new IOT
                                    {
                                        id = d.id,
                                        value = d.value,
                                        timestamp = d.timestamp,
                                        Sku_activo = d.Sku_activo,
                                        Cod_plan = d.Cod_plan,
                                        turno = d.turno,
                                        dia = d.dia,
                                        marca = d.marca,
                                        planificado = d.planificado,
                                        ndia = d.ndia,
                                        Minutos = d.Minutos,
                                        semana = d.semana
                                    });
                                }

                                if (at.Count() == vueltas)
                                {
                                    seleccionT = (from st in seleccionT
                                                  group st by new { st.timestamp.Hour } into stt
                                                  select new IOT
                                                  {
                                                      id = stt.Max(m => m.id),
                                                      value = decimal.Parse(((stt.Sum(s => s.value) / stt.Sum(s => decimal.Parse(s.turno))) * 100).ToString("N2")),
                                                      timestamp = stt.Max(m => m.timestamp),
                                                      Sku_activo = stt.Max(M => M.timestamp).ToString("dd/MM/yyyy HH:mm"),
                                                      Cod_plan = stt.Max(m => m.Cod_plan),
                                                      turno = stt.Max(m => m.turno),
                                                      dia = stt.Max(m => m.dia),
                                                      marca = stt.Sum(s => decimal.Parse(s.marca)).ToString(),
                                                      //planificado = d.planificado,
                                                      //ndia = d.ndia,
                                                      //Minutos = d.Minutos,
                                                      //semana = d.semana
                                                  }).ToList();


                                    series.Add(new SeriesDisponibilidad()
                                    {
                                        id = seleccionT.Max(s => s.id),
                                        fecha = seleccionT.Select(s => s.timestamp.ToString()).ToArray(),

                                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

                                        tiempo = seleccionT.Select(s => s.timestamp.ToString("dd/MM/yyyy HH:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 

                                        data = (seleccionT.Select(s => s.value)).ToArray(), //seleccionD.Select(s => decimal.Parse(((toD / topD) * 100).ToString("N2"))).ToArray(),
                                        activo = seleccionT.Select(m => "Total PLanta").ToArray(),
                                        cod_plan = seleccionT.Select(m => "Total").ToArray(),
                                        sku = seleccionT.Select(m => "Sku").ToArray(),
                                        nombreActivo = "Planta", //ctx.Activos.Where(w => w.Cod_activo == a.Cod_activo).Select(s => s.Des_activo + "X").FirstOrDefault().ToString()
                                        filtro = Tipo_Filtro
                                    });
                                }
                            }
                            catch (Exception ex)
                            { }

                        }
                        catch (Exception ex)
                        { }
                    }                
                }

            }
            return series;
        }

        public List<FechasSemana> FirstDateOfWeek(int annoI, int semanaI, int annoF, int semanaF)
        {
            var firstDate = new DateTime(annoI, 1, 4);
            var secondDate = new DateTime(annoF, 1, 4);
            //first thursday of the week defines the first week (https://en.wikipedia.org/wiki/ISO_8601) 
            //Wiki: the 4th of january is always in the first week 
            while (firstDate.DayOfWeek != DayOfWeek.Monday)
                firstDate = firstDate.AddDays(-1);

            while (secondDate.DayOfWeek != DayOfWeek.Monday)
                secondDate = secondDate.AddDays(-1);

            List<FechasSemana> dias = new List<FechasSemana>();

            dias.Add(new FechasSemana { fecha1 = firstDate.AddDays((semanaI - 1) * 7), fecha2 = (secondDate.AddDays((semanaF - 1) * 7)).AddDays(6) });

            return dias;
        }

        #region Metodos Antiguos

        #region Controlador viejo de Análisis de tiempos

        //[Microsoft.AspNetCore.Authorization.Authorize]
        //public async Task<List<SeriesUnidades>> Historicos_variables2(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string cod_activo, string variable)
        //{
        //    List<SeriesUnidades> series = new List<SeriesUnidades>();

        //    string fini = fini_.ToString("yyyyMMdd HH:mm");
        //    string ffin = ffin_.ToString("yyyyMMdd 23:59");

        //    //if (filtro == "hora")
        //    //{
        //    //    fini = fini_.ToString("yyyyMMdd HH:00");
        //    //    ffin = ffin_.ToString("yyyyMMdd HH:59");
        //    //}

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        //var prueba = (ctx.IOT.FromSql("Exec sp_prueba")).ToListAsync();

        //        var at = await ctx.Activos_tablas.ToListAsync();
        //        //var pro = await ctx.Productos.ToListAsync();
        //        List<IOT> iot = new List<IOT>();
        //        List<IOT> seleccion = new List<IOT>();
        //        List<IOT> seleccion2 = new List<IOT>();
        //        List<IOT> seleccion3 = new List<IOT>();
        //        List<IOT> seleccion4 = new List<IOT>();
        //        List<IOT2> seleccion5 = new List<IOT2>();
        //        List<IOT2> seleccion6 = new List<IOT2>();
        //        List<IOT2> seleccion7 = new List<IOT2>();
        //        List<IOT2> seleccion8 = new List<IOT2>();
        //        List<string> tpp_nombres = new List<string>();
        //        List<List<string>> tpp_valor = new List<List<string>>();
        //        List<List<string>> tpp_fecha = new List<List<string>>();
        //        List<string> tpnp_nombres = new List<string>();
        //        List<List<string>> tpnp_valor = new List<List<string>>();
        //        List<List<string>> tpnp_fecha = new List<List<string>>();

        //        int vueltas = 0;

        //        string NombreActivo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault();

        //        //foreach (var a in at)
        //        //{


        //        if (filtro == "mes")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "'," + variable + "'";

        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;
        //                seleccion5 = ListadoIOT2(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion6 = seleccion5;
        //                seleccion7 = seleccion5;
        //                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                //seleccion2 = await ctx.IOT.FromSql(p1).ToListAsync();

        //                var Min = (from s in seleccion
        //                           where s.turno != null
        //                           group s by new { s.turno, s.dia, s.dia.Month } into t
        //                           select new
        //                           {
        //                               minutos = t.Max(m => m.Minutos),
        //                               mes = t.Key.Month
        //                           }).ToList();

        //                Min = (from m in Min
        //                       group m by new { m.mes } into mm
        //                       select new
        //                       {
        //                           minutos = mm.Sum(s => s.minutos),
        //                           mes = mm.Key.mes
        //                       }).ToList();

        //                seleccion = (from iotX in seleccion               //to
        //                             where iotX.turno != null && iotX.Minutos > 0
        //                             group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value =
        //                                 seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 dia = mm.Max(m => m.dia),
        //                                 marca = cod_activo,
        //                                 turno = "Unidades" //unidad //Uso el turno para guardar en este caso las unidades
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2               //tpp
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                              select new IOT
        //                              {
        //                                  value = //Planificado
        //                                  seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
        //                                  dia = mm.Max(m => m.dia),
        //                                  turno = turno
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion3 = (from iotX in seleccion3                   //tpnp
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                              select new IOT
        //                              {
        //                                  value = //no Planificado
        //                                  seleccion3.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
        //                                  dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4                   //total horario
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Max(m => m.Minutos),
        //                                  dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4                   //total horario
        //                              group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                              select new IOT
        //                              {
        //                                  value = //mm.Sum(m => m.value),
        //                                  TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), cod_activo, turno, idEmpresa),
        //                                  dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion6 = (from iotX in seleccion6               //tpp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == true
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion7 = (from iotX in seleccion7               //tpnp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == false
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();


        //                foreach (var ddd in seleccion6)              //tpp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5               //tpp
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == true && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
        //                                      dia = mm.Max(m => m.dia)
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpp_valor_aux = new List<string>();
        //                    List<string> tpp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpp_valor_aux.Add(j.value.ToString());
        //                        tpp_fecha_aux.Add(ConvierteFecha(j.dia, "mes"));
        //                        //(j.dia.Month == 1 ? "01 Ene " + j.dia.Year :
        //                        //                            j.dia.Month == 2 ? "02 Feb " + j.dia.Year :
        //                        //                            j.dia.Month == 3 ? "03 Mar " + j.dia.Year :
        //                        //                            j.dia.Month == 4 ? "04 Abr " + j.dia.Year :
        //                        //                            j.dia.Month == 5 ? "05 May " + j.dia.Year :
        //                        //                            j.dia.Month == 6 ? "06 Jun " + j.dia.Year :
        //                        //                            j.dia.Month == 7 ? "07 Jul " + j.dia.Year :
        //                        //                            j.dia.Month == 8 ? "08 Ago " + j.dia.Year :
        //                        //                            j.dia.Month == 9 ? "09 Sep " + j.dia.Year :
        //                        //                            j.dia.Month == 10 ? "10 Oct " + j.dia.Year :
        //                        //                            j.dia.Month == 11 ? "11 Nov " + j.dia.Year : "12 Dic " + j.dia.Year).ToString());
        //                    }
        //                    tpp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpp_valor.Add(tpp_valor_aux);
        //                    tpp_fecha.Add(tpp_fecha_aux);
        //                }


        //                foreach (var ddd in seleccion7)         ///tpnp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == false && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count(),
        //                                      dia = mm.Max(m => m.dia)
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpnp_valor_aux = new List<string>();
        //                    List<string> tpnp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpnp_valor_aux.Add(j.value.ToString());
        //                        tpnp_fecha_aux.Add(ConvierteFecha(j.dia, "mes"));
        //                        //tpnp_fecha_aux.Add((j.dia.Month == 1 ? "01 Ene " + j.dia.Year :
        //                        //                                j.dia.Month == 2 ? "02 Feb " + j.dia.Year :
        //                        //                                j.dia.Month == 3 ? "03 Mar " + j.dia.Year :
        //                        //                                j.dia.Month == 4 ? "04 Abr " + j.dia.Year :
        //                        //                                j.dia.Month == 5 ? "05 May " + j.dia.Year :
        //                        //                                j.dia.Month == 6 ? "06 Jun " + j.dia.Year :
        //                        //                                j.dia.Month == 7 ? "07 Jul " + j.dia.Year :
        //                        //                                j.dia.Month == 8 ? "08 Ago " + j.dia.Year :
        //                        //                                j.dia.Month == 9 ? "09 Sep " + j.dia.Year :
        //                        //                                j.dia.Month == 10 ? "10 Oct " + j.dia.Year :
        //                        //                                j.dia.Month == 11 ? "11 Nov " + j.dia.Year : "12 Dic " + j.dia.Year).ToString());
        //                    }
        //                    tpnp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpnp_valor.Add(tpnp_valor_aux);
        //                    tpnp_fecha.Add(tpnp_fecha_aux);
        //                }

        //                try
        //                {
        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                        tiempo = seleccion.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
        //                        //seleccion.Select(s => s.dia.Month == 1 ? "01 Ene " + s.dia.Year :
        //                        //                                s.dia.Month == 2 ? "02 Feb " + s.dia.Year :
        //                        //                                s.dia.Month == 3 ? "03 Mar " + s.dia.Year :
        //                        //                                s.dia.Month == 4 ? "04 Abr " + s.dia.Year :
        //                        //                                s.dia.Month == 5 ? "05 May " + s.dia.Year :
        //                        //                                s.dia.Month == 6 ? "06 Jun " + s.dia.Year :
        //                        //                                s.dia.Month == 7 ? "07 Jul " + s.dia.Year :
        //                        //                                s.dia.Month == 8 ? "08 Ago " + s.dia.Year :
        //                        //                                s.dia.Month == 9 ? "09 Sep " + s.dia.Year :
        //                        //                                s.dia.Month == 10 ? "10 Oct " + s.dia.Year :
        //                        //                                s.dia.Month == 11 ? "11 Nov " + s.dia.Year : 
        //                        //                                "12 Dic " + s.dia.Year).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
        //                        //seleccion2.Select(s => s.dia.Month == 1 ? "01 Ene " + s.dia.Year :
        //                        //                                s.dia.Month == 2 ? "02 Feb " + s.dia.Year :
        //                        //                                s.dia.Month == 3 ? "03 Mar " + s.dia.Year :
        //                        //                                s.dia.Month == 4 ? "04 Abr " + s.dia.Year :
        //                        //                                s.dia.Month == 5 ? "05 May " + s.dia.Year :
        //                        //                                s.dia.Month == 6 ? "06 Jun " + s.dia.Year :
        //                        //                                s.dia.Month == 7 ? "07 Jul " + s.dia.Year :
        //                        //                                s.dia.Month == 8 ? "08 Ago " + s.dia.Year :
        //                        //                                s.dia.Month == 9 ? "09 Sep " + s.dia.Year :
        //                        //                                s.dia.Month == 10 ? "10 Oct " + s.dia.Year :
        //                        //                                s.dia.Month == 11 ? "11 Nov " + s.dia.Year : 
        //                        //                                "12 Dic " + s.dia.Year).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo3 = seleccion3.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),
        //                        //seleccion3.Select(s => s.dia.Month == 1 ? "01 Ene " + s.dia.Year :
        //                        //                                s.dia.Month == 2 ? "02 Feb " + s.dia.Year :
        //                        //                                s.dia.Month == 3 ? "03 Mar " + s.dia.Year :
        //                        //                                s.dia.Month == 4 ? "04 Abr " + s.dia.Year :
        //                        //                                s.dia.Month == 5 ? "05 May " + s.dia.Year :
        //                        //                                s.dia.Month == 6 ? "06 Jun " + s.dia.Year :
        //                        //                                s.dia.Month == 7 ? "07 Jul " + s.dia.Year :
        //                        //                                s.dia.Month == 8 ? "08 Ago " + s.dia.Year :
        //                        //                                s.dia.Month == 9 ? "09 Sep " + s.dia.Year :
        //                        //                                s.dia.Month == 10 ? "10 Oct " + s.dia.Year :
        //                        //                                s.dia.Month == 11 ? "11 Nov " + s.dia.Year : "12 Dic " + s.dia.Year).ToArray(),
        //                        data = seleccion.Select(s => s.value).ToArray(),     //to
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),   //tpp
        //                        data3 = seleccion3.Select(s => s.value).ToArray(),   //tpnp
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        //sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        //sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = Min.Select(s => (decimal)s.minutos).ToArray(), //seleccion4.Select(s => s.value).ToArray(),
        //                        tpp_nombres = tpp_nombres,
        //                        tpp_valor = tpp_valor,
        //                        tpp_fecha = tpp_fecha,
        //                        tpnp_nombres = tpnp_nombres,
        //                        tpnp_valor = tpnp_valor,
        //                        tpnp_fecha = tpnp_fecha,
        //                        filtro = filtro,
        //                        nombreActivo = NombreActivo,
        //                        sku = seleccion.Select(s => "").ToArray()
        //                    });
        //                }
        //                catch (Exception ex)
        //                { }

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "semana")
        //        {
        //            //Calculo anterior
        //            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
        //            //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


        //            //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;
        //                seleccion5 = ListadoIOT2(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion6 = seleccion5;
        //                seleccion7 = seleccion5;

        //                var Min = (from s in seleccion
        //                           where s.turno != null
        //                           group s by new { s.turno, s.dia, s.semana } into t
        //                           select new
        //                           {
        //                               minutos = t.Max(m => m.Minutos),
        //                               semana = t.Key.semana
        //                           }).ToList();

        //                Min = (from m in Min
        //                       group m by new { m.semana } into mm
        //                       select new
        //                       {
        //                           minutos = mm.Sum(s => s.minutos),
        //                           semana = mm.Key.semana
        //                       }).ToList();

        //                double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
        //                int separador = turno.IndexOf("|", 0, turno.Length);

        //                string anno1; // = turno.Substring(0, 4);
        //                string anno2; // = turno.Substring(separador+1, 4);

        //                try
        //                {
        //                    anno1 = turno.Substring(0, 4);
        //                    anno2 = turno.Substring(separador + 1, 4);
        //                }
        //                catch
        //                {
        //                    anno1 = "";
        //                    anno2 = "";
        //                }

        //                //int p1xxxx = turno.Length - (separador + 5);
        //                //string sem1 = turno.Substring(6, 2);
        //                //string sem2 = turno.Substring(separador+7, 2);

        //                int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
        //                int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                //int sini = int.Parse(turno.Substring(0, separador));
        //                //int sfin = int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                seleccion = (from iotX in seleccion                    //to
        //                             where iotX.turno != null && iotX.Minutos > 0
        //                             group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value =
        //                                 seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year)), //"Semana " + mm.Key.semana.ToString() + " " + mm.Max(m => m.dia.Year),//mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 marca = cod_activo
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2    //tpp
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                              select new IOT
        //                              {
        //                                  value = //Planificado
        //                                  seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
        //                                  Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year))
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion3 = (from iotX in seleccion3                   //tpnp
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                              select new IOT
        //                              {
        //                                  value = //Planificado
        //                                  seleccion3.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
        //                                  Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year))
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4                   //total horario
        //                              where iotX.turno != null && iotX.Minutos > 0
        //                              group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                              select new IOT
        //                              {
        //                                  value = //mm.Max(m => m.Minutos),
        //                                  TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), cod_activo, turno, idEmpresa),
        //                                  semana = mm.Max(m => m.semana)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4                   //total horario
        //                              group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year))
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion6 = (from iotX in seleccion6               //tpp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == true
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion7 = (from iotX in seleccion7               //tpnp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == false
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                foreach (var ddd in seleccion6)              //tpp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5               //tpp
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == true && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
        //                                      //semana = mm.Max(m => m.semana)
        //                                      Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year))
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpp_valor_aux = new List<string>();
        //                    List<string> tpp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpp_valor_aux.Add(j.value.ToString());
        //                        tpp_fecha_aux.Add(j.Sku_activo.ToString());
        //                    }
        //                    tpp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpp_valor.Add(tpp_valor_aux);
        //                    tpp_fecha.Add(tpp_fecha_aux);
        //                }

        //                foreach (var ddd in seleccion7)         ///tpnp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == false && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count(),
        //                                      //semana = mm.Max(m => m.semana)
        //                                      Sku_activo = FormatoSemana(mm.Key.semana, mm.Max(m => m.dia.Year))
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpnp_valor_aux = new List<string>();
        //                    List<string> tpnp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpnp_valor_aux.Add(j.value.ToString());
        //                        tpnp_fecha_aux.Add(j.Sku_activo.ToString());
        //                    }
        //                    tpnp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpnp_valor.Add(tpnp_valor_aux);
        //                    tpnp_fecha.Add(tpnp_fecha_aux);
        //                }


        //                series.Add(new SeriesUnidades()
        //                {
        //                    id = seleccion.Max(s => s.id),
        //                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                    tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                    tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
        //                    tiempo3 = seleccion3.Select(s => s.Sku_activo).ToArray(),
        //                    data = seleccion.Select(s => s.value).ToArray(),
        //                    data2 = seleccion2.Select(s => s.value).ToArray(),
        //                    data3 = seleccion3.Select(s => s.value).ToArray(),
        //                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                    //sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                    //sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                    sku_conteo_total = Min.Select(s => (decimal)s.minutos).ToArray(), //seleccion4.Select(s => s.value).ToArray(),
        //                    tpp_nombres = tpp_nombres,
        //                    tpp_valor = tpp_valor,
        //                    tpp_fecha = tpp_fecha,
        //                    tpnp_nombres = tpnp_nombres,
        //                    tpnp_valor = tpnp_valor,
        //                    tpnp_fecha = tpnp_fecha,
        //                    filtro = filtro,
        //                    nombreActivo = NombreActivo,
        //                    sku = seleccion.Select(s => "").ToArray()
        //                });

        //            }
        //            catch (Exception ex)
        //            { }

        //        }
        //        else if (filtro == "dia")
        //        {
        //            try
        //            {
        //                //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id ")).ToListAsync();
        //                //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(case when total = 0 then 0 else cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) end as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(dia_fecha) dia_fecha from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, day(timestamp) dia_fecha, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select dia_fecha, SUM(tot_min) total from(select z.dia, dia_fecha, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, day(timestamp) dia_fecha	from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin group by day(timestamp),DATEPART(dw,timestamp), day(timestamp))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, dia_fecha)t group by dia_fecha) w on t.dia_fecha = w.dia_fecha")).ToListAsync();

        //                //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //                fini = fini.Substring(0, 8);
        //                ffin = ffin.Substring(0, 8);

        //                DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
        //                DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


        //                try
        //                {
        //                    string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";



        //                    if (turno == "Todos" || turno == null)
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion2 = seleccion;
        //                        seleccion3 = seleccion;
        //                        seleccion4 = seleccion;
        //                        seleccion5 = ListadoIOT2(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion6 = seleccion5;
        //                        seleccion7 = seleccion5;
        //                        turno = null;
        //                    }
        //                    else
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion = seleccion.Where(w => w.turno == turno).ToList();
        //                        seleccion2 = seleccion;
        //                        seleccion3 = seleccion;
        //                        seleccion4 = seleccion;
        //                        seleccion5 = ListadoIOT2(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion5 = seleccion5.Where(w => w.turno == turno).ToList();
        //                        seleccion6 = seleccion5;
        //                        seleccion7 = seleccion5;
        //                    }

        //                    var Min = (from s in seleccion
        //                               where s.turno != null
        //                               group s by new { s.turno, s.dia } into t
        //                               select new
        //                               {
        //                                   minutos = t.Max(m => m.Minutos),
        //                                   dia = t.Key.dia
        //                               }).ToList();

        //                    Min = (from m in Min
        //                           group m by new { m.dia } into mm
        //                           select new
        //                           {
        //                               minutos = mm.Sum(s => s.minutos),
        //                               dia = mm.Key.dia
        //                           }).ToList();



        //                    //registro 131 del día 03/04
        //                    seleccion = (from iotX in seleccion    //to
        //                                 where iotX.turno != null && iotX.Minutos > 0 && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                 group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                 select new IOT
        //                                 {
        //                                     id = mm.Max(m => m.id),
        //                                     value =
        //                                     seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
        //                                     timestamp = mm.Max(m => m.timestamp),
        //                                     Sku_activo = mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),
        //                                     Cod_plan = mm.Max(m => m.Cod_plan),
        //                                     marca = cod_activo,
        //                                     turno = turno
        //                                 }).OrderBy(o => o.timestamp).ToList();

        //                    seleccion2 = (from iotX in seleccion2    //tpp
        //                                  where iotX.turno != null && iotX.Minutos > 0 && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                  group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
        //                                      Sku_activo = mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),
        //                                      turno = turno
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion3 = (from iotX in seleccion3    //tpnp
        //                                  where iotX.turno != null && iotX.Minutos > 0 && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                  group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion3.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
        //                                      Sku_activo = mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),                                            
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion4 = (from iotX in seleccion4                   //total horario
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = //mm.Max(m => m.Minutos),
        //                                      TiempoXmes(mm.Min(m => m.dia), mm.Max(m => m.dia), cod_activo, turno, idEmpresa),
        //                                      //semana = mm.Max(m => m.semana)
        //                                      Sku_activo = mm.Key.dia.ToString().Substring(0, 10),
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion6 = (from iotX in seleccion6               //tpp divison lista
        //                                  where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == true
        //                                  group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                                  select new IOT2
        //                                  {
        //                                      Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                      //dia = mm.Max(m => m.dia)
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion7 = (from iotX in seleccion7               //tpnp divison lista
        //                                  where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == false
        //                                  group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                                  select new IOT2
        //                                  {
        //                                      Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia),
        //                                      //dia = mm.Max(m => m.dia)
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    foreach (var ddd in seleccion6)              //tpp division
        //                    {
        //                        seleccion8 = (from iotX in seleccion5               //tpp
        //                                      where iotX.turno != null && iotX.Minutos > 0
        //                                      group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                      select new IOT2
        //                                      {
        //                                          value = //Planificado
        //                                          seleccion5.Where(w => w.planificado == true && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
        //                                          //semana = mm.Max(m => m.semana)
        //                                          Sku_activo = mm.Key.dia.ToString().Substring(0, 10),
        //                                      }).OrderBy(o => o.timestamp.Month).ToList();

        //                        List<string> tpp_valor_aux = new List<string>();
        //                        List<string> tpp_fecha_aux = new List<string>();
        //                        foreach (var j in seleccion8)
        //                        {
        //                            tpp_valor_aux.Add(j.value.ToString());
        //                            tpp_fecha_aux.Add(j.Sku_activo.ToString());
        //                        }
        //                        tpp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                        tpp_valor.Add(tpp_valor_aux);
        //                        tpp_fecha.Add(tpp_fecha_aux);
        //                    }

        //                    foreach (var ddd in seleccion7)         ///tpnp division
        //                    {
        //                        seleccion8 = (from iotX in seleccion5
        //                                      where iotX.turno != null && iotX.Minutos > 0
        //                                      group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                      select new IOT2
        //                                      {
        //                                          value = //Planificado
        //                                          seleccion5.Where(w => w.planificado == false && w.Cod_tipo_incidencia == ddd.Cod_tipo_incidencia && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count(),
        //                                          //semana = mm.Max(m => m.semana)
        //                                          Sku_activo = mm.Key.dia.ToString().Substring(0, 10),
        //                                      }).OrderBy(o => o.timestamp.Month).ToList();

        //                        List<string> tpnp_valor_aux = new List<string>();
        //                        List<string> tpnp_fecha_aux = new List<string>();
        //                        foreach (var j in seleccion8)
        //                        {
        //                            tpnp_valor_aux.Add(j.value.ToString());
        //                            tpnp_fecha_aux.Add(j.Sku_activo.ToString());
        //                        }
        //                        tpnp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                        tpnp_valor.Add(tpnp_valor_aux);
        //                        tpnp_fecha.Add(tpnp_fecha_aux);
        //                    }

        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                        tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
        //                        tiempo3 = seleccion3.Select(s => s.Sku_activo).ToArray(),
        //                        data = seleccion.Select(s => s.value).ToArray(),
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),
        //                        data3 = seleccion3.Select(s => s.value).ToArray(),
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        //sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        //sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = Min.Select(s => (decimal)s.minutos).ToArray(), //seleccion4.Select(s => s.value).ToArray(),
        //                        tpp_nombres = tpp_nombres,
        //                        tpp_valor = tpp_valor,
        //                        tpp_fecha = tpp_fecha,
        //                        tpnp_nombres = tpnp_nombres,
        //                        tpnp_valor = tpnp_valor,
        //                        tpnp_fecha = tpnp_fecha,
        //                        filtro = filtro,
        //                        nombreActivo = NombreActivo,
        //                        sku = seleccion.Select(s => s.turno).ToArray()
        //                    });

        //                }
        //                catch (Exception ex)
        //                { }
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "hora")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            //fini = fini; //.Substring(0, 8);
        //            //ffin = ffin.Substring(0, 8);

        //            fini = fini_.ToString("yyyyMMdd HH:mm");
        //            ffin = ffin_.ToString("yyyyMMdd HH:mm");

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";


        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;
        //                seleccion5 = ListadoIOT2(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion6 = seleccion5;
        //                seleccion7 = seleccion5;

        //                var Min = (from s in seleccion
        //                           where s.turno != null
        //                           group s by new { s.turno, s.timestamp.Hour } into t
        //                           select new
        //                           {
        //                               minutos = t.Max(m => m.Minutos),
        //                               hora = t.Key.Hour
        //                           }).ToList();

        //                Min = (from m in Min
        //                       group m by new { m.hora } into mm
        //                       select new
        //                       {
        //                           minutos = mm.Sum(s => s.minutos),
        //                           hora = mm.Key.hora
        //                       }).ToList();

        //                //vueltas++;

        //                seleccion = (from iotX in seleccion    //to
        //                             where iotX.turno != null && iotX.Minutos > 0 /*&& iotX.Minutos > 0*/
        //                             group iotX by new { iotX.timestamp.Hour } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value =
        //                                 seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 dia = mm.Max(m => m.dia),
        //                                 marca = cod_activo
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2           //tpp
        //                              where iotX.turno != null && iotX.Minutos > 0 /*&& iotX.Minutos > 0*/
        //                              group iotX by new { iotX.timestamp.Hour } into mm
        //                              select new IOT
        //                              {
        //                                  value =
        //                                  seleccion2.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                  timestamp = mm.Max(m => m.timestamp)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion3 = (from iotX in seleccion3           //tpnp
        //                              where iotX.turno != null && iotX.Minutos > 0 /*&& iotX.Minutos > 0*/
        //                              group iotX by new { iotX.timestamp.Hour } into mm
        //                              select new IOT
        //                              {
        //                                  value =
        //                                  seleccion2.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 /*&& w.value > 0*/ && (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                  timestamp = mm.Max(m => m.timestamp)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion6 = (from iotX in seleccion6               //tpp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == true
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia)
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion7 = (from iotX in seleccion7               //tpnp divison lista
        //                              where iotX.turno != null && iotX.Minutos > 0 && iotX.planificado == false
        //                              group iotX by new { iotX.Cod_tipo_incidencia } into mm
        //                              select new IOT2
        //                              {
        //                                  Cod_tipo_incidencia = mm.Max(m => m.Cod_tipo_incidencia)
        //                                  //dia = mm.Max(m => m.dia)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                foreach (var ddd in seleccion6)              //tpp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5               //tpp
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.timestamp.Hour } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                      //semana = mm.Max(m => m.semana)
        //                                      //timestamp = mm.Max(m => m.timestamp)
        //                                      Sku_activo = mm.Max(m => m.timestamp.Hour).ToString() + "h"

        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpp_valor_aux = new List<string>();
        //                    List<string> tpp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpp_valor_aux.Add(j.value.ToString());
        //                        tpp_fecha_aux.Add(j.Sku_activo.ToString());
        //                    }
        //                    tpp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpp_valor.Add(tpp_valor_aux);
        //                    tpp_fecha.Add(tpp_fecha_aux);
        //                }

        //                foreach (var ddd in seleccion7)         ///tpnp division
        //                {
        //                    seleccion8 = (from iotX in seleccion5
        //                                  where iotX.turno != null && iotX.Minutos > 0
        //                                  group iotX by new { iotX.timestamp.Hour } into mm
        //                                  select new IOT2
        //                                  {
        //                                      value = //Planificado
        //                                      seleccion5.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0 && /*w.value > 0 &&*/ (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                      //semana = mm.Max(m => m.semana)
        //                                      //timestamp = mm.Max(m => m.timestamp)
        //                                      Sku_activo = mm.Max(m => m.timestamp.Hour).ToString() + "h"
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    List<string> tpnp_valor_aux = new List<string>();
        //                    List<string> tpnp_fecha_aux = new List<string>();
        //                    foreach (var j in seleccion8)
        //                    {
        //                        tpnp_valor_aux.Add(j.value.ToString());
        //                        tpnp_fecha_aux.Add(j.Sku_activo.ToString());
        //                    }
        //                    tpnp_nombres.Add(ddd.Cod_tipo_incidencia);
        //                    tpnp_valor.Add(tpnp_valor_aux);
        //                    tpnp_fecha.Add(tpnp_fecha_aux);
        //                }
        //                try
        //                {
        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                        tiempo = seleccion.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
        //                        tiempo3 = seleccion3.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
        //                        data = seleccion.Select(s => s.value).ToArray(),
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),
        //                        data3 = seleccion3.Select(s => s.value).ToArray(),
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        //sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        //sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = Min.Select(s => (decimal)s.minutos).ToArray(), //seleccion4.Select(s => s.value).ToArray(),
        //                        tpp_nombres = tpp_nombres,
        //                        tpp_valor = tpp_valor,
        //                        tpp_fecha = tpp_fecha,
        //                        tpnp_nombres = tpnp_nombres,
        //                        tpnp_valor = tpnp_valor,
        //                        tpnp_fecha = tpnp_fecha,
        //                        filtro = filtro,
        //                        nombreActivo = NombreActivo,
        //                        sku = seleccion.Select(s => "").ToArray()
        //                    });
        //                }
        //                catch (Exception ex)
        //                { }

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        //}

        //    }
        //    return series;
        //}

        #endregion

        #region Calculos de disponibilidad y rendimiento para el OEE viejos

        ////Consulta para obtener lo datos de la disponibilidad, esta consulta emplea el campo marca para obtener el valor del TOP y en value se obtiene el de TO
        ////Esta consulta busca obtener estos datos para adaptarse a cualquier filtro, se diferencia de la que calcula la disponibilidad por que trae el TO y el
        ////TOP por separado
        //public async Task<List<IOT>> ConsultaDisponibilidad(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, string cod_activo, string variable)
        //{
        //    string fini = fini_.ToString("yyyyMMdd HH:mm");
        //    string ffin = ffin_.ToString("yyyyMMdd 23:59");

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        //var prueba = (ctx.IOT.FromSql("Exec sp_prueba")).ToListAsync();

        //        //var at = await ctx.Activos_tablas.ToListAsync();
        //        //agregado lv
        //        var at_aux = await ctx.Activos_tablas.ToListAsync();
        //        List<Activos_tablas> at = new List<Activos_tablas>();

        //        foreach (var a in at_aux)
        //        {
        //            if (a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CA" || a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CI")
        //            {
        //                at.Add(a);
        //            }
        //        }

        //        //fin agregado lv
        //        List<IOT> iot = new List<IOT>();
        //        List<IOT> seleccion    = new List<IOT>();
        //        List<IOT> seleccionSKU = new List<IOT>();
        //        int vueltas = 0;

        //        if (filtro == "mes")
        //        {
        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                if (vueltas == 0)
        //                {
        //                    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();

        //                }
        //                else
        //                {
        //                    seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);

        //                }

        //                if (sku == null)
        //                {
        //                    seleccionSKU = seleccion;
        //                }
        //                else
        //                {
        //                    seleccionSKU = seleccion.Where(w => w.Sku_activo == sku).ToList();
        //                }

        //                vueltas++;

        //                //debo buscar los días cuyos values sean mayores acero para considerarlos como dias laborados en la semana
        //                var diasTrabajados = (from dt in seleccion
        //                                      join te in ctx.Turnos_activos_extras on cod_activo equals te.Cod_activo
        //                                      where dt.dia >= DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4))
        //                                      && dt.dia <= DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4))
        //                                      group dt by new { dt.dia, dt.turno, dt.semana } into dtt
        //                                      select new
        //                                      {
        //                                          dia = dtt.Key.dia,
        //                                          valor = dtt.Sum(s => s.value),
        //                                          turno = dtt.Max(m => m.turno),
        //                                          semana = dtt.Key.semana
        //                                      }).Where(w => w.turno != null).ToList();

        //                //DISPONIBILIDAD
        //                seleccion = (from iotX in seleccion
        //                                where iotX.turno != null && iotX.Minutos > 0
        //                                group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                                select new IOT
        //                                {
        //                                    id = mm.Max(m => m.id),
        //                                    dia = mm.Max(m => m.dia),
        //                                    timestamp = mm.Max(m => m.timestamp),
        //                                    //TO
        //                                    value =
        //                                     decimal.Parse(
        //                                     (((seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count()))).ToString("N2")),

        //                                    //TOP
        //                                    marca =
        //                                    ((seleccion.Where(w => w.dia.Month == mm.Key.Month).Max(m => m.Minutos) * (diasTrabajados.Where(w => w.dia.Month == mm.Key.Month).Count()))
        //                                        //Planificados
        //                                        - (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())).ToString("N2"),
        //                                    Cod_plan = ConvierteFecha(mm.Max(m => m.dia), "mes")
        //                                    //"Mes: " +
        //                                    //   (mm.Key.Month == 1 ? "Enero" :
        //                                    //        mm.Key.Month == 2 ? "Febrero" :
        //                                    //        mm.Key.Month == 3 ? "Marzo" :
        //                                    //        mm.Key.Month == 4 ? "Abril" :
        //                                    //        mm.Key.Month == 5 ? "Mayo" :
        //                                    //        mm.Key.Month == 6 ? "Junio" :
        //                                    //        mm.Key.Month == 7 ? "Julio" :
        //                                    //        mm.Key.Month == 8 ? "Agosto" :
        //                                    //        mm.Key.Month == 9 ? "Septiembre" :
        //                                    //        mm.Key.Month == 10 ? "Octubre" :
        //                                    //        mm.Key.Month == 11 ? "Noviembre" :
        //                                    //        mm.Key.Month == 12 ? "Diciembre" : "")
        //                                        }).OrderBy(o => o.timestamp.Month).ToList();


        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "semana")
        //        {
        //            //Calculo anterior
        //            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
        //            //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();

        //            //Calculo el número de semanas entre las dos fechas


        //            //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                if (vueltas == 0)
        //                {
        //                    if (sku == null)
        //                    {
        //                        seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                    }
        //                    else
        //                    {
        //                        seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
        //                    }

        //                }
        //                else
        //                {
        //                    if (sku == null)
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                    }
        //                    else
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
        //                    }


        //                }

        //                vueltas++;


        //                double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
        //                int separador = turno.IndexOf("|", 0, turno.Length);

        //                string anno1; // = turno.Substring(0, 4);
        //                string anno2; // = turno.Substring(separador+1, 4);

        //                try
        //                {
        //                    anno1 = turno.Substring(0, 4);
        //                    anno2 = turno.Substring(separador + 1, 4);
        //                }
        //                catch
        //                {
        //                    anno1 = "";
        //                    anno2 = "";
        //                }

        //                //int p1xxxx = turno.Length - (separador + 5);
        //                //string sem1 = turno.Substring(6, 2);
        //                //string sem2 = turno.Substring(separador+7, 2);

        //                int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
        //                int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                if (sfin >= sini)
        //                {
        //                    try
        //                    {
        //                        //debo buscar los días cuyos values sean mayores acero para considerarlos como dias laborados en la semana
        //                        var diasTrabajados = (from dt in seleccion
        //                                              join te in ctx.Turnos_activos_extras on cod_activo equals te.Cod_activo
        //                                              where dt.semana >= sini && dt.semana <= sfin && turno != null && dt.dia == te.Fecha_ini
        //                                              group dt by new { dt.dia, dt.turno, dt.semana } into dtt
        //                                              select new
        //                                              {
        //                                                  dia = dtt.Key.dia,
        //                                                  valor = dtt.Sum(s => s.value),
        //                                                  turno = dtt.Max(m => m.turno),
        //                                                  semana = dtt.Key.semana
        //                                              }).Where(w => w.turno != null).ToList();

        //                        seleccion = (from iotX in seleccion
        //                                     where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sini && iotX.semana <= sfin)
        //                                     group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                     select new IOT
        //                                     {
        //                                         id = mm.Max(m => m.id),
        //                                         dia = mm.Max(m => m.dia),
        //                                         timestamp = mm.Max(m => m.timestamp),
        //                                         semana = mm.Key.semana,
        //                                         //TO                                            
        //                                         value =
        //                                         decimal.Parse(//TO
        //                                             seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count().ToString("N2")),
        //                                         //TOP
        //                                         marca =
        //                                         (
        //                                              //Total minutos del mes                                             
        //                                              //TiempoXmes(PrimerDía(mm.Max(m => m.dia)), PrimerDía(mm.Max(m => m.dia)) == PrimerDía(DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"))) ?
        //                                              //DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")) : PrimerDía(mm.Max(m => m.dia)).AddDays(6), a.Cod_activo, null, idEmpresa)
        //                                              (seleccion.Where(w => w.semana == mm.Key.semana).Max(m => m.Minutos) *
        //                                              (diasTrabajados.Where(w => w.semana == mm.Key.semana).Count()))
        //                                              -
        //                                              //Planificados
        //                                              (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count())
        //                                          ).ToString("N2"),

        //                                         Cod_plan = FormatoSemana(mm.Key.semana, mm.Key.Year) //"Semana:" + mm.Key.semana.ToString()
        //                                     }).OrderBy(o => o.timestamp.Month).ToList();
        //                    }
        //                    catch (Exception ex)
        //                    { 

        //                    }

        //                }
        //                else
        //                {
        //                    //debo buscar los días cuyos values sean mayores acero para considerarlos como dias laborados en la semana
        //                    var diasTrabajados = (from dt in seleccion
        //                                          join te in ctx.Turnos_activos_extras on cod_activo equals te.Cod_activo
        //                                          where dt.semana >= sfin && dt.semana <= sini && turno != null && dt.dia == te.Fecha_ini
        //                                          group dt by new { dt.dia, dt.turno, dt.semana } into dtt
        //                                          select new
        //                                          {
        //                                              dia = dtt.Key.dia,
        //                                              valor = dtt.Sum(s => s.value),
        //                                              turno = dtt.Max(m => m.turno),
        //                                              semana = dtt.Key.semana
        //                                          }).Where(w => w.turno != null).ToList();

        //                    seleccion = (from iotX in seleccion
        //                                 where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sfin && iotX.semana <= sini)
        //                                 group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                 select new IOT
        //                                 {
        //                                     id = mm.Max(m => m.id),
        //                                     dia = mm.Max(m => m.dia),
        //                                     timestamp = mm.Max(m => m.timestamp),
        //                                     semana = mm.Key.semana,
        //                                     //TO                                            
        //                                     value =
        //                                     decimal.Parse(//TO
        //                                         seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count().ToString("N2")),
        //                                     //TOP
        //                                     marca =
        //                                     (
        //                                          //Total minutos del mes                                             
        //                                          //TiempoXmes(PrimerDía(mm.Max(m => m.dia)), PrimerDía(mm.Max(m => m.dia)) == PrimerDía(DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"))) ?
        //                                          //DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")) : PrimerDía(mm.Max(m => m.dia)).AddDays(6), a.Cod_activo, null, idEmpresa)
        //                                          (seleccion.Where(w => w.semana == mm.Key.semana).Max(m => m.Minutos) *
        //                                          (diasTrabajados.Where(w => w.semana == mm.Key.semana).Count()))
        //                                          -
        //                                          //Planificados
        //                                          (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count())
        //                                      ).ToString("N2"),

        //                                     Cod_plan = FormatoSemana(mm.Key.semana, mm.Key.Year) //"Semana:" + mm.Key.semana.ToString()
        //                                 }).OrderBy(o => o.timestamp.Month).ToList();
        //                }



        //            }
        //            catch (Exception ex)
        //            { }

        //        }
        //        else if (filtro == "dia")
        //        {
        //            try
        //            {
        //                //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id ")).ToListAsync();
        //                //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(case when total = 0 then 0 else cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) end as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(dia_fecha) dia_fecha from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, day(timestamp) dia_fecha, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select dia_fecha, SUM(tot_min) total from(select z.dia, dia_fecha, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, day(timestamp) dia_fecha	from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin group by day(timestamp),DATEPART(dw,timestamp), day(timestamp))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, dia_fecha)t group by dia_fecha) w on t.dia_fecha = w.dia_fecha")).ToListAsync();

        //                //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //                fini = fini.Substring(0, 8);
        //                ffin = ffin.Substring(0, 8);

        //                DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
        //                DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


        //                try
        //                {
        //                    string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";


        //                    if (vueltas == 0)
        //                    {
        //                        if (turno == "Todos" || turno == null)
        //                        {
        //                            if (sku == null)
        //                            {
        //                                try
        //                                {
        //                                    seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                                }
        //                                catch (Exception ex)
        //                                { 

        //                                }

        //                            }
        //                            else
        //                            {
        //                                seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
        //                            }

        //                            turno = null;
        //                        }
        //                        else
        //                        {
        //                            if (sku == null)
        //                            {
        //                                seleccion = await ctx.IOT.FromSql(p1).Where(w => w.turno == turno).ToListAsync();
        //                            }
        //                            else
        //                            {
        //                                seleccion = await ctx.IOT.FromSql(p1).Where(w => w.turno == turno).Where(w => w.Sku_activo == sku).ToListAsync();
        //                            }

        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (turno == "Todos" || turno == null)
        //                        {
        //                            if (sku == null)
        //                            {
        //                                try
        //                                {
        //                                    seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                                }
        //                                catch (Exception ex)
        //                                { 

        //                                }

        //                            }
        //                            else
        //                            {
        //                                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
        //                            }

        //                            turno = null;
        //                        }
        //                        else
        //                        {
        //                            if (sku == null)
        //                            {
        //                                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                            }
        //                            else
        //                            {
        //                                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
        //                            }

        //                            seleccion = seleccion.Where(w => w.turno == turno).ToList();
        //                        }

        //                        //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
        //                    }

        //                    vueltas++;

        //                    var diasTrabajados = (from dt in seleccion
        //                                          join te in ctx.Turnos_activos_extras on cod_activo equals te.Cod_activo
        //                                          where dt.dia >= Dfini && dt.dia <= Dffin && dt.turno != null && dt.dia == te.Fecha_ini && dt.turno == te.Cod_turno
        //                                          group dt by new { dt.dia, dt.turno } into dtt
        //                                          select new
        //                                          {
        //                                              dia = dtt.Key.dia,
        //                                              valor = dtt.Sum(s => s.value),
        //                                              turno = dtt.Max(m => m.turno)
        //                                              //timestamp = dt.timestamp,
        //                                              //turno = te.Cod_turno
        //                                          }).Where(w => w.turno != null).ToList();

        //                    //registro 131 del día 03/04
        //                    seleccion = (from iotX in seleccion
        //                                    where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                    group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                    select new IOT
        //                                    {
        //                                        id = mm.Max(m => m.id),
        //                                        dia = mm.Max(m => m.dia),
        //                                        timestamp = mm.Max(m => m.timestamp),
        //                                        //TO
        //                                        value =
        //                                        decimal.Parse((seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2")),
        //                                        //TOP
        //                                        marca =
        //                                        (
        //                                             (seleccion.Where(w => w.dia == mm.Key.dia).Max(m => m.Minutos) *
        //                                             (diasTrabajados.Where(w => w.dia == mm.Key.dia).Count()))
        //                                             -
        //                                             //Planificados
        //                                             (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count())
        //                                         ).ToString("N2"),
        //                                        Cod_plan = "Día:" + mm.Key.dia.ToString("dd-MM-yyyy")
        //                                    }).OrderBy(o => o.timestamp.Month).ToList();

        //                    //seleccion = (from iotX in seleccion
        //                    //             where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                    //             group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                    //             select new IOT_
        //                    //             {
        //                    //                 id = mm.Max(m => m.id),
        //                    //                 Dia = mm.Max(m => m.dia),
        //                    //                 Timestamp = mm.Max(m => m.timestamp),
        //                    //                 //TO
        //                    //                 ToX =
        //                    //                 decimal.Parse((seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2")),
        //                    //                 //TOP
        //                    //                 TopX =                                             
        //                    //                      (seleccion.Where(w => w.dia == mm.Key.dia).Max(m => m.Minutos) *
        //                    //                      (diasTrabajados.Where(w => w.dia == mm.Key.dia).Count()))
        //                    //                      -
        //                    //                      //Planificados
        //                    //                      (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count())
        //                    //                  ,
        //                    //                 Leyanda = "Día:" + mm.Key.dia.ToString("dd-MM-yyyy")
        //                    //             }).OrderBy(o => o.Timestamp.Month).ToList();



        //                }
        //                catch (Exception ex)
        //                { }
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "hora")
        //        {

        //            //##############################################################################################
        //            //##############################################################################################
        //            //##############################################################################################

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            //fini = fini; //.Substring(0, 8);
        //            //ffin = ffin.Substring(0, 8);

        //            fini = fini_.ToString("yyyyMMdd HH:mm");
        //            ffin = ffin_.ToString("yyyyMMdd HH:mm");

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                if (vueltas == 0)
        //                {
        //                    if (sku == null)
        //                    {
        //                        seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                    }
        //                    else
        //                    {
        //                        seleccion = await ctx.IOT.FromSql(p1).Where(w => w.Sku_activo == sku).ToListAsync();
        //                    }

        //                }
        //                else
        //                {
        //                    if (sku == null)
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                    }
        //                    else
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa).Where(w => w.Sku_activo == sku).ToList();
        //                    }

        //                }

        //                vueltas++;




        //                seleccion = (from iotX in seleccion
        //                                where iotX.turno != null && iotX.Minutos > 0
        //                                group iotX by new { iotX.timestamp.Hour } into mm
        //                                select new IOT
        //                                {
        //                                    id = mm.Max(m => m.id),
        //                                    dia = mm.Max(m => m.dia),
        //                                    timestamp = mm.Max(m => m.timestamp),
        //                                    //TO
        //                                    value =                                                                                       
        //                                    seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                    (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count(),
        //                                    //TOP
        //                                    marca =                                           
        //                                    //Total minutos del mes                                             
        //                                    (TiempoXmes(fini_, ffin_, cod_activo, "HH", idEmpresa) -
        //                                    //Planificados
        //                                    (seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0 && (w.dia == mm.Max(m => m.dia) &&
        //                                    w.timestamp.Hour == mm.Key.Hour)).Count())).ToString("N2"),
        //                                    Cod_plan = "Hora:" + mm.Key.Hour.ToString()
        //                                }).OrderBy(o => o.timestamp.Month).ToList();                           

        //            }
        //            catch (Exception ex)
        //            { }
        //        }                

        //        return seleccion;
        //    }

        //}

        ////Consulta para obtener lo datos de el rendimiento, esta consulta emplea el campo marca para obtener el valor del TOP y en value se obtiene el de TO
        ////Esta consulta busca obtener estos datos para adaptarse a cualquier filtro, se diferencia de la que calcula la disponibilidad por que trae el TO y el
        ////TOP por separado usado para el OEE
        //public async Task<List<IOT>> ConsultaRendimiento(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string sku, string cod_activo, string variable)
        //{           

        //    string fini = fini_.ToString("yyyyMMdd HH:mm");
        //    string ffin = ffin_.ToString("yyyyMMdd 23:59");

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        //var at = await ctx.Activos_tablas.ToListAsync();
        //        //agregado lv
        //        var at_aux = await ctx.Activos_tablas.ToListAsync();
        //        List<Activos_tablas> at = new List<Activos_tablas>();

        //        foreach (var a in at_aux)
        //        {
        //            if (a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CA" || a.Nombre_tabla.Substring(a.Nombre_tabla.Length - 2, 2) == "CI")
        //            {
        //                at.Add(a);
        //            }
        //        }
        //        //fin agregado lv
        //        List<IOT> iot = new List<IOT>();
        //        List<IOT> seleccion = new List<IOT>();
        //        //List<IOT> seleccionSKU= new List<IOT>();
        //        List<IOT> seleccionTodos = new List<IOT>();
        //        int capA = ctx.Capacidades_activos.Max(m => Decimal.ToInt32(m.Capacidad_maxima));

        //        if (filtro == "mes")
        //        {
        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";
        //                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

        //                if (sku == null)
        //                {

        //                    seleccion = (from i in seleccionTodos
        //                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                 from d in ag.DefaultIfEmpty()
        //                                 select new IOT
        //                                 {
        //                                     value = i.value,
        //                                     timestamp = i.timestamp,
        //                                     Sku_activo = i.Sku_activo,
        //                                     Cod_plan = i.Cod_plan,
        //                                     turno = i.turno,
        //                                     dia = i.dia,
        //                                     marca = i.marca,
        //                                     planificado = i.planificado,
        //                                     ndia = (d == null ?
        //                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                     Minutos = i.Minutos,
        //                                     semana = i.semana
        //                                 }).ToList();
        //                }
        //                else
        //                {

        //                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
        //                                 join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                 from d in ag.DefaultIfEmpty()
        //                                 select new IOT
        //                                 {
        //                                     value = i.value,
        //                                     timestamp = i.timestamp,
        //                                     Sku_activo = i.Sku_activo,
        //                                     Cod_plan = i.Cod_plan,
        //                                     turno = i.turno,
        //                                     dia = i.dia,
        //                                     marca = i.marca,
        //                                     planificado = i.planificado,
        //                                     ndia = (d == null ?
        //                                     capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                     Minutos = i.Minutos,
        //                                     semana = i.semana
        //                                 }).ToList();
        //                }

        //                //RENDIMIENTO
        //                seleccion = (from iotX in seleccion
        //                                where iotX.turno != null && iotX.Minutos > 0
        //                                group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                                select new IOT
        //                                {
        //                                    id = mm.Max(m => m.id),
        //                                    dia = mm.Max(m => m.dia),
        //                                    timestamp = mm.Max(m => m.timestamp),
        //                                    //TO
        //                                    value =
        //                                    //(seleccion.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                    //(w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count()),

        //                                    mm.Where(w => w.planificado == null).Sum(s => s.value /
        //                                     (ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo).Max(m => m.Capacidad_maxima))),

        //                                    //TOP
        //                                    marca =
        //                                    //sku == null ? (mm.Where(w => w.planificado == null).Sum(s => s.value/s.ndia)).ToString("N2") :
        //                                    //(mm.Where(w => w.planificado == null && w.Sku_activo == sku).Sum(s => s.value / s.ndia)).ToString("N2"),

        //                                    ((seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                    (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())).ToString("N2"),

        //                                    turno = //TO

        //                                        ((seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                         (w.dia.Year == mm.Key.Year && w.dia.Month == mm.Key.Month)).Count())).ToString("N2"),

        //                                    Cod_plan = ConvierteFecha(mm.Max(m => m.dia), "mes")
        //                                    // "Mes: " +                                             
        //                                    //(mm.Key.Month == 1  ? "Enero" :
        //                                    // mm.Key.Month == 2  ? "Febrero" :
        //                                    // mm.Key.Month == 3  ? "Marzo" :
        //                                    // mm.Key.Month == 4  ? "Abril" :
        //                                    // mm.Key.Month == 5  ? "Mayo" :
        //                                    // mm.Key.Month == 6  ? "Junio" :
        //                                    // mm.Key.Month == 7  ? "Julio" :
        //                                    // mm.Key.Month == 8  ? "Agosto" :
        //                                    // mm.Key.Month == 9  ? "Septiembre" :
        //                                    // mm.Key.Month == 10 ? "Octubre" :
        //                                    // mm.Key.Month == 11 ? "Noviembre" :
        //                                    // mm.Key.Month == 12 ? "Diciembre" : "")

        //                                }).OrderBy(o => o.timestamp.Month).ToList();

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "semana")
        //        {
        //            //Calculo anterior
        //            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
        //            //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


        //            //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

        //                if (sku == null)
        //                {
        //                    seleccion = (from i in seleccionTodos
        //                                    join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                    from d in ag.DefaultIfEmpty()
        //                                    select new IOT
        //                                    {
        //                                        value = i.value,
        //                                        timestamp = i.timestamp,
        //                                        Sku_activo = i.Sku_activo,
        //                                        Cod_plan = i.Cod_plan,
        //                                        turno = i.turno,
        //                                        dia = i.dia,
        //                                        marca = i.marca,
        //                                        planificado = i.planificado,
        //                                        ndia = (d == null ?
        //                                        capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                        Minutos = i.Minutos,
        //                                        semana = i.semana
        //                                    }).ToList();
        //                }
        //                else
        //                {
        //                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
        //                                    join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                    from d in ag.DefaultIfEmpty()
        //                                    select new IOT
        //                                    {
        //                                        value = i.value,
        //                                        timestamp = i.timestamp,
        //                                        Sku_activo = i.Sku_activo,
        //                                        Cod_plan = i.Cod_plan,
        //                                        turno = i.turno,
        //                                        dia = i.dia,
        //                                        marca = i.marca,
        //                                        planificado = i.planificado,
        //                                        ndia = (d == null ?
        //                                        capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                        Minutos = i.Minutos,
        //                                        semana = i.semana
        //                                    }).ToList();
        //                }



        //                double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
        //                int separador = turno.IndexOf("|", 0, turno.Length);

        //                string anno1; // = turno.Substring(0, 4);
        //                string anno2; // = turno.Substring(separador+1, 4);

        //                try
        //                {
        //                    anno1 = turno.Substring(0, 4);
        //                    anno2 = turno.Substring(separador + 1, 4);
        //                }
        //                catch
        //                {
        //                    anno1 = "";
        //                    anno2 = "";
        //                }

        //                //int p1xxxx = turno.Length - (separador + 5);
        //                //string sem1 = turno.Substring(6, 2);
        //                //string sem2 = turno.Substring(separador+7, 2);

        //                int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
        //                int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                //int sini = int.Parse(turno.Substring(0, separador));
        //                //int sfin = int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                if (sfin >= sini)
        //                {
        //                    seleccion = (from iotX in seleccion
        //                                 where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sini && iotX.semana <= sfin)
        //                                 group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                 select new IOT
        //                                 {
        //                                     id = mm.Max(m => m.id),
        //                                     dia = mm.Max(m => m.dia),
        //                                     timestamp = mm.Max(m => m.timestamp),
        //                                     semana = mm.Key.semana,
        //                                     //TO
        //                                     value =
        //                                     //(seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                     //(w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()),

        //                                     mm.Where(w => w.planificado == null).Sum(s => s.value /
        //                                      ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo).Max(m => m.Capacidad_maxima)),

        //                                     //TOP
        //                                     marca =
        //                                         //(mm.Where(w => w.planificado == null).Sum(s => s.value / s.ndia)).ToString("N2"),
        //                                         //Cod_plan = "Semana:" + mm.Key.semana.ToString(),
        //                                         (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                         (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()).ToString("N2"),

        //                                     turno = //TO
        //                                          (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                          (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()).ToString("N2"),

        //                                     Cod_plan = FormatoSemana(mm.Key.semana, mm.Key.Year) //"Semana:" + mm.Key.semana.ToString()
        //                                 }).OrderBy(o => o.timestamp.Month).ToList();
        //                }
        //                else
        //                {
        //                    seleccion = (from iotX in seleccion
        //                                 where (iotX.turno != null && iotX.Minutos > 0) && (iotX.semana >= sfin && iotX.semana <= sini)
        //                                 group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                                 select new IOT
        //                                 {
        //                                     id = mm.Max(m => m.id),
        //                                     dia = mm.Max(m => m.dia),
        //                                     timestamp = mm.Max(m => m.timestamp),
        //                                     semana = mm.Key.semana,
        //                                     //TO
        //                                     value =
        //                                     //(seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                     //(w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()),

        //                                     mm.Where(w => w.planificado == null).Sum(s => s.value /
        //                                      ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo).Max(m => m.Capacidad_maxima)),

        //                                     //TOP
        //                                     marca =
        //                                         //(mm.Where(w => w.planificado == null).Sum(s => s.value / s.ndia)).ToString("N2"),
        //                                         //Cod_plan = "Semana:" + mm.Key.semana.ToString(),
        //                                         (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                         (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()).ToString("N2"),

        //                                     turno = //TO
        //                                          (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                          (w.dia.Year == mm.Key.Year && w.semana == mm.Key.semana)).Count()).ToString("N2"),

        //                                     Cod_plan = FormatoSemana(mm.Key.semana, mm.Key.Year) //"Semana:" + mm.Key.semana.ToString()
        //                                 }).OrderBy(o => o.timestamp.Month).ToList();
        //                }
        //            }
        //            catch (Exception ex)
        //            { }

        //        }
        //        else if (filtro == "dia")
        //        {
        //            try
        //            {
        //                //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //                fini = fini.Substring(0, 8);
        //                ffin = ffin.Substring(0, 8);

        //                DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
        //                DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


        //                try
        //                {
        //                    string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";



        //                    if (turno == "Todos" || turno == null)
        //                    {
        //                        seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

        //                        if (sku == null)
        //                        {
        //                            seleccion = (from i in seleccionTodos
        //                                            join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                            from d in ag.DefaultIfEmpty()
        //                                            select new IOT
        //                                            {
        //                                                value = i.value,
        //                                                timestamp = i.timestamp,
        //                                                Sku_activo = i.Sku_activo,
        //                                                Cod_plan = i.Cod_plan,
        //                                                turno = i.turno,
        //                                                dia = i.dia,
        //                                                marca = i.marca,
        //                                                planificado = i.planificado,
        //                                                ndia = (d == null ?
        //                                                capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                                Minutos = i.Minutos,
        //                                                semana = i.semana
        //                                            }).ToList();
        //                        }
        //                        else
        //                        {
        //                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
        //                                            join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                            from d in ag.DefaultIfEmpty()
        //                                            select new IOT
        //                                            {
        //                                                value = i.value,
        //                                                timestamp = i.timestamp,
        //                                                Sku_activo = i.Sku_activo,
        //                                                Cod_plan = i.Cod_plan,
        //                                                turno = i.turno,
        //                                                dia = i.dia,
        //                                                marca = i.marca,
        //                                                planificado = i.planificado,
        //                                                ndia = (d == null ?
        //                                                capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                                Minutos = i.Minutos,
        //                                                semana = i.semana
        //                                            }).ToList();
        //                        }

        //                        turno = null;
        //                    }
        //                    else
        //                    {
        //                        seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

        //                        if (sku == null)
        //                        {
        //                            seleccion = (from i in seleccionTodos
        //                                            join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                            from d in ag.DefaultIfEmpty()
        //                                            where i.turno == turno
        //                                            select new IOT
        //                                            {
        //                                                value = i.value,
        //                                                timestamp = i.timestamp,
        //                                                Sku_activo = i.Sku_activo,
        //                                                Cod_plan = i.Cod_plan,
        //                                                turno = i.turno,
        //                                                dia = i.dia,
        //                                                marca = i.marca,
        //                                                planificado = i.planificado,
        //                                                ndia = (d == null ?
        //                                                capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                                Minutos = i.Minutos,
        //                                                semana = i.semana
        //                                            }).ToList();
        //                        }
        //                        else
        //                        {
        //                            seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
        //                                            join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                            from d in ag.DefaultIfEmpty()
        //                                            where i.turno == turno
        //                                            select new IOT
        //                                            {
        //                                                value = i.value,
        //                                                timestamp = i.timestamp,
        //                                                Sku_activo = i.Sku_activo,
        //                                                Cod_plan = i.Cod_plan,
        //                                                turno = i.turno,
        //                                                dia = i.dia,
        //                                                marca = i.marca,
        //                                                planificado = i.planificado,
        //                                                ndia = (d == null ?
        //                                                capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                                Minutos = i.Minutos,
        //                                                semana = i.semana
        //                                            }).ToList();
        //                        }

        //                    }

        //                    decimal Capacidad = 0;
        //                    try
        //                    {
        //                        Capacidad = ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo).Max(m => m.Capacidad_maxima);
        //                    }
        //                    catch (Exception ex)
        //                    { 

        //                    }


        //                    //Falta asociar la capacidad al sku que tiene en la tabla de los datos del SQL

        //                    //registro 131 del día 03/04
        //                    seleccion = (from iotX in seleccion
        //                                    where (iotX.turno != null && iotX.Minutos > 0) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                    group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                    select new IOT
        //                                    {
        //                                        id = mm.Max(m => m.id),
        //                                        dia = mm.Max(m => m.dia),
        //                                        timestamp = mm.Max(m => m.timestamp),
        //                                        //TO
        //                                        value =
        //                                        //(seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                        //(w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()),
        //                                        Capacidad == 0 ? 0 :
        //                                        mm.Where(w => w.planificado == null).Sum(s => s.value / Capacidad),

        //                                        //TOP
        //                                        marca =
        //                                        //(mm.Where(w => w.planificado == null).Sum(s => s.value / s.ndia)).ToString("N2")
        //                                         (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                         (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2")

        //                                        ,
        //                                        turno = //TO
        //                                         (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                         (w.dia.Year == mm.Key.Year && w.dia == mm.Key.dia)).Count()).ToString("N2"),

        //                                        Minutos = mm.Max(s => s.ndia),
        //                                        Cod_plan = "Día:" + mm.Key.dia.ToString("dd-MM-yyyy")
        //                                    }).OrderBy(o => o.timestamp.Month).ToList();

        //                }
        //                catch (Exception ex)
        //                { }
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "hora")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            //fini = fini; //.Substring(0, 8);
        //            //ffin = ffin.Substring(0, 8);

        //            fini = fini_.ToString("yyyyMMdd HH:mm");
        //            ffin = ffin_.ToString("yyyyMMdd HH:mm");

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                seleccionTodos = await ctx.IOT.FromSql(p1).ToListAsync();

        //                if (sku == null)
        //                {
        //                    seleccion = (from i in seleccionTodos
        //                                    join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                    from d in ag.DefaultIfEmpty()
        //                                    select new IOT
        //                                    {
        //                                        value = i.value,
        //                                        timestamp = i.timestamp,
        //                                        Sku_activo = i.Sku_activo,
        //                                        Cod_plan = i.Cod_plan,
        //                                        turno = i.turno,
        //                                        dia = i.dia,
        //                                        marca = i.marca,
        //                                        planificado = i.planificado,
        //                                        ndia = (d == null ?
        //                                        capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                        Minutos = i.Minutos,
        //                                        semana = i.semana
        //                                    }).ToList();
        //                }
        //                else
        //                {
        //                    seleccion = (from i in seleccionTodos.Where(w => w.Sku_activo == sku)
        //                                    join c in ctx.Capacidades_activos.Where(w => w.Cod_activo == cod_activo) on i.Sku_activo equals c.Cod_producto into ag
        //                                    from d in ag.DefaultIfEmpty()
        //                                    select new IOT
        //                                    {
        //                                        value = i.value,
        //                                        timestamp = i.timestamp,
        //                                        Sku_activo = i.Sku_activo,
        //                                        Cod_plan = i.Cod_plan,
        //                                        turno = i.turno,
        //                                        dia = i.dia,
        //                                        marca = i.marca,
        //                                        planificado = i.planificado,
        //                                        ndia = (d == null ?
        //                                        capA : Decimal.ToInt32(d.Capacidad_maxima)),
        //                                        Minutos = i.Minutos,
        //                                        semana = i.semana
        //                                    }).ToList();
        //                }



        //                seleccion = (from iotX in seleccion
        //                                where iotX.turno != null && iotX.Minutos > 0
        //                                group iotX by new { iotX.timestamp.Hour } into mm
        //                                select new IOT
        //                                {
        //                                    id = mm.Max(m => m.id),
        //                                    dia = mm.Max(m => m.dia),
        //                                    timestamp = mm.Max(m => m.timestamp),
        //                                    //TO
        //                                    value =
        //                                    (seleccionTodos.Where(w => w.planificado == null && w.turno != null && w.Minutos > 0 && w.value > 0 &&
        //                                    (w.dia == mm.Max(m => m.dia) && w.timestamp.Hour == mm.Key.Hour)).Count()),
        //                                    //TOP
        //                                    marca =
        //                                    (mm.Where(w => w.planificado == null).Sum(s => s.value / s.ndia)).ToString("N2"),
        //                                    Cod_plan = "Hora:" + mm.Key.Hour.ToString()
        //                                }).OrderBy(o => o.timestamp.Month).ToList();


        //            }
        //            catch (Exception ex)
        //            { }
        //        }                

        //        return seleccion;
        //    }

        //}

        #endregion Calculos de disponibilidad y rendimiento para el OEE viejos

        #region Metodo antiguo para calculo de unidades producidas
        //[Microsoft.AspNetCore.Authorization.Authorize] //Metodo antiguo para calculo de unidades producidas
        //public async Task<List<Consolidado>> Historicos_variables_consolidado(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string maquina, string variable, string unidad)
        //{
        //    List<SeriesUnidades> series = new List<SeriesUnidades>();

        //    string fini = fini_.ToString("yyyyMMdd HH:mm");
        //    string ffin = ffin_.ToString("yyyyMMdd 23:59");

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        var at = await ctx.Activos_tablas.ToListAsync();
        //        List<IOT> iot = new List<IOT>();
        //        List<IOT> seleccion = new List<IOT>();
        //        List<IOT> seleccion2 = new List<IOT>();
        //        List<IOT> seleccion3 = new List<IOT>();
        //        List<IOT> seleccion4 = new List<IOT>();
        //        List<IOT> seleccion5 = new List<IOT>();

        //        int vueltas = 0;

        //        if (filtro == "mes")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                //string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

        //                //seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);

        //                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);

        //                var totales0 = (from sel in seleccion                                        
        //                                join act in ctx.Activos on maquina equals act.Cod_activo
        //                                where sel.value > 0 && sel.planificado == null
        //                                select new { 
        //                                    enTurno  = sel.turno != null ? sel.value : 0,
        //                                    sinTurno = sel.turno == null ? sel.value : 0,
        //                                    nombreActivo = act.Cod_activo + " - " + act.Des_activo,
        //                                    dia = sel.dia,
        //                                    fecha = sel.dia.Month,
        //                                    total = sel.value
        //                                }).ToList();

        //                List<Consolidado> totalesX = (from t in totales0
        //                                    group t by new { t.dia.Year, t.dia.Month } into tt
        //                                    select new Consolidado
        //                                    {
        //                                        enTurnox  = tt.Sum(s => s.enTurno),
        //                                        sinTurno = tt.Sum(s => s.sinTurno),
        //                                        nombreActivo = tt.Max(m => m.nombreActivo),
        //                                        fecha = ConvierteFecha(tt.Max(m => m.dia), "mes")
        //                                        //fecha = tt.Max(m => m.fecha) == 1 ? "Enero" :
        //                                        //        tt.Max(m => m.fecha) == 2 ? "Febrero" :
        //                                        //        tt.Max(m => m.fecha) == 3 ? "Marzo" :
        //                                        //        tt.Max(m => m.fecha) == 4 ? "Abril" :
        //                                        //        tt.Max(m => m.fecha) == 5 ? "Mayo" :
        //                                        //        tt.Max(m => m.fecha) == 6 ? "Junio" :
        //                                        //        tt.Max(m => m.fecha) == 7 ? "Julio" :
        //                                        //        tt.Max(m => m.fecha) == 8 ? "Agosto" :
        //                                        //        tt.Max(m => m.fecha) == 9 ? "Septiembre" :
        //                                        //        tt.Max(m => m.fecha) == 10 ? "Octubre" :
        //                                        //        tt.Max(m => m.fecha) == 11 ? "Noviembre" :
        //                                        //        tt.Max(m => m.fecha) == 12 ? "Diciembre" : "",
        //                                        //total = tt.Sum(s => s.total)
        //                                    }).ToList();

        //                //group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm

        //                return totalesX;

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "semana")
        //        {
        //            //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";

        //                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);

        //                double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
        //                int separador = turno.IndexOf("|", 0, turno.Length);

        //                string anno1; // = turno.Substring(0, 4);
        //                string anno2; // = turno.Substring(separador+1, 4);

        //                try
        //                {
        //                    anno1 = turno.Substring(0, 4);
        //                    anno2 = turno.Substring(separador + 1, 4);
        //                }
        //                catch
        //                {
        //                    anno1 = "";
        //                    anno2 = "";
        //                }

        //                int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
        //                int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                var totales0 = (from sel in seleccion
        //                                join act in ctx.Activos on maquina equals act.Cod_activo
        //                                where sel.value > 0 && sel.planificado == null && (sel.semana >= sini && sel.semana <= sfin)
        //                                select new
        //                                {
        //                                    enTurno = sel.turno != null ? sel.value : 0,
        //                                    sinTurno = sel.turno == null ? sel.value : 0,
        //                                    nombreActivo = act.Cod_activo + " - " + act.Des_activo,
        //                                    dia = sel.dia,
        //                                    semana = sel.semana,
        //                                    fecha = sel.dia.Month,
        //                                    total = sel.value
        //                                }).ToList();

        //                List<Consolidado> totalesX = (from t in totales0
        //                                              group t by new { t.dia.Year, t.semana} into tt
        //                                                select new Consolidado
        //                                                {
        //                                                    enTurnox = tt.Sum(s => s.enTurno),
        //                                                    sinTurno = tt.Sum(s => s.sinTurno),
        //                                                    nombreActivo = tt.Max(m => m.nombreActivo),
        //                                                    fecha = FormatoSemana(tt.Key.semana, tt.Key.Year),
        //                                                    total = tt.Sum(s => s.total)
        //                                                }).ToList();

        //                return totalesX;

        //            }
        //            catch (Exception ex)
        //            { }

        //        }
        //        else if (filtro == "dia")
        //        {
        //            try
        //            {
        //                //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //                fini = fini.Substring(0, 8);
        //                ffin = ffin.Substring(0, 8);

        //                DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
        //                DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));

        //                try
        //                {
        //                    string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";



        //                    if (turno == "Todos" || turno == null)
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
        //                        turno = null;
        //                    }
        //                    else
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);
        //                        seleccion = seleccion.Where(w => w.turno == turno).ToList();
        //                    }

        //                    var totales0 = (from sel in seleccion
        //                                    join act in ctx.Activos on maquina equals act.Cod_activo
        //                                    where sel.value > 0 && sel.planificado == null && (sel.dia >= Dfini && sel.dia <= Dffin)
        //                                    select new
        //                                    {
        //                                        enTurno = sel.turno != null ? sel.value : 0,
        //                                        sinTurno = sel.turno == null ? sel.value : 0,
        //                                        nombreActivo = act.Cod_activo + " - " + act.Des_activo,
        //                                        dia = sel.dia,
        //                                        fecha = sel.dia.Month,
        //                                        total = sel.value
        //                                    }).ToList();

        //                    List<Consolidado> totalesX = (from t in totales0
        //                                                    group t by new { t.dia.Year, t.dia } into tt
        //                                                    select new Consolidado
        //                                                    {
        //                                                        enTurnox = tt.Sum(s => s.enTurno),
        //                                                        sinTurno = tt.Sum(s => s.sinTurno),
        //                                                        nombreActivo = tt.Max(m => m.nombreActivo),
        //                                                        fecha = tt.Key.dia.ToString("dd/MM/yyyy"),
        //                                                        total = tt.Sum(s => s.total)
        //                                                    }).ToList();

        //                    return totalesX;
        //                }
        //                catch (Exception ex)
        //                { }
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "hora")
        //        {
        //            fini = fini_.ToString("yyyyMMdd HH:mm");
        //            ffin = ffin_.ToString("yyyyMMdd HH:mm");

        //            try
        //            {
        //                string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + maquina + "','" + variable + "'";


        //                seleccion = ListadoIOT(fini, ffin, maquina, variable, idEmpresa);

        //                var totales0 = (from sel in seleccion
        //                                join act in ctx.Activos on maquina equals act.Cod_activo
        //                                where sel.value > 0 && sel.planificado == null 
        //                                select new
        //                                {
        //                                    enTurno = sel.turno != null ? sel.value : 0,
        //                                    sinTurno = sel.turno == null ? sel.value : 0,
        //                                    nombreActivo = act.Cod_activo + " - " + act.Des_activo,
        //                                    dia = sel.dia,
        //                                    fecha = sel.dia.Month,
        //                                    timestamp = sel.timestamp,
        //                                    total = sel.value
        //                                }).ToList();

        //                List<Consolidado> totalesX = (from t in totales0
        //                                                group t by new { t.timestamp.Hour } into tt
        //                                                select new Consolidado
        //                                                {
        //                                                    enTurnox = tt.Sum(s => s.enTurno),
        //                                                    sinTurno = tt.Sum(s => s.sinTurno),
        //                                                    nombreActivo = tt.Max(m => m.nombreActivo),
        //                                                    fecha = tt.Max(m => m.timestamp).ToString("dd/MM/yyyy HH:mm"), //tt.Key.Hour.ToString(),
        //                                                    total = tt.Sum(s => s.total)
        //                                                }).ToList();

        //                return totalesX;

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        //}

        //    }

        //    List<Consolidado> ccc = new List<Consolidado>();

        //    return ccc;
        //}

        //Metodo tiempos para gestion KPI's
        #endregion

        #region Consolidado de unidades producidas

        //public async Task<object> ConsolidadoKpi01(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string maquina, string variable, string unidad)
        //{
        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        //List<JsonResult> res = new List<JsonResult>();
        //        List<ActivosVariables> av = Lista_activos2(idEmpresa).Result;
        //        List<Consolidado> Consol = new List<Consolidado>();

        //        foreach (var a in av)
        //        {
        //            try
        //            {
        //                List<Consolidado> hv = Historicos_variables_consolidado(idEmpresa, fini_, ffin_, filtro, turno, a.Cod_activo, a.Variable, a.Unidad).Result;

        //                string ff = "";

        //                switch (filtro)
        //                {
        //                    case "dia":
        //                        if (turno != null)
        //                        {
        //                            ff = "Día - Turno: " + turno; 
        //                        }
        //                        else
        //                            ff = "Día";
        //                        break;
        //                    case "mes":
        //                        ff = "Mes";
        //                        break;
        //                    case "semana":
        //                        ff = "Semana";
        //                        break;
        //                    case "hora":
        //                        ff = "Fecha y hora";
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                foreach (var c in hv)
        //                {
        //                    Consol.Add(new Consolidado()
        //                    {
        //                        fecha = c.fecha,
        //                        turno = filtro == "semana" ? "" : turno,
        //                        nombreActivo = c.nombreActivo,
        //                        enTurnox = c.enTurnox,
        //                        sinTurno = c.sinTurno,
        //                        total = c.total,
        //                        unidades = ff
        //                    });
        //                }                   

        //            }
        //            catch (Exception ex)
        //            { 

        //            }                   
        //        }

        //        return Consol;
        //    }


        //}

        //Consolidado de Análisis de tiempos

        #endregion

        #region Metodo contador de unides para gestion KPI's

        //Metodo contador de unides para gestion KPI's
        //[Microsoft.AspNetCore.Authorization.Authorize]
        //public async Task<List<SeriesUnidades>> Historicos_variables(int idEmpresa, DateTime fini_, DateTime ffin_, string filtro, string turno, string cod_activo, string variable, string unidad)
        //{
        //    List<SeriesUnidades> series = new List<SeriesUnidades>();

        //    string fini = fini_.ToString("yyyyMMdd HH:mm");
        //    string ffin = ffin_.ToString("yyyyMMdd 23:59");

        //    string ff = "";

        //    switch (filtro)
        //    {
        //        case "dia":
        //            ff = "Día";
        //            break;
        //        case "mes":
        //            ff = "Mes";
        //            break;
        //        case "semana":
        //            ff = "Semana";
        //            break;
        //        case "hora":
        //            ff = "Hora";
        //            break;
        //        default:
        //            break;
        //    }

        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        //var prueba = (ctx.IOT.FromSql("Exec sp_prueba")).ToListAsync();

        //        var at = await ctx.Activos_tablas.ToListAsync();
        //        //var pro = await ctx.Productos.ToListAsync();
        //        List<IOT> iot = new List<IOT>();
        //        List<IOT> seleccion = new List<IOT>();
        //        List<IOT> seleccion2 = new List<IOT>();
        //        List<IOT> seleccion3 = new List<IOT>();
        //        List<IOT> seleccion4 = new List<IOT>();
        //        List<IOT> seleccion5 = new List<IOT>();

        //        string NombreActivo = ctx.Activos.Where(w => w.Cod_activo == cod_activo).Select(s => s.Cod_activo + " - " + s.Des_activo).FirstOrDefault();

        //        int vueltas = 0;


        //        if (filtro == "mes")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                //string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;
        //                //seleccion = await ctx.IOT.FromSql(p1).ToListAsync();
        //                //seleccion2 = await ctx.IOT.FromSql(p1).ToListAsync();


        //                #region código comentado viejo

        //                //##############################################
        //                //Calculo el total de los minutos del mes inicio
        //                //##############################################
        //                //var dXmes = (from dia in seleccion
        //                //             group dia by new { dia.ndia, dia.timestamp.Month } into m
        //                //             select new {
        //                //                 mes = m.Key
        //                //             }).ToList();


        //                //lun = lun * ((seleccion.Where(w => w.ndia == 2).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 2).Max(m => m.Minutos)));
        //                //mar = mar * ((seleccion.Where(w => w.ndia == 3).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 3).Max(m => m.Minutos)));
        //                //mie = mie * ((seleccion.Where(w => w.ndia == 4).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 4).Max(m => m.Minutos)));
        //                //jue = jue * ((seleccion.Where(w => w.ndia == 5).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 5).Max(m => m.Minutos)));
        //                //vie = vie * ((seleccion.Where(w => w.ndia == 6).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 6).Max(m => m.Minutos)));
        //                //sab = sab * ((seleccion.Where(w => w.ndia == 7).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 7).Max(m => m.Minutos)));
        //                //dom = dom * ((seleccion.Where(w => w.ndia == 1).Count()) == 0 ? 0 : (seleccion.Where(w => w.ndia == 1).Max(m => m.Minutos)));
        //                //decimal totalMes = lun + mar + mie + jue + vie + sab + dom;

        //                //##############################################
        //                //Calculo el total de los minutos del mes fin
        //                //##############################################

        //                //##########################################################################
        //                //Busco el total de minutos de paradas planificadas y no planificadas inicio
        //                //##########################################################################
        //                //var paradas = await (from i in ctx.Incidencias
        //                //                     join t in ctx.Tipos_incidencia on i.Cod_tipo equals t.Cod_tipo
        //                //                     where i.Desde >= fini_ && i.Hasta <= ffin_ && 
        //                //                     i.Hasta != null && t.Planificado == true
        //                //                     select new
        //                //                     {
        //                //                         minutos = ((i.Hasta ?? DateTime.Parse("01-01-1900")) - i.Desde).TotalMinutes
        //                //                     }).SumAsync(s => s.minutos);                            
        //                //##########################################################################
        //                //Busco el total de minutos de paradas planificadas y no planificadas fin
        //                //##########################################################################



        //                //int Planificados = seleccion.Where(w => w.planificado == true && w.turno != null && w.Minutos > 0).Count();
        //                //int NoPlanificados = seleccion.Where(w => w.planificado == false && w.turno != null && w.Minutos > 0).Count();
        //                //int TO = seleccion.Where(w => w.planificado != false && w.turno != null && w.Minutos > 0).Count();

        //                #endregion

        //                seleccion = (from iotX in seleccion               //inturno
        //                             where iotX.value > 0 && iotX.planificado == null && iotX.turno != null
        //                             group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value = mm.Sum(m => m.value),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 dia = mm.Max(m => m.dia),
        //                                 marca = cod_activo,
        //                                 turno = "Unidades" //unidad //Uso el turno para guardar en este caso las unidades
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2          //Sin turno asignado
        //                              where iotX.value > 0 && iotX.planificado == null && iotX.turno == null
        //                              group iotX by new { iotX.dia.Year, iotX.dia.Month } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  dia = mm.Max(m => m.dia),
        //                                  turno = turno
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion3 = (from iotX in seleccion3
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4
        //                              join pr in ctx.Productos on iotX.Sku_activo equals pr.Cod_producto
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  Sku_activo = mm.Max(m => m.Sku_activo)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion5 = (from act in ctx.Activos_tablas
        //                              where act.Cod_activo == cod_activo
        //                              select new IOT
        //                              {
        //                                  Sku_activo = act.Unidad
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                try
        //                {
        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                        tiempo = seleccion.Select(s => ConvierteFecha(s.dia,"mes")).ToArray(),
        //                        //seleccion.Select(s => s.dia.Month == 1 ? "01 Ene " + s.dia.Year :
        //                        //                                s.dia.Month == 2 ? "02 Feb " + s.dia.Year :
        //                        //                                s.dia.Month == 3 ? "03 Mar " + s.dia.Year :
        //                        //                                s.dia.Month == 4 ? "04 Abr " + s.dia.Year :
        //                        //                                s.dia.Month == 5 ? "05 May " + s.dia.Year :
        //                        //                                s.dia.Month == 6 ? "06 Jun " + s.dia.Year :
        //                        //                                s.dia.Month == 7 ? "07 Jul " + s.dia.Year :
        //                        //                                s.dia.Month == 8 ? "08 Ago " + s.dia.Year :
        //                        //                                s.dia.Month == 9 ? "09 Sep " + s.dia.Year :
        //                        //                                s.dia.Month == 10 ? "10 Oct " + s.dia.Year :
        //                        //                                s.dia.Month == 11 ? "11 Nov " + s.dia.Year : 
        //                        //                                "12 Dic " + s.dia.Year).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => ConvierteFecha(s.dia, "mes")).ToArray(),

        //                        //seleccion2.Select(s => s.dia.Month == 1 ? "01 Ene " + s.dia.Year :
        //                        //                                s.dia.Month == 2 ? "02 Feb " + s.dia.Year :
        //                        //                                s.dia.Month == 3 ? "03 Mar " + s.dia.Year :
        //                        //                                s.dia.Month == 4 ? "04 Abr " + s.dia.Year :
        //                        //                                s.dia.Month == 5 ? "05 May " + s.dia.Year :
        //                        //                                s.dia.Month == 6 ? "06 Jun " + s.dia.Year :
        //                        //                                s.dia.Month == 7 ? "07 Jul " + s.dia.Year :
        //                        //                                s.dia.Month == 8 ? "08 Ago " + s.dia.Year :
        //                        //                                s.dia.Month == 9 ? "09 Sep " + s.dia.Year :
        //                        //                                s.dia.Month == 10 ? "10 Oct " + s.dia.Year :
        //                        //                                s.dia.Month == 11 ? "11 Nov " + s.dia.Year : 
        //                        //                                "12 Dic " + s.dia.Year).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        data = seleccion.Select(s => s.value).ToArray(),
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = seleccion3.Select(s => s.value).ToArray(),
        //                        unidades = seleccion.Select(s => s.turno).ToArray(),//Uso el turno para guardar en este caso las unidades   //seleccion5.Select(m => m.Sku_activo).ToArray(),
        //                        nombreActivo = NombreActivo,
        //                        filtro = ff,
        //                        tiempo3 = seleccion.Select(s => "").ToArray()
        //                    });
        //                }
        //                catch (Exception ex)
        //                { }

        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "semana")
        //        {
        //            //Calculo anterior
        //            //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id")).ToListAsync();
        //            //ültima //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(semana) semana from (select id, value, 'Sem-' + cast(format(cast(DATEPART(week, dateadd(DAY, -1, timestamp)) as int), '0#') as varchar(255)) + '-' + cast(year(timestamp) as varchar(255)) tiempo, DATEPART(week, dateadd(DAY, -1, timestamp)) semana, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo, case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select semana, SUM(tot_min) total from(select z.dia, semana, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, DATEPART(week, dateadd(DAY, -1, timestamp)) semana from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin	group by day(timestamp),DATEPART(dw,timestamp), DATEPART(week, dateadd(DAY, -1, timestamp)))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, semana)t group by semana) w on t.semana = w.semana")).ToListAsync();


        //            //Como es semanal no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            fini = fini.Substring(0, 8);
        //            ffin = ffin.Substring(0, 8);

        //            try
        //            {
        //                //string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";

        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;


        //                double semanas = ((ffin_ - fini_).TotalDays + 1) / 7;
        //                int separador = turno.IndexOf("|", 0, turno.Length);

        //                string anno1; // = turno.Substring(0, 4);
        //                string anno2; // = turno.Substring(separador+1, 4);

        //                try
        //                {
        //                    anno1 = turno.Substring(0, 4);
        //                    anno2 = turno.Substring(separador + 1, 4);
        //                }
        //                catch
        //                {
        //                    anno1 = "";
        //                    anno2 = "";
        //                }

        //                //int p1xxxx = turno.Length - (separador + 5);
        //                //string sem1 = turno.Substring(6, 2);
        //                //string sem2 = turno.Substring(separador+7, 2);

        //                int sini = int.Parse(turno.Substring(6, 2)); //int.Parse(turno.Substring(0, separador));
        //                int sfin = int.Parse(turno.Substring(separador + 7, 2)); //int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                //int sini = int.Parse(turno.Substring(0, separador));
        //                //int sfin = int.Parse(turno.Substring(separador + 1, turno.Length - (separador + 1)));

        //                seleccion = (from iotX in seleccion
        //                             where iotX.value > 0 && iotX.planificado == null && iotX.turno != null && (iotX.semana >= sini && iotX.semana <= sfin)
        //                             group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value = mm.Sum(m => m.value),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = FormatoSemana(mm.Key.semana,mm.Key.Year), //"Semana " + mm.Key.semana.ToString() + " - " + mm.Max(m => m.dia.Year),//mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 marca = cod_activo,
        //                                 turno = "Unidades" //unidad //Uso el turno para guardar en este caso las unidades
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2
        //                              where iotX.value > 0 && iotX.planificado == null && iotX.turno == null && (iotX.semana >= sini && iotX.semana <= sfin)
        //                              group iotX by new { iotX.dia.Year, iotX.semana } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  Sku_activo = FormatoSemana(mm.Key.semana, mm.Key.Year),
        //                                  turno = turno
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion3 = (from iotX in seleccion3
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4
        //                              join pr in ctx.Productos on iotX.Sku_activo equals pr.Cod_producto
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  Sku_activo = mm.Max(m => m.Sku_activo)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();
        //                seleccion5 = (from act in ctx.Activos_tablas
        //                              where act.Cod_activo == cod_activo
        //                              select new IOT
        //                              {
        //                                  Sku_activo = act.Unidad
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                series.Add(new SeriesUnidades()
        //                {
        //                    id = seleccion.Max(s => s.id),
        //                    fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                    //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                    tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                    tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
        //                    data = seleccion.Select(s => s.value).ToArray(),
        //                    data2 = seleccion2.Select(s => s.value).ToArray(),
        //                    activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                    cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                    sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                    sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                    sku_conteo_total = seleccion3.Select(s => s.value).ToArray(),
        //                    unidades = seleccion.Select(s => s.turno).ToArray(), //seleccion5.Select(m => m.Sku_activo).ToArray(),
        //                    nombreActivo = NombreActivo,
        //                    tiempo3 = seleccion.Select(s => "").ToArray(),
        //                    filtro = ff
        //                });

        //            }
        //            catch (Exception ex)
        //            { }

        //        }
        //        else if (filtro == "dia")
        //        {
        //            try
        //            {
        //                //seleccion = await (ctx.IOT.FromSql("select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= '" + fini + "' and timestamp <= '" + ffin + "'  and value > 0) x group by tiempo order by id ")).ToListAsync();
        //                //seleccion = await (ctx.IOT.FromSql("declare @fini date, @ffin date set @fini = '" + fini + "' set @ffin = '" + ffin + "' select id, cast(case when total = 0 then 0 else cast((cast(value as decimal(10,2))/cast(total as decimal(10,2)))*100 as decimal(10,2)) end as varchar(255)) value, timestamp, Sku_activo, Cod_plan, tiempo from (select max(id) id, cast(COUNT(value) as varchar(255)) value, getdate() timestamp, '' Sku_activo, '' Cod_plan, max(tiempo) tiempo, max(dia_fecha) dia_fecha from(select id, value, cast(format(cast(day(timestamp) as int), '0#') as varchar(255)) + '-' + cast(format(cast(month(timestamp) as int), '0#') as varchar(255))  + '-' + cast(year(timestamp) as varchar(255)) tiempo, day(timestamp) dia_fecha, case when Sku_activo is NULL then '' else Sku_activo end Sku_activo , case when Cod_plan is NULL then '' else Cod_plan end Cod_plan from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin  and value > 0) x group by tiempo) t inner join (select dia_fecha, SUM(tot_min) total from(select z.dia, dia_fecha, count(z.dia) cuantos, max(h.Hora_ini1) hi1, max(h.Hora_fin1) hf2, DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) minutos, count(z.dia) * DATEDIFF(minute, cast(max(h.Hora_ini1) as datetime), cast(max(h.Hora_fin1) as datetime)) tot_min from (select day(timestamp) fecha, DATEPART(dw,timestamp) dia, day(timestamp) dia_fecha	from " + a.Nombre_tabla + " where timestamp >= @fini and timestamp <= @ffin group by day(timestamp),DATEPART(dw,timestamp), day(timestamp))z inner join Horarios_activos h on z.dia = h.Dia where h.Cod_activo = substring('" + a.Nombre_tabla + "', 5, len('" + a.Nombre_tabla + "') - 7) group by z.dia, dia_fecha)t group by dia_fecha) w on t.dia_fecha = w.dia_fecha")).ToListAsync();

        //                //Como es diario no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //                fini = fini.Substring(0, 8);
        //                ffin = ffin.Substring(0, 8);

        //                DateTime Dfini = DateTime.Parse(fini.Substring(6, 2) + "-" + fini.Substring(4, 2) + "-" + fini.Substring(0, 4));
        //                DateTime Dffin = DateTime.Parse(ffin.Substring(6, 2) + "-" + ffin.Substring(4, 2) + "-" + ffin.Substring(0, 4));


        //                try
        //                {
        //                    //string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";



        //                    if (turno == "Todos" || turno == null)
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion2 = seleccion;
        //                        seleccion3 = seleccion;
        //                        seleccion4 = seleccion;
        //                        turno = null;
        //                    }
        //                    else
        //                    {
        //                        seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                        seleccion = seleccion.Where(w => w.turno == turno).ToList();
        //                        seleccion2 = seleccion;
        //                        seleccion3 = seleccion;
        //                        seleccion4 = seleccion;
        //                    }

        //                    //seleccion = ListadoIOT(fini, ffin, a.Cod_activo, a.Variable, idEmpresa);
        //                    //vueltas++;
        //                    //registro 131 del día 03/04
        //                    seleccion = (from iotX in seleccion
        //                                 where (iotX.value > 0 && iotX.planificado == null && iotX.turno != null /*&& iotX.Minutos > 0*/) && (iotX.dia >= Dfini && iotX.dia <= Dffin)

        //                                 group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                 select new IOT
        //                                 {
        //                                     id = mm.Max(m => m.id),
        //                                     value = mm.Sum(m => m.value),
        //                                     timestamp = mm.Max(m => m.timestamp),
        //                                     Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo),
        //                                     Cod_plan = mm.Max(m => m.Cod_plan),
        //                                     marca = cod_activo,
        //                                     turno = turno
        //                                 }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion2 = (from iotX in seleccion2
        //                                  where (iotX.value > 0 && iotX.planificado == null && iotX.turno == null /*&& iotX.Minutos > 0*/) && (iotX.dia >= Dfini && iotX.dia <= Dffin)
        //                                  group iotX by new { iotX.dia.Year, iotX.dia } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = mm.Sum(m => m.value),
        //                                      Sku_activo = "Día " + mm.Key.dia.ToString().Substring(0, 10),//mm.Max(m => m.Sku_activo), 
        //                                      turno = "Unidades" //unidad //Uso el turno para guardar en este caso las unidades
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion3 = (from iotX in seleccion3
        //                                  where iotX.value > 0 && iotX.planificado == null
        //                                  group iotX by new { iotX.Sku_activo } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = mm.Sum(m => m.value),
        //                                      turno = turno
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion4 = (from iotX in seleccion4
        //                                  join pr in ctx.Productos on iotX.Sku_activo equals pr.Cod_producto
        //                                  where iotX.value > 0 && iotX.planificado == null
        //                                  group iotX by new { iotX.Sku_activo } into mm
        //                                  select new IOT
        //                                  {
        //                                      value = mm.Sum(m => m.value),
        //                                      Sku_activo = mm.Max(m => m.Sku_activo)
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    seleccion5 = (from act in ctx.Activos_tablas
        //                                  where act.Cod_activo == cod_activo
        //                                  select new IOT
        //                                  {
        //                                      Sku_activo = act.Unidad
        //                                  }).OrderBy(o => o.timestamp.Month).ToList();

        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes
        //                        tiempo = seleccion.Select(s => s.Sku_activo).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => s.Sku_activo).ToArray(),
        //                        data = seleccion.Select(s => s.value).ToArray(),
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = seleccion3.Select(s => s.value).ToArray(),
        //                        unidades = seleccion.Select(s => "Unidades").ToArray(), //seleccion5.Select(m => m.Sku_activo).ToArray(),
        //                        nombreActivo = NombreActivo,
        //                        tiempo3 = seleccion.Select(s => s.turno).ToArray(),
        //                        filtro = ff
        //                    });

        //                }
        //                catch (Exception ex)
        //                { }
        //            }
        //            catch (Exception ex)
        //            { }
        //        }
        //        else if (filtro == "hora")
        //        {

        //            //Como es mensual no interesa pasar horas solo fechas por ello se establecen fechas redondas
        //            //fini = fini; //.Substring(0, 8);
        //            //ffin = ffin.Substring(0, 8);

        //            fini = fini_.ToString("yyyyMMdd HH:mm");
        //            ffin = ffin_.ToString("yyyyMMdd HH:mm");

        //            try
        //            {
        //                //string p1 = "exec sp_Registros_IOT '" + fini + "','" + ffin + "','" + cod_activo + "','" + variable + "'";


        //                seleccion = ListadoIOT(fini, ffin, cod_activo, variable, idEmpresa);
        //                seleccion2 = seleccion;
        //                seleccion3 = seleccion;
        //                seleccion4 = seleccion;


        //                //vueltas++;

        //                seleccion = (from iotX in seleccion
        //                             where iotX.value > 0 && iotX.planificado == null && iotX.turno != null /*&& iotX.Minutos > 0*/
        //                             group iotX by new { iotX.timestamp.Hour } into mm
        //                             select new IOT
        //                             {
        //                                 id = mm.Max(m => m.id),
        //                                 value = mm.Sum(m => m.value),
        //                                 timestamp = mm.Max(m => m.timestamp),
        //                                 Sku_activo = mm.Max(m => m.Sku_activo),
        //                                 Cod_plan = mm.Max(m => m.Cod_plan),
        //                                 dia = mm.Max(m => m.dia),
        //                                 marca = cod_activo,
        //                                 turno = "Unidades" //unidad //Uso el turno para guardar en este caso las unidades
        //                             }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion2 = (from iotX in seleccion2
        //                              where iotX.value > 0 && iotX.planificado == null && iotX.turno == null /*&& iotX.Minutos > 0*/
        //                              group iotX by new { iotX.timestamp.Hour } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  timestamp = mm.Max(m => m.timestamp),
        //                                  turno = turno
        //                              }).OrderBy(o => o.timestamp.Month).ToList();
        //                seleccion3 = (from iotX in seleccion3
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                              }).OrderBy(o => o.timestamp.Month).ToList();

        //                seleccion4 = (from iotX in seleccion4
        //                              join pr in ctx.Productos on iotX.Sku_activo equals pr.Cod_producto
        //                              where iotX.value > 0 && iotX.planificado == null
        //                              group iotX by new { iotX.Sku_activo } into mm
        //                              select new IOT
        //                              {
        //                                  value = mm.Sum(m => m.value),
        //                                  Sku_activo = mm.Max(m => m.Sku_activo)
        //                              }).OrderBy(o => o.timestamp.Month).ToList();
        //                seleccion5 = (from act in ctx.Activos_tablas
        //                              where act.Cod_activo == cod_activo
        //                              select new IOT
        //                              {
        //                                  Sku_activo = act.Unidad
        //                              }).OrderBy(o => o.timestamp.Month).ToList();
        //                try
        //                {
        //                    series.Add(new SeriesUnidades()
        //                    {
        //                        id = seleccion.Max(s => s.id),
        //                        fecha = seleccion.Select(s => s.timestamp.ToString()).ToArray(),

        //                        //TODO: Hay que buscar una forma de poder ordenar los registros, sobre todo cuando estan por año y mes

        //                        tiempo = seleccion.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(), //seleccion.Select(s => s.timestamp.Month.ToString() + " - " + s.timestamp.Year).ToArray(), 
        //                        tiempo2 = seleccion2.Select(s => s.timestamp.ToString("dd/MM/yyyy hh:mm")).ToArray(),
        //                        data = seleccion.Select(s => s.value).ToArray(),
        //                        data2 = seleccion2.Select(s => s.value).ToArray(),
        //                        activo = seleccion.Select(m => m.marca.ToString()).ToArray(),
        //                        cod_plan = seleccion.Select(m => m.Cod_plan.ToString()).ToArray(),
        //                        sku = seleccion4.Select(m => m.Sku_activo.ToString()).ToArray(),
        //                        sku_conteo = seleccion4.Select(s => s.value).ToArray(),
        //                        sku_conteo_total = seleccion3.Select(s => s.value).ToArray(),
        //                        unidades = seleccion.Select(s => s.turno).ToArray(), //seleccion5.Select(m => m.Sku_activo).ToArray(),
        //                        nombreActivo = NombreActivo,
        //                        tiempo3 = seleccion.Select(s => "").ToArray(),
        //                        filtro = ff
        //                    });
        //                }
        //                catch (Exception ex)
        //                { }

        //            }
        //            catch (Exception ex)
        //            { }
        //        }

        //    }
        //    return series;
        //}


        //Historicos Fin2

        //Metodo indicadores para gestion KPI's

        #endregion

        #endregion
    }

    public class IndicadoresTot
    { 
        public decimal Disp { get; set; }
        public decimal Rend { get; set; }
        public decimal Oee { get; set; }
        public DateTime Fecha { get; set; }
        public string Agrupacion { get; set; }
    }

    public class ActivosVariables
    { 
        public string Cod_activo { get; set; }
        public string Des_activo { get; set; }
        public string Variable { get; set; }
        public string Unidad { get; set; }
    }

    public class FechasSemana { 
        public DateTime fecha1 { get; set; }
        public DateTime fecha2 { get; set; }
    }

    public class ConsultaPX
    { 
        public string Cod_activo { get; set; }
        public string Cod_producto { get; set; }
    }

    public class CompletaIncidencias
    { 
        public string Agrupacion { get; set; }
        public string DesTipoIncidencia { get; set; }
        public decimal Valor { get; set; }
    }

    public class fechasIniciales
    { 
        public DateTime fini { get; set; }
        public DateTime ffin { get; set; }
    }
}