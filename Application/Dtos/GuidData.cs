using ToDoWeb.Application.Services;

namespace ToDoWeb.Application.Dtos
{
    public class GuidData
    {
        public IGuidGenerator guidGenerator { get; set; }

        public Guid GetGuid() { return guidGenerator.Generate(); }

    }
}
