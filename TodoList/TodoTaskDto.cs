namespace TodoList;

public class TodoTaskDto
{
    public int? Id { get; set; } 
    public int? ParentId { get; set; }
    public string Summary { get; set; } 
    public string Description { get; set; } 
    public DateTime Created { get; set; } 
    public DateTime DueDate { get; set; } 
    public int Priority { get; set; }
    public TodoTaskStatus Status { get; set; }
}