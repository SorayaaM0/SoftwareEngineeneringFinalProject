using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class CartItemDisplay
    {
        public CartItem CartItem { get; set; }
        public Product Product { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity
        {
            get => CartItem?.Quantity ?? 0;
            set { if (CartItem != null) CartItem.Quantity = value; }
        }
    }
}