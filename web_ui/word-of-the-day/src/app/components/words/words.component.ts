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
  viewname: string = "";

  constructor(
    private wordService: WordService, 
    private activatedRoute: ActivatedRoute,
    private loader: LoadingService
    ) { }

  ngOnInit(): void {
    this.loading$ = this.loader.loading$;
    this.loader.show();
    //dynamically call backend based on listname required
    const listname = this.activatedRoute.snapshot.paramMap.get('listname')!;

    switch(listname){
      case "previously-viewed-words":
        this.words$ = this.getPreviouslyViewedWords()!;
        this.viewname = "Previously Viewed Words";
        break;
      case "liked-words":
        this.words$ = this.getLikedWords()!;
        this.viewname = "My Liked Words";
        break;
    }

    this.words$.subscribe(() => this.loader.hide());
  }

  private getPreviouslyViewedWords(): Observable<Word[]>{
    return this.wordService.getPreviouslyUsedWords();
  }

  private getLikedWords(){
     this.wordService.getLikedWords();
  }

}
