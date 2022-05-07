using System.Reflection;

namespace WawAPI;

public abstract class Enumeration
{
    protected Enumeration(int id, string name)
    {
        (Id, Name) = (id, name);
    }

    public int Id { get; }
    public string Name { get; }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    public static T? Get<T>(string name) where T : Enumeration
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>()
            .FirstOrDefault(e => e.Name.Equals(name));
    }
}