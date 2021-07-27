using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common
{
    public class CheckInputDto
    {
        [JsonPropertyName("source")]
        public SourceDto Source { get; set; }

        [JsonPropertyName("version")]
        public VersionDto Version { get; set; }
    }

    public class InInputDto
    {
        [JsonPropertyName("source")]
        public SourceDto Source { get; set; }

        [JsonPropertyName("version")]
        public VersionDto Version { get; set; }

        [JsonPropertyName("params")]
        public object Params { get; set; }
    }

    public class InOutputDto
    {
        [JsonPropertyName("version")]
        public VersionDto Version { get; set; }

        [JsonPropertyName("metadata")]
        public Meta[] Metadata { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
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
