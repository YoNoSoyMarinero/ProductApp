using Microsoft.EntityFrameworkCore;
using ProductsApp.Intrefaces;
using ProductsApp.Models;

namespace ProductsApp.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products.Include("Category");
        }

        public Product Get(int id)
        {
            return _context.Products.Include("Category").FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
