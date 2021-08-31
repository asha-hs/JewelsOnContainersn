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
                if(brand.HasValue && type.HasValue)
                {
                   // var brandQs = brand.HasValue ? brand.Value.ToString() : "null";
                   // var typeQs = type.HasValue ? type.Value.ToString() : "null";

                    return $"{baseuri}items/filtered?catalogTypeId={type}&catalogBrandId={brand}&pageIndex={page}&pageSize={take}";
                }else if (brand.HasValue)
                {
                    return $"{baseuri}items/filtered?catalogBrandId={brand}&pageIndex={page}&pageSize={take}";
                }
                else if (type.HasValue)
                {
                    return $"{baseuri}items/filtered?catalogTypeId={type}&pageIndex={page}&pageSize={take}";
                }
                return $"{baseuri}items?pageIndex={page}&pageSize={take}";

            }
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }

            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }
    }

   
}
