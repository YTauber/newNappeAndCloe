using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace nappeandcloe.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureName { get; set; }
        public string Notes { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductLabel> ProductLabels { get; set; }
        //public List<PickUp> PickUps { get; set; }
        //public List<Return> Returns { get; set; }
        //public List<Damage> Damages { get; set; }
        //public List<Lost> Losts { get; set; }
    }

    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }

    public class ProductSize
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Size Size { get; set; }
        public int SizeId { get; set; }

        public int Quantity { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }

    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<ProductLabel> ProductLabels { get; set; }
    }

    public class ProductLabel
    {
        [JsonIgnore]
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Label Label { get; set; }
        public int LabelId { get; set; }
    }

}
