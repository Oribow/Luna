using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Luna.Extensions
{
    public static class INavigationExtensions
    {
        public static async Task SwapPage(this INavigation nav, Page newPage)
        {
            await nav.PushAsync(newPage);
            nav.RemovePage(nav.NavigationStack[nav.NavigationStack.Count - 2]);
        }

        public static async Task ClearAndSetPage(this INavigation nav, Page newPage)
        {
            await nav.PushAsync(newPage);
            while (nav.NavigationStack.Count > 1)
            {
                nav.RemovePage(nav.NavigationStack[0]);
            }
        }
    }
}
