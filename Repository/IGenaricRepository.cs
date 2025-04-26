namespace Transport__system_prototype.Repository
{
    public interface IGenaricRepository<T> where T : class
    {
        public void Add(T entity);
        public void Delete(T entity);
        public T GetById(object id);
        public List<T> GetAll();
        public void Update(T entity);
        public void Save();
    }
}
