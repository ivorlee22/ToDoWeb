using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ToDoWeb.Infrastructures.Interceptors
{
    //log lại những câu query có thời gian thực thi lớn hơn 1s

    public class SqlQuerryLoggingInterceptor : DbCommandInterceptor
    {
        private Stopwatch stopwatch = new Stopwatch();
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {

            stopwatch.Start();



            return base.ReaderExecuting(command, eventData, result);
        }
        public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            using StreamWriter writer = new StreamWriter("F:\\everedu\\learning\\ToDoWeb\\sqllog.txt", append: true);
            stopwatch.Stop();
            var miliseconds = stopwatch.ElapsedMilliseconds;
            if (miliseconds > 2)
            {
                writer.WriteLine(command.CommandText);
            }
            return base.ReaderExecuted(command, eventData, result);
        }
    }
}
