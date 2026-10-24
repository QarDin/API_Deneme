namespace ApiOrnek
{
    public class Baslik
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public List<Not> Notes { get; set; } = new();
    }
}