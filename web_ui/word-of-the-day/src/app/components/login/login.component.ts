import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from 'src/app/services/loading.service';

interface login{
  username: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit{

  constructor(private loader: LoadingService) {}

  loading$!: Observable<boolean>;

  loginForm: login = {
    username: ""
  }

  ngOnInit(){
    this.loading$ = this.loader.loading$;
    this.loader.hide();
  }

  onLogin(){
    if(this.loginForm.username != ""){
      this.loader.show();
      //call backend to validate username
    }

  }

}
