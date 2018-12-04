using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HGGM.Services
{
    public static class CultureService
    {
        public static List<string> GetCountries()
        {
            return CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.Name).EnglishName)
                .Distinct()
                .OrderBy(r => r)
                .ToList();
        }
    }
}