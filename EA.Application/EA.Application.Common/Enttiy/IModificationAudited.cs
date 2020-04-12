using System;

namespace EA.Application.Common.Enttiy
{
    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifierUserId { get; set; }
    }
}