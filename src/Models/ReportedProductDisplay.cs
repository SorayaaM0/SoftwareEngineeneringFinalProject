using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.src.Models;
public class ReportedProductDisplay
{
    public ReportedProduct Report { get; set; }
    public Product Product { get; set; }
    public User Seller { get; set; }

    public string ProductName => Product?.Name ?? "Unknown";
    public string SellerEmail => Seller?.Email ?? "Unknown";
    public string SellerName => Seller?.Name ?? "Unknown";
    public string Reason => Report?.Reason ?? "No reason given";
    public int ReportCount { get; set; }  // how many times this product was reported
    public DateTime ReportedAt => Report?.ReportedAt ?? DateTime.MinValue;
}