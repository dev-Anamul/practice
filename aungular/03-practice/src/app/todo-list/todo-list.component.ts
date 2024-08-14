import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoItemComponent } from '../todo-item/todo-item.component';
import { TodoModel } from '../todo/todo.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [TodoItemComponent, CommonModule],
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.scss',
})
export class TodoListComponent {
  @Input('todoList') todos: TodoModel[] = [];
  @Output() deleteTodo = new EventEmitter<string>();

  deleteItem(id: string) {
    this.deleteTodo.emit(id);
  }
}
