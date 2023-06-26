using System;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
	public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderViewModel>>
	{
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository repository, IMapper mapper)
		{
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrderViewModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrdersByUserName(request.UserName, cancellationToken);

            return _mapper.Map<List<OrderViewModel>>(orders);
        }
    }
}

