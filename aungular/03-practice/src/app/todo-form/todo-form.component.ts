import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TodoModel } from '../todo/todo.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-form.component.html',
  styleUrl: './todo-form.component.scss',
})
export class TodoFormComponent {
  newTodo: string = '';
  isEmptyTodo: boolean = false;

  @Output() addTodo = new EventEmitter<TodoModel>();

  crateTodo() {
    if (!this.newTodo) {
      this.isEmptyTodo = true;
      return;
    }
    this.addTodo.emit({
      id: Math.random().toString(36).substring(2, 9),
      title: this.newTodo,
      description: this.newTodo,
      completed: false,
    });
    this.newTodo = '';
  }

  resetEmptyTodo() {
    this.isEmptyTodo = false;
  }

  constructor() {}
}
