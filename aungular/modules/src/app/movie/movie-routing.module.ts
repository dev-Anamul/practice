import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MovieDetailsComponent } from './movie-details/movie-details.component';
import { MovieItemCreateComponent } from './movie-item-create/movie-item-create.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { MovieComponent } from './movie.component';

const routes: Routes = [
  {
    path: '',
    component: MovieComponent,
    children: [
      {
        path: 'create',
        component: MovieItemCreateComponent,
      },
      {
        path: ':id',
        component: MovieDetailsComponent,
      },
      {
        path: '',
        component: MovieListComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MovieRoutingModule {}
