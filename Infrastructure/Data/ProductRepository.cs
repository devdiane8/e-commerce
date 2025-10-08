using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    private readonly StoreContext _context = context;

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);

    }

    public void DeleteProduct(Product product)
    {
        _context.Products.Remove(product);

    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(brand))
        {
            query = query.Where(p => p.Brand == brand);
        }
        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(p => p.Type == type);
        }
        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Name)
            };
        }
        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {

        return await _context.Products.FindAsync(id);
    }

    public bool ProductExist(int id)
    {

        return _context.Products.Any(x => x.Id == id);

    }

    public void UpdateProduct(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;

    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<string>> GetBrandAsync()
    {
        return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetBTypesAsync()
    {
        return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }

}
