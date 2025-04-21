using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public interface IStationRepository
    {
        public void Add(Station station);
        public void Update(Station station);
        public void Delete(int id);
        public List<Station> GetAll();
        public Station GetById(int id);
        public void Save();
    }
}
