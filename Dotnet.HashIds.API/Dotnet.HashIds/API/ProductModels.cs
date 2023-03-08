using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.HashIds.API;

public record ProductResponse(string Id, string Name);
public record ProductStronglyTypedIdResponse(ProductId Id, string Name);

[JsonConverter(typeof(ProductIdJsonConverter))]
[TypeConverter(typeof(ProductIdConverter))]
[ModelBinder(BinderType = typeof(ProductIdBinder))]
public record ProductId(int Value); // Name value or Id?