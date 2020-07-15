namespace Miniblog.Core.Services
{
    /*
     *  Theming implementation taken from Hisham's Blog
     *  http://www.hishambinateya.com/theming-in-asp.net-core
     *  https://github.com/hishamco/Theming/blob/master/Theming/Views/Themes/DarkTheme/Home/Index.cshtml
     *  
     */


    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;

    public class ThemeViewLocationExpander : IViewLocationExpander
    {
        private const string ValueKey = "Theme";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            context.Values.TryGetValue(ValueKey, out string theme);
            theme = string.IsNullOrEmpty(theme) ? "Default" : theme;

            if (!string.IsNullOrEmpty(theme))
            {
                return this.ExpandViewLocationsCore(viewLocations, theme);
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var appSettings = context.ActionContext.HttpContext.RequestServices
                .GetService(typeof(IOptions<BlogSettings>)) as IOptions<BlogSettings>;

            context.Values[ValueKey] = appSettings.Value.Theme;
        }

        private IEnumerable<string> ExpandViewLocationsCore(IEnumerable<string> viewLocations, string theme)
        {
            var newLocations = new List<string>();
            foreach (var location in viewLocations)
            {
                var l = location.Insert(7, $"Themes/{theme}/");
                newLocations.Add(l);

                /* the yield return did not work to update the values. So I created a new modified collection and returned it. */

                //yield return location;
                //yield return location.Insert(7, $"Themes/{theme}/");
            }
            return newLocations as IEnumerable<string>;
        }
    }
}
