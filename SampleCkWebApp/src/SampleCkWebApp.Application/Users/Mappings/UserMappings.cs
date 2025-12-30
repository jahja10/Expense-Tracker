
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.Users.Data;
using SampleCkWebApp.Users;

namespace SampleCkWebApp.Application.Users.Mappings;


public static class UserMappings
{
    
    public static GetUsersResponse ToResponse(this GetUsersResult result)
    {
        return new GetUsersResponse
        {
            Users = result.Users.Select(u => u.ToResponse()).ToList(),
            TotalCount = result.Users.Count
        };
    }
    
    
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}