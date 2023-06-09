﻿using System;
using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
	public record DeleteOrderCommand :IRequest
	{
		public int Id { get; set; }
	}
}

