import { Component, OnInit } from '@angular/core';
import { UserInfo } from './interfaces/userAuth.interface';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'word-of-the-day';
  userInfo!: UserInfo;

  constructor(private auth: AuthService){}

  ngOnInit(): void{
    this.auth.username = localStorage.getItem("username")!;
  }
}