import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { MovieItemComponent } from './movie-item/movie-item.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { MovieRoutingModule } from './movie-routing.module';
import { MovieComponent } from './movie.component';
import { MovieDetailsComponent } from './movie-details/movie-details.component';
import { MovieItemCreateComponent } from './movie-item-create/movie-item-create.component';

@NgModule({
  declarations: [MovieListComponent, MovieItemComponent, MovieComponent, MovieDetailsComponent, MovieItemCreateComponent],
  imports: [CommonModule, MovieRoutingModule, SharedModule],
})
export class MovieModule {}
