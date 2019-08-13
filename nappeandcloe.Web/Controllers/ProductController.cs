using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using nappeandcloe.Data;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace nappeandcloe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IHostingEnvironment _environment;
        private string _connectionString;
        public ProductController(IConfiguration configuration, IHostingEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _environment = environment;
        }

        [Route("AddProduct")]
        [HttpPost]
        public Product AddProduct(Product product)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            productRepo.AddProduct(product);
            return product;
        }

        [Route("AddPicture")]
        [HttpPost]
        public string AddPicture(IFormFile file)
        {
            string PictureName;
            if (file != null)
            {

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string fullPath = Path.Combine(_environment.ContentRootPath, "ClientApp/public/UploadedImages", fileName);

                using (var imageStream = file.OpenReadStream())
                using (Image<Rgba32> image = Image.Load(imageStream))
                {
                    var y = (200 * image.Height) / image.Width;
                    image.Mutate(x => x.Resize(200, y));
                    image.Save(fullPath);
                }

                //using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
                //{
                //    file.CopyTo(stream);
                //}
                PictureName = fileName;

            }
            else
            {
                PictureName = "Default.jpg";
            }

            return PictureName;
        }

        [Route("AddLabels")]
        [HttpPost]
        public void AddLabels(TagView labelView)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            productRepo.AddLabels(labelView.Tags, labelView.ProductId);
        }

        [Route("AddSizes")]
        [HttpPost]
        public void AddSizes(TagView sizes)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            foreach (SizeView s in sizes.Sizes)
            {
                int SizeId = productRepo.AddSize(s.Size);

                productRepo.AddProductSize( new ProductSize
                {
                    ProductId = sizes.ProductId,
                    Quantity = s.Quantity,
                    SizeId = SizeId
                });
            }
        }

        [Route("UpdateProduct")]
        [HttpPost]
        public void UpdateProduct(Product product)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            productRepo.UpdateProduct(product);
        }

        #region
        //[Route("AddTags")]
        //[HttpPost]
        //public IEnumerable<Label> AddTags(TagView labelView)
        //{
        //    ProductRepository productRepo = new ProductRepository(_connectionString);
        //    return productRepo.AddTags(labelView.Tags);
        //}
        #endregion

        [Route("GetAllProducts")]
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            return productRepo.GetAllProducts();
        }

        [Route("GetAllLabels")]
        [HttpGet]
        public IEnumerable<Label> GetAllLabels()
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            return productRepo.GetAllLabels();
        }

        [Route("GetAllSizes")]
        [HttpGet]
        public IEnumerable<Size> GetAllSizes()
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            return productRepo.GetAllSizes();
        }

        [Route("GetAllProductLabelss")]
        [HttpGet]
        public IEnumerable<ProductLabel> GetAllProductLabels()
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            return productRepo.GetAllProductLabels();
        }

        [Route("GetProductById/{productId}/{month}/{year}")]
        [HttpGet]
        public ProductView GetProductById(int productId, int month, int year)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            OrderRepository orderRepo = new OrderRepository(_connectionString);
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();

            Product p =  productRepo.GetProductById(productId);
            ProductView productView = new ProductView
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Notes = p.Notes,
                PictureName = p.PictureName,
                ProductLabels = p.ProductLabels,

                ProductSizeViews = p.ProductSizes.Select((s) => new ProductSizeView
                {
                    Id = s.Id,
                    Size = s.Size.Name,
                    PricePer = p.Price,
                    Quantity = s.Quantity,

                    MinAvail = 0,

                    MaxAvail = order.Date == null ? 0 : productRepo.GetMaxAvail(order.Date.Value, s.Id),
                    Checked = order.ProductViews.Any(pr => pr.ProductSizeViews.Any(sz => sz.Id == s.Id)),

                    OrderPrice = p.Price,
                    OrderAmount = GetOrderAmount(s.Id)

                }).ToList(),

                CalendarEvents = orderRepo.GetOrdersForCalendarByProductId(month, year, p.Id).ToList()
            };
            if (order.Date.HasValue && order.Date.Value.Year == year && order.Date.Value.Month == month)
            {
                productView.CalendarEvents.Add(new CalendarEvent
                {
                    title = "Draft",
                    Id = 12212,
                    From = order.Date.Value,
                    To = order.Date.Value,
                    Color = "#125422"
                });
            }
            return productView;
        }

        private int GetOrderAmount(int id)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            ProductSizeView productSize = order.ProductViews.SelectMany(p => p.ProductSizeViews).FirstOrDefault(s => s.Id == id);
            if (productSize == null)
            {
                return 0;
            }
            else
            {
                return productSize.OrderAmount;
            }
        }

    }

    public class TagView
    {
        public List<string> Tags { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<SizeView> Sizes { get; set; }
    }

    public class SizeView
    {
        public string Size { get; set; }
        public int Quantity { get; set; }
    }

   

}