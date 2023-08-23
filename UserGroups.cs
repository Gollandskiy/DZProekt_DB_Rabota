using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZProekt
{
   public class UserGroups
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null;
        public String Pass { get; set; } = null;
        public String Picture { get; set; } = null;
    }
}
