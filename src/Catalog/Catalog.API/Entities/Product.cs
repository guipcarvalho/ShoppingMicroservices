using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

public record Product
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public required string Id { get; init; }

	public required string Name { get; init; }

	public required string Category { get; set; }

	public required string Summary { get; set; }

	public required string Description { get; set; }

	public required string ImageFile { get; set; }

	public decimal Price { get; set; }
}

