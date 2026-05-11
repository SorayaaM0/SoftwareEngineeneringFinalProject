using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreApp.Models;
namespace StoreApp.src.Models;



public class SellerDisplay
{
    public User Seller { get; set; }
    public int ProductCount { get; set; }
    public int ReportCount { get; set; }

    public string Name => Seller?.Name ?? "Unknown";
    public string Email => Seller?.Email ?? "Unknown";
    public string StoreName => Seller?.StoreName ?? "No store name";
    public bool IsBanned => Seller?.IsBanned ?? false;
}
