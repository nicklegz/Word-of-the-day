import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Word } from 'src/app/interfaces/word.interface';
import { LoadingService } from 'src/app/services/loading.service';
import { WordService } from 'src/app/services/word.service';

@Component({
  selector: 'app-words',
  templateUrl: './words.component.html',
  styleUrls: ['./words.component.css']
})
export class WordsComponent implements OnInit {

  words$!: Observable<Word[]>;
  loading$!: Observable<boolean>;
  viewname$!: Observable<string>;

  constructor(
    private wordService: WordService, 
    private activatedRoute: ActivatedRoute,
    private loader: LoadingService) {
  }

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.loader.show();
    this.viewname$ = this.wordService.listWordsViewName;
    this.words$ = this.wordService.listOfWords;
    this.words$.subscribe(() => this.loader.hide());
  }

}
