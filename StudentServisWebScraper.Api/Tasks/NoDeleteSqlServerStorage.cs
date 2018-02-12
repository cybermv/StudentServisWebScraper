using Hangfire.SqlServer;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Server;
using Hangfire.Annotations;
using System.Data.Common;

namespace StudentServisWebScraper.Api.Tasks
{
    public class NoDeleteSqlServerStorage : SqlServerStorage
    {
        public NoDeleteSqlServerStorage(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public NoDeleteSqlServerStorage(string nameOrConnectionString, SqlServerStorageOptions options) : base(nameOrConnectionString, options) { }
        public NoDeleteSqlServerStorage([NotNull] DbConnection existingConnection) : base(existingConnection) { }
        public NoDeleteSqlServerStorage([NotNull] DbConnection existingConnection, [NotNull] SqlServerStorageOptions options) : base(existingConnection, options) { }

        public override IEnumerable<IServerComponent> GetComponents()
        {
            var x = base.GetComponents().Where(c => !c.GetType().Name.Contains("ExpirationManager"));

            return x;
        }
    }
}
