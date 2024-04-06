using System.Text.Json.Serialization;

namespace VideoManagement.API.Dtos;

public class ProductUpdateDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IFormFile? Video { get; set; }

}
