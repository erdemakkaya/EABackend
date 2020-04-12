using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Enttiy
{
    public interface IDeletionAudited : ISoftDelete
    {
        long? DeleterUserId { get; set; }

        DateTime? DeletionTime { get; set; }
    }
}
