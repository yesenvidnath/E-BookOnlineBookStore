
using System;
using System.Collections.Generic;

namespace E_BookOnlineBookStore.Models;

public partial class Shop
{
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string BookImage { get; set; }
        public string ISBN { get; set; }
        public string BookDescription { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
}

