using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);

    Task<Product?> GetProductByIdAsync(int id);

    void AddProduct(Product product);

    void DeleteProduct(Product product);

    void UpdateProduct(Product product);

    Task<IReadOnlyList<string>> GetBrandAsync();

    Task<IReadOnlyList<string>> GetBTypesAsync();


    bool ProductExist(int id);

    Task<bool> SaveChangesAsync();


}
