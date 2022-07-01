using System.Web;
using System.Web.Mvc;

namespace WebGymTrivelloniBattaglioli
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
