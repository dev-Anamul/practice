import { Observable, concat, interval } from "rxjs";
import { concatAll, map, shareReplay } from "rxjs/operators";

const observable = new Observable((subscriber) => {
  setInterval(() => {
    subscriber.next(Math.floor(Math.random() * 100));
  }, 1000);
});

// observable.subscribe((value) => {
//   console.log(value);
// });

const obser2 = interval(1000);

observable
  .pipe(
    map(() => obser2.pipe(map((val) => val * 10))),
    concatAll()
  )
  .subscribe((value) => {
    console.log(value);
  });
