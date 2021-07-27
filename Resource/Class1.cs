using System;
using System.Text.Json.Serialization;

namespace Resource
{
    public class CheckDto
    {
        [JsonPropertyName("source")]
        public SourceDto Source { get; set; }

        [JsonPropertyName("version")]
        public VersionDto Version { get; set; }
    }

    public class SourceDto
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; }

        [JsonPropertyName("package_id")]
        public string PackageId { get; set; }
    }

    public class VersionDto
    {
        [JsonPropertyName("package_id")]
        public string PackageId { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
