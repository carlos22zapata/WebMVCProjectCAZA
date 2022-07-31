using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Text;
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
using Rotativa.AspNetCore;

namespace FactoryX.Controllers
{
    public class TablasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private EmpresaDbContext _Econtext;

        public TablasController(ApplicationDbContext context) 
        {
            _context = context;
            CultureInfo.CurrentCulture = new CultureInfo("es-MX");
        }

        public static int idEmpresaX;

        public void AsignaEmpresa(int idEmpresa)
        { 
            idEmpresaX  = idEmpresa;
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

        public string UserHostAddress { get; }

        public async Task<bool> ValidaUsuario()
        {
            //Reviso que sea el mismo usuario y la misma MacAdrress
            var userId = User.getUserId();
            var IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            int existe = await _context.LogOn.Where(w => w.UserId == userId && w.IpAddress == IP).CountAsync();

            //var prueba = this.HttpContext.Connection.RemoteIpAddress;

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

        #region Productos

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpGet]
        public async Task<IActionResult> Productos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            idEmpresaX = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Productos> Prod = await (from p in ctx.Productos
                                              select new Productos
                                              {
                                                  Cod_producto = p.Cod_producto,
                                                  Des_producto = p.Des_producto,
                                                  Cod_grupo = p.Cod_grupo
                                              }).ToListAsync();

                return View(Prod);
                //return Request.CreateResponse(DataSourceLoader.Load(Prod, loadOptions));
            }
        }

        public async Task<IActionResult> Abre_reporte_qr(int idEmpresa, string cod_producto, int cantidad)
        {
            //try
            //{
            //    View("ReporteEtiquetasProductos");
            //}
            //catch (Exception ex)
            //{ 

            //}


            return null;
        }

        public IActionResult ReporteEtiquetasProductos(string datos)
        {           
            //return new ViewAsPdf("ReporteEtiquetasProductos", datos)
            //{

            //};

            return View();
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
                                                  Des_producto = p.Des_producto,
                                                  Cod_grupo = p.Cod_grupo
                                              }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Prod, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateProductos(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Productos Prod = await ctx.Productos.Where(w => w.Cod_producto == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, Prod);

                Prod.Usuario_modifica = User.getUserId();
                Prod.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(Prod);
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteProductos(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Productos Prod = await ctx.Productos.Where(w => w.Cod_producto == key).FirstOrDefaultAsync();
                ctx.Productos.Remove(Prod);
                ctx.SaveChanges();
                return Ok();
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertProductos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Prod = new Productos();
                JsonConvert.PopulateObject(values, Prod);                                

                var p = new Productos
                {
                    Cod_producto = Prod.Cod_producto,
                    Des_producto = Prod.Des_producto,
                    Cod_grupo = Prod.Cod_grupo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Productos.Add(p);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                }

                return Ok(p);
            }
        }

        #endregion Productos

        #region Grupo_productos

        [Microsoft.AspNetCore.Mvc.HttpGet]
        //public async Task<object> GetAgrupacion(DataSourceLoadOptions loadOptions)
        public async Task<object> GetAgrupacion(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Grup = await (from g in ctx.Grupos
                                  select new
                                  {
                                      Cod_grupo = g.Cod_grupo,
                                      Des_grupo = g.Des_grupo
                                  }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Grup, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateAgrupacion(int key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Grupos Grup = await ctx.Grupos.Where(w => w.Cod_grupo == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, Grup);

                Grup.Usuario_modifica = User.getUserId();
                Grup.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }
                
                return Ok(Grup);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertAgrupacion(string values)
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

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteAgrupacion(int key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Grupos Grup = await ctx.Grupos.Where(w => w.Cod_grupo == key).FirstOrDefaultAsync();
                ctx.Grupos.Remove(Grup);

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo eliminar el registro, es posible que sea debido a que esta asociado a algún producto");
                }
                    
                return Ok();
            }
        }

        #endregion Grupo_productos

        #region Activos

        public async Task<IActionResult> Activos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            idEmpresaX = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                return View();
            }
        }
                
        [System.Web.Http.HttpGet]
        public async Task<object> GetActivos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<ActivosLista> Act = await (from a in ctx.Activos
                                                //join h in ctx.Horarios_activos on a.Cod_activo equals h.Cod_activo into ha
                                                //from h in ha.DefaultIfEmpty()
                                                select new ActivosLista
                                                {
                                                    Cod_activo = a.Cod_activo,
                                                    Des_activo = a.Des_activo,
                                                    Cod_grupo = a.Cod_grupo,
                                                    //id = h.id == null ? 0 : h.id
                                                }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Act, loadOptions);
            }
        }

        //GetTurnos1
        [System.Web.Http.HttpGet]
        public async Task<object> GetTurnos1(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Turnos> Tur = await (from t in ctx.Turnos
                                                //join h in ctx.Horarios_activos on a.Cod_activo equals h.Cod_activo into ha
                                                //from h in ha.DefaultIfEmpty()
                                            select new Turnos
                                            {
                                                Cod_turno = t.Cod_turno
                                            }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Tur, loadOptions);
            }
        }

        public static string cod_activoX;
        public void AsignaCod_activo(string cod_activo)
        {
            cod_activoX = cod_activo;
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetTurnosActivos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {

                    var ta = await (from t in ctx.Turnos_activos
                                    where t.Cod_activo == cod_activoX
                                    select new Turnos_activos
                                    {
                                        Cod_activo = t.Cod_activo,
                                        Cod_turno = t.Cod_turno,
                                        Dia = t.Dia //Obtener_dias.Obtiene_dias().Where(w => w.Cod_dia == t.Dia).Max(m => m.Des_dia)
                                    }).ToListAsync();

                    return DataSourceLoader.Load(ta, loadOptions);
                }
                catch (Exception Ex)
                { 
                
                }
                
                return DataSourceLoader.Load("", loadOptions);
            }                
        }

        [System.Web.Http.HttpGet]
        public async Task<object> GetDias(DataSourceLoadOptions loadOptions)
        {
            var dias = (from d in Obtener_dias.Obtiene_dias()
                        select new Lista_dias
                        {
                            Cod_dia = d.Cod_dia,
                            Des_dia = d.Des_dia
                        }).ToList();

            return DataSourceLoader.Load(dias, loadOptions);
            
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertTurnosActivos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var TAct = new Turnos_activos();
                JsonConvert.PopulateObject(values, TAct);

                var ta = new Turnos_activos
                {
                    Cod_activo = cod_activoX,
                    Cod_turno = TAct.Cod_turno,
                    Dia = TAct.Dia
                };

                ctx.Turnos_activos.Add(ta);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                }

                return Ok(ta);
            }
        }

        //public async Task<IActionResult> UpdateTurnosActivos(string key, string values)
        //{
        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        string CT, CA;
        //        int DI;

        //        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(key)))
        //        {
        //            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Turnos_activos));
        //            Turnos_activos taa = (Turnos_activos)deserializer.ReadObject(ms);
        //            CA = taa.Cod_activo;
        //            CT = taa.Cod_turno;
        //            DI = taa.Dia;
        //        }

        //        Turnos_activos ta = await ctx.Turnos_activos.Where(w => w.Cod_activo == CA && w.Cod_turno == CT && w.Dia == DI).FirstOrDefaultAsync();
        //        JsonConvert.PopulateObject(values, ta);

        //        ta.Usuario_modifica = User.getUserId();
        //        ta.Fecha_usuario_modifica = DateTime.Now;

        //        try
        //        {
        //            ctx.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest("No se pudo actualizar el registro");
        //        }

        //        return Ok(ta);
        //    }
        //}

        public async Task<IActionResult> DeleteTurnosActivos(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string CT, CA;
                int DI;

                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(key)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Turnos_activos));
                    Turnos_activos taa = (Turnos_activos)deserializer.ReadObject(ms);
                    CA = taa.Cod_activo;
                    CT = taa.Cod_turno;
                    DI = taa.Dia;
                }

                Turnos_activos ta = await ctx.Turnos_activos.Where(w => w.Cod_activo == CA && w.Cod_turno == CT && w.Dia == DI).FirstOrDefaultAsync();

                ctx.Turnos_activos.Remove(ta);
                ctx.SaveChanges();
                return Ok();
            }
        }

        //[System.Web.Http.HttpGet]
        //public async Task<object> GetDatosHorariosActivos(DataSourceLoadOptions loadOptions)
        //{
        //    using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
        //    {
        //        try
        //        {
        //            List<Horarios_activos> ha = await (from h in ctx.Horarios_activos
        //                                               select new Horarios_activos
        //                                               {
        //                                                   id = h.id,
        //                                                   Dia = 0, //h.Dia,
        //                                                   Hora_ini1 = h.Hora_ini1.ToString(),
        //                                                   Hora_fin1 = h.Hora_fin1.ToString(),
        //                                                   Hora_ini2 = h.Hora_ini2 == null ? "" : h.Hora_ini2.ToString(),
        //                                                   Hora_fin2 = h.Hora_fin2 == null ? "" : h.Hora_fin2.ToString()
        //                                               }).ToListAsync();

        //            return DataSourceLoader.Load(ha, loadOptions);
        //        }
        //        catch (Exception ex)
        //        {
        //            return DataSourceLoader.Load("", loadOptions);
        //        }



        //    }


        //}

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertActivos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Act = new Activos();
                JsonConvert.PopulateObject(values, Act);
                               
                var a = new Activos
                {
                    Cod_activo = Act.Cod_activo,
                    Des_activo = Act.Des_activo,
                    Cod_grupo = Act.Cod_grupo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Activos.Add(a);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                }

                return Ok(a);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateActivos(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Activos Act = await ctx.Activos.Where(w => w.Cod_activo == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, Act);

                Act.Usuario_modifica = User.getUserId();
                Act.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(Act);
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteActivos(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Activos Act = await ctx.Activos.Where(w => w.Cod_activo == key).FirstOrDefaultAsync();
                ctx.Activos.Remove(Act);
                ctx.SaveChanges();
                return Ok();
            }
        }

        #endregion Activos

        #region Grupos de activos

        public async Task<object> GetAgrupacionActivos(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var Grup = await (from g in ctx.Grupos_activos
                                  select new
                                  {
                                      Cod_grupo = g.Cod_grupo,
                                      Des_grupo = g.Des_grupo
                                  }).ToListAsync();

                //var prueba = DataSourceLoader.Load(Prod, loadOptions);

                return DataSourceLoader.Load(Grup, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertAgrupacionActivos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var GrupA = new Grupos_activos();
                JsonConvert.PopulateObject(values, GrupA);

                var ga = new Grupos_activos
                {
                    Des_grupo = GrupA.Des_grupo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Grupos_activos.Add(ga);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(ga);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateAgrupacionActivos(int key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Grupos_activos GrupA = await ctx.Grupos_activos.Where(w => w.Cod_grupo == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, GrupA);

                GrupA.Usuario_modifica = User.getUserId();
                GrupA.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(GrupA);
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteAgrupacionActivos(int key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Grupos_activos GrupA = await ctx.Grupos_activos.Where(w => w.Cod_grupo == key).FirstOrDefaultAsync();
                ctx.Grupos_activos.Remove(GrupA);

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo eliminar el registro, es posible que sea debido a que esta asociado a algún activo");
                }

                return Ok();
            }
        }

        #endregion Grupos de activos

        #region Turnos

        public async Task<IActionResult> Turnos(int idEmpresa)
        {
            ViewBag.idEmpresa = idEmpresa;
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            AsignaEmpresa(idEmpresa);

            //enviaTelegramColineal("¡Hola! Han transcurrido más de 4 horas sin tener reporte de actividad por parte del activo PRENSA PARA ENCHAPAR. Te recomiendo verificar que todo esté en orden.", "");

            return View();
        }

        public async Task<object> GetTurnos(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var ht = await (from h in ctx.Turnos
                                select new Turnos
                                { 
                                    Cod_turno = h.Cod_turno + " - Desde: " + h.Hora_ini1.ToString("hh:mm tt") + " - Hasta: " + h.Hora_fin1.ToString("hh:mm tt"),
                                    Hora_ini1 = h.Hora_ini1,
                                    Hora_fin1 = h.Hora_fin1
                                }).ToListAsync();

                return DataSourceLoader.Load(ht, loadOptions);
            }            
        }

        public async Task<object> GetTurnos2(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var ht = await (from h in ctx.Turnos
                                select new Turnos
                                {
                                    Cod_turno = h.Cod_turno,
                                    Hora_ini1 = h.Hora_ini1,
                                    Hora_fin1 = h.Hora_fin1
                                }).ToListAsync();

                return DataSourceLoader.Load(ht, loadOptions);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> InsertTurnos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var HT = new Turnos();
                JsonConvert.PopulateObject(values, HT);

                var ht = new Turnos
                {
                    Cod_turno = HT.Cod_turno,
                    Hora_ini1 =HT.Hora_ini1,
                    Hora_fin1 = HT.Hora_fin1,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Turnos.Add(ht);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(ht);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateTurnos(string key, string values)
        {
            key = key.Substring(0, key.IndexOf("- Desde") - 1);

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    Turnos HT = await ctx.Turnos.Where(w => w.Cod_turno == key).FirstOrDefaultAsync();
                    JsonConvert.PopulateObject(values, HT);

                    HT.Usuario_modifica = User.getUserId();
                    HT.Fecha_usuario_modifica = DateTime.Now;

                
                    ctx.SaveChanges();
                    return Ok(HT);
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteTurnos(string key)
        {
            key = key.Substring(0, key.IndexOf("- Desde:")-1);

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Turnos HT = await ctx.Turnos.Where(w => w.Cod_turno == key).FirstOrDefaultAsync();
                ctx.Turnos.Remove(HT);
                ctx.SaveChanges();
                return Ok();
            }
        }

        #endregion Turnos

        #region Capacidades de activos

        public async Task<IActionResult> CapacidadesActivos(int idEmpresa)
        {
            ViewBag.idEmpresa = idEmpresa;
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            AsignaEmpresa(idEmpresa);
                        
            List<Lista_dias> ld = Obtener_dias.Obtiene_dias();
            
            return View();
        }

        public async Task<object> GetCapacidades_activos(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    var ca = await (from c in ctx.Capacidades_activos
                                    select new Capacidades_activos
                                    {
                                        Cod_activo = c.Cod_activo,
                                        Cod_producto = c.Cod_producto,
                                        Capacidad_maxima = c.Capacidad_maxima,
                                        Unidad = c.Unidad,
                                        UnidadesXciclo = (c.UnidadesXciclo == null ? 0 : c.UnidadesXciclo),
                                        Variable = c.Variable
                                    }).ToListAsync();

                    return DataSourceLoader.Load(ca, loadOptions);
                }
                catch (Exception ex)
                { 
                    
                }

                return DataSourceLoader.Load("", loadOptions);
            }
        }

        public async Task<object> GetVariable(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var v = await (from va in ctx.Activos_tablas
                               group va by new { va.Variable } into vv
                               select new
                               { 
                                    variable = vv.Key.Variable
                               }).ToListAsync();
                
                return DataSourceLoader.Load(v, loadOptions);
            }
        }

        public async Task<IActionResult> InsertCapacidades_activos(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var CapAct = new Capacidades_activos();
                JsonConvert.PopulateObject(values, CapAct);

                var ca = new Capacidades_activos
                {
                    Cod_activo = CapAct.Cod_activo,
                    Cod_producto = CapAct.Cod_producto,
                    Capacidad_maxima = CapAct.Capacidad_maxima,
                    Capacidad_minima = 0,
                    Unidad = CapAct.Unidad,
                    Pico = 0,
                    Ajuste_cantidad = 0,
                    UnidadesXciclo = CapAct.UnidadesXciclo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Capacidades_activos.Add(ca);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(ca);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateCapacidades_activos(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                string ka, kp;

                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(key)))
                {   
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Capacidades_activos));
                    Capacidades_activos caa = (Capacidades_activos)deserializer.ReadObject(ms);
                    ka = caa.Cod_activo;
                    kp = caa.Cod_producto;
                }

                try
                {
                    Capacidades_activos ca = await ctx.Capacidades_activos.Where(w => w.Cod_activo == ka && w.Cod_producto == kp).FirstOrDefaultAsync();
                    JsonConvert.PopulateObject(values, ca);

                    ca.Usuario_modifica = User.getUserId();
                    ca.Fecha_usuario_modifica = DateTime.Now;
                }
                catch (Exception ex)
                { 
                    
                }
                

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok();
            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteCapacidades_activos(string key)
        {
            string ka, kp;
            
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(key)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Capacidades_activos));
                    Capacidades_activos caa = (Capacidades_activos)deserializer.ReadObject(ms);
                    ka = caa.Cod_activo;
                    kp = caa.Cod_producto;
                }

                Capacidades_activos ca = await ctx.Capacidades_activos.Where(w => w.Cod_activo == ka && w.Cod_producto == kp).FirstOrDefaultAsync();
                ctx.Capacidades_activos.Remove(ca);
                ctx.SaveChanges();
                return Ok();
            }
        }

        public async Task<object> GetUnidades(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    var un = await (from u in ctx.Unidades
                                    select new Unidades
                                    {
                                        Cod_unidad = u.Cod_unidad,
                                        Des_unidad = u.Des_unidad
                                    }).ToListAsync();

                    return DataSourceLoader.Load(un, loadOptions);
                }
                catch (Exception ex)
                {

                }

                return DataSourceLoader.Load("", loadOptions);
            }
        }

        public async Task<object> ConsultaCapacidadesActivos(int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var cap = await (from tt in ctx.Tiempo_inactivo_activos
                                 select new Tiempo_inactivo_activos
                                 {
                                     Fecha_desde = tt.Fecha_desde,
                                     Fecha_hasta = tt.Fecha_hasta,
                                     id_Tipo = tt.id_Tipo,
                                     Observacion = tt.Observacion
                                 }).ToListAsync();

                return Json(cap);
            }

            return Json("");
        }

        #endregion Capacidades de activos

        #region Días inactivos

        [System.Web.Http.HttpGet]
        public async Task<object> GetTia(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    List<Tiempo_inactivo_activos_view> TIA = await (from t in ctx.Tiempo_inactivo_activos
                                                                    join tt in ctx.Tipo_tiempos_activos on t.id_Tipo equals tt.id
                                                                   select new Tiempo_inactivo_activos_view
                                                                   {
                                                                       id= t.id,
                                                                       Observacion = t.Observacion,
                                                                       Fecha = t.Fecha_desde,
                                                                       Fecha_desde = t.Fecha_desde,
                                                                       Fecha_hasta = t.Fecha_hasta,
                                                                       Cod_activo = t.Cod_activo,
                                                                       Des_tipo = tt.Des_tipo,
                                                                       id_Tipo = t.id_Tipo
                                                                   }).ToListAsync();

                    return DataSourceLoader.Load(TIA, loadOptions);
                }
                catch (Exception ex)
                { 
                
                }

                return DataSourceLoader.Load("", loadOptions);
            }               

        }
               
        public async Task<IActionResult> InsertTia(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var tia = new Tiempo_inactivo_activos_view();
                JsonConvert.PopulateObject(values, tia);

                var prueba = tia.Fecha_desde;

                DateTime FechaDesde = DateTime.Parse(tia.Fecha.ToString("yyyy-MM-dd") + " 00:00"); //+ tia.Fecha_desde.ToString("HH:mm"));
                DateTime FechaHasta = DateTime.Parse(tia.Fecha.ToString("yyyy-MM-dd") + " 23:59"); //+ tia.Fecha_hasta.ToString("HH:mm"));

                var Tia = new Tiempo_inactivo_activos
                {
                    Fecha_desde = FechaDesde,
                    Fecha_hasta = FechaHasta,
                    id_Tipo = tia.id_Tipo,
                    Observacion = tia.Observacion,
                    Cod_activo = tia.Cod_activo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Tiempo_inactivo_activos.Add(Tia);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(Tia);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateTia(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                int k = int.Parse(key);
                
                try
                {
                    Tiempo_inactivo_activos ti = await ctx.Tiempo_inactivo_activos.Where(w => w.id == k).FirstOrDefaultAsync();
                    JsonConvert.PopulateObject(values, ti);

                    ti.Usuario_modifica = User.getUserId();
                    ti.Fecha_usuario_modifica = DateTime.Now;

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return BadRequest("No se pudo actualizar el registro");
                    }

                    return Ok(ti);
                }
                catch (Exception ex)
                { 
                }

                return Ok();


            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteTia(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {                
                Tiempo_inactivo_activos ti = await ctx.Tiempo_inactivo_activos.Where(w => w.id == int.Parse(key)).FirstOrDefaultAsync();
                ctx.Tiempo_inactivo_activos.Remove(ti);
                ctx.SaveChanges();
                return Ok();
            }
        }

        public async Task<IActionResult> DiasInactivos(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            idEmpresaX = idEmpresa;

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var viewModel = new Dias_inactivos_model
                {
                    Tipo_tiempos_activos_view = await (from t in ctx.Tipo_tiempos_activos
                                                       select new Tipo_tiempos_activos
                                                       {
                                                           id = t.id,
                                                           Des_tipo = t.Des_tipo
                                                       }).ToListAsync(),

                    Activos_view = await (from ac in ctx.Activos
                                          select new Activos
                                          {
                                              Cod_activo = ac.Cod_activo,
                                              Des_activo = ac.Des_activo
                                          }).ToListAsync()
                };

                return View(viewModel);
            }            
        }

        public async Task<List<IdentityError>> guardaFechas(int idEmpresa, DateTime fecha, int tipo, string activo, string observacion, DateTime? fechafin)
        {
            DateTime fhasta = (fechafin == null ? fecha : (DateTime)fechafin); 

            var errorList = new List<IdentityError>();
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    var TIA = new Tiempo_inactivo_activos
                    {
                        Fecha_desde = fecha,
                        Fecha_hasta = fhasta,
                        id_Tipo = tipo,
                        Observacion = observacion,
                        Cod_activo = activo,
                        Fecha_usuario_incluye = DateTime.Now,
                        Usuario_incluye = User.getUserId(),
                        Fecha_usuario_modifica = null,
                        Usuario_modifica = null
                    };

                    ctx.Add(TIA);
                    await ctx.SaveChangesAsync();

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

        public async Task<List<IdentityError>> eliminaFechas(int idEmpresa, DateTime fecha, DateTime fecha_hasta, int tipo, string activo)
        {
            var errorList = new List<IdentityError>();
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Tiempo_inactivo_activos tia = await ctx.Tiempo_inactivo_activos.Where(w => w.Fecha_desde == fecha && w.id_Tipo == tipo && w.Cod_activo == activo).FirstOrDefaultAsync();

                try
                {
                    ctx.Tiempo_inactivo_activos.Remove(tia);
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                { 
                    
                }                

                return errorList;
            }
        }

        #endregion Días inactivos

        #region Tipo de días de inactividad

        [System.Web.Http.HttpGet]
        public async Task<object> GetTipoTia(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                try
                {
                    List<Tipo_tiempos_activos> TTIA = await (from t in ctx.Tipo_tiempos_activos
                                                             select new Tipo_tiempos_activos
                                                             {
                                                                 id = t.id,
                                                                 Des_tipo = t.Des_tipo,
                                                             }).ToListAsync();

                    return DataSourceLoader.Load(TTIA, loadOptions);
                }
                catch (Exception ex)
                {

                }

                return DataSourceLoader.Load("", loadOptions);
            }

        }

        public async Task<IActionResult> InsertTipoTia(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var ttia = new Tipo_tiempos_activos();
                JsonConvert.PopulateObject(values, ttia);

                var TTia = new Tipo_tiempos_activos
                {
                    Des_tipo = ttia.Des_tipo,
                    Fecha_usuario_incluye = DateTime.Now,
                    Usuario_incluye = User.getUserId(),
                    Fecha_usuario_modifica = null,
                    Usuario_modifica = null
                };

                ctx.Tipo_tiempos_activos.Add(TTia);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(TTia);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateTipoTia(string key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                int k = int.Parse(key);

                try
                {
                    Tipo_tiempos_activos tti = await ctx.Tipo_tiempos_activos.Where(w => w.id == k).FirstOrDefaultAsync();
                    JsonConvert.PopulateObject(values, tti);

                    tti.Usuario_modifica = User.getUserId();
                    tti.Fecha_usuario_modifica = DateTime.Now;
                    tti.Des_tipo = tti.Des_tipo;

                    try
                    {
                        ctx.SaveChanges();
                    }
                    catch
                    {
                        return BadRequest("No se pudo actualizar el registro");
                    }

                    return Ok(tti);
                }
                catch (Exception ex)
                {
                }

                return Ok();


            }
        }

        [System.Web.Http.HttpDelete]
        public async Task<IActionResult> DeleteTipoTia(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Tipo_tiempos_activos tti = await ctx.Tipo_tiempos_activos.Where(w => w.id == int.Parse(key)).FirstOrDefaultAsync();
                ctx.Tipo_tiempos_activos.Remove(tti);
                ctx.SaveChanges();
                return Ok();
            }
        }

        #endregion importar Excel

        #region Incidencias

        public async Task<IActionResult> Incidencias(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            idEmpresaX = idEmpresa;

            return View();
        }

        public async Task<object> GetTipoIncidencias(DataSourceLoadOptions loadOptions)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Tipos_incidencia> ti = new List<Tipos_incidencia>();
                
                try
                {
                    ti = await (from t in ctx.Tipos_incidencia
                                   select new Tipos_incidencia
                                   {
                                       Cod_tipo = t.Cod_tipo,
                                       Des_tipo = t.Des_tipo,
                                       Planificado = t.Planificado
                                   }).ToListAsync();                    
                }
                catch (Exception ex)
                { }

                return DataSourceLoader.Load(ti, loadOptions);
            }
        }

        public async Task<IActionResult> InsertTipoIncidencia(string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var TipoIncidencia = new Tipos_incidencia();
                JsonConvert.PopulateObject(values, TipoIncidencia);

                var ca = new Tipos_incidencia
                {
                    //Cod_tipo = TipoIncidencia.Cod_tipo,
                    Des_tipo = TipoIncidencia.Des_tipo,
                    Planificado = TipoIncidencia.Planificado
                };

                ctx.Tipos_incidencia.Add(TipoIncidencia);
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }

                return Ok(ca);
            }
        }

        public async Task<IActionResult> UpdateTipoIncidencia(int key, string values)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Tipos_incidencia TI = await ctx.Tipos_incidencia.Where(w => w.Cod_tipo == key).FirstOrDefaultAsync();
                JsonConvert.PopulateObject(values, TI);

                TI.Usuario_modifica = User.getUserId();
                TI.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok(TI);
            }
        }

        public async Task<IActionResult> DeleteTipoIncidencia(string key)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Tipos_incidencia TI = await ctx.Tipos_incidencia.Where(w => w.Cod_tipo == int.Parse(key)).FirstOrDefaultAsync();
                ctx.Tipos_incidencia.Remove(TI);
                ctx.SaveChanges();
                return Ok();
            }
        }

        #endregion Incidencias

        #region Analisis o detalles de los productos

        public async Task<IActionResult> DetallesProductos(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            idEmpresaX = idEmpresa;

            return View();
        }

        #endregion Analisis o detalles de los productos

        #region Turnos Dinamicos

        public async Task<object> GetCalendario(DataSourceLoadOptions loadOptions, int idEmpresa)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                List<Turnos_activos_extras_view3> tex = await (from e in ctx.Turnos_activos_extras
                                                               join a in ctx.Activos on e.Cod_activo equals a.Cod_activo
                                                         select new Turnos_activos_extras_view3
                                                         {
                                                             id = e.id,
                                                             Fecha_ini = e.Fecha_ini,
                                                             Fecha_fin = e.Fecha_fin,
                                                             Cod_activo = e.Cod_activo,
                                                             Des_activo = a.Des_activo,
                                                             Mes = e.Fecha_ini.Month,
                                                             Ano = e.Fecha_ini.Year,
                                                             Cod_turno = e.Cod_turno
                                                         }).OrderByDescending(o => o.Fecha_ini).ToListAsync();

                return DataSourceLoader.Load(tex, loadOptions);
            }

            return View();
        }

        public async Task<IActionResult> TurnosDinamicos(int idEmpresa) //Inicial para cargar la vista principal
        {
            try
            {
                ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
                ViewBag.idEmpresa = idEmpresa;
                idEmpresaX = idEmpresa;
            }
            catch (Exception ex)
            { }


            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var ta = new Turnos_activos_extras_view
                {
                    turnos_activos_extras = null,

                    //await (from t in ctx.Turnos_activos_extras
                    //                               select new Turnos_activos_extras
                    //                               {
                    //                                   Cod_activo = t.Cod_activo,
                    //                                   Fecha_ini = t.Fecha_ini,
                    //                                   Fecha_fin = t.Fecha_fin
                    //                               }).ToListAsync(),

                    turnos = await (from t in ctx.Turnos
                                    select new Turnos
                                    {
                                        Cod_turno = t.Cod_turno,
                                        Hora_ini1 = t.Hora_ini1,
                                        Hora_fin1 = t.Hora_fin1
                                    }).ToListAsync(),

                    activos = await (from a in ctx.Activos
                                     select new Activos
                                     {
                                         Cod_activo = a.Cod_activo,
                                         Des_activo = a.Des_activo
                                     }).ToListAsync()

                };

                return View(ta);
            }


            return View();
        }

        public JsonResult RegCalendario(int idEmpresa)
        {

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                List<Turnos_activos_extras> ta = new List<Turnos_activos_extras>();

                try
                {
                    ta = (from h in ctx.Turnos_activos_extras
                          join t in ctx.Turnos on h.Cod_turno equals t.Cod_turno
                          join a in ctx.Activos on h.Cod_activo equals a.Cod_activo
                          select new Turnos_activos_extras
                          {
                              id = h.id,
                              Cod_activo = h.Cod_activo + " - " + a.Des_activo + ", Turno: " + t.Cod_turno + " - inicio: " + t.Hora_ini1.ToString("HH:mm") + " - Fin: " + t.Hora_fin1.ToString("HH:mm"),

                              //Fecha_ini = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm")) <
                              //            DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm")) ?
                              //            DateTime.Parse(h.Fecha_ini.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm")).AddDays(-1)
                              //            : DateTime.Parse(h.Fecha_ini.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm")),

                              Fecha_ini = DateTime.Parse(h.Fecha_ini.ToString("dd/MM/yyyy") + " " + t.Hora_ini1.ToString("HH:mm")),

                              Fecha_fin = DateTime.Parse(h.Fecha_fin.ToString("dd/MM/yyyy") + " " + t.Hora_fin1.ToString("HH:mm"))
                          }).ToList();
                }
                catch (Exception ex)
                {

                }

                return Json(ta);
            }

            //return "";
        }

        public async Task<bool> ValidaTurnos(int idEmpresa, string valores)
        {
            List<Turnos> tu = JsonConvert.DeserializeObject<List<Turnos>>(valores);
            
            tu = (from t0 in tu
                  select new Turnos
                  {
                      Cod_turno = t0.Cod_turno.Substring(0, t0.Cod_turno.IndexOf(" -", 0, t0.Cod_turno.Length))
                  }).ToList();

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                var turnos = await (from tt in ctx.Turnos
                                    select new
                                    {
                                        Cod_turno = tt.Cod_turno,
                                        Hora_ini1 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")) > DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_fin1.ToString("HH:mm tt")) ?
                                                    DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")) :
                                                    DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")),
                                        Hora_fin1 = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_fin1.ToString("HH:mm tt"))
                                    }).ToListAsync();

                foreach (var t in tu)
                {

                    try 
                    {
                        var hsel = turnos.Where(w => w.Cod_turno == t.Cod_turno).FirstOrDefault();

                        var s1 = turnos.Where(w => (w.Hora_ini1 > hsel.Hora_ini1 && w.Hora_fin1 < hsel.Hora_fin1) ||
                                                   (w.Hora_ini1 < hsel.Hora_ini1 && w.Hora_fin1 > hsel.Hora_fin1) ||
                                                   (w.Hora_ini1 > hsel.Hora_ini1 && w.Hora_fin1 > hsel.Hora_fin1 && w.Hora_ini1 < hsel.Hora_fin1) ||
                                                   (w.Hora_ini1 < hsel.Hora_ini1 && w.Hora_fin1 < hsel.Hora_fin1 && w.Hora_fin1 > hsel.Hora_ini1)
                                             ).ToList();

                        //var s2 = turnos.Where(w => w.Hora_ini1 < hsel.Hora_ini1 && w.Hora_fin1 > hsel.Hora_fin1).ToList();
                        //var s3 = turnos.Where(w => w.Hora_ini1 > hsel.Hora_ini1 && w.Hora_fin1 > hsel.Hora_fin1 && w.Hora_ini1 < hsel.Hora_fin1).ToList();
                        //var s4 = turnos.Where(w => w.Hora_ini1 < hsel.Hora_ini1 && w.Hora_fin1 < hsel.Hora_fin1 && w.Hora_fin1 > hsel.Hora_ini1).ToList();

                        if (s1.Count() > 0)
                        {
                            int contador0 = 0;

                            foreach (var t2 in tu)
                            {
                                //int compara = s1.Where(w => w.Cod_turno == t2.Cod_turno).Count();
                                if (s1.Where(w => w.Cod_turno == t2.Cod_turno).Count() > 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception ex) 
                    { }
                }

            }                
            
            return true;
        }

        public DateTime CFechaHora(DateTime FechaInicio, DateTime FechaFin) {

            DateTime FechaRetorno1 = DateTime.Now;
            FechaInicio = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + FechaInicio.ToString("HH:mm"));
            FechaFin    = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + FechaFin.ToString("HH:mm"));

            if (FechaInicio > FechaFin)
            {
                FechaRetorno1 = FechaFin.AddDays(1);
            }
            else if (FechaInicio == FechaFin)
            {
                FechaRetorno1 = FechaInicio;
            }
            else
            {
                FechaRetorno1 = FechaFin;
            }

            DateTime dt = FechaRetorno1;

            return dt;            
        }

        public async Task<int> InsertCalendario(int idEmpresa, string valores)
        {
            int RegistrosIncompletos = 0;

            try
            {
                List<Turnos_activos_extras_view2> tv2 = JsonConvert.DeserializeObject<List<Turnos_activos_extras_view2>>(valores);

                using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
                { 
                    var turnos = await (from tt in ctx.Turnos
                                        select new
                                        {
                                            Cod_turno_ = tt.Cod_turno,
                                            Hora_ini1_ = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")) > DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_fin1.ToString("HH:mm tt")) ?
                                                        DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")) :
                                                        DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_ini1.ToString("HH:mm tt")),
                                            Hora_fin1_ = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy") + " " + tt.Hora_fin1.ToString("HH:mm tt"))
                                        }).ToListAsync();

                    DateTime fecha_ir = tv2.Select(s => DateTime.Parse(DateTime.Parse(s.fecha).ToString("dd/MM/yyyy"))).FirstOrDefault();
                    

                    foreach (var tv in tv2)
                    {
                        tv.cod_turno = tv.cod_turno.Substring(0, tv.cod_turno.IndexOf(" -", 0, tv.cod_turno.Length));

                        //var hsel = turnos.Where(w => w.Cod_turno_ == tv.cod_turno).FirstOrDefault();

                        DateTime ff = DateTime.Parse(tv.fecha);

                        var hsel = (from t in ctx.Turnos
                                    where t.Cod_turno == tv.cod_turno
                                    select new
                                    {
                                        fini = CFechaHora(t.Hora_ini1,t.Hora_ini1),
                                        ffin = CFechaHora(t.Hora_ini1,t.Hora_fin1)
                                        
                                    }).FirstOrDefault();


                        var s1 = await (from tae in ctx.Turnos_activos_extras
                                        join tu in turnos on tae.Cod_turno equals tu.Cod_turno_
                                        where (DateTime.Parse(tae.Fecha_ini.ToString("dd/MM/yyyy")) == DateTime.Parse(ff.ToString("dd/MM/yyyy")) &&  tae.Cod_activo == tv.cod_activo) &&                                      
                                        (
                                            (
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) >= hsel.fini &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_fin1_) <= hsel.ffin
                                            )
                                                ||
                                            (
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) <= hsel.fini &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_fin1_) >= hsel.ffin
                                            )
                                                ||
                                            (
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) >= hsel.fini &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_fin1_) >= hsel.ffin &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) <  hsel.ffin      //Aqui esta el problema
                                            )
                                                ||
                                            (
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) <= hsel.fini &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_fin1_) <= hsel.ffin &&
                                                CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_) >  hsel.ffin
                                            )
                                        )
                                        select new
                                        {
                                            Fecha_ini_ = DateTime.Parse(tae.Fecha_ini.ToString("dd/MM/yyyy")),
                                            prueba1 = CFechaHora(tu.Hora_ini1_, tu.Hora_ini1_),
                                            prueba2 = hsel.ffin
                                        }).ToListAsync();

                        DateTime fecha = DateTime.Parse(tv.fecha);                        

                        var existe = ctx.Turnos_activos_extras.Where(w => DateTime.Parse(w.Fecha_ini.ToString("dd/MM/yyyy")) == DateTime.Parse(fecha.ToString("dd/MM/yyyy")) && w.Cod_turno == tv.cod_turno && w.Cod_activo == tv.cod_activo).Count();

                        //var prueba = ctx.Turnos_activos_extras.Where(w => w.id == 509).ToList();

                        if (s1.Count() == 0)
                        {
                            if (existe == 0)
                            {
                                var ta = new Turnos_activos_extras
                                {
                                    Cod_activo = tv.cod_activo,
                                    Fecha_ini = fecha,
                                    Fecha_fin = fecha,
                                    Cod_turno = tv.cod_turno,
                                    Usuario_incluye = User.getUserId(),
                                    Fecha_usuario_incluye = DateTime.Now
                                };

                                await ctx.Turnos_activos_extras.AddAsync(ta);

                                //La tabla IOT_Conciliado se actualiza con un trigger en la tabla Turnos_activos_extras
                                try
                                {
                                    ctx.Database.SetCommandTimeout(540);
                                    await ctx.SaveChangesAsync();

                                }
                                catch (Exception ex)
                                {
                                    return 0; //"No fue posible insertar el registro, asegurese de que el código no este repetido."; //BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                                }

                                await ctx.SaveChangesAsync();
                            }
                        }

                        //Cuando no se agrega un registro a la tabla de Turnos_activos_extras
                        if (s1.Count() > 0 || existe > 0)
                        {
                            RegistrosIncompletos++;
                        }

                    }

                    //try
                    //{
                    //    await ctx.SaveChangesAsync();

                    //}
                    //catch (Exception ex)
                    //{
                    //    return 0; //"No fue posible insertar el registro, asegurese de que el código no este repetido."; //BadRequest("No fue posible insertar el registro, asegurese de que el código no este repetido.");
                    //}


                }
            }
            catch (Exception ex)
            { 
                
            }

            return RegistrosIncompletos;
        }

        public async Task<int> minutosFecha(int idEmpresa, string Cod_Turno)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {

                int tiempo_turno = 0;

                var t1 = (from t in ctx.Turnos
                          where t.Cod_turno == Cod_Turno
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

                return tiempo_turno;
            }
        }

        public async Task<bool> ActualizoIOT_Conciliado(int idEmpresa, string cod_activo, DateTime Fecha_ini, DateTime Fecha_fin, string turno)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                //actualizar registros de IOT_conciliado e IOT_Conciliado_optimizado
               var IOT_C = (from i in ctx.IOT_Conciliado
                                    where i.Tiempo >= Fecha_ini && i.Tiempo <= Fecha_fin
                                    select new { i.IOT_id }).ToList();
            }

            return true;
        }


        public async Task<IActionResult> DeleteCalendario(int idEmpresa, int idx)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Turnos_activos_extras ta = await ctx.Turnos_activos_extras.Where(w => w.id == idx).FirstOrDefaultAsync();
                ctx.Turnos_activos_extras.Remove(ta);

                ctx.Database.SetCommandTimeout(540);
                await ctx.SaveChangesAsync();
                //ctx.SaveChanges();
                return Ok();
            }
        }

        public async Task<IActionResult> UpdateCalendario(int idEmpresa, int idx, string Cod_activo, DateTime Fecha_ini, DateTime Fecha_fin)
        {
            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                Turnos_activos_extras ta = await ctx.Turnos_activos_extras.Where(w => w.id == idx).FirstOrDefaultAsync();

                ta.Cod_activo = Cod_activo;
                ta.Fecha_ini = Fecha_ini;
                ta.Fecha_fin = Fecha_fin;
                ta.Usuario_modifica = User.getUserId();
                ta.Fecha_usuario_modifica = DateTime.Now;

                try
                {
                    ctx.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }

                return Ok();
            }
        }

        public async Task<IActionResult> DeleteCalendarioLote(int idEmpresa, string valores)
        {
            List<Turnos_activos_extras_viewDL> tv = JsonConvert.DeserializeObject<List<Turnos_activos_extras_viewDL>>(valores);

            using (var ctx = ConBD(_context.Institucion.Where(w => w.Id == idEmpresaX).Select(s => s.Bd).FirstOrDefault().ToString()))
            {
                foreach (var t in tv)
                {
                    t.cod_turno = t.cod_turno.Substring(0, t.cod_turno.IndexOf(" -", 0, t.cod_turno.Length));

                    DateTime ff = DateTime.Parse(t.fecha.ToString("dd-MM-yyyy"));

                    List<Turnos_activos_extras> te = await ctx.Turnos_activos_extras.Where(w =>
                                                        w.Cod_turno == t.cod_turno &&
                                                        w.Cod_activo == t.cod_activo && 
                                                        DateTime.Parse(w.Fecha_ini.ToString("dd-MM-yyyy")) == DateTime.Parse(ff.ToString("dd-MM-yyyy"))
                                                        ).ToListAsync(); //DateTime.Parse(t.fecha.ToString("dd-MM-yyyy"))).ToListAsync();

                    foreach (var t2 in te)
                    {
                        Turnos_activos_extras te2 = await ctx.Turnos_activos_extras.Where(w => w.id == t2.id).FirstOrDefaultAsync();
                        ctx.Turnos_activos_extras.Remove(te2);

                        await ctx.SaveChangesAsync();              
                    }
                }
            }
            return View();
        }

        #endregion Turnos Dinamicos

        private object GetFullErrorMessage()
        {
            var messages = new List<string>();

            //foreach (var entry in modelState)
            //{
            //    foreach (var error in entry.Value.Errors)
            //        messages.Add(error.ErrorMessage);
            //}

            return String.Join(" ", "Error al insertar.");
        }

        public void enviaTelegramColineal(string mensaje, string grupoTelegram)
        {
            string token = "718436457:AAFfzBwEdcbO0Qj689mEb_MvtebTZ2y6LGo";
            string id_grupo = "-394659957";

            var request = WebRequest.Create("https://api.telegram.org/bot" + token + "/sendMessage?chat_id=" + id_grupo + "&text=" + mensaje);
            var resp = request.GetResponse();
            var reader = new StreamReader(resp.GetResponseStream());

            reader.Close();
            resp.Dispose();
        }
    }

}

