using SistemaDeVentasCoder.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.ADO.NET
{
    public class VentaHandler
    {
        private SqlConnection conexion;
        private string CadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public VentaHandler() 
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
        public List<Venta> obtenerVenta(int? id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            List<Venta> lista = new List<Venta>();
            try
            {
                string query = "SELECT A.Id, A.Comentarios, A.IdUsuario, B.Id AS IdProductoVendido, B.IdProducto, B.IdVenta, B.Stock, C.Descripciones, C.PrecioVenta " +
                    "FROM Venta AS A " +
                    "INNER JOIN ProductoVendido AS B " +
                    "ON A.Id = B.IdVenta " +
                    "INNER JOIN Producto AS C " +
                    "ON B.IdProducto = C.Id";
                if (id != null)
                {
                    query += " WHERE A.Id = @id";
                }
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    if (id != null)
                    {
                        cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    }
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int ultimoIdVenta = 0;
                            Venta venta = new Venta();
                            while (reader.Read())
                            {
                                int IdVenta = Convert.ToInt32(reader["Id"].ToString());
                                if (IdVenta != ultimoIdVenta)
                                {
                                    ProductoVendido productoVendido = new ProductoVendido()
                                    {
                                        Id = Convert.ToInt32(reader["IdProductoVendido"].ToString()),
                                        IdProducto = Convert.ToInt32(reader["IdProducto"].ToString()),
                                        Stock = Convert.ToInt32(reader["Stock"].ToString()),
                                        producto = new Producto()
                                        {
                                            Descripciones = reader["Descripciones"].ToString(),
                                            PrecioVenta = Convert.ToDouble(reader["PrecioVenta"].ToString())
                                        }
                                    };
                                    if (venta.ProductosVendidos != null)
                                    {
                                        venta.ProductosVendidos.Add(productoVendido);
                                    }
                                }
                                else
                                {
                                    if (ultimoIdVenta != 0)
                                    {
                                        lista.Add(venta);
                                    }
                                    venta = new Venta()
                                    {
                                        Id = IdVenta,
                                        Comentarios = reader["Comentarios"].ToString(),
                                        IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                                        ProductosVendidos = new List<ProductoVendido>(),
                                    };
                                    ultimoIdVenta = IdVenta;
                                }
                            }
                            lista.Add(venta);
                        }
                    }
                }
                return lista;
            }
            catch
            {
                throw;
            }
            finally
            {
                conexion.Close();
            }
        }
        public void CargarVenta (Venta venta) 
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Venta(Comentarios, IdUsuario) VALUES(@comentarios, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = venta.Comentarios });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = venta.IdUsuario });
                    venta.Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    if (venta.ProductosVendidos != null && venta.ProductosVendidos.Count > 0)
                    {
                        foreach (ProductoVendido productoVendido in venta.ProductosVendidos)
                        {
                            productoVendido.IdVenta = venta.Id;
                            ProductoVendido productoVendidoRegistrado = RegistrarProducto(productoVendido);
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

        private ProductoVendido RegistrarProducto(ProductoVendido productoVendido)
        {
            Producto? producto = ProductoVendidoHandler.obtenerProductoSimplificadoPorId(productoVendido.IdProducto, conexion);
            if (producto != null) 
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ProductoVendido(Stock, IdProducto, IdVenta) VALUES(@stock, @idProducto, @idVenta); SELECT @@Identity;", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.BigInt) { Value = productoVendido.Stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.Int) { Value = productoVendido.IdProducto });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = productoVendido.IdVenta });
                    productoVendido.Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
                DisminuirStock(producto, productoVendido.Stock);
            }
            else
            {
                throw new Exception("Producto no encontrado");
            }
            return productoVendido;
        }

        private void DisminuirStock(Producto producto, int cantidadVendida)
        {
            using (SqlCommand cmd = new SqlCommand("UPDATE Producto SET Stock = @stock WHERE Id = @id", conexion))
            {
                cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.Stock - cantidadVendida });
                cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = producto.Id });
                cmd.ExecuteNonQuery();
            }
        }
        public bool EliminarVenta(int? id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                int filasAfectadas = 0;
                string query = "SELECT * FROM Venta V INNER JOIN ProductoVendido PV on V.Id = PV.IdVenta";
                if (id != null)
                {
                    query += " WHERE V.Id = @id";
                }
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                return filasAfectadas < 0;
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
    }
}
