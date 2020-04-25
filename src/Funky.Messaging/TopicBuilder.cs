using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Messaging
{
    public class TopicBuilder
    {
        private static char[] validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~:/?#[]@!$&'()*+,;=".ToCharArray();

        private readonly List<string> fragments = new List<string>();

        public TopicBuilder(string root)
        {
            ValidateAndThrow(root);

            this.fragments.Add(root);
        }

        public TopicBuilder With(string fragment)
        {
            ValidateAndThrow(fragment);

            this.fragments.Add(fragment);

            return this;
        }

        public Topic Build()
        {
            var path = string.Join("/", this.fragments)
                .Insert(0, "/");

            return new Topic(path);
        }

        private static void ValidateAndThrow(string fragment)
        {
            if (string.IsNullOrEmpty(fragment))
            {
                throw new ArgumentException($"Argument '{fragment}' must be not null or empty");
            }
            if (ContainsSlash(fragment))
            {
                throw new ArgumentException($"Argument '{fragment}' contains a slash");
            }
            if (ContainsInvalidChar(fragment))
            {
                throw new ArgumentException($"Fragment '{fragment}' contains invalid characters", nameof(fragment));
            }
        }

        private static bool ContainsSlash(string fragment)
        {
            if (fragment is null)
            {
                throw new ArgumentNullException(nameof(fragment));
            }

            return fragment.Contains("/", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsInvalidChar(string fragment)
        {
            if (fragment is null)
            {
                throw new ArgumentNullException(nameof(fragment));
            }

            return !fragment.All(c => validChars.Contains(c));
        }
    }
}
