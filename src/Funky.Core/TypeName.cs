using System;
using System.Collections.Generic;
using System.Reflection;

namespace Funky.Core
{
    public sealed class TypeName : IEquatable<TypeName>
    {
        public TypeName(string fullQualifiedTypeName)
        {
            if (string.IsNullOrEmpty(fullQualifiedTypeName))
                throw new ArgumentException($"'{nameof(fullQualifiedTypeName)}' cannot be null or empty", nameof(fullQualifiedTypeName));

            this.FullQualifiedName = fullQualifiedTypeName;
            var typeSperatorIndex = fullQualifiedTypeName.IndexOf(',');

            this.FullName = fullQualifiedTypeName.Substring(0, typeSperatorIndex);
            this.Name = this.FullName.Split('.')[^1];

            var assemblyNameStart = typeSperatorIndex + 1;
            var assemblyNameString = fullQualifiedTypeName[assemblyNameStart..];

            this.Assembly = new AssemblyName(assemblyNameString);
        }

        public string Name { get; }

        public string FullName { get; }

        public string FullQualifiedName { get; }

        public AssemblyName Assembly { get; }

        public override bool Equals(object obj)
            => this.Equals(obj as TypeName);

        public bool Equals(TypeName other)
            => other != null
            && this.Name == other.Name
            && this.FullName == other.FullName
            && this.FullQualifiedName == other.FullQualifiedName
            && this.Assembly.ToString() == other.Assembly.ToString();

        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.FullName, this.FullQualifiedName, this.Assembly);
    }
}