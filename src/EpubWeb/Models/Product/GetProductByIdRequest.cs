using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;


namespace EpubWeb.Models.Product
{
    public class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
        public int Id { get; set; }
    }
}
