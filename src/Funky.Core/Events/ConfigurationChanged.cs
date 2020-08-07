using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Funky.Core.Events
{
    public class ConfigurationChanged
    {
        public ConfigurationChanged(DateTimeOffset timestamp, Dictionary<string, string> value)
        {
            this.Timestamp = timestamp;
            this.Value = value;
        }

        public DateTimeOffset Timestamp { get; set; }

        public Dictionary<string, string> Value { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(this.Timestamp.ToString(CultureInfo.InvariantCulture));

            foreach (var c in this.Value)
            {
                builder.Append(c.Key).Append(": ").AppendLine(c.Value);
            }

            return builder.ToString();
        }
    }
}
