using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models
{
    public class Venta
    {
        [Key]
        public int Id_venta { get; set; }

        public DateTime DiaVenta { get; set; }

        public float Subtotal { get; set; }

        public float Iva { get; set; }

        public float Total { get; set; }

        public ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}