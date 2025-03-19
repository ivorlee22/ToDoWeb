using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToDoWeb.Domains.Interfaces;

namespace ToDoWeb.Infrastructures.Interceptors
{
    public class CreateInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context as ApplicationDbContext;
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.State is ICreatedAt)
                    {
                        (entry.Entity as ICreatedAt).CreatedAt = DateTime.Now;
                    }
                    if (entry is ICreatedBy)
                    {
                        (entry.Entity as ICreatedBy).CreatedBy = 1;
                    }
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IUpdatedAt)
                    {
                        (entry.Entity as IUpdatedAt).UpdatedAt = DateTime.Now;
                    }
                    if (entry.State is IUpdatedBy)
                    {
                        (entry.Entity as IUpdatedBy).UpdatedBy = 1;
                    }
                }
            }
            return base.SavingChanges(eventData, result);
        }
    }
}
