import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
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
  username: string = "";
  liked: boolean = false;

  constructor(
    private wordService: WordService,
    private loader: LoadingService,
    private auth: AuthService) 
    {}

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.word$ = this.wordService.getWordOfTheDay();
    this.word$.subscribe(() => this.loader.hide())
  }

  onClickLike(){
    this.liked = !this.liked;
  }
}
