using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ToDoWeb.Domains.Interfaces
{
    public interface ICreatedBy
    {
        public int CreatedBy { get; set; }

    }
}
