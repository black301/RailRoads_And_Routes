using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public interface ITripRepository
    {
        public void Add(Trip trip);
        public void Update(Trip trip);
        public void Delete(int id);
        public List<Trip> GetAll();
        public Trip GetById(int id);
        public void Save();
    }
}
