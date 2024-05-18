using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Exceptions;
using WebApi.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services.Implementations
{
    public class RefreshTokenSessionService : IRefreshTokenSessionService
    {
        private readonly AppDbContext context;


        public RefreshTokenSessionService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task DeleteSessionById(string id)
        {
            context.RefreshTokenSessions.Remove(context.RefreshTokenSessions.Where(session => session.Id == id).FirstOrDefault());
            await context.SaveChangesAsync();
        }

        public async Task<RefreshTokenSession> GetExistSessionOrCreate(User user)
        {
            if (!SessionExist(user))
            {
                var newRefreshSession = new RefreshTokenSession()
                {
                    User = user,
                    Connections = new List<RefreshTokenSessionConnection>()
                };
                context.RefreshTokenSessions.Add(newRefreshSession);
                return newRefreshSession;
            }
            var session =  context.RefreshTokenSessions.Where(session => session.User.Id == user.Id).
                FirstOrDefault();
            LoadRelationSessionData(session);
            return session;
        }

        public RefreshTokenSession GetSessionByRefreshToken(string refreshToken)
        {

            var connection = context.RefreshTokenSessionConnections
                .Include(conn => conn.RefreshToken)
                .Include(conn => conn.FingerPrint)
                .Include(conn => conn.RefreshTokenSession)
                .Where(conn => conn.RefreshToken.Token.Equals(refreshToken))
                .FirstOrDefault();

            if (connection == null)
            {
                throw new SessionNotExistException();
            }

            connection.RefreshTokenSession.User = context.Users.Where(user => user.Id.Equals(connection.RefreshTokenSession.UserId))
                .FirstOrDefault();


            LoadRelationSessionData(connection.RefreshTokenSession);
            return connection.RefreshTokenSession;
        }

        public bool SessionExist(User user)
        {
            int isSessionExist = context.RefreshTokenSessions.Where(session => session.User.Id == user.Id).Count();

            if (isSessionExist == 1)
                return true;
            else
                return false;
        }

        private void LoadRelationSessionData(RefreshTokenSession session)
        {
            context.RefreshTokenSessionConnections.Where(connection => connection.RefreshTokenSession.Id.Equals(session.Id)).Load();

        }
    }
}
