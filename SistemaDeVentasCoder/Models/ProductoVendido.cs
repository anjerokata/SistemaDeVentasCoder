namespace SistemaDeVentasCoder.Models
{
    public class ProductoVendido
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int IdProducto { get; set; }
        public int IdVenta { get; set; }

        public Producto? producto { get; set; }

        public ProductoVendido() 
        {
            Id = 0;
            Stock = 0;
            IdProducto = 0;
            IdVenta = 0;
        }
        public ProductoVendido(int id, int stock, int idProducto, int idVenta)
        {
            Id = id;
            Stock = stock;
            IdProducto = idProducto;
            IdVenta = idVenta;
        }
    }
}
