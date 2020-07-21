using AdventureWorksMVCCore.Web.Models;
using System.Collections.Generic;

namespace AdventureWorksMVCCore.Web.Service.Interface
{
    public interface ICategoryService
    {
        List<ProductCategory> GetCategoriesWithSubCategory();
    }
}
