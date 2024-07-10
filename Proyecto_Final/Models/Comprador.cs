using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Models
{
    public class Comprador
    {
        [Key]
        public int Id_comprador { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Direccion { get; set; }

        public int Telefono { get; set; }

        public string Metodo_pago { get; set; }

        public ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
