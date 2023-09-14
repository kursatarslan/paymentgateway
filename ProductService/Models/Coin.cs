namespace ProductService.Models
{
    public class Coin : ICoin
    {
        public Coin(string id, string name, string nameKey, string value )
        {
            Id = id;
            Name = name;
            NameKey = nameKey;
            Value = value;
        }
        public string Id  { get; set; }
        public string Name { get; set; }
        public string NameKey { get; set; }
        public string Value { get; set; }
    }
}