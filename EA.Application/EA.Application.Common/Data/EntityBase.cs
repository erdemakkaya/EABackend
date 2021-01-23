using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Enums;

namespace EA.Application.Common.Data
{
    public class EntityBase
    {
       

        private AppStatus status;
        private DateTime createdDate;

        public Guid Id { get; set; }

        public DateTime? CreateDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator { get; set; }

        public AppStatus? Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value ?? AppStatus.Aktif;
            }
        }
    }
}
