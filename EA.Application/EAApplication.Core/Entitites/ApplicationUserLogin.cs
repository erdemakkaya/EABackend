using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Enttiy;
using Microsoft.AspNetCore.Identity;

namespace EA.Application.Data.Entitites
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>,IEntity
    {
        public Guid Id { get; set; }
    }
}
