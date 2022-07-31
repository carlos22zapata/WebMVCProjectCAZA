using FactoryX.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace FactoryX.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int idEmpresaX;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> InventarioTR(int idEmpresa)
        {
            ViewBag.nombreEmpresa = await _context.Institucion.Where(w => w.Id == idEmpresa).Select(s => s.Des_institucion).FirstOrDefaultAsync();
            ViewBag.idEmpresa = idEmpresa;

            var i = await _context.Institucion.Where(w => w.Id == idEmpresa).FirstOrDefaultAsync();

            if (i.Mod_Inventario != true)
            {
                Response.Redirect(Url.Content("~/Home/ModuloNoDisponible?idEmpresa=" + idEmpresa));
            }

            return View();
        }
    }
}
