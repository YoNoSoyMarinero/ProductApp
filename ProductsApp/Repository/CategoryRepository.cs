using Microsoft.EntityFrameworkCore;
using ProductsApp.Intrefaces;
using ProductsApp.Models;

namespace ProductsApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category Get(int id)
        {
            return _context.Categories.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
