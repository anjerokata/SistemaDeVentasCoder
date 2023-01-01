using Microsoft.AspNetCore.Mvc;
using SistemaDeVentasCoder.ADO.NET;
using SistemaDeVentasCoder.Models;
using System.Data.SqlClient;
using System.Text.Json;

namespace SistemaDeVentasCoder.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class LoginController : ControllerBase
    {
        LoginHandler handler = new LoginHandler();
        
        [HttpPost]
        public ActionResult <Usuario> Login(Usuario usuario)
        {
            try
            {
                

               bool usuarioExiste = handler.LoginUsuario(usuario);
                return usuarioExiste ? Ok() : NotFound();                
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }
  
    }
}