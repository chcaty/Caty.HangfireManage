using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caty.ContextMaster.Models
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime UpdateTime { get; set; }
    }
}
