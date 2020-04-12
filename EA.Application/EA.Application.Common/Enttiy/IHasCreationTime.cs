using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Enttiy
{
   public interface IHasCreationTime
    {
        public DateTime? CreateDate { get; set; }
    }
}
