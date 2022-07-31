using FactoryX.Data;
using FactoryX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nancy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace FactoryX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int idEmpresaX;

        public HomeController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            Response.Redirect(Url.Content("~/Identity/Account/Login?Lon=0"));
            //HttpContext.Session.SetString("SessionVariable1", User.getUserId());
            return View();

            //var user = User.getUserId();
            //if (user == null)
            //{
            //    Response.Redirect(Url.Content("~/Identity/Account/Login"));
            //    return View();
            //}
            //else
            //{
            //    Response.Redirect(Url.Content("~/Home/Empresas"));
            //    return View();
            //}
        }

        public async Task<IActionResult> ActualizaIp()
        {
            var userId = User.getUserId();

            var lo = await _context.LogOn.Where(w => w.UserId == userId).CountAsync();
            

            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            //Primero reviso si esta o no el registro que deberia estar pero mediante el proceso si de otra computadora le dan en cerrar sesión esto causara que 
            //se produzca este problema
            
            //Para ello reviso primero asegurandome de que si el registro existe o no, si existe actuializo y si no existe inserto el nuevo registro

            if (lo > 0) //Si existe se actualiza
            {
                LogOn l = _context.LogOn.Where(w => w.UserId == userId).FirstOrDefault();

                l.Date = DateTime.Now;
                l.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                l.MacAdress = macAddr;

                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    return BadRequest("No se pudo actualizar el registro");
                }
            }
            else //Si no existe se inserta
            {
                var IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                var su = new LogOn
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    IpAddress = IP,
                    MacAdress = macAddr
                };

                _context.LogOn.Add(su);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }
            }
            
            return View(true);
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Empresas()
        {
            HttpContext.Session.SetString("SessionVariable1", User.getUserId());

            ViewBag.Message = ""; //HttpContext.Session.GetString("SessionVariable1");

            //await DynamicHubClients.Client("")

            var userId = User.getUserId();

            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();

            var lo = await _context.LogOn.Where(w => w.UserId == userId).CountAsync();
            var IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var MC = await _context.LogOn.Where(w => w.MacAdress == macAddr).CountAsync();

            if (lo == 0)
            {
                //************* Inserto valor en la sesión ini *************
                var su = new LogOn
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    IpAddress = IP,
                    MacAdress = macAddr
                };

                _context.LogOn.Add(su);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest("No fue posible insertar el registro.");
                }
                //************* Inserto valor en la sesión fin *************

                ViewBag.Cond = 1;
            }
            else
            {
                string IpRegistrada = await _context.LogOn.Where(w => w.UserId == userId).Select(s => s.IpAddress).FirstOrDefaultAsync();

                if (IP == IpRegistrada)
                    ViewBag.Cond = 1;
                else
                    ViewBag.Cond = 0;
            }

            List<Institucion> Empresas = new List<Institucion>();

            Empresas = await (from i in _context.Institucion
                              join ue in _context.UsuariosEmpresas on i.Id equals ue.IdEmpresa
                              where ue.IdUser == userId
                              select new Institucion
                              {
                                  Id = i.Id,
                                  Des_institucion = i.Des_institucion
                              }).ToListAsync();

            return View(Empresas.ToList());

        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> VistaPrincipal(int idEmpresa)
        {
            @ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            var userId = User.getUserId();
            var verificar = _context.UsuariosEmpresas.Where(u => u.IdUser == userId && u.IdEmpresa == idEmpresa).FirstOrDefault();


            return View();
        }


        [Authorize]
        public IActionResult Privacy(string nombreEmpresa)
        {
            @ViewBag.nombreEmpresa = nombreEmpresa;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Lean(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            var i = await _context.Institucion.Where(w => w.Id == idEmpresa).FirstOrDefaultAsync();

            if (i.Mod_Lean != true)
            {
                Response.Redirect(Url.Content("~/Home/ModuloNoDisponible?idEmpresa=" + idEmpresa));
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> CCostos(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            var i = await _context.Institucion.Where(w => w.Id == idEmpresa).FirstOrDefaultAsync();

            if (i.Mod_Ccostos != true)
            {
                Response.Redirect(Url.Content("~/Home/ModuloNoDisponible?idEmpresa=" + idEmpresa));
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> ModuloNoDisponible(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Listado de las empresas
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> ListaEmpresas()
        {
            List<Institucion> Empresas = new List<Institucion>();

            Empresas = await (from e in _context.Institucion
                              select new Institucion
                              { 
                                Id = e.Id,
                                Des_institucion = e.Des_institucion
                              }).ToListAsync();

            return new JsonResult(Empresas);
        }
        
    }

    

    public static class ExtensionMethods
    {
        /// <summary>
        /// User ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string getUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                return null;

            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}


