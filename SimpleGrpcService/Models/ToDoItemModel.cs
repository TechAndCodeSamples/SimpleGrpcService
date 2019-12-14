using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGrpcService.Models
{
    public class ToDoItemModel
    {
        public int Id { get; set; }
        public string Beschreibung { get; set; }
        public bool Erledigt { get; set; }
    }
}
