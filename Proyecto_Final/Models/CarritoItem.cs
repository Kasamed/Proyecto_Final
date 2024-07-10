namespace Proyecto_Final.Models
{
    public class CarritoItem
    {
        private Producto _producto;
        private int _cantidad;

        public CarritoItem()
        {

        }

        public CarritoItem(Producto producto, int cantidad)
        {
            this._producto = producto;
            this._cantidad = cantidad;
        }

        public Producto Producto { get => _producto; set => _producto = value; }
        public int Cantidad { get => _cantidad; set => _cantidad = value; }
    }
}
