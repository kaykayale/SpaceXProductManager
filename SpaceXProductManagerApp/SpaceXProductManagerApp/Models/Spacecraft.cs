public class Spacecraft
{
    public int Id { get; set; }
    public byte[]? SpaceCraftImage { get; set; } 
    public string Name { get; set; }
    public List<Component> Components { get; set; }
    public decimal TotalCost => Components.Sum(c => c.Cost);
}