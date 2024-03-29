﻿using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using UsersRestApi.Database.EF;
using UsersRestApi.Database.Entities;
using UsersRestApi.Repositories.OperationStatus;

namespace UsersRestApi.Repositories.Interfaces
{
    public class UserRepository : IUserRepository<BaseUserEntity, OperationStatusResponseBase>
    {
        private DatabaseContext _db;
        private ILogger<UserRepository> _logger;

        public UserRepository(DatabaseContext db, ILogger<UserRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<OperationStatusResponseBase> Create(BaseUserEntity? entity)
        {
            try
            {
                var user = await _db.Users
                    .Where(w => w.Username == entity.Username && w.Email == entity.Email)
                    .FirstOrDefaultAsync();

                if (user is not null)
                {
                    string WARRNING_MESSAGE = $"User with that username: [{entity.Username}] or email: [{entity.Email}] - alredy exist in database";
                    _logger.LogWarning(WARRNING_MESSAGE);
                    return OperationStatusResonceBuilder.CreateStatusAuthorized();
                }

                if (entity is BuyerEntity buyer)
                    await _db.Buyers.AddAsync(buyer);

                if (entity is EmployeeEntity employee)
                    await _db.Employees.AddAsync(employee);

                var id = await _db.SaveChangesAsync();


                _logger.LogInformation($"Users Successful added id: [{entity.Id}]");
                return OperationStatusResonceBuilder.CreateStatusSuccessfully("User have been added in Database");
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to create entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogWarning(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }

        public async Task<OperationStatusResponseBase> Delete(BaseUserEntity? entity)
        {
            try
            {
                var user = await _db.Users
                    .Where(w => w.Id == entity.Id)
                    .FirstOrDefaultAsync();

                if (user is null)          
                    return OperationStatusResonceBuilder.CreateStatusWarning($"User by id: {entity.Id} not found");
                
               _db.Users.Remove(user);

                var id = await _db.SaveChangesAsync();

                return OperationStatusResonceBuilder.CreateStatusSuccessfully("User have been removed from Database");
            }
            catch (Exception ex)
            {
                string ERROR_MESSAGE = $"Failed to removed entity. Reason: [{ex.Message}]. Time: " + DateTime.Now;
                _logger.LogWarning(ERROR_MESSAGE);
                return OperationStatusResonceBuilder.CreateStatusError(message: ERROR_MESSAGE);
            }
        }

        public async Task<BaseUserEntity> GetByName(string name)
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

