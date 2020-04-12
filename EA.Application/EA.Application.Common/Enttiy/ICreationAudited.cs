using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Enttiy
{
    public interface ICreationAudited : IHasCreationTime
    {
        Guid? Creator { get; set; }
    }
}