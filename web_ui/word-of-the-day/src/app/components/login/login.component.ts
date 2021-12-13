import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from 'src/app/services/loading.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit{

  loading$!: Observable<boolean>;
  form!: FormGroup;
  userException: string = "";
  username: string = "";

  constructor(
    private loader: LoadingService, 
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
    ) {}

  ngOnInit(){
    this.loading$ = this.loader.loading$;

    this.form = this.fb.group({
      username: ["", Validators.required]
    })

    this.loader.hide();

    this.username = localStorage.getItem("username")!;
    if(this.username != null && this.username != ""){
      this.setAuthenticatedAndNavHome();
    }
  }

  onLogin(){
    this.loader.show();
    this.auth.getUserInfo(this.form.value.username).subscribe(
      data =>{
      if(data.createUser == true){
          this.userException = "Username does not exist."
          this.loader.hide()
      }
      else{
        localStorage.setItem("username", this.form.value.username);
        this.setAuthenticatedAndNavHome();
      }
    },
      (err : HttpErrorResponse) =>{
      console.error(err);
      this.userException = `An  ${err.statusText.toLowerCase()} occured. Please try again.`
      this.loader.hide()
    })
  }

  onUserInput(){
    this.userException = "";
  }

  private setAuthenticatedAndNavHome(){
    this.auth.setIsAuthenticated(true);
    this.router.navigate(['/home'])
  }
}
