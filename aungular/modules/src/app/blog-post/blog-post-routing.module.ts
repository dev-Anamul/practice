import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogPostCreateComponent } from './blog-post-create/blog-post-create.component';
import { BlogPostDetailsComponent } from './blog-post-details/blog-post-details.component';
import { BlogPostListComponent } from './blog-post-list/blog-post-list.component';
import { BlogPostComponent } from './blog-post.component';

const routes: Routes = [
  {
    path: '',
    component: BlogPostComponent,
    children: [
      {
        path: 'create',
        component: BlogPostCreateComponent,
      },
      {
        path: ':id',
        component: BlogPostDetailsComponent,
      },
      {
        path: '',
        component: BlogPostListComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BlogPostRoutingModule {}
