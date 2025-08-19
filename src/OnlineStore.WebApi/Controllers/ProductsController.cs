using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Common;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Repositories;
using OnlineStore.Domain.Entities;

namespace OnlineStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductResponse>>> GetProducts([FromQuery] ProductListQuery query)
    {
        var products = await _productRepository.GetInStockPagedAsync(
            query.PageNumber, 
            query.PageSize, 
            query.CategoryId, 
            query.Q);

        var productResponses = _mapper.Map<List<ProductResponse>>(products.Items);

        var result = new PagedResult<ProductResponse>
        {
            Items = productResponses,
            PageNumber = products.PageNumber,
            PageSize = products.PageSize,
            TotalCount = products.TotalCount
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetProduct(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<ProductResponse>(product);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> CreateProduct(CreateProductRequest request)
    {
        // Check if category exists
        var categoryExists = await _categoryRepository.ExistsAsync(c => c.Id == request.CategoryId);
        if (!categoryExists)
        {
            return BadRequest(new { message = "Category not found." });
        }

        // Check if product name already exists
        var nameExists = await _productRepository.NameExistsAsync(request.Name);
        if (nameExists)
        {
            return BadRequest(new { message = "Product name already exists." });
        }

        var product = _mapper.Map<Product>(request);
        await _productRepository.AddAsync(product);

        var response = _mapper.Map<ProductResponse>(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductResponse>> UpdateProduct(Guid id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Check if name already exists (excluding current product)
        if (!string.IsNullOrEmpty(request.Name))
        {
            var nameExists = await _productRepository.NameExistsAsync(request.Name, id);
            if (nameExists)
            {
                return BadRequest(new { message = "Product name already exists." });
            }
            product.Name = request.Name;
        }

        // Update other properties if provided
        if (request.Price.HasValue)
            product.Price = request.Price.Value;
        
        if (request.StockQuantity.HasValue)
            product.StockQuantity = request.StockQuantity.Value;
            
        if (request.ImageUrl != null)
            product.ImageUrl = request.ImageUrl;
            
        if (request.Description != null)
            product.Description = request.Description;
            
        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _categoryRepository.ExistsAsync(c => c.Id == request.CategoryId.Value);
            if (!categoryExists)
            {
                return BadRequest(new { message = "Category not found." });
            }
            product.CategoryId = request.CategoryId.Value;
        }

        product.UpdatedAt = DateTime.UtcNow;
        await _productRepository.UpdateAsync(product);

        var response = _mapper.Map<ProductResponse>(product);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Check if product is referenced in any orders
        var hasOrders = await _productRepository.ExistsAsync(p => p.OrderItems.Any() && p.Id == id);
        if (hasOrders)
        {
            return BadRequest(new { message = "Cannot delete product that has been ordered." });
        }

        await _productRepository.RemoveAsync(product);
        return NoContent();
    }
}
