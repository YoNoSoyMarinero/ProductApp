using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsApp.Models;
using ProductsApp.Repository;
using ProductsApp.Intrefaces;
using Moq;
using AutoMapper;
using ProductsApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace TestProductsApp.Controllers
{
    public class ProductControllerTest
    {
        public List<Product> GetDummyProducts () {
            List<Product> products = new List<Product>();
            Product p1 = new Product() { Name = "Ball", Id = 1, Price = 1.50, Category = new Category() { Id = 1, Name = "Toy" }, CategoryId = 1 };
            Product p2 = new Product() { Name = "Apple", Id = 2, Price = 2.50, Category = new Category() { Id = 2, Name = "Food" }, CategoryId = 2 };
            Product p3 = new Product() { Name = "Apple", Id = 3, Price = 3.50, Category = new Category() { Id = 3, Name = "Clothes" }, CategoryId = 3 };
            products.Add(p1);
            products.Add(p2);
            products.Add(p3);
            return products;
        }

        public Product GetDummyProduct()
        {
            return new Product()
            {
                Id = 1,
                Name = "Recket",
                Price = 1.50,
                Category = new Category() { Id = 1, Name = "Toy" },
                CategoryId = 1
            };
        }

        [Fact]
        public void GetProduct_ValidId_ReturnsObject()
        {
            Product product = GetDummyProduct();

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.Get(1)).Returns(product);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockProductRepository.Object, mapper);
            var actionResult = controller.GetProduct(1) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            Product resultProduct = (Product)actionResult.Value;
            Assert.Equal(resultProduct.Name, product.Name);
            Assert.Equal(resultProduct.Price, product.Price);
            Assert.Equal(resultProduct.CategoryId, product.CategoryId);
        }

        [Fact]
        public void GetProduct_Invalid_BadRequestResult()
        {
            var mockRepository = new Mock<IProductRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockRepository.Object, mapper);
            var actionResult = controller.GetProduct(12) as BadRequestResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void GetProducts_Valid_ReturnsCollection()
        {

            List<Product> products = GetDummyProducts();
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAll()).Returns(products.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockProductRepository.Object, mapper);
            var actionResult = controller.GetProducts() as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            EnumerableQuery<Product> resultProducts = actionResult.Value as EnumerableQuery<Product>;
            Assert.Equal(resultProducts.Count(), products.Count);
        }

        [Fact]
        public void DeleteProduct_ValidId_DeletesProduct()
        {
            List<Product> products = GetDummyProducts();
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.Get(1)).Returns(products[0]);
            mockProductRepository.Setup(x => x.Delete(products[0])).Callback(() => products.RemoveAt(0));

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockProductRepository.Object, mapper);
            var actionResult = controller.DeleteProduct(1) as NoContentResult;

            Assert.NotNull(actionResult);
            Assert.Equal(products.Count, 2);
        }

        [Fact]
        public void Delete_Invalid_BadRequestResult()
        {
            var mockRepository = new Mock<IProductRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockRepository.Object, mapper);
            var actionResult = controller.DeleteProduct(12) as NotFoundResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void Put_ValidId_ReturnsObject()
        {
            List<Product> products = GetDummyProducts();
            Product newProduct = GetDummyProduct();

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.Update(newProduct)).Callback(() => products[0] = newProduct);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockRepository.Object, mapper);
            var actionResult = controller.PutProduct(1, newProduct) as OkObjectResult;
            Assert.Equal(newProduct, products[0]);
        }

        [Fact]
        public void Put_IvalidId_ReturnsBadRequest()
        {
            List<Product> products = GetDummyProducts();
            Product newProduct = GetDummyProduct();

            var mockRepository = new Mock<IProductRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockRepository.Object, mapper);
            var actionResult = controller.PutProduct(2, newProduct) as BadRequestResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void FilterByPrice_Valid_ReturnsFilterCollections()
        {
            List<Product> products = GetDummyProducts();
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAll()).Returns(products.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockProductRepository.Object, mapper);
            var actionResult = controller.ProductsCheaperThanMaxPrice(3.00) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<Product> resultProducts = actionResult.Value as List<Product>;
            Assert.Equal(resultProducts.Count, 2);
            Assert.True(resultProducts[0].Price < 3.00);
            Assert.True(resultProducts[1].Price < 3.00);
        }

        [Fact]
        public void LowestPrice_Valid_ReturnsFilterCollections()
        {
            List<Product> products = GetDummyProducts();
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(x => x.GetAll()).Returns(products.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new ProductController(mockProductRepository.Object, mapper);
            var actionResult = controller.TwoCheapestProducts() as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<Product> resultProducts = actionResult.Value as List<Product>;
            Assert.Equal(resultProducts.Count, 2);
            Assert.Equal(products.OrderBy(p => p.Price).Take(2), resultProducts);
        }
    }
}
