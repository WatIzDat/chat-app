using System.Reflection;

namespace Domain;

public static class DomainAssemblyReference
{
    public static readonly Assembly Assembly = typeof(DomainAssemblyReference).Assembly;
}
