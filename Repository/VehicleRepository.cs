using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext context;

        public VehicleRepository(AppDbContext _context)
        {
            context=_context;
        }
        public void Add(Vehicle vehicle)
        {
            context.Add(vehicle);
        }

        public void Delete(int id)
        {
            context.Remove(GetById(id));
        }

        public List<Vehicle> GetAll()
        {
           return context.vehicles.ToList();
        }

        public Vehicle GetById(int id)
        {
            return context.vehicles.FirstOrDefault(x => x.Id==id)!;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Vehicle vehicle)
        {
            context.Update(vehicle);
        }
    }
}
