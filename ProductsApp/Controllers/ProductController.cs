using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApp.Intrefaces;
using ProductsApp.Models;

namespace ProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            return Ok(_productRepository.Get(id));
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                _productRepository.Update(product);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productRepository.Add(product);
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product);
            return NoContent();
        }

        [HttpGet("searchByCategory/{category}")]
        public IActionResult ProductsFromSingleCategory(string category)
        {
            return Ok(_productRepository.GetAll().Where(p => p.Category.Name.ToLower() == category.ToLower()).ToList());
        }

        [HttpGet("searchByPrice/{maxPrice}")]
        public IActionResult ProductsCheaperThanMaxPrice(double maxPrice)
        {
            return Ok(_productRepository.GetAll().Where(p => p.Price <= maxPrice).OrderBy(p => p.Price).ToList());
        }

        [HttpGet("statistics")]
        public IActionResult TotalMostExpensiveCategoriesByProducts()
        {
            IQueryable<Product> products = _productRepository.GetAll();
            var totlaCategoryPrice = products.AsEnumerable()
                .GroupBy(p => p.Category)
                .Select(x => new CategoryTotalPriceDTO
                {
                    CategoryName = x.Key.Name,
                    TotalPrice = x.Sum(x => x.Price)
                }).OrderByDescending(x => x.TotalPrice).ToList().Take(2);
            return Ok(totlaCategoryPrice);
        }

        [HttpGet("cheapest")]
        public IActionResult TwoCheapestProducts()
        {
            return Ok(_productRepository.GetAll().OrderBy(p => p.Price).Take(2));
        }
    }
}
