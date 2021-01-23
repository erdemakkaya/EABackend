using System;
using System.Collections.Generic;
using System.Text;
using EA.Application.Common.Data;
using EA.Application.Common.Enttiy;

namespace EA.Application.Data.Entitites
{
   public  class Product:EntityBase, IEntity, IFullAudited
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
