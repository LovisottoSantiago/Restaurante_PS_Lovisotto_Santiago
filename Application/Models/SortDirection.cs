using System.Text.Json.Serialization;

namespace Application.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        asc,
        desc
    }
}
