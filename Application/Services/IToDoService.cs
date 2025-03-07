using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToDoWeb.Application.Dtos;
using ToDoWeb.Domains.Entities;
using ToDoWeb.Infrastructures;

namespace ToDoWeb.Application.Services
{
    public interface IToDoService
    {
        int Post(ToDoCreatedModel toDo);

        Guid Generate();

    }

    public class ToDoService : IToDoService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IGuidGenerator _guidGenerator;

        public ToDoService(IApplicationDbContext dbContext, IGuidGenerator guidGenerator)
        {
            _dbContext = dbContext;
            _guidGenerator = guidGenerator;
        }

        public int Post(ToDoCreatedModel toDo)
        {
            var data = new ToDo
            {
                Description = toDo.Description,

            };
            _dbContext.ToDos.Add(data);
            _dbContext.SaveChanges();

            return data.Id;
        }
        public Guid Generate()
        {
            return _guidGenerator.Generate();
        }
    }
}
