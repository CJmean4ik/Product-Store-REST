using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.Entities;
using UsersRestApi.Entities;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Interfaces
{
    public class UserRepositoryEF : IUserRepository<UserEntity, OperationStatusResponseBase>
    {
        private DatabaseContext _db;
        private ILogger<UserRepositoryEF> _logger;

        public UserRepositoryEF(DatabaseContext db, ILogger<UserRepositoryEF> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<OperationStatusResponseBase> Create(UserEntity entity)
        {
            try
            {
                var user = await _db.Users
                    .Where(w => w.Username == entity.Username && w.Email == entity.Email)
                    .FirstOrDefaultAsync();

                if (user is not null)
                {
                    string WARRNING_MESSAGE = $"User with that username: [{entity.Username}] and email: [{entity.Email}] - alredy exist in database";
                    _logger.LogWarning(WARRNING_MESSAGE);
                    return OperationStatusResonceBuilder.CreateStatusAuthorized();
                }

                await _db.Users.AddAsync(entity);
                await _db.SaveChangesAsync();

                _logger.LogInformation($"Users Successful added id: [{entity.Id}]");
                return OperationStatusResonceBuilder.CreateStatusAdding(entity);
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to create entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogWarning(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }
        public async Task<UserEntity> GetByName(string name)
        {
            try
            {
                var user = await _db.Users
                    .Where(w => w.Username == name)
                    .FirstOrDefaultAsync();

                if (user is null)
                {
                    string WARRNING_MESSAGE = $"User with that username: [{name}] - didnt exist in database";
                    _logger.LogWarning(WARRNING_MESSAGE);
                    return user;
                }
                
                _logger.LogInformation($"Users Successful added id: [{user.Id}]");
                return user;
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to take entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogWarning(ERROR_MESSAGE);
                return null;
            }
        }
    }
}
            
