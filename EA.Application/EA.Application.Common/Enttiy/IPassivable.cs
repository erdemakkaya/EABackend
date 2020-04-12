using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Enttiy
{
    public interface IPassivable
    {
        public bool IsActive { get; set; }
    }
}
