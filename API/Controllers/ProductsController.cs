using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {

        return Ok(await productRepository.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
    {
        return Ok(await productRepository.GetBrandAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
    {
        return Ok(await productRepository.GetBTypesAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {

        productRepository.AddProduct(product);
        if (await productRepository.SaveChangesAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Failed to create product");

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {

        if (product.Id != id || !ProductExist(id))
        {
            return BadRequest("Product dont exist");
        }
        productRepository.UpdateProduct(product);
        if (!await productRepository.SaveChangesAsync())
        {
            return BadRequest("Failed to update product");
        }
        return NoContent();



    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return BadRequest("Product dont exist");
        }

        productRepository.DeleteProduct(product);

        if (!await productRepository.SaveChangesAsync())
        {
            return BadRequest("Failed to delete product");
        }

        return NoContent();

    }

    private bool ProductExist(int id)
    {
        return productRepository.ProductExist(id);
    }


}
