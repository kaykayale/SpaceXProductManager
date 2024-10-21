public class SpacecraftService
{
    private readonly AppDbContext _context;

    public SpacecraftService(AppDbContext context)
    {
        _context = context;
    }

    // Topological Sort method to resolve component dependencies
    public List<Component> TopologicalSort(Spacecraft spacecraft)
    {
        // Build the graph from components and dependencies
        Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
        Dictionary<int, int> indegree = new Dictionary<int, int>();

        foreach (var component in spacecraft.Components)
        {
            graph[component.Id] = new List<int>();

            foreach (var depId in component.DependentOn.Select(d => d.DependencyId))
            {
                graph[depId].Add(component.Id);

                if (!indegree.ContainsKey(component.Id))
                    indegree[component.Id] = 0;

                indegree[component.Id]++;
            }
        }

        // Start with components with zero indegree (no dependencies)
        Queue<int> queue = new Queue<int>();
        foreach (var component in spacecraft.Components)
        {
            if (!indegree.ContainsKey(component.Id))
            {
                queue.Enqueue(component.Id);
            }
        }

        List<int> sortedOrder = new List<int>();
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sortedOrder.Add(current);

            foreach (var neighbor in graph[current])
            {
                indegree[neighbor]--;
                if (indegree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Return sorted components in the correct assembly order
        return spacecraft.Components.Where(c => sortedOrder.Contains(c.Id))
                                    .OrderBy(c => sortedOrder.IndexOf(c.Id))
                                    .ToList();
    }
}
