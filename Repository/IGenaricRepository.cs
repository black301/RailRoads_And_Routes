namespace Transport__system_prototype.Repository
{
    public interface IGenaricRepository<T> where T : class
    {
        public void Add(T entity);
        public Task AddAsync(T entity);
        public void Delete(T entity);
        public Task DeleteAsync(T entity);
        public T GetById(object id);
        public Task<T> GetByIdAsync(object id);
        public List<T> GetAll();
        public Task<List<T>> GetAllAsync();
        public void Update(T entity);
        public Task UpdateAsync(T entity);
        public void Save();
        public Task<int> SaveAsync();
    }
}
