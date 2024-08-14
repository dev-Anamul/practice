import { concat, from } from "rxjs";
import {
  concatAll,
  exhaustAll,
  map,
  mergeAll,
  switchAll,
  switchMap,
} from "rxjs/operators";
import { ajax } from "rxjs/ajax";
import { Product } from "./product-type";

const productContainer = document.getElementById("product");

const productIds = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

const productObservable = from(productIds);

productObservable
  .pipe(
    map((id) => ajax.get<Product>(`https://dummyjson.com/products/${id}`)),
    mergeAll()
  )
  .subscribe((response) => {
    const h1 = document.createElement("h1");
    h1.textContent = response.response.title;
    productContainer?.appendChild(h1);

    console.log(response.response);
  });
