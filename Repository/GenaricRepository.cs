using Microsoft.EntityFrameworkCore;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly AppDbContext context;
        private DbSet<T> table = null!;
        public GenaricRepository(AppDbContext _context)
        {
            context=_context;
            table = context.Set<T>();
        }
        public void Add(T entity)
        {
            table.Add(entity);
        }
        public void Delete(T entity)
        {
            table.Remove(entity);
        }
        public T GetById(object id)
        {
            return table.Find(id)!;
        }
        public List<T> GetAll()
        {
            return table.ToList();
        }

        public void Update(T entity)
        {
            table.Update(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
