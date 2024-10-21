using System.ComponentModel.DataAnnotations.Schema;

public class Component
{
    public int Id { get; set; }
    public byte[]? ComponentImage { get; set; } 
    public string Name { get; set; }
    public decimal Cost { get; set; }

    // Self-referencing many-to-many relationship for dependencies
    public List<ComponentDependency> ComponentDependencies { get; set; } = new List<ComponentDependency>();

    public List<ComponentDependency> DependentOn { get; set; } = new List<ComponentDependency>();
}

public class ComponentDependency
{
    public int ComponentId { get; set; }
    public Component Component { get; set; }

    public int DependencyId { get; set; }
    public Component Dependency { get; set; }
}