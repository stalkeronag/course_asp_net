
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class RefreshTokenSessionBuilderService : IRefreshTokenSessionBuilderService
    {

        private List<RefreshTokenSessionConnection> ConnectionsAdd {  get; set; }

        private List<RefreshTokenSessionConnection> ConnectionsRemove { get; set; }

        private readonly AppDbContext context;

        public RefreshTokenSessionBuilderService(AppDbContext context)
        {
            this.context = context;
            ConnectionsAdd = new List<RefreshTokenSessionConnection>();
            ConnectionsRemove = new List<RefreshTokenSessionConnection>();
        }

        public async Task Build(RefreshTokenSession refreshTokenSession)
        {
            for (int i = 0; i < ConnectionsRemove.Count; i++)
            {
                var connection = refreshTokenSession.Connections.Where(conn => conn.RefreshToken.Token
                .Equals(ConnectionsRemove[i].RefreshToken.Token)).FirstOrDefault();
                refreshTokenSession.Connections.Remove(connection);
            }
            
            refreshTokenSession.Connections.AddRange(ConnectionsAdd);
            await context.SaveChangesAsync();
        }

        public IRefreshTokenSessionBuilderService AddRefreshTokenConnection(RefreshTokenSessionConnection connection)
        {
            ConnectionsAdd.Add(connection);
            return this;
        }

        public IRefreshTokenSessionBuilderService RemoveRefreshTokenConnection(RefreshTokenSessionConnection connection)
        {
            ConnectionsRemove.Add(connection);
            return this;
        }
    }
}
