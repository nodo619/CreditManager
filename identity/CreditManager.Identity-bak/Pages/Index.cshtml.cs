// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CreditManager.Identity.Data;
using System.Security.Claims;

namespace CreditManager.Identity.Pages.Home;

[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<CreditManagerUser> _userManager;
    private readonly SignInManager<CreditManagerUser> _signInManager;

    public Index(
        UserManager<CreditManagerUser> userManager,
        SignInManager<CreditManagerUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string? UserName { get; set; }
    public string? UserRole { get; set; }
    public bool IsAuthenticated { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                UserName = $"{user.FirstName} {user.LastName}";
                var roles = await _userManager.GetRolesAsync(user);
                UserRole = roles.FirstOrDefault();
                IsAuthenticated = true;
            }
        }

        return Page();
    }
}
