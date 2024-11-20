using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BookLibrary.Authentication;

public class NoTokenAuthSchemeHandler(
    IOptionsMonitor<NoTokenAuthSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<NoTokenAuthSchemeOptions>(options, logger, encoder)
{
    public const string NoTokenAuth = nameof(NoTokenAuth);

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Logger.LogInformation($"Authentication goes here");

        // returns pseudo authentication data when token is not available
        // there are no claims so this identity wont have access to anything
        var claims = Array.Empty<Claim>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, NoTokenAuth));
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        await Task.Yield();

        return AuthenticateResult.Success(ticket);
    }
}
