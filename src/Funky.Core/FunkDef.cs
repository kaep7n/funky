using System;
using System.Collections.Generic;

namespace Funky.Core
{
    public class FunkDef : IEquatable<FunkDef>
    {
        public FunkDef(string fullQualifiedName, string topic)
        {
            if (fullQualifiedName is null)
                throw new ArgumentNullException(nameof(fullQualifiedName));

            this.TypeName = new TypeName(fullQualifiedName);
            this.Topic = topic ?? throw new ArgumentNullException(nameof(topic));
        }

        public TypeName TypeName { get; }

        public string Topic { get; }

        public override bool Equals(object obj)
            => this.Equals(obj as FunkDef);

        public bool Equals(FunkDef other)
            => other != null
            && EqualityComparer<TypeName>.Default.Equals(this.TypeName, other.TypeName)
            && EqualityComparer<IEnumerable<string>>.Default.Equals(this.Topics, other.Topics);

        public override int GetHashCode()
            => HashCode.Combine(this.TypeName, this.Topics);
    }
}
