namespace Webshop_Frontend.Models;

using System.Text.Json.Serialization;

public class Category
{
    [JsonPropertyName("id")] // Mapira "id" iz JSON-a na ovo svojstvo
    public int Id { get; set; }

    [JsonPropertyName("name")] // Mapira "name" iz JSON-a na ovo svojstvo
    public string Name { get; set; }
}
