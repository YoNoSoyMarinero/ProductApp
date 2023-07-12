using ProductsApp.Models;

namespace ProductsApp.Intrefaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAll();
        Category Get(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
