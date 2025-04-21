using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public class StationRepository : IStationRepository
    {
        private readonly AppDbContext context;

        public StationRepository(AppDbContext _context)
        {
            context=_context;
        }
        public void Add(Station station)
        {
            context.Add(station);
        }

        public void Delete(int id)
        {
            context.Remove(GetById(id));
        }

        public List<Station> GetAll()
        {
            return context.Stations.ToList();
        }

        public Station GetById(int id)
        {
            Station stationFromDB = context.Stations.FirstOrDefault(station => station.Id==id)!;
            return stationFromDB;
        }

        public void Update(Station station)
        {
            context.Update(station);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}