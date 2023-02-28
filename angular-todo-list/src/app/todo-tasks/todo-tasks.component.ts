import { Component, OnInit } from '@angular/core';
import { TodoTask } from '../todoTask';
import { TaskService } from '../task.service';
import { formatDate } from '@angular/common';
import { TodoTaskParent } from '../todoTaskParent';

interface sortedBy {
  column?: string,
  isAsc: boolean
};

@Component({
  selector: 'app-todo-tasks',
  templateUrl: './todo-tasks.component.html',
  styleUrls: ['./todo-tasks.component.css']
})
export class TodoTasksComponent implements OnInit {

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.refresh();
  }

  tasks: TodoTask[] = [];
  parents: TodoTaskParent[] = [];

  selectedTask?: TodoTask;
  parentTasksStack: TodoTask[] = [];

  selectedParentIdFromDropDown?: number;

  sortedBy: sortedBy = {
    column: undefined,
    isAsc: true
  };

  onSelect(task: TodoTask): void {
    this.selectedTask = task;
  }

  onPressCreate(): void {
    const newTask = {
      summary: "new task",
      description: "",
      dueDate: formatDate(Date.now(), 'yyyy-MM-dd', 'en-us'),
      priority: 0,
      status: 0,
      parentId: this.getCurrentParent()?.id
    } as TodoTask;

    this.selectedTask = newTask;
    this.tasks.push(newTask);
  }

  onPressSave(): void {
    if (!this.selectedTask)
      return;

    if (this.selectedTask.id)
      this.taskService.updateTask(this.selectedTask);
    else
      this.taskService.addTask(this.selectedTask);
  }

  onParentDropdownValueChange(value: any): void {
    this.selectedParentIdFromDropDown = value;
  }

  // moving tasks under new parents 
  onPressNewParent(): void {
    console.log('onPressNewParent', this.selectedTask?.id, this.selectedParentIdFromDropDown);
    if (!this.selectedTask)
      return;

    this.taskService.setNewParent(
      this.selectedTask.id, this.selectedParentIdFromDropDown).then(() => this.refresh());
  }

  onPressDelete(task: TodoTask): void {
    this.taskService.deleteTask(task.id);
    this.tasks.splice(this.tasks.indexOf(task), 1);
  }

  onPressChildren(task: TodoTask): void {
    this.parentTasksStack.push(task);
    this.taskService.getChildrenTasks(task.id).subscribe(tasks => this.tasks = tasks);
    this.selectedTask = undefined;
  }

  onPressParent(): void {
    if (this.parentTasksStack.pop()) {
      this.refresh();
    }
  }

  // on press sorting
  sortOn(column: string): void {
    const parent = this.getCurrentParent();
    if (!parent) {

      if (this.sortedBy.column == column) {
        this.sortedBy.isAsc = !this.sortedBy.isAsc;
      } else {
        this.sortedBy.isAsc = true;
        this.sortedBy.column = column;
      }

      this.getTasks();
    } else {
      // ignore sorting subtasks for now
    }
  }

  getCurrentParent(): TodoTask | null {
    return this.parentTasksStack.length > 0
      ? this.parentTasksStack[this.parentTasksStack.length - 1]
      : null;
  }

  refresh() {
    console.log('refresh executed');

    const parent = this.getCurrentParent();

    if (parent)
      this.taskService.getChildrenTasks(parent.id).subscribe(tasks => this.tasks = tasks);
    else
      this.getTasks();

    this.selectedTask = undefined;
    this.getParents();
  }

  getTasks(): void {
    this.taskService.getTasks(this.sortedBy.column, this.sortedBy.isAsc).subscribe(tasks => this.tasks = tasks);
  }

  // load the list for parent dropdown
  getParents(): void {
    this.taskService.getTaskParents().subscribe(parents => {
      this.parents = parents;
      this.parents.splice(0, 0, { id: undefined, summary: "---ROOT LEVEL---" });
    });
  }

  //TODO: separate todo-tasks on several components    
  //TODO: display string equivalents for status values
  //TODO: use special UI controls for numbers, dates, enum values
}
