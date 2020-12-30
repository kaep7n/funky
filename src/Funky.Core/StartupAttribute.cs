using System;

namespace Funky.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class StartupAttribute : Attribute
    {
        public Type StartupType { get; set; }
    }
}
