import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {

  word$?: Observable<Word>;

  constructor(private wordService: WordService) { }

  ngOnInit(): void {
    this.word$ = this.wordService.getWordOfTheDay();
  }
}
