import { Injectable } from '@angular/core';
import { TodoTask } from './todoTask';
import { catchError, from, map, Observable, of, tap } from 'rxjs';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import axios, { AxiosResponse } from 'axios';
import { TodoTaskParent } from './todoTaskParent';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private tasksUrl = 'http://localhost:5034/TodoTask';  //TODO: take it from configuration

  httpHeaders = {
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*'
  };

  constructor(private http: HttpClient, private messageService: MessageService) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }

  getTasks(column?: string, isAsc?: boolean): Observable<TodoTask[]> {

    const suffix = column ? `?sortBy=${column}&isAsc=${isAsc}` : '';

    const promise = axios
      .get<TodoTask[]>(`${this.tasksUrl}${suffix}`)
      .then(response => {
        this.logResponse(response);
        this.log('fetched root tasks');
        return response.data;
      })
      .catch(error => {
        this.log(`getTasks failed: ${error}`);
        return [] as TodoTask[];
      });

    return from(promise);
  }

  getTaskParents(): Observable<TodoTaskParent[]> {
    const promise = axios
      .get<TodoTaskParent[]>(this.tasksUrl + '/getAllShort')
      .then(response => {
        this.logResponse(response);
        this.log('fetched parent dropdown values');
        return response.data;
      })
      .catch(error => {
        this.log(`getTaskParents failed: ${error}`);
        return [] as TodoTaskParent[];
      });

    return from(promise);
  }

  getChildrenTasks(id: number): Observable<TodoTask[]> {

    const promise = axios
      .get<TodoTask[]>(`${this.tasksUrl}/getChildren/${id}`)
      .then(response => {
        this.logResponse(response);
        this.log('fetched child tasks');
        return response.data;
      })
      .catch(error => {
        this.log(`getChildrenTasks failed: ${error}`);
        return [] as TodoTask[];
      });

    return from(promise);
  }

  addTask(task: TodoTask): any {
    console.log(task);

    return axios
      .post(this.tasksUrl, task, { headers: this.httpHeaders })
      .then(response => {
        this.logResponse(response);
        this.log(`added task with id = ${response.data} `);
        task.id = response.data;
      })
      .catch(error => this.log(`addTask failed: ${error}`));
  }

  deleteTask(id: number): any {
    return axios
      .delete(`${this.tasksUrl}/${id}`, { headers: this.httpHeaders })
      .then(response => {
        this.logResponse(response);
        this.log(`deleted task with id=${id}`);
      })
      .catch(error => this.log(`deleteTask failed: ${error}`));
  }

  updateTask(task: TodoTask): any {
    console.log(task);

    return axios
      .put(this.tasksUrl, task, { headers: this.httpHeaders })
      .then(response => {
        this.logResponse(response);
        this.log(`updated task with id=${task.id}`);
      })
      .catch(error => this.log(`updateTask failed: ${error}`));
  }

  setNewParent(id: number, parentId?: number): any {
    if (parentId)
      return axios
        .patch(`${this.tasksUrl}makeChild/${id}/${parentId}`, { headers: this.httpHeaders })
        .then(response => {
          this.logResponse(response);
          this.log(`moved task with id=${id} under parent ${parentId}`);
        })
        .catch(error => this.log(`setNewParent failed: ${error}`))
    else
      return axios
        .patch(`${this.tasksUrl}makeRoot/${id}`, { headers: this.httpHeaders })
        .then(response => {
          this.logResponse(response);
          this.log(`moved task with id=${id} to root level`);
        })
        .catch(error => this.log(`setNewParent failed: ${error}`));
  }

  private log(message: string) {
    this.messageService.add(message);
  }

  private logResponse(response: AxiosResponse<any, any>) {
    console.log(response.data);
    console.log(response);
    const message = `Request URL: ${response.request.responseURL}, Status: ${response.status}, headers: ${response.headers}, data: ${response.request.responseText}`;
    this.messageService.add(message);
  }
}
