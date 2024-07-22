namespace Library.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Dictionary<string, List<string>> MainCategoriesWithSubCategories { get; set; }
        public string SelectedMainCategory { get; set; }
    }
}
