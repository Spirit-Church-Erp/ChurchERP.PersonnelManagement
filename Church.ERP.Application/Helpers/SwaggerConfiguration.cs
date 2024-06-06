using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Application.Helpers
{
    public static class SwaggerConfiguration
    {
        public static string CustomSchema(this Type type)
        {

            if (type.FullName.Contains("CreateApplicationUser+Request"))
            {
                return "CreateApplicationUserRequest";
            }
            if (type.FullName.Contains("CreateUserAsAdmin+Request"))
            {
                return "CreateUserAsAdminRequest";
            }
            return type.FullName; // default behavior
        }
    }
}
