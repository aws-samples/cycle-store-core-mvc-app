using AdventureWorksMVCCore.Web.Models;
using AdventureWorksMVCCore.Web.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksMVCCore.Web.Service.Implementation
{
  
    public class CategoryService : ICategoryService
    {
        private readonly CYCLE_STOREContext _context;
        public CategoryService(CYCLE_STOREContext context)
        {
            _context = context;
        }
        public List<ProductCategory> GetCategoriesWithSubCategory()
        {
            return _context.ProductCategory
                    .Include(category => category.ProductSubcategory)
                    .ToList();


        }
    }
}
