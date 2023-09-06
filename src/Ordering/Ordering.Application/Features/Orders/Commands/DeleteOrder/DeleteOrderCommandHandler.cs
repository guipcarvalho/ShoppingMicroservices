﻿using System;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
	{
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}

