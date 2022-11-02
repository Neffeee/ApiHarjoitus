using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Services.Interfaces
{
    public interface IAuthenticateService
    {
        LoggedUser Authenticate(string username, string password);
    }
}
