using System;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public record GetOrdersListQuery : IRequest<List<OrderViewModel>>
	{
		public string UserName { get; set; }

		public GetOrdersListQuery(string userName)
		{
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
	}
}

