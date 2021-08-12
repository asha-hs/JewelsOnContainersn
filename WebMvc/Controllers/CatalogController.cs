using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {

        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(int? page,int? brandFilterApplied,int? typesFilterApplied)
        {
            var itemsOnPage = 10;

            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brandFilterApplied, typesFilterApplied);

            var vm = new CatalogIndexViewModel
            {
                Brands = await _service.GetBrandsAsync(),
                Types = await _service.GetTypesAsync(),
                CatalogItems = catalog.Data,
                PaginationInfo = new PaginationInfo
                {
                    TotalItems = catalog.Count,
                    ItemsPerPage = catalog.PageSize,
                    ActualPage = page??0,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count/itemsOnPage),

                },
                BrandFilterApplied = brandFilterApplied??0,
                TypesFilterApplied = typesFilterApplied ?? 0
            };

            return View(vm);
        }
    }
}
