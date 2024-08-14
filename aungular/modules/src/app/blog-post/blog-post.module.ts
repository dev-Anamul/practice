import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { BlogPostItemComponent } from './blog-post-item/blog-post-item.component';
import { BlogPostListComponent } from './blog-post-list/blog-post-list.component';
import { BlogPostRoutingModule } from './blog-post-routing.module';
import { BlogPostComponent } from './blog-post.component';
import { BlogPostDetailsComponent } from './blog-post-details/blog-post-details.component';
import { BlogPostCreateComponent } from './blog-post-create/blog-post-create.component';

@NgModule({
  declarations: [
    BlogPostListComponent,
    BlogPostItemComponent,
    BlogPostComponent,
    BlogPostDetailsComponent,
    BlogPostCreateComponent,
  ],
  imports: [CommonModule, BlogPostRoutingModule, SharedModule],
  // bootstrap: [BlogPostComponent],
})
export class BlogPostModule {}
