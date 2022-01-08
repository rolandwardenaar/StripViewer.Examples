namespace Voorbeeld.WebApplication.Models
{
    public class CarViewModel
    {
        public int Status { get; set; }
        public string Plate { get; set; }
        //
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Model Model { get; set; }
        public Type Type { get; set; }

        public CarViewModel()
        {
            Category = new Category();
            Brand = new Brand();
            Model = new Model();
            Type = new Type();
        }
    }
}