using Microsoft.AspNetCore.Mvc;
using SistemaDeVentasCoder.ADO.NET;
using SistemaDeVentasCoder.Models;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductoVendidoController : ControllerBase
    {
        private ProductoVendidoHandler handler = new ProductoVendidoHandler();

        [HttpGet]
        public ActionResult<List<ProductoVendido>> Get()
        {
            try
            {
                List<ProductoVendido> lista = handler.GetProductosVendidos();
                return Ok(lista);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProductoVendido> Get(int id)
        {
            try
            {
                ProductoVendido productoVendido = handler.ObtenerProductoVendido(id);
                if (productoVendido != null)
                {
                    return Ok(productoVendido);
                }
                else
                {
                    return NotFound("El producto vendido no fue encontrado");
                }
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }
    }
}