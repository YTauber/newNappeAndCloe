using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nappeandcloe.Data
{
    public class ProductRepository
    {
        private string _connectionString;
        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name) || product.Price.Equals(null))
            {
                return null;
            }
            using (MyContext context = new MyContext(_connectionString))
            {
                context.Products.Add(product);
                context.SaveChanges();
                return product;
            }
        }

        public void AddLabels(List<string> labels, int productId)
        {
            if (labels == null)
            {
                return;
            }
            using (MyContext context = new MyContext(_connectionString))
            {
                List<Label> l = context.Labels.ToList();

                foreach (string label in labels.Select(d => d.ToLower()).Distinct())
                {

                    Label lab = l.FirstOrDefault(la => la.Name.ToLower() == label.ToLower());
                    if (lab == null)
                    {
                        lab = new Label { Name = label };
                        context.Labels.Add(lab);
                    }
                    context.ProductLabels.Add(new ProductLabel { ProductId = productId, LabelId = lab.Id });


                }
                context.SaveChanges();

            }
        }

   
        public int AddSize(string size)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
               
                    Size sz = context.Sizes.FirstOrDefault(sa => sa.Name.ToLower() == size.ToLower());
                    if (sz == null)
                    {
                        sz = new Size { Name = size };
                        context.Sizes.Add(sz);
                    }

                
                context.SaveChanges();
                return sz.Id;
            }
        }

        public void AddProductSize(ProductSize productSize)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                ProductSize ps = context.ProductSizes.FirstOrDefault(p => p.ProductId == productSize.ProductId && p.SizeId == productSize.SizeId);
                if (ps == null)
                {
                    context.ProductSizes.Add(productSize);
                }
                else
                {
                    ps.Quantity = productSize.Quantity;
                    context.ProductSizes.Attach(ps);
                    context.Entry(ps).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }


        #region
        //public List<Label> AddTags(List<string> labels)
        //{
        //    List<Label> tags = new List<Label>();
        //    if (labels == null)
        //    {
        //        return tags;
        //    }
        //    using (MyContext context = new MyContext(_connectionString))
        //    {
        //        List<Label> l = context.Labels.ToList();

        //        foreach (string label in labels.Select(d => d.ToLower()).Distinct())
        //        {

        //            Label lab = l.FirstOrDefault(la => la.Name.ToLower() == label.ToLower());
        //            if (lab == null)
        //            {
        //                lab = new Label { Name = label };
        //                context.Labels.Add(lab);
        //            }

        //            tags.Add(lab);
        //        }
        //        context.SaveChanges();
        //        return tags;
        //    }
        //}
        #endregion

        public IEnumerable<Product> GetAllProducts()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Products.Include(p => p.ProductLabels).ThenInclude(l => l.Label)
                    .Include(s => s.ProductSizes).ThenInclude(s => s.Size)
                    .ToList();
            }
        }

        public IEnumerable<ProductLabel> GetAllProductLabels()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.ProductLabels.ToList();
            }
        }

        public IEnumerable<Label> GetAllLabels()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Labels.Where(l => l.ProductLabels.Count > 0).OrderBy(l => l.Name).ToList();
            }
        }

        public IEnumerable<Label> GetLabelsByProductId(int productId)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Labels.Where(l => l.ProductLabels.Any(p => p.ProductId == productId)).ToList();
            }
        }

        public IEnumerable<Size> GetAllSizes()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Sizes.Where(s => s.ProductSizes.Count() > 0).OrderBy(l => l.Name).ToList();
            }
        }

        public Product GetProductById(int id)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Products.Include(p => p.ProductLabels).ThenInclude(p => p.Label)
                    .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                    .FirstOrDefault(p => p.Id == id);
            }
        }

        public ProductSize GetProductSizeById(int id)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.ProductSizes.FirstOrDefault(p => p.Id == id);
            }
        }

        public Size GetSizeById(int id)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Sizes.FirstOrDefault(p => p.Id == id);
            }
        }

        public Product GetProductByProductSizeId(int id)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Products.FirstOrDefault(p => p.ProductSizes.Any(s => s.Id == id));
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var context = new MyContext(_connectionString))
            {
                context.ProductLabels.RemoveRange(context.ProductLabels.Where(p => p.ProductId == product.Id));
                context.Products.Attach(product);
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateSize(Size size)
        {
            using (var context = new MyContext(_connectionString))
            {
                context.Sizes.Attach(size);
                context.Entry(size).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public int GetMaxAvail(DateTime date, int productSizeId)
        {
            using (var context = new MyContext(_connectionString))
            {
                int productSize = context.ProductSizes.FirstOrDefault(p => p.Id == productSizeId).Quantity;
                int booked = context.Orders.Where(o => o.Date == date).SelectMany(o => o.OrderDetails).Where(od => od.ProductSizeId == productSizeId).Sum(q => q.Quantity);
                return productSize - booked;
            }
        }


        public int GetMinAvail(DateTime date, int productSizeId)
        {
            using (var context = new MyContext(_connectionString))
            {
                return context.Orders.Where(o => o.Date == date).SelectMany(o => o.OrderDetails).Where(od => od.ProductSizeId == productSizeId).Sum(q => q.PickUps);
            }
        }

    }


}
