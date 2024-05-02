using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SD.Common.Services
{
    public class UserService : IUserService
    {
        /* Users als Mockup */
        private List<User> users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                UserName = "Test",
                Password = new NetworkCredential("Test", "12345").SecurePassword
            }
        };


        public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            /* var user = this.userRepository.QueryFrom(u => u.UserName == username).FirstOrDefault() */
            var user = users.SingleOrDefault(w => string.Compare(w.UserName, username, true) == 0
                                                  && new NetworkCredential(w.UserName, w.Password).Password == password);

            if(user == null)
            {
                return null;
            }


            return await Task.FromResult(user.WithoutPassword);
        }
    }
}
