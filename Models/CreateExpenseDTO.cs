public class CreateExpenseDTO
{
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int CategoryId { get; set; }

}