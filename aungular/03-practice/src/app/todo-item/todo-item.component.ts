import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoModel } from '../todo/todo.model';

@Component({
  selector: 'app-todo-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './todo-item.component.html',
  styleUrl: './todo-item.component.scss',
})
export class TodoItemComponent {
  @Output() removeTodo = new EventEmitter<string>();

  @Input('todoItem') todo: TodoModel = {
    id: '',
    title: '',
    description: '',
    completed: false,
  };

  remove() {
    this.removeTodo.emit(this.todo.id);
  }
}
