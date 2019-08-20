using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nappeandcloe.Data
{
    public class OrderViewRepository
    {
        private string _connectionString;
        private OrderView order;
        public OrderViewRepository(string connectionString, OrderView orderView)
        {
            _connectionString = connectionString;
            order = orderView;
        }

        public IEnumerable<OrderView> GetOrderViewsForOrders(List<Order> orders)
        {
            


            List<OrderView> orderViews = new List<OrderView>();


            foreach (Order o in orders)
            {
                orderViews.Add(GetOrderViewForOrder(o));
            }
            return orderViews;
        }

        public OrderView GetOrderViewForOrder(Order o)
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            ProductRepository productRepo = new ProductRepository(_connectionString);
            OrderView orderView = new OrderView
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Date = o.Date,
                DeliveryCharge = o.DeliveryCharge,
                Discount = o.Discount,
                Notes = o.Notes,
                TaxExemt = o.TaxExemt,
                CustomerId = o.CustomerId,
                Customer = costumerRepo.GetCustomerById(o.CustomerId)
            };

            foreach (OrderDetail od in o.OrderDetails)
            {
                Product product = productRepo.GetProductByProductSizeId(od.ProductSizeId);
                ProductView productView = orderView.ProductViews.FirstOrDefault(p => p.Id == product.Id);
                if (productView == null)
                {
                    productView = new ProductView
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        PictureName = product.PictureName,
                        Notes = product.Notes
                    };
                    orderView.ProductViews.Add(productView);
                }

                productView.ProductSizeViews.Add(new ProductSizeView
                {
                    Id = od.Id,
                    Size = productRepo.GetSizeById(productRepo.GetProductSizeById(od.ProductSizeId).SizeId).Name,
                    Quantity = od.Quantity,
                    PricePer = productView.Price,
                    MinAvail = productRepo.GetMinAvail(o.Date, od.ProductSizeId),
                    MaxAvail = productRepo.GetMaxAvail(o.Date, od.ProductSizeId),
                    Checked = order.ProductViews.Any(pr => pr.ProductSizeViews.Any(sz => sz.Id == od.ProductSizeId)),
                    OrderAmount = GetOrderAmount(od.ProductSizeId),
                    OrderPrice = od.PricePer
                });
            }
            orderView = SetTotal(orderView);
            return orderView;
        }

        public Inventory GetInventory()
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            Inventory inventory = new Inventory();

            foreach (Product p in productRepo.GetAllProducts())
            {
                inventory.ProductViews.Add(GetProductViewForProduct(p));
            }

            inventory.Sizes = productRepo.GetAllSizes().ToList();
            inventory.Labels = productRepo.GetAllLabels().ToList();

            return inventory;
        }

        public ProductView GetProductViewForProduct(Product p)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);
            OrderRepository orderRepo = new OrderRepository(_connectionString);

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

            };
           
            return productView;
        }


        public OrderView SetTotal(OrderView order)
        {
            double t = 1.08735;
            decimal ta = (decimal)t;
            order.Total = 0;
            order.Tax = 0;
            order.DiscuntAmount = 0;
            foreach (ProductSizeView size in order.ProductViews.SelectMany(p => p.ProductSizeViews))
            {
                order.Total += size.OrderAmount * size.PricePer;
            }
            order.Total += order.Liner.Quantity * order.Liner.Cahrge;
            order.Total += order.DeliveryCharge;
            order.DiscuntAmount = (order.Total * order.Discount) / 100;
            if (!order.TaxExemt)
            {
                order.Tax = (order.Total * ta) - order.Total;
                order.Total += order.Tax;
            }
            order.Total -= order.DiscuntAmount;
            return order;
        }


        public int GetOrderAmount(int id)
        {
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

}
