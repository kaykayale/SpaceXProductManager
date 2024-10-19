public class Component
{
    public int Id { get; set; }
    public byte[]? ComponentImage { get; set; } 
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public List<int> DependencyIds { get; set; }  // To store dependencies
}