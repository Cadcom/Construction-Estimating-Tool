using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Shared.Entities
{
    public class Estimate:BaseEntity
    {        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
