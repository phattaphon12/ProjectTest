using Authentication.Models;
using Authentication.Repositories;

namespace Authentication.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepo;

        public AuthService(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        public async Task<string> LoginAsync(LoginReq req)
        {
            if (req == null) throw new Exception("Username and password is required");

            User userInDB = await _authRepo.GetUserAsync(req.username);
            if (userInDB == null) throw new Exception("No data");

            if (userInDB.password != req.password) throw new Exception("Invalid password");

            return "Success";
        }
    }
}
