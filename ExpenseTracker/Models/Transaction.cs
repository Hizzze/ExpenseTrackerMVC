using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }
    
    // CategoryID (Foreign key)
    public int CategoryId { get; set; }
    public 

    public int Amount { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}