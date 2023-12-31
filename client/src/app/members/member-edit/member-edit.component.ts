import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm:NgForm;
member:Member;
user:User;
@HostListener('window:beforeunload',['$event']) unloadNotification($event:any){
  if(this.editForm.dirty)
  {
    $event.returnValue=true;

  }
}

 constructor(private accounrService:AccountService, private memberService:MembersService , private toaster: ToastrService){
  this.accounrService.currentUser$.pipe(take(1)).subscribe(user=>this.user= user);
 }

  ngOnInit(): void {
 this.loadmember();
  }
  loadmember(){
    this.memberService.getMember(this.user.username).subscribe(member=>{this.member=member})
  }

 updateMember(){
  this.memberService.updateMember(this.member).subscribe(()=>{
    this.toaster.success('profile Updated successfuly');
    this.editForm.reset(this.member);
  })
 }
}
