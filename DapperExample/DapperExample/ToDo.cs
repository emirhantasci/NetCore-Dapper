using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample
{
    public class ToDo
    {
        public int Id { get; set; }
        public string TodoName { get; set; }
        public string ToDoDescription { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public DateTime LastDatetime { get; set; }
    }
}
