using Microsoft.AspNetCore.Identity;

namespace Website_Progress.ModelsDTO
{
    public class UserDTO : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

    }
}
