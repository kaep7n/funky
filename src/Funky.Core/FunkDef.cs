using System;
using System.Collections.Generic;

namespace Funky.Core
{
    public class FunkDef : IEquatable<FunkDef>
    {
        public FunkDef(string fullQualifiedName, IEnumerable<string> topics)
        {
            this.TypeName = new TypeName(fullQualifiedName);
            this.Topics = topics;
        }

        public TypeName TypeName { get; }

        public IEnumerable<string> Topics { get; }

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
