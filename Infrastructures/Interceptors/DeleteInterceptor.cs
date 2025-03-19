using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToDoWeb.Domains.Interfaces;

namespace ToDoWeb.Infrastructures.Interceptors
{
    public class DeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {

            var context = eventData.Context as ApplicationDbContext;
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted)
                {
                    (entry.Entity as IDelete).DeleteBy = 1;
                    (entry.Entity as IDelete).DeleteAt = DateTime.Now;
                    entry.State = EntityState.Modified;
                }
            }
            return base.SavingChanges(eventData, result);
        }
    }
}
