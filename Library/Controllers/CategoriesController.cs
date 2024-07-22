using Microsoft.AspNetCore.Mvc;
using Library.Models.ViewModels;
using System.Collections.Generic;

public class CategoriesController : Controller
{
    public IActionResult Index()
    {
        var model = new Dictionary<string, List<string>>
        {
            { "Coding", new List<string> { "C#", "JavaScript", "Python" } },
            { "Framework", new List<string> { "Asp.net", "Angular" } },
            {"Novel", new List<string> {"Fiction", "NonFiction"} }
        };

        var viewModel = new CategoryViewModel
        {
            MainCategoriesWithSubCategories = model
        };

        return View(viewModel);
    }
}
