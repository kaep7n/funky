using System;
using System.Reflection;

public sealed class TypeName : IEquatable<TypeName>
{
    public TypeName(string fullQualifiedTypeName)
    {
        if (string.IsNullOrEmpty(fullQualifiedTypeName))
            throw new ArgumentException($"'{nameof(fullQualifiedTypeName)}' cannot be null or empty", nameof(fullQualifiedTypeName));

        this.FullQualifiedName = fullQualifiedTypeName;    
        var typeSperatorIndex = fullQualifiedTypeName.IndexOf(',');
    
        this.Name = fullQualifiedTypeName.Split('.')[^1];
        this.FullName = fullQualifiedTypeName.Substring(0, typeSperatorIndex);

        var assemblyNameStart = typeSperatorIndex + 1;
        var assemblyNameString = fullQualifiedTypeName[assemblyNameStart..];

        this.Assembly = new AssemblyName(assemblyNameString);
    }

    public string Name{ get; }

    public string FullName { get; }

    public string FullQualifiedName { get; }

    public AssemblyName Assembly { get; }

         public override bool Equals(object obj)
            => obj is TypeName other
            && this.FullName == other.FullName
            && this.FullQualifiedName == other.FullQualifiedName
            && this.Assembly == other.Assembly;

        public override int GetHashCode() => HashCode.Combine(this.FullName, this.FullQualifiedName, this.Assembly);

        public bool Equals(TypeName other)
            => this.Equals(other);

        public override string ToString() => this.FullQualifiedName;
}