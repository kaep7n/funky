using System;
using System.Collections.Generic;

namespace Funky.Core
{
    public class FunkDef
    {
        public FunkDef(string fullQualifiedName)
            => this.TypeName = new TypeName(fullQualifiedName);

        public TypeName TypeName { get; }

        public override bool Equals(object obj)
            => obj is FunkDef def && EqualityComparer<TypeName>.Default.Equals(this.TypeName, def.TypeName);

        public override int GetHashCode()
            => HashCode.Combine(this.TypeName);
    }
}
