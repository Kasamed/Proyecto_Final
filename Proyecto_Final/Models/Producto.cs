using System.ComponentModel.DataAnnotations;

namespace Proyecto_Final.Models
{
    public class Producto
    {
        [Key]

        public int Id{ get; set; }

        [Required(ErrorMessage= "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range (1,float.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        public string Imagen {  get; set; }
    }
}
