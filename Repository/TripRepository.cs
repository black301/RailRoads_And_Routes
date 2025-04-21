using Microsoft.EntityFrameworkCore;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext context;

        public TripRepository(AppDbContext _context)
        {
            context=_context;
        }
        public void Add(Trip trip)
        {
            context.Add(trip);
        }

        public void Delete(int id)
        {
            context.Remove(GetById(id));
        }

        public List<Trip> GetAll()
        {
           return context.Trips.ToList();
        }

        public Trip GetById(int id)
        {
            var tripFromDB = context.Trips.FirstOrDefault(trip => trip.Id==id);
            return tripFromDB!;
        }

        public void Save()
        {
            context.SaveChanges();
        }
        public void Update(Trip trip)
        {
            context.Update(trip);
        }
    }
}
