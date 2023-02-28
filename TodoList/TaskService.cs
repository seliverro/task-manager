using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Postgre;

namespace TodoList;

public class TaskService
{
    private readonly TodoListDataContext DataContext;

    public TaskService(TodoListDataContext dataContext)
    {
        DataContext = dataContext;
    }


    public async Task<List<TodoTask>> Get(int? parentId, string? sortBy = null, bool? isAsc = null)
    {
        var query = parentId.HasValue
            ? DataContext.TodoTasks.Where(o => o.ParentTask != null && o.ParentTask.Id == parentId)
            : DataContext.TodoTasks.Where(o => o.ParentTask == null);

        if (sortBy == null || isAsc == null)
        {
            return await query.ToListAsync();
        }

        //TODO: try to improve this code to prevent copypasting
        var queryOrderedBy = sortBy.ToLowerInvariant() switch
        {
            "summary" => isAsc.Value ? query.OrderBy(o => o.Summary) : query.OrderByDescending(o => o.Summary),
            "description" => isAsc.Value
                ? query.OrderBy(o => o.Description)
                : query.OrderByDescending(o => o.Description), //TODO: not a good idea to sort by description
            "created" => isAsc.Value ? query.OrderBy(o => o.Created) : query.OrderByDescending(o => o.Created),
            "duedate" => isAsc.Value ? query.OrderBy(o => o.DueDate) : query.OrderByDescending(o => o.DueDate),
            "priority" => isAsc.Value ? query.OrderBy(o => o.Priority) : query.OrderByDescending(o => o.Priority),
            "status" => isAsc.Value ? query.OrderBy(o => o.Status) : query.OrderByDescending(o => o.Status),
            _ => throw new ArgumentOutOfRangeException(nameof(sortBy))
        };

        return await queryOrderedBy.ToListAsync();
    }

    public async Task<List<TodoTaskShortDto>> GetAllShort()
    {
        var result = await DataContext.TodoTasks
            .OrderBy(o => o.Summary)
            .Select(o => new TodoTaskShortDto {Id = o.Id, Summary = o.Summary})
            .ToListAsync();
        return result;
    }

    public async Task Move(int id, int? newParentId)
    {
        var todoTask = await DataContext.TodoTasks.FindAsync(id);
        if (todoTask == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found");
        }

        todoTask.ParentTaskId = newParentId;
        DataContext.TodoTasks
            .Update(todoTask); // if there is no task with id = newParentId in the database, there will be a database exception here, it's OK
        
        await DataContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var todoTask = await DataContext.TodoTasks.FindAsync(id);
        if (todoTask == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found");
        }

        DataContext.TodoTasks.Remove(todoTask);
        await DataContext.SaveChangesAsync();
    }

    public async Task Update(TodoTaskUpdateDto dto)
    {
        var todoTask = await DataContext.TodoTasks.FindAsync(dto.Id);
        if (todoTask == null)
        {
            throw new KeyNotFoundException($"Entity with id {dto.Id} not found");
        }

        todoTask.Summary = dto.Summary;
        todoTask.Description = dto.Description;
        todoTask.DueDate = dto.DueDate.ToUniversalTime();
        todoTask.Priority = dto.Priority;
        todoTask.Status = dto.Status;

        DataContext.TodoTasks.Update(todoTask);
        await DataContext.SaveChangesAsync();
    }

    public async Task<int> Create(TodoTaskCreateDto dto)
    {
        var todoTask = new TodoTask()
        {
            Created = DateTime.UtcNow,

            ParentTaskId = dto.ParentId,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = dto.Status,
            Summary = dto.Summary,
            DueDate = dto.DueDate.ToUniversalTime()
        };

        var result = await DataContext.TodoTasks.AddAsync(todoTask);
        await DataContext.SaveChangesAsync();

        return result.Entity.Id;
    }
}