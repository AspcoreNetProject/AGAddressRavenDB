using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using System.Globalization;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using Raven.Client.Exceptions.Database;

namespace AGAddressRavenDB.Persistence
{
    public interface IRavenDBContext
    {
        public IDocumentStore store { get; }
    }

    public class RavenDbConext : IRavenDBContext
    {
        private readonly DocumentStore _store;
        public IDocumentStore store => _store;
        private readonly PersistenceSettings _persistenceSettings;

        public RavenDbConext(IOptionsMonitor<PersistenceSettings> settings)
        {
            _persistenceSettings = settings.CurrentValue;

            _store = new DocumentStore()
            {
                Database = _persistenceSettings.DatabaseName,
                Urls = _persistenceSettings.Urls
            };

            _store.Initialize();

            EnsureDatabaseIsCreated();
        }
        public void EnsureDatabaseIsCreated()
        {
            try
            {
                _store.Maintenance.ForDatabase(_persistenceSettings.DatabaseName).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                _store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(_persistenceSettings.DatabaseName)));
            }

        }
    }

}
