using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;

namespace WebApplication1.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
       

       

        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> ListAsync(RegistrationModel model);

       

    }
}
