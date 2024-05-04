using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string Title { get; set; }

    [Required]
    [Column(TypeName = "varchar(5)")] public string Icon { get; set; } = string.Empty;
    [Column(TypeName = "varchar(10)")] public string Type { get; set; } = "Expense";
    [NotMapped]
    public string? TitleWithIcon {
        get
        {
            return this.Icon + " " + this.Title;
        } 
    }
}