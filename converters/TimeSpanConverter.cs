using System.Text.Json;
using System.Text.Json.Serialization;
namespace TemplumStudii.converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            int totalHours = (int)value.TotalHours;
            writer.WriteStringValue($"{totalHours:D2}:{value.Minutes:D2}:{value.Seconds:D2}");
        }
    }
}
