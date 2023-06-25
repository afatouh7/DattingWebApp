import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountservice:AccountService, private toastr: ToastrService){}

  canActivate(): Observable<boolean >{

    return this.accountservice.currentUser$.pipe(
      map(user => {
        if(user){ return true;}
        this.toastr.error('You Shall not Pass!')
        return false;
      })
    );
  }

}
