using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.src.Models;

    [Table("ReportedProducts")]
    public class ReportedProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProductId { get; set; }      // FK to Products
        public int ReportedByUserId { get; set; } // FK to Users (buyer who reported)
        public string Reason { get; set; }
        public DateTime ReportedAt { get; set; }
        public bool IsResolved { get; set; }    // admin marked as reviewed

        public ReportedProduct() { }
    }

