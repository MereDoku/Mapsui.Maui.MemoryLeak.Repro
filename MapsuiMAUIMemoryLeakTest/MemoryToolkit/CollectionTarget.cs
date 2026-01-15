namespace MapsuiMAUIMemoryLeakTest.MemoryToolkit;

public class CollectionTarget(object reference, string? name = null)
{
    public string Name { get; } = name ?? reference.GetType().Name;
    public WeakReference Reference { get; } = new(reference);
}