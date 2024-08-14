import { Component } from '@angular/core';
import { SQLite } from '@ionic-native/sqlite/ngx';
import { IonApp, IonRouterOutlet } from '@ionic/angular/standalone';
import { DatabaseService } from './services/sqlite.service';
import { Platform } from '@ionic/angular';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  standalone: true,
  imports: [IonApp, IonRouterOutlet],
  providers: [SQLite, DatabaseService],
})
export class AppComponent {
  constructor(platform: Platform, databaseService: DatabaseService) {
    platform.ready().then(() => {
      databaseService
        .createDatabase()
        .then(() => {
          console.log('Database created');
        })
        .catch((error) => {
          console.log('Error creating database', error);
        });
    });
  }
}
