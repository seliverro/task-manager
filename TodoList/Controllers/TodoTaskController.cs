using Microsoft.AspNetCore.Mvc;
using TodoList.Postgre;

namespace TodoList.Controllers;

[ApiController]
[Route("[controller]")]
// [EnableCors(Const.MY_ALLOW_SPECIFIC_ORIGINS)]
public class TodoTaskController : ControllerBase
{
    //TODO: make it more REST

    private readonly ILogger<TodoTaskController> Logger;
    private readonly TaskService TaskService;

    public TodoTaskController(ILogger<TodoTaskController> logger, TaskService taskService)
    {
        Logger = logger;
        TaskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? sortBy, [FromQuery] bool? isAsc)
    {
        return Ok(await TaskService.Get(null, sortBy, isAsc));
    }

    [HttpGet]
    [Route("getChildren/{parentTaskId:int}")]
    public async Task<IEnumerable<TodoTask>> Get(int parentTaskId)
    {
        return await TaskService.Get(parentTaskId);
    }

    [HttpGet]
    [Route("getAllShort")]
    public async Task<IEnumerable<TodoTaskShortDto>> GetAll()
    {
        return await TaskService.GetAllShort();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoTaskCreateDto dto)
    {
        return Ok(await TaskService.Create(dto)); //TODO: better return Created (?)
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TodoTaskUpdateDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        await TaskService.Update(dto);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await TaskService.Delete(id);
        return Ok();
    }

    [HttpPatch]
    [Route("makeRoot/{id:int}")]
    public async Task<IActionResult> Move(int id)
    {
        await TaskService.Move(id, null);
        return Ok();
    }

    [HttpPatch]
    [Route("makeChild/{id:int}/{newParentId:int}")]
    public async Task<IActionResult> Move(int id, int newParentId)
    {
        await TaskService.Move(id, newParentId);
        return Ok();
    }
}