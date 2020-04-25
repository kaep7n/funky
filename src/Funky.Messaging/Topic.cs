using System;

namespace Funky.Messaging
{
    public struct Topic : IEquatable<Topic>, ICloneable
    {
        internal Topic(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            this.Path = path;
        }

        public string Path { get; }

        public object Clone() => new Topic((string)this.Path.Clone());

        public override string ToString() => this.Path;

        public override bool Equals(object obj) => obj is Topic topic && this.Equals(topic);
        
        public bool Equals(Topic other) => this.Path == other.Path;
        
        public override int GetHashCode() => HashCode.Combine(this.Path);

        public static bool operator ==(Topic left, Topic right) => left.Equals(right);
        
        public static bool operator !=(Topic left, Topic right) => !(left == right);
    }
}
