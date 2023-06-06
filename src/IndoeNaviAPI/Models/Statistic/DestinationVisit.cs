﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IndoeNaviAPI.Models.Statistic;

public class DestinationVisit : IHasIdProp
{
    [BsonId]
    public Guid Id { get; set; }
	public string Destination { get; set; }
	public int Count { get; set; }
}
