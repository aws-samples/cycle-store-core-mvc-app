using AdventureWorksMVCCore.Web.Models;
using AdventureWorksMVCCore.Web.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksMVCCore.Web.Views.Components
{
    public class ContentViewComponent : ViewComponent
    {
        private readonly CYCLE_STOREContext _context;
        private readonly ICategoryService _categoryService;
        public ContentViewComponent(CYCLE_STOREContext context,
            ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            ContentLayoutModel content = new ContentLayoutModel();
            content.ProductCategories = _categoryService.GetCategoriesWithSubCategory();
            return View(content);
        }


    }
}
