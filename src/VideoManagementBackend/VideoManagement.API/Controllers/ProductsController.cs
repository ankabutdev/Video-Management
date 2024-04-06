using Microsoft.AspNetCore.Mvc;
using VideoManagement.API.Dtos;
using VideoManagement.API.Services;
using VideoManagement.API.Validations;

namespace VideoManagement.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetByIdasync(int Id)
    {
        return Ok(await _productService.GetByIdAsync(Id));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(string search)
    {
        return Ok(await _productService.SearchAsync(search));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] ProductCreateDto dto)
    {
        var validator = new VideoValidator();
        var check = validator.Validate(dto);
        if(check.IsValid)
        {
            var resultProduct = await _productService.CreateAsync(dto);
            return Ok(resultProduct);
        }
        return BadRequest(check.Errors);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync(int Id, ProductUpdateDto dto)
    {
        return Ok(await _productService.UpdateAsync(Id, dto));
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync(int Id)
    {
        return Ok(await _productService.DeleteAsync(Id));
    }

}
