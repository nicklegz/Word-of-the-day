import { Component, OnInit } from '@angular/core';
import { CustomAuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(public auth0: CustomAuthService) { }

  ngOnInit(): void {
  }
}
