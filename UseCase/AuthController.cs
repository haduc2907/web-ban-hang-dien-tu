using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace UseCase
{
    public class AuthController
    {
        private readonly IAuthRepository auth;
        public AuthController(IAuthRepository auth)
        {
            this.auth = auth;
        }
        public void Register(Users user)
        {
           auth.Register(user);
        }
        public Users? Login(string username, string password)
        {
            return auth.Login(username, password);
        }

    }
}
