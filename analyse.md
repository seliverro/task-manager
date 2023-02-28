• The user can create new tasks and delete, update the existing tasks
• The task is going to have,
- Summary: string
- Description: string
- Create date: date
- Due date: date
- Priority: integer
- Status: Reserved / Ongoing / Done / Pending
• Unlimited numbers of subtasks can be created under the task.
• Unlimited numbers of the depth of subtasks.
• All the data is going to be stored in DB. You can choose MySQL or DynamoDB. If you want to
use any other, please let us know beforehand.
• The tasks can be moved into a different location in the list. Any subtask can be moved under
a different task, or even it can be another top-level task.
• A Top-level task can be moved under the other task. For this case, if there were any subtasks
with the moved top-level task, all the subtasks are going to follow the moved top-level task.
• On the list, the tasks can be sorted by each data field. You do not need to sort subtasks.

■ Nice to have
• Able to search the task and list up the result.
• Leave transaction logs (history) and list up on the screen when you want to see.
• If you can use Docker, it is a good idea to set up a Docker package.
• You can also add your own idea if you want.

Итак, сущности - таски. У таски есть несколько полей, по каждому можно сортировать. 
В требованиях написано, что сортировать сабтаски не надо, но я не вижу разницы. 
В общем случае у таски есть родитель и коллекция детей. 
Неясно, по каким правилам таска может менять статус. Можно уточнить. 
Операции над тасками - создать, удалить, отредактировать, назначить родителя.

Ответы на вопросы:
1. "The tasks can be moved into a different location in the list. Any subtask can be moved under a different task, or even it can be another top-level task.": It is as it describes. It means a task can be a sub-task of another task and vice versa.
2. You can choose any DB you want. MySQL and DynamoDB are recommended as we use those two.
3. You can choose any language for the frontend. Javascript is recommended as we use it.
4. There is no limit for moving tasks between statuses. You can make your own flow, and please describe it in your document.
5. There is no limit for "priority" field values.