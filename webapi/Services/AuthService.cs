using Microsoft.EntityFrameworkCore;
using webapi.Dtos;
using webapi.Entities;

namespace webapi.Services
{
    public class AuthService
    {
        private readonly TimeSpan timeToExpire = new(2, 0, 0, 0);
        private readonly ClothingContext _context;
        public AuthService(ClothingContext context) { _context = context; }

        private DateTime SessionExpire() => DateTime.UtcNow.Add(timeToExpire);
        
        public async Task<(Session, User)> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(us => us.Email == dto.Email && us.Password == dto.Password)
                ?? throw new Exception("User is not found");
            
            var session = new Session { 
                Id = new Guid(), 
                User = user, 
                Expire = SessionExpire()
            };
            
            await _context.AddAsync(session);
            await _context.SaveChangesAsync();
            
            return (session, user);
        }

        public async Task<(Session, User)> Register(RegisterDto dto)
        {
            var isAnyWithSameEmail = await _context.Users.AnyAsync(us => us.Email == dto.Email);
            
            if (isAnyWithSameEmail)
            {
                throw new Exception("User w/ same Email is already exist");
            }

            var user = new User 
            {
                Email = dto.Email,
                Password = dto.Password,
                Id = new Guid(),
                IsAdmin = false
            };
            await _context.AddAsync(user);
            
            var session = new Session { 
                Id = new Guid(), 
                User = user, 
                Expire = SessionExpire() 
            };
            await _context.AddAsync(session);
            
            await _context.SaveChangesAsync();

            return (session, user);
        }
        public async Task<bool> AuthCheck(string sessId)
        {
            var sessIdGuid = new Guid(sessId);
            var session = await _context.Sessions.FirstOrDefaultAsync(sess => sess.Id == sessIdGuid);
            return session != null;
        } 
        public async Task<Session> RegenerateSession(string sessId)
        {
            var sessIdGuid = new Guid(sessId);
            var session = await _context.Sessions.FirstOrDefaultAsync(sess => sess.Id == sessIdGuid)
                ?? throw new Exception("Session is incorrect");
            
            var newSession = new Session { 
                Id = new Guid(), 
                User = session.User, 
                Expire = SessionExpire() 
            };
            
            await _context.AddAsync(newSession);
            await _context.SaveChangesAsync();
            
            return newSession;
        }
        public async Task<User> GetUser(string sessId)
        {
            var sessIdGuid = new Guid(sessId);
            var session = await _context.Sessions
                .Include(el => el.User)
                .FirstOrDefaultAsync(sess => sess.Id == sessIdGuid)
                ?? throw new Exception("Session is not found");

            return session.User 
                ?? throw new Exception("User is not defined");
        }
    }
}
