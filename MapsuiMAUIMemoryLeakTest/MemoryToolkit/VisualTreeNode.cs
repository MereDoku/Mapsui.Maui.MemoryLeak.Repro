namespace MapsuiMAUIMemoryLeakTest.MemoryToolkit;

public class VisualTreeNode
{
    public VisualTreeNode(string name, WeakReference reference, List<VisualTreeNode> children)
    {
        Name = name;
        Reference = reference;
        Children = children;
    }

    public string Name { get; }
    public WeakReference Reference { get; }
    public List<VisualTreeNode> Children { get; }
}
