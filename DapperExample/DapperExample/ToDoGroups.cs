using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample
{
    public class ToDoGroups
    {
        public int Id { get; set; }
        public string ToDoGroupName { get; set; }
        public string ToDoGroupDescription { get; set; }
        public int UserId { get; set; }
    }
}
