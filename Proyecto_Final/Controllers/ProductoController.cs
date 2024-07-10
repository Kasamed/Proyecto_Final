    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Datos;
using Proyecto_Final.Models;

namespace Proyecto_Final.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto;
            return View(lista);
        }

        public IActionResult Crear() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _db.Producto.Add(producto);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
            
        }

        public IActionResult Editar(int? Id) 
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.Producto.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _db.Producto.Update(producto);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);

        }

        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var obj = _db.Producto.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if (producto == null)
            {
                return NotFound();
            }
            _db.Producto.Remove(producto);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }

}
