import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
members: Partial<Member[]>;
predicate='liked';
pagnumber=1;
pageSize=5;
currenPage=1;
pagination:Pagination;

  constructor(private memberservice:MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }
  loadLikes(){
    this.memberservice.getlike(this.predicate).subscribe(response=>{
 this.members=response;
  })

}

pageChange(event:any){
  this.pagnumber=event.page;
}
}
