import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable} from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { WordService } from 'src/app/services/word.service';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.css']
})
export class TopNavComponent implements OnInit{

  isAuthenticated$!: Observable<boolean>;

  constructor(
    private auth: AuthService,
    private router: Router,
    private wordService: WordService) { 
  }

  ngOnInit(){
    this.isAuthenticated$ = this.auth.isAuthenticated$;
  }

  onClickPreviousWords(){
    this.wordService.getPreviouslyUsedWords().subscribe(words => this.wordService.setListOfWords(words));
    this.wordService.setListWordsViewName("Previously Viewed Words");
    this.router.navigate(['/words/previously-viewed-words']);
  }

  onClickLikedWords(){
    this.wordService.getLikedWords().subscribe(words => this.wordService.setListOfWords(words));
    this.wordService.setListWordsViewName("Liked Words");
    this.router.navigate(['/words/liked-words']);
  }

  signOut(){
    this.auth.removeUsername();
    this.auth.signOut();
  }
}
