using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToDoWeb.Domains.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ToDoWeb.Infrastructures.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        List<EntityEntry> addedEntity = new List<EntityEntry>();
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context as ApplicationDbContext;
            var auditLogs = new List<AuditLog>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog) continue;
                var log = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = entry.State.ToString(),

                };
                if (entry.State == EntityState.Added)
                {
                    addedEntity.Add(entry);

                    //log.NewValue = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                if (entry.State == EntityState.Modified)
                {
                    log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    log.NewValue = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    auditLogs.Add(log);
                }
                if (entry.State == EntityState.Deleted)
                {
                    log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    auditLogs.Add(log);
                }

            }
            if (auditLogs.Any())
            {
                context.AuditLog.AddRange(auditLogs); //state = added
            }

            return base.SavingChanges(eventData, result);

        }
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            var context = eventData.Context as ApplicationDbContext;


            if (addedEntity.Any())
            {
                var auditLogs = addedEntity.Select(entity => new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = entity.State.ToString(),
                    NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject())
                });
                context.AuditLog.AddRange(auditLogs);
                addedEntity.Clear();
                context.SaveChanges();
            }

            return base.SavedChanges(eventData, result);
        }
    }
}
