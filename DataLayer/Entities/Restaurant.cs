namespace DataLayer.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Loacation { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
