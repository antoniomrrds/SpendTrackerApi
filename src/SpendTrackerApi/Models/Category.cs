namespace SpendTrackerApi.Models;

public sealed class Category
{
    public int Id { get; set; }
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set => _name = value.Trim() ;
    }

    private string _description   = string.Empty;

    public string Description
    {
        get => _description;
        set => _description = value.Trim() ;
    }
}
