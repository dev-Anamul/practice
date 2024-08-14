import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ProductItemComponent } from './product-item/product-item.component';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product.component';

@NgModule({
  declarations: [ProductListComponent, ProductItemComponent, ProductComponent],
  imports: [CommonModule, ProductRoutingModule],
  bootstrap: [ProductComponent],
})
export class ProductModule {}
