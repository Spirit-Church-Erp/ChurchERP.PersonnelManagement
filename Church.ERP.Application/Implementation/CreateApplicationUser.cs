using AutoMapper;
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
            public string FirstName { get; set; }
            public string LastName { get; set; }
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
            private readonly IMapper _mapper;


            public Handler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJWTConfiguration jwt)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwt = jwt;

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Request, ApplicationUser>()
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

              });

                _mapper = config.CreateMapper();
            }
            public async Task<ApiResponse<Result>> Handle(Request request, CancellationToken cancellationToken)
            {

                if (string.IsNullOrEmpty(request.Username))
                {
                    request.Username = request.Email.Split('@')[0];
                }

                ApplicationUser existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
                if (existingUserByEmail != null)
                {
                    return new ApiResponse<Result>($"Email {existingUserByEmail.Email} already in use", 409);
                }

                // Check if the username is already in use
                ApplicationUser existingUserByUsername = await _userManager.FindByNameAsync(request.Username);
                if (existingUserByUsername != null)
                {
                    return new ApiResponse<Result>($"Username {existingUserByUsername.UserName} already in use", 409);
                }

                // Check password complexity (you can customize the validation rules as needed)
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, null, request.Password);
                if (!passwordValidationResult.Succeeded)
                {
                    var error = passwordValidationResult.Errors.Select(e => e.Description).ToList();
                    return new ApiResponse<Result>("Password does not meet complexity requirements", 400, error);
                }

                // Map the request to ApplicationUser entity
                ApplicationUser user = _mapper.Map<ApplicationUser>(request);
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    // Optionally sign in the user after creation
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
