using SimpleGrpcService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGrpcService.Repositorys
{
    public class ToDoRepository
    {
        public static List<ToDoItemModel> todos = new List<ToDoItemModel>() {
            new ToDoItemModel { Id = 1, Beschreibung = "Milch kaufen", Erledigt = false },
            new ToDoItemModel { Id = 2, Beschreibung = "Auto Waschen", Erledigt = false },
            new ToDoItemModel { Id = 3, Beschreibung = "Paket zur Post bringen", Erledigt = false }
        };

        public Todos GetToDoItems()
        {
            var result = MapList();
            return result;
        }

        public ToDoItem GetTodoItem(int id)
        {
            return MapToToDoItem(todos.First(x => x.Id == id));
        }

        public Ergebnis DeleteToDoItem(ToDoItem item)
        {
            return MapBoolToErgebnis(todos.Remove(MapToToDoItemModel(item)));
        }

        public Ergebnis CheckTodoItem(int id)
        {
            var item = todos.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return MapBoolToErgebnis(false);
            }
            item.Erledigt = !item.Erledigt;
            return MapBoolToErgebnis(true);
        }

        private Todos MapList()
        {
            var list = new Todos();
            foreach (var item in todos)
            {
                list.Todos_.Add(MapToToDoItem(item));
            }

            return list;
        }

        private Ergebnis MapBoolToErgebnis(bool ergebnis)
        {
            return new Ergebnis { Erfolgreich = ergebnis };
        }

        private ToDoItem MapToToDoItem(ToDoItemModel model)
        {
            return new ToDoItem
            {
                Id = model.Id,
                Beschreibung = model.Beschreibung,
                Erledigt = model.Erledigt
            };
        }

        private ToDoItemModel MapToToDoItemModel(ToDoItem item)
        {
            return new ToDoItemModel
            {
                Id = item.Id,
                Beschreibung = item.Beschreibung,
                Erledigt = item.Erledigt
            };
        }
    }
}
