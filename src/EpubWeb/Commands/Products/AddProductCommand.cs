using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpubWeb.Models.Product;
using EpubWeb.Entities;

namespace EpubWeb.Commands.Products
{
    public class AddProductCommand
    {
        public class AddProductCommandHandler : IRequestHandler<AddProductRequest, int>
        {
            private readonly AppDbContext _dbContext;

            public AddProductCommandHandler(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<int> Handle(AddProductRequest request, CancellationToken cancellationToken)
            {
                var model = new Product
                {
                    ProductName = request.ProductName,
                    SupplierID = request.SupplierID,
                    CategoryID = request.CategoryID,
                    QuantityPerUnit = request.QuantityPerUnit,
                    UnitPrice = request.UnitPrice,
                    UnitsInStock = request.UnitsInStock,
                    UnitsOnOrder = request.UnitsOnOrder,
                    ReorderLevel = request.ReorderLevel,
                    Discontinued = request.Discontinued
                };

                await _dbContext.AddAsync(model, cancellationToken);
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}