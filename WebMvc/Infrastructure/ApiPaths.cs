using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public static class ApiPaths
    {
        public static class Catalog
        {
            public static string GetAllTypes(string baseuri)
            {
                return $"{baseuri}catalogtypes";
            }
            public static string GetAllBrands(string baseuri)
            {
                return $"{baseuri}catalogbrands";
            }
            public static string GetAllCatalogItems(string baseuri,int page, int take,int? brand,int? type)
            {
                var filterQs = string.Empty;
                if(brand.HasValue || type.HasValue)
                {
                    var brandQs = brand.HasValue ? brand.Value.ToString() : "null";
                    var typeQs = type.HasValue ? type.Value.ToString() : "null";

                    filterQs = $"/type/{typeQs}/brand/{brandQs}";
                }
                return $"{baseuri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }
        }
    }
}
