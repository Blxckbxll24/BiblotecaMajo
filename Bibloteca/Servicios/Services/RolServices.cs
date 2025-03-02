using Bibloteca.Context;
using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Microsoft.EntityFrameworkCore;

namespace Bibloteca.Servicios.Services
{
    public class RolServices : IRolServices
    {
        private readonly ApplicationDBContext _context;

        public RolServices(ApplicationDBContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Rol>> GetAll()
        {
            List<Rol> result = await _context.Roles.ToListAsync();
            return result;
        }

        public async Task<Rol?> GetById(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<bool> Add(Rol rol)
        {
            _context.Roles.Add(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Rol rol)
        {
            _context.Roles.Update(rol);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            _context.Roles.Remove(rol);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
