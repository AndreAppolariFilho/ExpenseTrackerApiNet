using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly InMemoryDBSetContext _dbContext;

    public CategoriesController(InMemoryDBSetContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _dbContext.Categories.ToList();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {

        var category = _dbContext.Categories.FirstOrDefault(e => e.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] CreateCategoryDTO createCategory)
    {
        Category category = new Category();
        category.Name = createCategory.Name;
        _dbContext.Categories.Add(
            category
        );
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }
}