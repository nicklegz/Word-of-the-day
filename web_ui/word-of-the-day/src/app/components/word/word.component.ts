import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from 'src/app/services/loading.service';
import { Word } from '../../interfaces/word.interface';
import { WordService } from '../../services/word.service';

@Component({
  selector: 'app-word',
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css']
})
export class WordComponent implements OnInit {
  word$!: Observable<Word>;
  loading$!: Observable<boolean>;

  constructor(
    private wordService: WordService,
    private loader: LoadingService) 
    {}

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.word$ = this.wordService.getWordOfTheDay();
    this.word$.subscribe(() => this.loader.hide())
  }
}
