using Microsoft.AspNetCore.Mvc;
using Taller2.Interface;
using Taller2.Models;

namespace Taller2.Controllers
{
    [Route("api/producto")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductoController(IProductoRepository productoRepository, ICloudinaryService cloudinaryService)
        {
            _productoRepository = productoRepository;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
        {
            var productos = await _productoRepository.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado." });
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre no puede estar vacío." });
            }
            if (producto.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio no puede ser negativo." });
            }
            if (producto.Stock < 0)
            {
                return BadRequest(new { mensaje = "El stock no puede ser negativo." });
            }
            if (string.IsNullOrWhiteSpace(producto.ImagenUrl))
            {
                return BadRequest(new { mensaje = "La imagen es obligatoria." });
            }

            try
            {
                // Si llega una imagen en base64, se sube a Cloudinary y se guarda la URL
                if (EsBase64(producto.ImagenUrl))
                {
                    producto.ImagenUrl = await _cloudinaryService.UploadImage(producto.ImagenUrl);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            await _productoRepository.AddAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Producto productoDto)
        {
            if (string.IsNullOrWhiteSpace(productoDto.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre no puede estar vacío." });
            }
            if (productoDto.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio no puede ser negativo." });
            }
            if (productoDto.Stock < 0)
            {
                return BadRequest(new { mensaje = "El stock no puede ser negativo." });
            }

            var productoExistente = await _productoRepository.GetByIdAsync(id);
            if (productoExistente == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado." });
            }

            productoExistente.Nombre = productoDto.Nombre;
            productoExistente.Descripcion = productoDto.Descripcion;
            productoExistente.Precio = productoDto.Precio;
            productoExistente.Stock = productoDto.Stock;

            try
            {
                // Si llega una imagen nueva en base64, se sube a Cloudinary
                if (EsBase64(productoDto.ImagenUrl))
                {
                    productoExistente.ImagenUrl = await _cloudinaryService.UploadImage(productoDto.ImagenUrl!);
                }
                // Si llega una URL normal, se guarda tal cual
                else if (!string.IsNullOrWhiteSpace(productoDto.ImagenUrl))
                {
                    productoExistente.ImagenUrl = productoDto.ImagenUrl;
                }
                // Si no llega nada, se conserva la imagen que ya tenía
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }

            await _productoRepository.UpdateAsync(productoExistente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado." });
            }

            await _productoRepository.DeleteAsync(producto);
            return NoContent();
        }

        // Revisa si el texto es una imagen en base64 (empieza por "data:image")
        private static bool EsBase64(string? valor)
        {
            return !string.IsNullOrWhiteSpace(valor) && valor.StartsWith("data:image");
        }
    }
}