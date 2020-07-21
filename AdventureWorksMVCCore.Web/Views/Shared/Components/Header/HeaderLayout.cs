using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksMVCCore.Web.Views.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}