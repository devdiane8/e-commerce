using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class ProductsController(IGenericRepository<Product> genericRepository) : ControllerBase
{


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {

        var specification = new ProductSpecification(brand, type, sort);
        var products = await genericRepository.ListAsync(specification);
        return Ok(products);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
    {
        var specification = new BrandListSpecification();
        return Ok(await genericRepository.ListAsync(specification));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
    {
        var specification = new TypeListSpecification();
        return Ok(await genericRepository.ListAsync(specification));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {

        var product = await genericRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {

        genericRepository.Add(product);
        if (await genericRepository.saveAllAsync())
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
        genericRepository.Update(product);
        if (!await genericRepository.saveAllAsync())
        {
            return BadRequest("Failed to update product");
        }
        return NoContent();



    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await genericRepository.GetByIdAsync(id);
        if (product == null)
        {
            return BadRequest("Product dont exist");
        }

        genericRepository.Remove(product);

        if (!await genericRepository.saveAllAsync())
        {
            return BadRequest("Failed to delete product");
        }

        return NoContent();

    }

    private bool ProductExist(int id)
    {
        return genericRepository.Exist(id);
    }


}
