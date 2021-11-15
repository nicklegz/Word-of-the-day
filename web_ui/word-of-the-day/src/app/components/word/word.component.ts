import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {

  word$?: Observable<Word>;

  constructor(private wordService: WordService, private auth: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.auth.getUserInfo().subscribe(data =>{
      if(data.createUser == true){
        this.auth.createUser();
      }
    })

    this.word$ = this.wordService.getWordOfTheDay();
  }
}
