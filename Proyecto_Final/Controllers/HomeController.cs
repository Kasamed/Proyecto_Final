using Microsoft.AspNetCore.Mvc;
using Proyecto_Final.Datos;
using Proyecto_Final.Models;
using Proyecto_Final.Services;
using System;
using System.Diagnostics;

namespace Proyecto_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly OpenAIService _openAIService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, OpenAIService openAIService)
        {
            _logger = logger;
            _db = db;
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto;
            return View(lista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerDescripcion(string nombre)
        {
            try
            {
                // Aquí imprimimos el nombre recibido para visualizarlo en la consola
                Console.WriteLine($"Nombre recibido: {nombre}");

                var resumen = await _openAIService.GetSummaryByNameAsync(nombre);

                return Json(new { resumen });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error al obtener la descripción:", ex.Message);

                return BadRequest("Error al obtener la descripción");
            }
        }
    }
}


