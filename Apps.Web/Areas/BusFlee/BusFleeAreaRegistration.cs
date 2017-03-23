using System.Web.Mvc;

namespace Apps.Web.Areas.BusFlee
{
    public class BusFleeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BusFlee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "BusFleeGlobalization", // 路由名称
               "{lang}/BusFlee/{controller}/{action}/{id}", // 带有参数的 URL
               new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
               new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
           );

            context.MapRoute(
                "BusFlee_default",
                "BusFlee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "BusFlee_Main",
                "BusFlee/{controller}/{action}/{id}",
                new { Controller="Home", action = "Main", id = UrlParameter.Optional }
            );
        }
    }
}