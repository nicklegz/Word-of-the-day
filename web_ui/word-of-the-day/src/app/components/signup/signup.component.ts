import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { LoadingService } from 'src/app/services/loading.service';
import { WordService } from 'src/app/services/word.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  loading$!: Observable<boolean>;
  form!: FormGroup;
  userException: string = "";

  constructor(
    private loader: LoadingService,
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.form = this.fb.group({
      username: ["", Validators.required],
      email: ["", Validators.required]
    })

  }

  onSignUp(){
    this.loader.show();
    this.auth.getUserInfo(this.form.value.username).subscribe(
      data =>{
      if(data.createUser == false){
          this.userException = "Username already exist. Please try again."
          this.loader.hide()
          return;
        }

      this.auth.createUser(this.form.value.username).subscribe(()=>{
          this.auth.setIsAuthenticated(true);
          this.auth.setUsername(this.form.value.username);
          localStorage.setItem("username", this.form.value.username);
          this.loader.hide();
          this.router.navigate(['home']);
        })
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

}
