using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Expense
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public DateTime ExpenseDate { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}