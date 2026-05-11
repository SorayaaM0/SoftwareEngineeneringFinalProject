using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace StoreApp.src.Models;

[Table("SecurityLogs")]
public class SecurityLog
{
    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public int? UserId { get; set; }
    public string Details { get; set; }
    public DateTime OccurredAt { get; set; }
    public bool IsRead { get; set; }

    public SecurityLog() { }
}