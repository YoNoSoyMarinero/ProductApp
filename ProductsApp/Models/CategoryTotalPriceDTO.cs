using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsApp.Models
{
    public class CategoryTotalPriceDTO
    {
        public string CategoryName { get; set; }
        public double TotalPrice { get; set; }
    }
}
