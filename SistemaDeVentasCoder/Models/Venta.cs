namespace SistemaDeVentasCoder.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public string? Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public List<ProductoVendido>? ProductosVendidos { get; set; }
        public Venta (int id,string comentarios, int idUsuario)
        {
            Id = id;
            Comentarios = comentarios;
            IdUsuario = idUsuario;
        }

        public Venta()
        {
            ProductosVendidos = new List<ProductoVendido>();
        }
    }
}
