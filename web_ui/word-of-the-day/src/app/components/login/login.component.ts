import { Component, OnInit } from '@angular/core';
import { CustomAuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public auth0: CustomAuthService) { }

  ngOnInit(): void {
    this.auth0.login();
  }
}
