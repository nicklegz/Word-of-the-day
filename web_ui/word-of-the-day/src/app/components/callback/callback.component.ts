import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(private http: HttpClient, private auth: AuthService) { }

  ngOnInit(): void {
    this.auth.getUserInfo().subscribe(data => {
      if(data.isAuthenticated == false){
        
      }

      if(data.createUser == true){
        this.auth.createUser();
      }

    })

    window.location.href = "http://localhost:4200/home";

  }
}
