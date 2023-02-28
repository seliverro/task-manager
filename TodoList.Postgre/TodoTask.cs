using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Postgre;

[PrimaryKey("Id")]
[Table("todo_tasks")]
public class TodoTask
{
    [Column("id")]
    public int Id { get; } // Id in the database
    
    public TodoTask? ParentTask { get; set; }
    
    [Column("parent_task_id")]
    public int? ParentTaskId { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Column("summary")]
    public string Summary { get; set; } // there is no need to make summary unique
    
    [Required]
    [MaxLength(1024)]
    [Column("description")]
    public string Description { get; set; }
    
    [Required]
    [Column("created")]
    public DateTime Created { get; set; }
    
    [Required]
    [Column("due_date")]
    public DateTime DueDate { get; set; }
    
    [Required]
    [Column("priority")]
    public int Priority { get; set; }
    
    [Required]
    [Column("status")]
    public TodoTaskStatus Status { get; set; }
}