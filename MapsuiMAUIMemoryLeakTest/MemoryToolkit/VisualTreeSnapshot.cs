namespace MapsuiMAUIMemoryLeakTest.MemoryToolkit;

public class VisualTreeSnapshot
{
    public VisualTreeSnapshot(VisualTreeNode root)
    {
        Root = root;
        CollectionItems = ToCollectionTargets();
    }

    public VisualTreeNode Root { get; }
    public List<CollectionTarget> CollectionItems { get; }
    public bool IsCollected { get; private set; }

    public static VisualTreeSnapshot BuildVisualTreeSnapshot(object visualTreeElement)
    {
        VisualTreeNode root = BuildNode(visualTreeElement, true);
        if (root == null)
        {
            root = new VisualTreeNode(visualTreeElement.GetType().Name, new WeakReference(visualTreeElement), new List<VisualTreeNode>());
        }

        return new VisualTreeSnapshot(root);

        VisualTreeNode BuildNode(object monitorTarget, bool isRoot)
        {
            if (monitorTarget is IVisualTreeElement vte)
            {
                var children = new List<VisualTreeNode>();
                foreach (IVisualTreeElement childElement in vte.GetVisualChildren())
                {
                    var childNode = BuildNode(childElement, false);
                    if (childNode != null)
                    {
                        children.Add(childNode);
                    }
                }

                return new VisualTreeNode(vte.GetType().Name, new WeakReference(vte), children);
            }

            return new VisualTreeNode(monitorTarget.GetType().Name, new WeakReference(monitorTarget), new List<VisualTreeNode>());
        }
    }
    private List<CollectionTarget> ToCollectionTargets()
    {
        var result = new List<CollectionTarget>();
        var stack = new Stack<VisualTreeNode>();
        stack.Push(Root);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node.Reference?.Target is object target)
            {
                result.Add(new CollectionTarget(target, node.Name));
            }

            for (int i = node.Children.Count - 1; i >= 0; i--)
            {
                stack.Push(node.Children[i]);
            }
        }

        return result;
    }

    public void MarkCollected()
    {
        IsCollected = true;
    }
}


