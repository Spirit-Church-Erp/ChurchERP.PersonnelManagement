using AutoMapper;
using Church.ERP.Application.Interfaces;
using Church.ERP.Domain.Entities;
using Church.ERP.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Application.Implementation
{
    public static class CreateUserAsAdmin
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
                ApplicationUser user = _mapper.Map<ApplicationUser>(request);
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    // Assign the "Admin" role to the user
                    await _userManager.AddToRoleAsync(user, "Admin");

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
