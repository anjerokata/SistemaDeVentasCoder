using SistemaDeVentasCoder.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.ADO.NET
{
    public class UsuarioHandler
    {
        private SqlConnection conexion;
        private string CadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=ajomuch92_coderhouse_csharp_40930;" +
            "User Id=ajomuch92_coderhouse_csharp_40930;" +
            "Password=ElQuequit0Sexy2022;";

        public UsuarioHandler() 
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
        private Usuario ObtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.Id = Convert.ToInt32(reader["Id"].ToString());
            usuario.Nombre = reader["Nombre"].ToString();
            usuario.Apellido = reader["Apellido"].ToString();
            usuario.NombreUsuario = reader["NombreUsuario"].ToString();
            usuario.Contraseña = reader["Contraseña"].ToString();
            usuario.Mail = reader["Mail"].ToString();
            return usuario;
        }

        public List<Usuario> GetUsuario() 
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            if (CadenaConexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand command = new SqlCommand ("SELECT * FROM Usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = ObtenerUsuarioDesdeReader(reader);
                                listaUsuarios.Add(usuario);
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
            return listaUsuarios;
        }
        public Usuario ObtenerUsuario (int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using(SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using(SqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario usuario = ObtenerUsuarioDesdeReader(reader);
                            return usuario;
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
        public void CrearUsuario(Usuario usuario)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Usuario(Nombre, Apellido, NombreUsuario, Contraseña, Mail) VALUES(@nombre, @apellido, @nombreUsuario, @contraseña, @mail); SELECT @@Identity", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuario.Nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuario.Apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contraseña", SqlDbType.VarChar) { Value = usuario.Contraseña });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.Mail });
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
        public Usuario ActualizarUsuario(int id, Usuario usuarioAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                Usuario usuario = ObtenerUsuario(usuarioAActualizar.Id);
                if (usuario == null)
                {
                    return null;
                }
                List<string> camposAActualizar = new List<string>();
                if (usuario.Nombre != usuarioAActualizar.Nombre && !string.IsNullOrEmpty(usuarioAActualizar.Nombre))
                {
                    camposAActualizar.Add("nombre = @nombre");
                    usuario.Nombre = usuarioAActualizar.Nombre;
                }
                if (usuario.Apellido != usuarioAActualizar.Apellido && !string.IsNullOrEmpty(usuarioAActualizar.Apellido))
                {
                    camposAActualizar.Add("apellido = @apellido");
                    usuario.Apellido = usuarioAActualizar.Apellido;
                }
                if (usuario.NombreUsuario != usuarioAActualizar.NombreUsuario && !string.IsNullOrEmpty(usuarioAActualizar.NombreUsuario))
                {
                    camposAActualizar.Add("nombreUsuario = @nombreUsuario");
                    usuario.NombreUsuario = usuarioAActualizar.NombreUsuario;
                }
                if (usuario.Contraseña != usuarioAActualizar.Contraseña && !string.IsNullOrEmpty(usuarioAActualizar.Contraseña))
                {
                    camposAActualizar.Add("contraseña = @contraseña");
                    usuario.Contraseña = usuarioAActualizar.Contraseña;
                }
                if (usuario.Mail != usuarioAActualizar.Mail && !string.IsNullOrEmpty(usuarioAActualizar.Mail))
                {
                    camposAActualizar.Add("mail = @mail");
                    usuario.Mail = usuarioAActualizar.Mail;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay nuevas filas actualizadas");
                }

                using (SqlCommand cmd = new SqlCommand($"UPDATE Usuario SET {string.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuarioAActualizar.Nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuarioAActualizar.Apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuarioAActualizar.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contraseña", SqlDbType.VarChar) { Value = usuarioAActualizar.Contraseña });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.Mail });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id});
                    cmd.ExecuteNonQuery();
                    return usuario;
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
        public bool EliminarUsuario(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no realizada");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Usuario WHERE Id = @id", conexion))
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
