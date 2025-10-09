using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams productSpecificationParams) : base(x =>
    (string.IsNullOrEmpty(productSpecificationParams.Search) || x.Name.ToLower().Contains(productSpecificationParams.Search)) &&
      (productSpecificationParams.Brands.Count == 0 || productSpecificationParams.Brands.Contains(x.Brand)) &&
       (productSpecificationParams.Types.Count == 0 || productSpecificationParams.Types.Contains(x.Type))

    )
    {
        ApplyPagging(productSpecificationParams.PageSize, productSpecificationParams.PageSize * (productSpecificationParams.PageIndex - 1));
        switch (productSpecificationParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceAscDescending":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }


    }
}
