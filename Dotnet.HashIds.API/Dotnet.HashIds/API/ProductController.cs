using Dotnet.HashIds.Database;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.HashIds.API;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    
    private readonly ILogger<ProductController> _logger;
    private readonly ProductDatebase _productDatabase;
    private readonly IHashids _hashids;

    public ProductController(ILogger<ProductController> logger, ProductDatebase productDatabase, IHashids hashids)
    {
        _logger = logger;
        _productDatabase = productDatabase;
        _hashids = hashids;
    }

    [HttpGet("id")]
    public IEnumerable<ProductResponse> GetId()
    {
        return _productDatabase.Products.Select(x => new ProductResponse(x.Id.ToString(), x.Name));
    }
    
    [HttpGet("hashId")]
    public IEnumerable<ProductResponse> GetHashId()
    {
        return _productDatabase.Products.Select(x => new ProductResponse(_hashids.Encode(x.Id), x.Name));
    }
    
    [HttpGet("uid")]
    public IEnumerable<ProductResponse> GetUid()
    {
        return _productDatabase.Products.Select(x => new ProductResponse(x.Uid.ToString(), x.Name));
    }
    
    
    [HttpGet("id/{id}")]
    public ActionResult<ProductResponse> GetById(int id)
    {
        var product = _productDatabase.Products.SingleOrDefault(x => x.Id == id);

        if (product == null)
            return NotFound();
        
        var (_,_,name) = product;

        return Ok(new ProductResponse(id.ToString(), name));
    }
    
    [HttpGet("hashId/{hashId}")]
    public ActionResult<ProductResponse> GetByHashId(string hashId)
    {
        if(_hashids.TryDecodeSingle(hashId, out int id))
        {
            var product = _productDatabase.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound();
            var (_,_,name) = product;
            
            return Ok(new ProductResponse(hashId,name));
        }

        return NotFound();
    }
    
    [HttpGet("uid/{uid}")]
    public ActionResult<ProductResponse> GetByHashUid(Guid uid)
    {
        var product = _productDatabase.Products.SingleOrDefault(x => x.Uid == uid);

        if (product == null)
            return NotFound();
        
        var (_,_,name) = product;

        return Ok(new ProductResponse(uid.ToString(), name));
    }
}