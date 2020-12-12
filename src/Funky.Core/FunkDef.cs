using System;
using System.Reflection;

namespace Funky.Core
{ 
    public readonly struct FunkDef : IEquatable<FunkDef>
    {
        public FunkDef(string fullQualifiedName)
        {
            if (string.IsNullOrEmpty(fullQualifiedName))
                throw new ArgumentException("expected not null or empty", nameof(fullQualifiedName));

            var typeSperatorIndex = fullQualifiedName.IndexOf(',');
            this.Type = fullQualifiedName.Substring(0, typeSperatorIndex);

            var assemblyNameStart = typeSperatorIndex + 1;
            var assemblyNameString = fullQualifiedName[assemblyNameStart..];
            this.Assembly = new AssemblyName(assemblyNameString);
        }

        public string Type { get; }

        public AssemblyName Assembly {get;}

        public override bool Equals(object obj)
            => obj is FunkDef definition
            && this.Type == definition.Type
            && this.Assembly == definition.Assembly;

        public override int GetHashCode() => HashCode.Combine(this.Type, this.Assembly);

        public bool Equals(FunkDef other)
            => this.Equals(other);

        public override string ToString() => $"{this.Type}, {this.Assembly.FullName}";
    }
}
