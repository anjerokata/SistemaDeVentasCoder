using SistemaDeVentasCoder.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.ADO.NET
{
    public class ProductoVendidoHandler
    {
        private SqlConnection conexion;
        private string CadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public ProductoVendidoHandler() 
        {
            try
            {
                conexion = new SqlConnection(CadenaConexion);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private ProductoVendido ObtenerProductoVendidoDesdeReader(SqlDataReader reader)
        {
            ProductoVendido productoVendido = new ProductoVendido();
            productoVendido.Id = Convert.ToInt32(reader["Id"].ToString());
            productoVendido.Stock = Convert.ToInt32(reader["Stock"].ToString());
            productoVendido.IdProducto = Convert.ToInt32(reader["IdProducto"].ToString());
            productoVendido.IdVenta = Convert.ToInt32(reader["IdVenta"].ToString());
            return productoVendido;
        }

        public List<ProductoVendido> GetProductosVendidos() 
        {
            List<ProductoVendido> listaProductosVendidos = new List<ProductoVendido>();
            if (CadenaConexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand ("SELECT * FROM ProductoVendido", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductoVendido productoVendido = ObtenerProductoVendidoDesdeReader(reader);
                                listaProductosVendidos.Add(productoVendido);
                            }
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }
            return listaProductosVendidos;
        }
        public ProductoVendido ObtenerProductoVendido(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            ProductoVendido productoVendido = ObtenerProductoVendidoDesdeReader(reader);
                            return productoVendido;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
        public static Producto? obtenerProductoSimplificadoPorId(int id, SqlConnection conexion)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Stock FROM producto WHERE id = @id", conexion))
                {
                    if (conexion.State == ConnectionState.Closed) conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = new Producto()
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                Stock = Convert.ToInt32(reader["Stock"].ToString())
                            };
                            return producto;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
