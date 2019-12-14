using System;
using System.Threading.Tasks;
using Grpc.Core;
using SimpleGrpcService.Repositorys;

namespace SimpleGrpcService
{
    public class ToDoVerwaltungService : ToDoVerwaltung.ToDoVerwaltungBase
    {
        private readonly ToDoRepository _repo;

        public ToDoVerwaltungService()
        {
            _repo = new ToDoRepository();
        }

        public override async Task GetToDosStream(Empty request, IServerStreamWriter<ToDoItem> responseStream, ServerCallContext context)
        { 
            var items = _repo.GetToDoItems();
            if(items.Todos_.Count > 0)
            {
                foreach (var item in items.Todos_)
                {
                    await Task.Delay(500);
                    await responseStream.WriteAsync(item);
                }
            }
        }

        public override Task<Todos> GetToDos(Empty request, ServerCallContext context)
        {
            return Task.FromResult(
                _repo.GetToDoItems()
            );
        }

        public override Task<ToDoItem> GetToDo(ToDoId request, ServerCallContext context)
        {
            return Task.FromResult(
                _repo.GetTodoItem(request.Id)
            );
        }

        public override Task<Ergebnis> CheckToDoItem(ToDoId request, ServerCallContext context)
        {
            return Task.FromResult(
                _repo.CheckTodoItem(request.Id)
            ); 
        }

        public override Task<Ergebnis> DeleteToDo(ToDoItem request, ServerCallContext context)
        {
            return Task.FromResult(
                _repo.DeleteToDoItem(request)
            );
        }
    }
}
