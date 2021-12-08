import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from 'src/app/services/loading.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit{

  loading$!: Observable<boolean>;
  form: any;

  constructor(private loader: LoadingService, private fb: FormBuilder) {}

  ngOnInit(){
    this.loading$ = this.loader.loading$;

    this.form! = this.fb.group({
      "username": ["", Validators.required]
    })

    this.loader.hide();
  }

  onLogin(){
    if(this.form.username != ""){
      this.loader.show();
      //call backend to validate username
    }

  }
}
