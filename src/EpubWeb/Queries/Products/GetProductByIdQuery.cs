using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpubWeb.Models.Product;
using EpubWeb.Entities;
using Microsoft.EntityFrameworkCore;

namespace EpubWeb.Queries.Products
{
    public class GetProductByIdQuery {
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
        {
            private readonly AppDbContext _db;

            public GetProductByIdQueryHandler(AppDbContext db)
            {
                _db = db;
            }

            public async Task<GetProductByIdResponse> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
            {
                var data = await _db.Products.SingleOrDefaultAsync(x => x.ProductID == request.Id, cancellationToken);

                if (data is null)
                {
                    return null;
                }

                return new GetProductByIdResponse
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName,
                    QuantityPerUnit = data.QuantityPerUnit,
                    UnitPrice = data.UnitPrice,
                    UnitsInStock = data.UnitsInStock,
                    UnitsOnOrder = data.UnitsOnOrder,
                    ReorderLevel = data.ReorderLevel,
                    Discontinued = data.Discontinued
                };
            }
        }
    }
}