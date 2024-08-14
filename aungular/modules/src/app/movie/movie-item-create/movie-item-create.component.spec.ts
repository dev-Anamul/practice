import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieItemCreateComponent } from './movie-item-create.component';

describe('MovieItemCreateComponent', () => {
  let component: MovieItemCreateComponent;
  let fixture: ComponentFixture<MovieItemCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MovieItemCreateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MovieItemCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
