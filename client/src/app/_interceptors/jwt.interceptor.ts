import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor( private acountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentuser : User;
    this.acountService.currentUser$.pipe(take(1)).subscribe(user=>currentuser=user);
    if (currentuser){
      request = request.clone({
        setHeaders:{
          Authorization:'Bearer ' +currentuser.token
        }
      })
    }
    return next.handle(request);
  }
}
