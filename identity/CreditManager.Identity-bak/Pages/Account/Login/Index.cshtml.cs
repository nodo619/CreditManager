// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreditManager.Identity.Data;

namespace CreditManager.Identity.Pages.Account.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<CreditManagerUser> _userManager;
    private readonly SignInManager<CreditManagerUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IClientStore _clientStore;

    public ViewModel View { get; set; } = default!;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        UserManager<CreditManagerUser> userManager,
        SignInManager<CreditManagerUser> signInManager,
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IClientStore clientStore,
        IEventService events)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _clientStore = clientStore;
        _events = events;
    }

    public async Task<IActionResult> OnGet(string? returnUrl)
    {
        await BuildModelAsync(returnUrl);
            
        if (View.IsExternalLoginOnly)
        {
            return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
        }

        return Page();
    }
        
    public async Task<IActionResult> OnPost()
    {
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        if (Input.Button != "login")
        {
            if (context != null)
            {
                ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                if (context.IsNativeClient())
                {
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                return Redirect("~/");
            }
        }

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberLogin, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(Input.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName, clientId: context?.Client.ClientId));

                if (context != null)
                {
                    ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage(Input.ReturnUrl);
                    }

                    return Redirect(Input.ReturnUrl ?? "~/");
                }

                if (Url.IsLocalUrl(Input.ReturnUrl))
                {
                    return Redirect(Input.ReturnUrl);
                }
                else if (string.IsNullOrEmpty(Input.ReturnUrl))
                {
                    return Redirect("~/");
                }
                else
                {
                    throw new ArgumentException("invalid return URL");
                }
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(Input.Username, "invalid credentials", clientId:context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, "Invalid username or password");
        }

        await BuildModelAsync(Input.ReturnUrl);
        return Page();
    }

    private async Task BuildModelAsync(string? returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };
            
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

            View = new ViewModel
            {
                EnableLocalLogin = local,
            };

            Input.Username = context.LoginHint;

            if (!local)
            {
                View.ExternalProviders = new[] { new ViewModel.ExternalProvider(authenticationScheme: context.IdP) };
            }

            return;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ViewModel.ExternalProvider
            (
                authenticationScheme: x.Name,
                displayName: x.DisplayName ?? x.Name
            )).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                {
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }
        }

        View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }
}

public class ViewModel
{
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    public class ExternalProvider
    {
        public string? DisplayName { get; set; }
        public string? AuthenticationScheme { get; set; }

        public ExternalProvider(string? authenticationScheme, string? displayName = null)
        {
            AuthenticationScheme = authenticationScheme;
            DisplayName = displayName;
        }
    }
}

public class InputModel
{
    public string? ReturnUrl { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool RememberLogin { get; set; }
    public string? Button { get; set; }
}

public static class LoginOptions
{
    public static bool AllowRememberLogin = true;
    public static bool AllowLocalLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
    public static string InvalidCredentialsErrorMessage = "Invalid username or password";
}
