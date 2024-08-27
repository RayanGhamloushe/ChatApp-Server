using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
   
        public interface IUserRepository
        {
            Task<bool> UserExistsAsync(string email);
            Task AddUserAsync(Users user);
            Task<Users> GetUserByEmailAsync(string email);
        }



    
}
