import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TodoModel } from './todo.model';
import { TodoListComponent } from '../todo-list/todo-list.component';
import { TodoFormComponent } from '../todo-form/todo-form.component';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, TodoListComponent, TodoFormComponent],
  templateUrl: './todo.component.html',
  styleUrl: './todo.component.scss',
})
export class TodoComponent {
  todos: TodoModel[] = [
    {
      id: '1',
      title: 'Todo 1',
      description: 'This is the first todo item',
      completed: false,
    },
    {
      id: '2',
      title: 'Todo 2',
      description: 'This is the second todo item',
      completed: true,
    },
    {
      id: '3',
      title: 'Todo 3',
      description: 'This is the third todo item',
      completed: false,
    },
  ];

  addTodo(todo: TodoModel) {
    this.todos.push(
      new TodoModel(todo.id, todo.title, todo.description, todo.completed)
    );
  }

  removeTodo(id: string) {
    this.todos = this.todos.filter((todo) => todo.id !== id);
  }

  toggleCompleted(id: string) {
    this.todos = this.todos.map((todo) => {
      if (todo.id === id) {
        todo.completed = !todo.completed;
      }
      return todo;
    });
  }
}
