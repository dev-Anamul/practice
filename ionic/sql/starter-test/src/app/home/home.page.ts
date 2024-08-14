import { Component } from '@angular/core';
import {
  IonHeader,
  IonToolbar,
  IonTitle,
  IonContent,
  IonButton,
  IonItem,
  IonList,
  IonLabel,
} from '@ionic/angular/standalone';
import { DatabaseService } from '../services/sqlite.service';
import { CommonModule } from '@angular/common';
import { SQLite } from '@ionic-native/sqlite/ngx';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
  standalone: true,
  imports: [
    IonLabel,
    IonList,
    IonItem,
    IonButton,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    CommonModule,
  ],
  providers: [SQLite, DatabaseService],
})
export class HomePage {
  user: any;
  users: any[] | undefined = [];

  constructor(private sqliteService: DatabaseService) {}

  async addUsers() {
    try {
      console.log('Adding users');

      await this.sqliteService.addCategory('Category 1');
      await this.sqliteService.addCategory('Category 2');
      await this.sqliteService.addCategory('Category 3');
    } catch (error) {
      console.log('Error adding users', error);
    }
  }

  async getUsers() {
    try {
      const users = await this.sqliteService.getCategories();
      console.log('Users:', users);
      this.users = users;
    } catch (error) {
      console.log('Error getting users', error);
    }
  }
}
