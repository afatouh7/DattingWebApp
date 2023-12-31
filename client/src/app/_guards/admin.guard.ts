import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService:AccountService, private toaster:ToastrService){}


  canActivate(): Observable<boolean> {

    return  this.accountService.currentUser$.pipe(
      map(user =>{
        if(user.roles.includes('Admin')||user.roles.includes('Moderator')){
          return true;
        }
        this.toaster.error('You cannot enter this area');
        return false
      })
    )
  }

}
