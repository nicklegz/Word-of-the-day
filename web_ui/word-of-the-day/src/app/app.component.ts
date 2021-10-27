import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CustomAuthService } from './services/auth.service';

interface Authenticated{
  isAuthenticated: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'word-of-the-day';

  constructor(public auth0: CustomAuthService, private http: HttpClient, public router: Router){}

  ngOnInit(): void{

    // this.auth0.handleAuthentication();
    this.getAuthenticated();
  }

  getAuthenticated(){
    this.http.get<Authenticated>("https://localhost:5001/api/auth/user").subscribe(data =>{
      if(data.isAuthenticated == false){
        window.location.href = "https://localhost:5001/api/auth/login";
      }
    })
  }
}
