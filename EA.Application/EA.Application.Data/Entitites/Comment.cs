using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;

namespace EA.Application.Data.Entitites
{
   public class Comment:EntityBase, IEntity, IFullAudited
    {
        public string Body { get; set; }
        public virtual Product Product { get; set; }
        public  virtual  ApplicationUser Author { get; set;}
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
