namespace VideoManagement.API.Dtos;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IFormFile VideoUrl { get; set; } = default!;

    public int SortNumber { get; set; }
}
