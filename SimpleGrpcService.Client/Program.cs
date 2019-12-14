using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace SimpleGrpcService.Client
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var todoVerwaltung = new ToDoVerwaltung.ToDoVerwaltungClient(channel);

            Console.WriteLine("Deine Aufgaben für heute: ");

            //PrintToDos(todoVerwaltung.GetToDos(new Empty()));
            await Streaming(todoVerwaltung);

            PrintAktion();

            var aktion = Convert.ToInt32(Console.ReadLine());

            while(aktion != 4)
            {
                Console.WriteLine("Id des ToDo Items: ");
                var id = Convert.ToInt32(Console.ReadLine());
                var todoItem = FindToDo(todoVerwaltung.GetToDos(new Empty()), id);
                switch (aktion)
                {
                    case 1:
                        todoVerwaltung.CheckToDoItem(new ToDoId { Id = todoItem.Id });
                        break;
                    case 2:
                        todoVerwaltung.DeleteToDo(todoItem);
                        break;
                    case 3:
                        var tempTodo = todoVerwaltung.GetToDo(new ToDoId { Id = todoItem.Id });
                        Console.WriteLine($" {tempTodo.Id} | {tempTodo.Beschreibung} | {tempTodo.Erledigt}");
                        break;
                    default:
                        break;
                }

                PrintAktion();
                aktion = Convert.ToInt32(Console.ReadLine());
            }
        }

        private static void PrintToDos(Todos todos)
        {
            foreach (var item in todos.Todos_)
            {
                Console.WriteLine($" {item.Id} | {item.Beschreibung} | {item.Erledigt}");
            }
        }


        private static void PrintAktion()
        {
            Console.WriteLine("Was möchten Sie machen?");
            Console.WriteLine("1 = ToDo erledigen | 2 = ToDo Löschen | 3 = Todo anzeigen | 4 = Beenden");
        }

        private static ToDoItem FindToDo(Todos todos, int id)
        {
            foreach (var item in todos.Todos_)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
        }

        private static async Task Streaming(ToDoVerwaltung.ToDoVerwaltungClient client)
        {
            using var streamingRequest = client.GetToDosStream(new Empty());

            await foreach(var item in streamingRequest.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($" {item.Id} | {item.Beschreibung} | {item.Erledigt}");
            }
        }

    }
}
