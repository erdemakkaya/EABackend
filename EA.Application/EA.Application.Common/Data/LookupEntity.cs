using EA.Application.Common.Enttiy;
using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Data
{
    public class LookupEntity<TString> : IEntity
    {
        public int Id { get; set; }
        public TString Name { get; set; }
    }
}
