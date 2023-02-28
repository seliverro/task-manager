using Microsoft.EntityFrameworkCore;

namespace TodoList.Postgre;

public class TodoListDataContext : DbContext
{
    public TodoListDataContext(DbContextOptions<TodoListDataContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql("Server=localhost;Database=todolist;Port=5435;User Id=user;Password=password");
    // }
    
    // public TodoListDataContext() 
    // {
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
    }

    public DbSet<TodoTask> TodoTasks { get; set; }
}