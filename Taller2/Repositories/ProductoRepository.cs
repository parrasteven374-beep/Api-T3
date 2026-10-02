using Microsoft.EntityFrameworkCore;
using Taller2.Db;
using Taller2.Interface;
using Taller2.Models;

namespace Taller2.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        //VER
        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Producto.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Producto.FindAsync(id);
        }

        //CREAR
        public async Task AddAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.ImagenUrl))
            {
                throw new Exception("La URL de la imagen es obligatoria");
            }

            await _context.Producto.AddAsync(producto);
            await _context.SaveChangesAsync();
        }


        //UPDATE
        public async Task UpdateAsync(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.ImagenUrl))
            {
                throw new Exception("La URL de la imagen es obligatoria");
            }

            _context.Producto.Update(producto);
            await _context.SaveChangesAsync();
        }




        //BORRAR

        public async Task DeleteAsync(Producto producto)
        {
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
        }
    }
}