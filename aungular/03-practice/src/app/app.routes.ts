import { Routes } from '@angular/router';
import { TodoComponent } from './todo/todo.component';

export const routes: Routes = [
  {
    path: '',
    component: TodoComponent,
  },
  //   {
  //     path: 'about',
  //     loadChildren: './about/about.module#AboutModule',
  //   },
  //   {
  //     path: 'contact',
  //     loadChildren: './contact/contact.module#ContactModule',
  //   },
  {
    path: '**',
    redirectTo: '',
  },
];
