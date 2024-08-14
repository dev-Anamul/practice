import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogPostComponent } from './blog-post/blog-post.component';

const routes: Routes = [
  {
    path: 'movies',
    loadChildren: () =>
      import('./movie/movie.module').then((m) => m.MovieModule),
  },
  {
    path: 'products',
    loadChildren: () =>
      import('./product/product.module').then((m) => m.ProductModule),
  },
  {
    path: 'blog-posts',
    component: BlogPostComponent,
    loadChildren: () =>
      import('./blog-post/blog-post.module').then((m) => m.BlogPostModule),
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'blog-posts',
  },
  {
    path: '**',
    redirectTo: 'movies',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
