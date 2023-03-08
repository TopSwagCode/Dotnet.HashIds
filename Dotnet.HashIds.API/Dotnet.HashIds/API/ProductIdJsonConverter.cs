using System.Text.Json;
using System.Text.Json.Serialization;
using HashidsNet;

namespace Dotnet.HashIds.API;

public class ProductIdJsonConverter : JsonConverter<ProductId>
{
    // Needed if the Id is in Json.
    public override ProductId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var id = (new Hashids("Our super secret salt!", 5)).DecodeSingle(reader.GetString()!); // Will throw exception if not valid. Could use TryDecodeSingle instead and return null or -1;
        return new ProductId(id);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ProductId productId,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue((new Hashids("Our super secret salt!", 5).Encode(productId.Value)));
    }
}