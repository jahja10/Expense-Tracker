using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Users.Data;


public class GetUsersResult

{
    
    public List<User> Users { get; set; } = new();

}