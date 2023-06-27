import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';
import { User } from '../_models/user';

@Directive({
  selector: '[appHasRoledDirective]'
})
export class HasRoledDirectiveDirective implements OnInit {
  @Input() appHasRoledDirective:string[];
  user:User;

  constructor(private viewContainerRef:ViewContainerRef
    , private templeteRef:TemplateRef<any>, private accountService:AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      })
    }
  ngOnInit(): void {
    if(!this.user?.roles ||this.user== null){
      this.viewContainerRef.clear();
      return;
    }

    if(this.user?.roles.some(r=>this.appHasRoledDirective.includes(r))){
      this.viewContainerRef.createEmbeddedView(this.templeteRef);
    }else{
      this.viewContainerRef.clear();
    }
  }

}
