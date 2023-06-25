import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
members : Member[];

pagination:Pagination;
userParams:UserParams;
user:User;
genderList=[{value:'male',display:'male'},{value:'female',display:'female'}]

  constructor(private memberservice:MembersService){
    this.userParams=this.memberservice.getUserParams();

  }



  ngOnInit(): void {
    this.laodMembers();
  }


  laodMembers(){
    this.memberservice.setUserParams(this.userParams);
    this.memberservice.getMembers(this.userParams).subscribe(response=>{
      if (response && response.result) {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    });
  }




  resetFilter(){
    this.userParams= this.memberservice.restUserParams();
    this.laodMembers();
  }

  pageChanged(event:any){
    this.userParams.pageNumber=event.page;
    this.memberservice.setUserParams(this.userParams);
    this.laodMembers();
  }


}
