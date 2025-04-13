using assignment_to_do_list.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace assignment_to_do_list.Persistence
{
    public class TaskDbContext:DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}
