using Church.ERP.Application.Interfaces;
using Church.ERP.Domain.Entities;
using Church.ERP.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Church.ERP.Application.Implementation
{
    public static class CreateApplicationUser
    {
        public class Request : IRequest<ApiResponse<Result>>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

           
        }

        public class Result
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string JwtToken { get; set; }
        }

        public class Handler : IRequestHandler<Request, ApiResponse<Result>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IJWTConfiguration _jwt;

            public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJWTConfiguration jwt)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwt = jwt;
            }
            public async Task<ApiResponse<Result>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser { UserName = request.Username?? request.Email.Split('@')[0], Email = request.Email };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var token = _jwt.GenerateJwtToken(user);
                    var responseResult = new Result { Username = user.UserName, Email = user.Email, JwtToken = token };
                    return new ApiResponse<Result>(responseResult, true, "User created successfully", 200);
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return new ApiResponse<Result>("Failed to create user", 400, errors);
            }
        }
    }
}
