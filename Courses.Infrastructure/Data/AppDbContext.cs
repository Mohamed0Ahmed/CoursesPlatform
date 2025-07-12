using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Courses.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
    }
}
