using System;
using System.Collections.Generic;

namespace Funky.Core
{
    public class FunkDef
    {
        public FunkDef(string fullQualifiedName, IEnumerable<string> topics)
        {
            this.TypeName = new TypeName(fullQualifiedName);
            this.Topics = topics;
        }

        public TypeName TypeName { get; }

        public TypeName TypeName { get; }
        public IEnumerable<string> Topics { get; }

        public override bool Equals(object obj)
            => obj is FunkDef def && EqualityComparer<TypeName>.Default.Equals(this.TypeName, def.TypeName);

        public override int GetHashCode()
            => HashCode.Combine(this.TypeName);
    }
}
