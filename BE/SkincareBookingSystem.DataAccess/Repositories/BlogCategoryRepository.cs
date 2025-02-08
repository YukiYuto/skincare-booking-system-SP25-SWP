using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class BlogCategoryRepository : Repository<BlogCategory>, IBlogCategoryRepository
    {
        private readonly ApplicationDbContext _context; 
        public BlogCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// Updates the target BlogCategory object with the values of the source BlogCategory object.
        /// This ensures that the target object is updated with the necessary values, not the entire object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public void Update(BlogCategory target, BlogCategory source)
        {
            _context.Set<BlogCategory>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }
    }
}
