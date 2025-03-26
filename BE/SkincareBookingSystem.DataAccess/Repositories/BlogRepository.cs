using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _context; 
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update (Blog target, Blog source)
        {
            _context.Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }

        //public async Task<Blog> GetAsync(Func<Blog, bool> filter, params string[] includeProperties)
        //{
        //    IQueryable<Blog> query = _context.Set<Blog>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return await Task.Run(() => query.FirstOrDefault(filter));
        //}

        //public async Task<IEnumerable<Blog>> GetAllAsync(Func<Blog, bool> filter, params string[] includeProperties)
        //{
        //    IQueryable<Blog> query = _context.Set<Blog>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return await Task.Run(() => query.Where(filter).ToList());
        //}
    }
}
