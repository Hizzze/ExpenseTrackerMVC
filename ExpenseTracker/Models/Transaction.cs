using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }
    
    // CategoryID (Foreign key)
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int Amount { get; set; }
    [Column(TypeName = "varchar(75)")]
    public string? Note { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}