using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models
{
    public class DetalleVenta
    {
        [Key]
        public int Id_detalleventa { get; set; }

        [ForeignKey("Venta")]
        public int Id_venta { get; set; }
        public Venta Venta { get; set; }

        public int Id_producto { get; set; }

        [ForeignKey("Comprador")]
        public int Id_comprador { get; set; }
        public Comprador Comprador { get; set; }

        public int Cantidad { get; set; }

        public float Total { get; set; }

    }
}