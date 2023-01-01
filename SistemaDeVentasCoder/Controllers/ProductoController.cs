using Microsoft.AspNetCore.Mvc;
using SistemaDeVentasCoder.ADO.NET;
using SistemaDeVentasCoder.Models;
using System.Data.SqlClient;

namespace SistemaDeVentasCoder.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private ProductoHandler handler = new ProductoHandler();

        [HttpGet]
        public ActionResult<List<Producto>> Get()
        {
            try
            {
                List<Producto> lista = handler.GetProductos();
                return Ok(lista);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> Get(int id) 
        {
            try
            {
                Producto producto = handler.ObtenerProducto(id);
                if (producto != null)
                {
                    return Ok(producto);
                }
                else
                {
                    return NotFound("El producto no fue encontrado");
                }
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Producto producto) 
        {
            try
            {
                handler.CrearProducto(producto);
                return StatusCode(StatusCodes.Status201Created, producto);
            }
            catch (Exception ex)
            {

                return Problem (ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Producto> Put(int id, [FromBody] Producto productoAActualizar) 
        {
            try
            {
                Producto? productoActualizado = handler.ActualizarProducto(id, productoAActualizar);
                if (productoActualizado != null)
                {
                    return Ok(productoActualizado);
                }
                else
                {
                    return NotFound("El producto no fue encontrado");
                }
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] int id) 
        {
            try
            {
                bool seElimino = handler.EliminarProducto(id);
                if (seElimino) 
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}