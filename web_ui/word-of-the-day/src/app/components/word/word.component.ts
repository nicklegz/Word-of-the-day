import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomAuthService } from 'src/app/services/auth.service';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {

  wordObject!: Observable<Word>;
  profile: any;

  constructor(private wordService: WordService, private auth0: CustomAuthService) { }

  ngOnInit(): void {
    this.wordObject = this.wordService.getWord();
  }
}