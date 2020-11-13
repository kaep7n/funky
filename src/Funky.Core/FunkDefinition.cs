using System;
using System.Reflection;

namespace Funky.Core
{ 
    public readonly struct FunkDefinition : IEquatable<FunkDefinition>
    {
        public FunkDefinition(string fullQualifiedName)
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
            => obj is FunkDefinition definition
            && this.Type == definition.Type
            && this.Assembly == definition.Assembly;

        public override int GetHashCode() => HashCode.Combine(this.Type, this.Assembly);

        public bool Equals(FunkDefinition other)
            => this.Equals(other);

        public override string ToString() => $"{this.Type}, {this.Assembly.FullName}";

        public static implicit operator string(FunkDefinition definition) => definition.ToString();

        public static implicit operator FunkDefinition(string fullQualifiedName) => new(fullQualifiedName);

        public static bool operator ==(FunkDefinition left, FunkDefinition right) => left.Equals(right);

        public static bool operator !=(FunkDefinition left, FunkDefinition right) => !(left == right);
    }
}
