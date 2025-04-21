using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public interface IVehicleRepository
    {
        public void Add(Vehicle vehicle);
        public void Update(Vehicle trip);
        public void Delete(int id);
        public List<Vehicle> GetAll();
        public Vehicle GetById(int id);
        public void Save();
    }
}
