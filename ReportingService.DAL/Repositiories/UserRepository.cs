using ReportingService.DAL.DTOs;
using ReportingService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportingService.DAL.Repositiories
{
    public class UserRepository : IRepository<UserDTO>
    {
        private readonly ApplicationContext db;
        public UserRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<UserDTO> Create(UserDTO userDto)
        {
            try
            {
                if (userDto != null)
                {
                    var user = new User
                    {
                        Login = userDto.Login,
                        Password = userDto.Password,
                        Email = userDto.Email
                    };

                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return userDto;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(UserDTO _object)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserDTO> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public UserDTO GetById(int Id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UserDTO _object)
        {
            throw new System.NotImplementedException();
        }
    }
}
