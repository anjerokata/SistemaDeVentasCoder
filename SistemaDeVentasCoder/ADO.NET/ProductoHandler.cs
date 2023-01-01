using SistemaDeVentasCoder.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.ADO.NET
{
    public class ProductoHandler
    {
        private SqlConnection conexion;
        private string CadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public ProductoHandler() 
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
        private Producto ObtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.Id = Convert.ToInt32(reader["Id"].ToString());
            producto.Descripciones = reader["Descripciones"].ToString();
            producto.Costo = Convert.ToDouble(reader["Costo"].ToString());
            producto.PrecioVenta = Convert.ToDouble(reader["PrecioVenta"].ToString());
            producto.Stock = Convert.ToInt32(reader["Stock"].ToString());
            producto.IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString());
            return producto;
        }

        public List<Producto> GetProductos() 
        {
            List<Producto> listaProductos = new List<Producto>();
            if (CadenaConexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand ("SELECT * FROM Producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Producto producto = ObtenerProductoDesdeReader(reader);
                                listaProductos.Add(producto);
                            }
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
            return listaProductos;
        }
        public Producto ObtenerProducto (int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Producto WHERE Id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = ObtenerProductoDesdeReader(reader);
                            return producto;
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

        public void CrearProducto (Producto producto) 
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("descripciones", SqlDbType.VarChar) { Value = producto.Descripciones });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Money) { Value = producto.Costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Money) { Value = producto.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.Stock });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario});
                    cmd.ExecuteNonQuery();
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
        public Producto ActualizarProducto(int id, Producto productoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                Producto producto = ObtenerProducto(productoAActualizar.Id);
                if (producto == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (producto.Descripciones != productoAActualizar.Descripciones && !string.IsNullOrEmpty(productoAActualizar.Descripciones))
                {
                    camposAActualizar.Add("descripciones = @descripciones");
                    producto.Descripciones = productoAActualizar.Descripciones;
                }
                if (producto.Costo != productoAActualizar.Costo && productoAActualizar.Costo > 0)
                {
                    camposAActualizar.Add("costo = @costo");
                    producto.Costo = productoAActualizar.Costo;
                }
                if (producto.PrecioVenta != productoAActualizar.PrecioVenta && productoAActualizar.PrecioVenta > 0)
                {
                    camposAActualizar.Add("precioVenta = @precioVenta");
                    producto.PrecioVenta = productoAActualizar.PrecioVenta;
                }
                if (producto.Stock != productoAActualizar.Stock && productoAActualizar.Stock > 0)
                {
                    camposAActualizar.Add("Stock = @stock");
                    producto.Stock = productoAActualizar.Stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas actualizadas");
                }

                using(SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {string.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("descripciones", SqlDbType.VarChar) { Value = productoAActualizar.Descripciones });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Money) { Value = productoAActualizar.Costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Money) { Value = productoAActualizar.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoAActualizar.Stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    cmd.ExecuteNonQuery();
                    return producto;
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
        public bool EliminarProducto(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Producto WHERE Id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                
                return filasAfectadas > 0;
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
