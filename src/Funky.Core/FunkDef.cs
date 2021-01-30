using System;
using System.Collections.Generic;

namespace Funky.Core
{
    public class FunkDef
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
            => obj is FunkDef def && EqualityComparer<TypeName>.Default.Equals(this.TypeName, def.TypeName);

        public override int GetHashCode()
            => HashCode.Combine(this.TypeName);
    }
}
