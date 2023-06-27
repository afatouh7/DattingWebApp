import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Messages } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
@ViewChild('membertabs',{static:true}) memberTabs:TabsetComponent;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab:TabDirective;
  messages: Messages[]=[];
  user: User;
  constructor(public presence:PresenceService, private route: ActivatedRoute,
    private messageService:MessageService , private accountservice:AccountService) {
    this.accountservice.currentUser$.pipe(take(1)).subscribe(user=> this.user==user)
     }




  ngOnInit(): void {
    this.route.data.subscribe(data=>{
      this.member=data['member'];
    })

    this.route.queryParams.subscribe(params=>{

      params['tab'] ? this.selectTabe(params['tab']): this.selectTabe(0);
    })

    this.galleryOptions=[{
   width:'500px',
   height:'500px',
   imagePercent:100,
   thumbnailsColumns:4 ,
   imageAnimation: NgxGalleryAnimation.Slide,
   preview:false
    }]

    this.galleryImages=this.getImages();
  }
 getImages():NgxGalleryImage[]{
  const imagesUrls=[];
  for(const photo of this.member.photos){
    imagesUrls.push({
      small:photo?.url,
      medium: photo?.url,
      big:photo?.url
    })
  }
  return imagesUrls;
 }

LoadMessages(){
  this.messageService.getMessageThread(this.member.username).subscribe(messages=>{
    this.messages=messages;
  })
 }
 selectTabe(tabeId:number){
  this.memberTabs.tabs[tabeId].active=true;

 }

onTabActivated(data:TabDirective){
  this.activeTab= data;
  if(this.activeTab.heading === 'messages' && this.messages.length ===0){
    this.messageService.createHubConnection(this.user , this.member.username);


  }
}
ngOnDestroy(): void {
  this.messageService.stopHubConnection();
}
}
