import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { concatMap } from 'rxjs/operators';
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
  liked$!: Observable<boolean>;
  isAuthenticated!: Observable<boolean>;

  constructor(
    private wordService: WordService,
    private loader: LoadingService,
    private auth: AuthService){
      this.isAuthenticated =  this.auth.isAuthenticated$;
    }

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.loader.show();
    this.word$ = this.wordService.getWordOfTheDay();

    this.word$.subscribe((word) => {
      this.wordService.getIsLikedWordOfTheDay(word.WordId).subscribe(isLiked =>{
        this.wordService.setIsLikedWordOfTheDay(isLiked);
        this.liked$ = this.wordService.isLikedWordOfTheDay$;
        this.liked$.subscribe(() => this.loader.hide());
      })
    });
  }

  onClickLike(){
    const liked = this.wordService.isLikedWordOfTheDay.value;
    this.wordService.setIsLikedWordOfTheDay(!this.wordService.isLikedWordOfTheDay.value);

    if(liked == true){
      //delete liked word from database
      this.word$.pipe(
        concatMap((word) =>
          this.wordService.deleteLikedWord(word.WordId)))
        .subscribe()
      return;
    }

    //add liked word to database
    this.word$.pipe(
      concatMap((word) =>
      this.wordService.addLikedWord(word.WordId))).subscribe()
  }
}
