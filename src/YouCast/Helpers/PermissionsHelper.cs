using System.Security.Principal;
using System.Windows;

namespace YouCast.Helpers
{
    public class PermissionsHelper
    {
        public static bool IsRunAsAdministrator()
        {
            var windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static Visibility NoAdministratorPermissions => IsRunAsAdministrator() ? Visibility.Collapsed : Visibility.Visible;
    }
}