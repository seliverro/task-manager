namespace TodoList;

public class TodoTaskUpdateDto
{
    public int Id { get; set; } 
    public string Summary { get; set; } 
    public string Description { get; set; } 
    public DateTime DueDate { get; set; } 
    public int Priority { get; set; }
    public TodoTaskStatus Status { get; set; }
}