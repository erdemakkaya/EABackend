using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EA.Application.WebApi.VM
{
    public class UserRoleModel
    {
        public Guid Role { get; set; }
        public Guid User { get; set; }
    }
}
