using Xunit;
using Moq;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using SGRH.Domain.Entities.UserManagement;
using SGRH.Persistence.Repositories.UserManagement;
using System.Collections.Generic;


namespace SGRH.Persistence.Test.UserManagement
{
    
    public class ClientRepositoryTest
    {
        private readonly Mock<IDbConnection> _dbConnectionMock;
        
        private readonly ClientRepository _clientRepository;

        public ClientRepositoryTest()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _clientRepository = new ClientRepository("dummy_connection_string");
        }

       

        [Fact(Skip = "Dapper methods are hard to mock for pure unit tests. Prefer integration tests.")]
        public async Task CreateClientAsync_ShouldExecuteInsertQuery()
        {
        }

        [Fact(Skip = "Dapper methods are hard to mock for pure unit tests. Prefer integration tests.")]
        public async Task GetClientByIdAsync_ShouldReturnClient()
        {
        }

        [Fact(Skip = "Dapper methods are hard to mock for pure unit tests. Prefer integration tests.")]
        public async Task UpdateClientAsync_ShouldExecuteUpdateQuery()
        {
            
        }

        [Fact(Skip = "Dapper methods are hard to mock for pure unit tests. Prefer integration tests.")]
        public async Task DisableClientAsync_ShouldExecuteUpdateQuery()
        {
            
        }
    }
}