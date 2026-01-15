namespace MapsuiMAUIMemoryLeakTest.MemoryToolkit;

public static class VisualLeakCheckQueue
{
    private static readonly object _pendingVisualTreeSnapshotsLock = new();
    private static readonly List<VisualTreeSnapshot> _pendingVisualTreeSnapshots = new();

    public static void Enqueue(object view)
    {
        if (view == null)
        {
            return;
        }
        var snapshot = VisualTreeSnapshot.BuildVisualTreeSnapshot(view);
        lock (_pendingVisualTreeSnapshotsLock)
        {
            _pendingVisualTreeSnapshots.Add(snapshot);
        }
    }

    public static void Monitor()
    {
        List<VisualTreeSnapshot> pending;
        lock (_pendingVisualTreeSnapshotsLock)
        {
            pending = new List<VisualTreeSnapshot>(_pendingVisualTreeSnapshots);
            _pendingVisualTreeSnapshots.Clear();
        }

        if (pending.Count == 0)
        {
            return;
        }
        _ = MonitorPendingVisualTreeSnapshots(pending);
    }
    private static async Task MonitorPendingVisualTreeSnapshots(List<VisualTreeSnapshot> pending)
    {
            const int msBetweenGCCollect = 1000;
            const int maxSnapshotCollectionPasses = 3;
            int pass = 0;
            while (pass < maxSnapshotCollectionPasses)
            {
                pass++;
                await Task.Delay(msBetweenGCCollect);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                bool anyRemaining = false;
                foreach (var snapshot in pending.Where(s => !s.IsCollected))
                {
                    RemoveCollectedItems(snapshot.CollectionItems);
                    if (!snapshot.CollectionItems.Any())
                    {
                        snapshot.MarkCollected();
                        System.Diagnostics.Debug.WriteLine($"{snapshot.Root.Name} => ✅ Collected and all his children (Pass : {pass})");
                    }
                    else
                    {
                        anyRemaining = true;
                    }
                }
                if (!anyRemaining)
                {
                    break;
                }
            }
            foreach (var snapshot in pending.Where(s => !s.IsCollected))
            {
                LogVisualTreeStatus(snapshot.Root, 0);
            }
    }
    private static void LogVisualTreeStatus(VisualTreeNode node, int depth)
    {
        var indent = new string(' ', depth * 2);
        var prefix = depth == 0 ? string.Empty : "- ";
        var status = node.Reference.IsAlive ? "💦 Leak" : "✅ Collected";
        System.Diagnostics.Debug.WriteLine($"{indent}{prefix}{node.Name} => {status}");
        foreach (var child in node.Children)
        {
            LogVisualTreeStatus(child, depth + 1);
        }
    }
    private static void RemoveCollectedItems(List<CollectionTarget> collectionItems)
    {
        foreach (CollectionTarget item in collectionItems.ToArray())
        {
            if (!item.Reference.IsAlive)
            {
                collectionItems.Remove(item);
            }
            else
            {
                break;
            }
        }
    }
}
