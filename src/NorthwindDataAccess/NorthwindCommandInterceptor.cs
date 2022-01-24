using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NorthwindDataAccess
{
    public class NorthwindCommandInterceptor : DbCommandInterceptor
    {
        private readonly ILogger<NorthwindCommandInterceptor> logger;

        public NorthwindCommandInterceptor(ILogger<NorthwindCommandInterceptor> logger)
        {
            this.logger = logger;
        }

        public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
        {
            return base.CommandCreating(eventData, result);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public async override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var res = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);

            return res;
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            return base.NonQueryExecuting(command, eventData, result);
        }

        public async override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            var res = await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            return res;
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            return base.ScalarExecuting(command, eventData, result);
        }
    }
}
