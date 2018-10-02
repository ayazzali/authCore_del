using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthCore.Core
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB>opt):base(opt) {}

        public DbSet<User> Users { get; set; }

    }
}
