using System;
using System.Text;
using EA.Application.Common.Enttiy;
using Microsoft.AspNetCore.Identity;

namespace EA.Application.Data.Entitites
{
    public class ApplicationUserToken : IdentityUserToken<Guid>, IEntity
    {
        public Guid Id { get; set; }
    }
}
