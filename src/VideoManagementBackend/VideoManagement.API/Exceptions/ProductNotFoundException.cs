namespace VideoManagement.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException()
    {
        TitleMessage = "Product not Found";
    }
}
