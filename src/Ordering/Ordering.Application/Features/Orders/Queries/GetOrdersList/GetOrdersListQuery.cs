using System;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public record GetOrdersListQuery : IRequest<List<OrderViewModel>>
	{
		public required string UserName { get; set; }

		public GetOrdersListQuery(string userName)
		{
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
	}
}

