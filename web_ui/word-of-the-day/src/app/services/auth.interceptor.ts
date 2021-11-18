import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize, retryWhen, tap } from 'rxjs/operators';
import { LoadingService } from './loading.service';

const retryLimit = 3;
let attempt = 0;

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private loader: LoadingService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  //   request = request.clone({
  //     withCredentials: true
  // });
    return next.handle(request).pipe(
      retryWhen(errors => errors.pipe(
        tap(error =>{
          if (++attempt >= retryLimit || (error.status !== 500 && error.status !== 502)) {
            throw error;
          }
        })
      )),
      catchError(error => {     
        if (error.status === 401 || error.status === 400 || error.status === 403) {
          this.router.navigateByUrl('abort-access', { replaceUrl: true });
        } else if (error.status === 500 || error.status === 502) {
          this.router.navigateByUrl('server-error', { replaceUrl: true });
          // do not return the error
        }

        return throwError("error occured");
      }))
  }
}
