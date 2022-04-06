namespace Dotnet.HashIds.Database;

public class ProductDatebase
{
    public List<ProductDbEntity> Products = new();

    public ProductDatebase()
    {
        Products = Enumerable.Range(1, 5).Select(index => 
                new ProductDbEntity(index, Guid.NewGuid(),$"Product: {index}"))
            .ToList();
    }
    
}