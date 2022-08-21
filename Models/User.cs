using Microsoft.AspNetCore.Identity;

namespace WawAPI.Models;

public class User : IdentityUser
{
    public ICollection<Event> Events { get; set; } = default!;
}
