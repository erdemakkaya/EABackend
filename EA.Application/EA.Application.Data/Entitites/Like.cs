using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;

namespace EA.Application.Data.Entitites
{
   public class Like:EntityBase,IFullAudited,IEntity
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? Creator { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
