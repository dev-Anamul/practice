import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blog-post-create',
  templateUrl: './blog-post-create.component.html',
  styleUrl: './blog-post-create.component.scss',
})
export class BlogPostCreateComponent {
  constructor(private router: Router) {}

  handleCancelButtonClick(): void {
    this.router.navigate(['/blog-posts']);
  }
}
