using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Domain.Entities;

namespace OnlineStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
    {
        var categories = await _categoryRepository.GetPagedAsync(1, 1000); // Get all categories
        var response = _mapper.Map<List<CategoryResponse>>(categories.Items);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetCategory(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<CategoryResponse>(category);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
    {
        // Check if category name already exists
        var nameExists = await _categoryRepository.NameExistsAsync(request.Name);
        if (nameExists)
        {
            return BadRequest(new { message = "Category name already exists." });
        }

        var category = _mapper.Map<Category>(request);
        await _categoryRepository.AddAsync(category);

        var response = _mapper.Map<CategoryResponse>(category);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(Guid id, CreateCategoryRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Check if name already exists (excluding current category)
        var nameExists = await _categoryRepository.NameExistsAsync(request.Name, id);
        if (nameExists)
        {
            return BadRequest(new { message = "Category name already exists." });
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(category);

        var response = _mapper.Map<CategoryResponse>(category);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Check if category has products
        var hasProducts = await _categoryRepository.ExistsAsync(c => c.Products.Any() && c.Id == id);
        if (hasProducts)
        {
            return BadRequest(new { message = "Cannot delete category that has products." });
        }

        await _categoryRepository.RemoveAsync(category);
        return NoContent();
    }
}
