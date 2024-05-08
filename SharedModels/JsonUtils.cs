using System.Text.Json.Serialization;
using System.Text.Json;

namespace SharedModels {
    public class JsonUtils {

        public static JsonSerializerOptions JsonOptions() 
            => new JsonSerializerOptions {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip // can skip unassigned properties in .net8 w/o issues
            };
        
        public static StringContent GenerateStringContent(string serializedObj)
            => new StringContent(serializedObj, System.Text.Encoding.UTF8, "application/json");
        public static string SerializeObject(object obj) => JsonSerializer.Serialize(obj, JsonOptions());
        public static T DeserializeJsonString<T>(string json) => JsonSerializer.Deserialize<T>(json, JsonOptions())!;
        //public static IEnumerable<T> DeserializeJsonStringList<T>(string json)
        //    => JsonSerializer.Deserialize<IEnumerable<T>>(json, JsonOptions())!;


    }
}
