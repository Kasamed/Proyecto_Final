using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto_Final.Datos;
using Proyecto_Final.Models;
using Proyecto_Final.Services;

namespace Proyecto_Final.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarritoController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult AgregarCarrito(int id, int cantidad)
        {
            var producto = _db.Producto.Find(id);
            if (producto == null)
            {
                return NotFound(); // Manejar caso donde el producto no existe
            }

            List<CarritoItem> compras;
            if (HttpContext.Session.GetString("carrito") == null)
            {
                compras = new List<CarritoItem>();
            }
            else
            {
                compras = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CarritoItem>>(HttpContext.Session.GetString("carrito"));
            }

            var item = compras.FirstOrDefault(c => c.Producto.Id == id);
            if (item == null)
            {
                compras.Add(new CarritoItem(producto, cantidad));
            }
            else
            {
                item.Cantidad += cantidad; // Sumar la cantidad recibida al existente
            }

            HttpContext.Session.SetString("carrito", Newtonsoft.Json.JsonConvert.SerializeObject(compras));

            // Pasar la lista de compras a la vista
            return RedirectToAction("AgregarCarrito"); // Redirigir a la acción GET para mostrar el carrito
        }

        public IActionResult AgregarCarrito()
        {
            if (HttpContext.Session.GetString("carrito") == null)
            {
                // Si no hay elementos en el carrito, inicializar una lista vacía
                var compras = new List<CarritoItem>();
                HttpContext.Session.SetString("carrito", Newtonsoft.Json.JsonConvert.SerializeObject(compras));
            }

            var carrito = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CarritoItem>>(HttpContext.Session.GetString("carrito"));

            return View(carrito);
        }

        public IActionResult Delete(int id)
        {
            List<CarritoItem> compras;
            if (HttpContext.Session.GetString("carrito") == null)
            {
                compras = new List<CarritoItem>();
            }
            else
            {
                compras = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CarritoItem>>(HttpContext.Session.GetString("carrito"));
            }

            var item = compras.FirstOrDefault(c => c.Producto.Id == id);
            if (item != null)
            {
                compras.Remove(item);
                HttpContext.Session.SetString("carrito", Newtonsoft.Json.JsonConvert.SerializeObject(compras));
            }

            return View("AgregarCarrito", compras);
        }

        [HttpPost]
        public IActionResult FinalizarCompra(Comprador comprador)
        {
            List<CarritoItem> compras;
            if (HttpContext.Session.GetString("carrito") == null)
            {
                compras = new List<CarritoItem>();
            }
            else
            {
                compras = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CarritoItem>>(HttpContext.Session.GetString("carrito"));
            }

            if (compras != null && compras.Count > 0)
            {
                // Validar si hay suficiente cantidad disponible para cada producto
                List<string> errores = new List<string>();
                foreach (var item in compras)
                {
                    var producto = _db.Producto.FirstOrDefault(p => p.Id == item.Producto.Id);
                    if (producto == null || producto.Cantidad < item.Cantidad)
                    {
                        errores.Add($"No hay suficiente cantidad de {item.Producto.Nombre}. Cantidad disponible: {producto?.Cantidad ?? 0}");
                    }
                }

                // Si hay errores, añadirlos al ViewData y retornar la vista con el mensaje de error
                if (errores.Any())
                {
                    ViewData["Errores"] = errores;
                    return View("Carrito", compras); // Suponiendo que la vista del carrito se llama "Carrito"
                }

                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        // Guardar el comprador en la base de datos
                        _db.Comprador.Add(comprador);
                        _db.SaveChanges();

                        Venta nuevaVenta = new Venta
                        {
                            DiaVenta = DateTime.Now,
                            Subtotal = (float)compras.Sum(x => x.Producto.Precio * x.Cantidad),
                            Iva = (float)(compras.Sum(x => x.Producto.Precio * x.Cantidad) * 0.16),
                            Total = (float)(compras.Sum(x => x.Producto.Precio * x.Cantidad) * 1.16),
                            DetalleVenta = compras.Select(item => new DetalleVenta
                            {
                                Id_producto = item.Producto.Id,
                                Cantidad = item.Cantidad,
                                Total = (float)(item.Cantidad * item.Producto.Precio),
                                Id_comprador = comprador.Id_comprador // Asignar el Id_comprador al detalle de venta
                            }).ToList()
                        };

                        _db.Venta.Add(nuevaVenta);
                        _db.SaveChanges();

                        // Actualizar la cantidad de cada producto en la base de datos
                        foreach (var item in compras)
                        {
                            var producto = _db.Producto.FirstOrDefault(p => p.Id == item.Producto.Id);
                            if (producto != null)
                            {
                                producto.Cantidad -= item.Cantidad;
                                _db.Producto.Update(producto);
                            }
                        }

                        _db.SaveChanges();

                        // Limpiar el carrito después de la compra
                        HttpContext.Session.Remove("carrito");

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Manejar el error según sea necesario, por ejemplo, registrar el error o mostrar un mensaje al usuario
                        throw new InvalidOperationException("Error al finalizar la compra", ex);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
