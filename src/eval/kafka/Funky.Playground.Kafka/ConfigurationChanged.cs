using System;

namespace Funky.Playground.Kafka
{
    public class ConfigurationChanged
    {
        public const string TOPIC = "configuration.changed";

        public ConfigurationChanged(string configuration)
        {
            this.Configuration = configuration;
            this.ChangedAtUtc = DateTimeOffset.UtcNow;
        }

        public string Configuration { get; set; }

        public DateTimeOffset ChangedAtUtc { get; set; }
    }
}
