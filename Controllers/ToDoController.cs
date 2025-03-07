using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToDoWeb.Application.Dtos;
using ToDoWeb.Application.Services;
using ToDoWeb.Domains.Entities;
using ToDoWeb.Infrastructures;

namespace ToDoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {

        private readonly IApplicationDbContext _dbContext;
        private readonly IToDoService _toDoService;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISingletonGenerator _singletonGenerator;
        private readonly GuidData _guiData;

        public ToDoController(
            IApplicationDbContext dbcontext,
            IToDoService toDoService,
            IGuidGenerator guidGenerator,
            ISingletonGenerator singletonGenerator,
            GuidData guiData)
        {
            _dbContext = dbcontext;
            _toDoService = toDoService;
            _guidGenerator = guidGenerator;
            _singletonGenerator = singletonGenerator;
            _guiData = guiData;
        }

        [HttpGet("guid")]
        public Guid[] GetGuid()
        {
            _guiData.guidGenerator = _guidGenerator;
            return
                new Guid[]
                {
                    _guiData.GetGuid(),
                    _singletonGenerator.Generate(),
                };
        }


        [HttpGet]
        public IEnumerable<ToDoViewModel> Get(bool isCompleted)
        {
            var data = _dbContext.ToDos.Where(x => x.IsCompleted == isCompleted)
                .Select(x =>
                    new ToDoViewModel
                    {
                        Description = x.Description,
                        IsCompleted = x.IsCompleted
                    })
                .ToList();

            return data;
        }

        [HttpPost]
        public int Post(ToDoCreatedModel toDo)
        {
            return _toDoService.Post(toDo);
        }

        [HttpPut]
        public int Put(ToDoUpdatedModel toDo)
        {
            var data = _dbContext.ToDos.Find(toDo.Id);
            if (data == null) { return -1; }
            data.Description = toDo.Description;
            data.IsCompleted = toDo.IsCompleted;
            _dbContext.SaveChanges();
            return toDo.Id;
        }

        [HttpDelete]
        public int Delete(int id)
        {
            var data = _dbContext.ToDos.Find(id);
            if (data == null) { return -1; }
            _dbContext.ToDos.Remove(data);
            _dbContext.SaveChanges();
            return id;
        }
    }

}
