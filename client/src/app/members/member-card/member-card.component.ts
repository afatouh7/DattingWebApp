import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

@Input() member:Member;

  constructor(private memberservice:MembersService, private toaster:ToastrService, public presence:PresenceService){}
  ngOnInit(): void {

  }

 addlike(member:Member){
  this.memberservice.addLike(member.username).subscribe(()=>{
    this.toaster.success('you have liked' +member.knownAs);
 })

}
}
