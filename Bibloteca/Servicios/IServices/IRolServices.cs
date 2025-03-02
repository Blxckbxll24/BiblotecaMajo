using Bibloteca.Models.Domain;

namespace Bibloteca.Servicios.IServices
{
    public interface IRolServices
    {
        public Task<List<Rol>> GetAll();
        public Task<Rol?> GetById(int id);
        public Task<bool> Add(Rol rol);
        public Task<bool> Update(Rol rol);
        public Task<bool> Delete(int id);
    }
}
