using Church.ERP.Domain.Enums;
using Church.ERP.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.ERP.Application.Implementation
{
    public static class GetAllRoles
    {
        public class Request : IRequest<ApiResponse<Result>> { }

        public class Result
        {
            public Dictionary<int, string> Roles { get; set; }

            public Result(Dictionary<int, string> roles)
            {
                Roles = roles;
            }
        }

        public class Handler : IRequestHandler<Request, ApiResponse<Result>>
        {
            public async Task<ApiResponse<Result>> Handle(Request request, CancellationToken cancellationToken)
            {
                try
                {
                    Dictionary<int, string> result = new Dictionary<int, string>();
                    List<UserRole> roles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToList();

                    int count = 1;
                    foreach (UserRole role in roles)
                    {
                        result.Add(count, role.ToString());
                        count++;
                    }

                    var responseResult = new Result(result);
                    return new ApiResponse<Result>(responseResult, true, "Roles retrieved successfully", 200);

                }
                catch (Exception ex)
                {
                    var errors = new List<string> { ex.Message };
                    return new ApiResponse<Result>("An error occurred while retrieving roles", 500, errors);
                }
            }
        }
    }
}

