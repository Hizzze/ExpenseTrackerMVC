using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }
    
    // CategoryID (Foreign key)
    [ForeignKey("CategoryId")]
    [Range(1,int.MaxValue, ErrorMessage = "Please select a category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    [Range(1,int.MaxValue, ErrorMessage = "Amount should be greater than 0")]
    public int Amount { get; set; }
    [Column(TypeName = "varchar(75)")]
    public string? Note { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;
    [NotMapped]
    public string? CategoryWithIcon {
        get
        {
            return Category == null ? "" : Category.Icon + " " + Category.Title;
        }
        
    }
    public string? FormattedAmount {
        get
        {
            return ((Category == null || Category.Type=="Expense") ? "- " : "+")  + Amount.ToString("C0");
        }
        
    }
    
}