using MovieTracker_API.Helpers;

namespace MovieTracker_API.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticationResponse> BuildToken(UserCredentials userCredentials);
    }
}
