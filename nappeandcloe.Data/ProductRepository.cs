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
                context.ProductSizes.Add(productSize);
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
                return context.Labels.OrderBy(l => l.Name).ToList();
            }
        }

        public IEnumerable<Size> GetAllSizes()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Sizes.OrderBy(l => l.Name).ToList();
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

        public void UpdateProduct(Product product)
        {
            using (var context = new MyContext(_connectionString))
            {
                context.ProductLabels.RemoveRange(context.ProductLabels.Where(p => p.ProductId == product.Id));
                context.ProductLabels.AddRange(product.ProductLabels);

                context.Products.Attach(product);
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
