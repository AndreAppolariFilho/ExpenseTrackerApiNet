using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly InMemoryDBSetContext _dbContext;

    public ExpensesController(InMemoryDBSetContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var expenses = _dbContext.Expenses.ToList();
        var expensesDTO = new List<CreateExpenseDTO>();
        foreach (var expense in expenses)
        {
            var expenseDTO = new CreateExpenseDTO();
            expenseDTO.Amount = expense.Amount;
            expenseDTO.CategoryId = expense.CategoryId;
            expenseDTO.Description = expense.Description;
            expenseDTO.ExpenseDate = expense.ExpenseDate;
            expensesDTO.Add(expenseDTO);
        }
        return Ok(expensesDTO);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {

        var expense = _dbContext.Expenses.FirstOrDefault(e => e.Id == id);
        if (expense == null)
        {
            return NotFound();
        }
        var expenseDTO = new CreateExpenseDTO();
        expenseDTO.Amount = expense.Amount;
        expenseDTO.CategoryId = expense.CategoryId;
        expenseDTO.Description = expense.Description;
        expenseDTO.ExpenseDate = expense.ExpenseDate;
        return Ok(expenseDTO);
    }
    [HttpPost]
    public async Task<ActionResult<Expense>> Create([FromBody] CreateExpenseDTO createExpenseDTO)
    {
        Category category;
        try
        {
            category = _dbContext.Categories.Where(c => c.Id == createExpenseDTO.CategoryId).First();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        Expense expense = new Expense();
        expense.Amount = createExpenseDTO.Amount;
        expense.Description = createExpenseDTO.Description;
        expense.ExpenseDate = createExpenseDTO.ExpenseDate;
        expense.CategoryId = createExpenseDTO.CategoryId;
        expense.Category = category;
        _dbContext.Expenses.Add(expense);
        await _dbContext.SaveChangesAsync();
        var expenseDTO = new CreateExpenseDTO();
        expenseDTO.Amount = expense.Amount;
        expenseDTO.CategoryId = expense.CategoryId;
        expenseDTO.Description = expense.Description;
        expenseDTO.ExpenseDate = expense.ExpenseDate;
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expenseDTO);
    }
}