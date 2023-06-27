import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Messages } from '../_models/message';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { BehaviorSubject, map, take } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';
import { group } from '@angular/animations';
import { Group } from '../_models/Group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl=environment.apiUrl;
  hubUrl=environment.hubUrl;
  private hubConnection :HubConnection | null = null;
private meesageThreadSource= new BehaviorSubject<Messages[]>([]);
meesageThread$= this.meesageThreadSource.asObservable();

user:User;
userParams:UserParams;
paginatedResult:PaginatedResult<Messages[]> = new PaginatedResult<Messages[]>();
otherUsername:string;

constructor(private http: HttpClient) {
  this.setUser(); // Call the setUser method to initialize the user object
}

private setUser(): void {
  const user = JSON.parse(localStorage.getItem('user')!);
  if (user) {
    this.user = user;
    this.createHubConnection(this.user,this.otherUsername); // Call the createHubConnection method to create the hub connection
  }
}

//  createHubConnections(): void {
//   if (this.user?.token) { // Add null check to make sure that the token property is not undefined
//     this.hubConnection = new HubConnectionBuilder()
//       .withUrl(this.hubUrl + 'message?user=' + this.otherUsername, {
//         accessTokenFactory: () => this.user.token
//       }).withAutomaticReconnect().build()

//       this.hubConnection.start().catch(error=>console.log());

//       this.hubConnection.on('ReciveMessageThread',messages=>{
//     this.meesageThreadSource.next(messages);
//   });
//   this.hubConnection.on('NewMessage', message=>{

//     this.meesageThread$.pipe(take(1)).subscribe(messages=>{
//       const updateMessages=[...messages, message];

//       this.meesageThreadSource.next(updateMessages);
//     })
//   })


    // Rest of the code for configuring the hub connection and handling events
  // }
//  }
createHubConnection(user:User, otherUsername:string){
  this.hubConnection= new HubConnectionBuilder().withUrl(this.hubUrl+'message?user='+otherUsername,{
    accessTokenFactory:()=>this.user.token
  }).withAutomaticReconnect().build()

  this.hubConnection.start().catch(error=>console.log());
  this.hubConnection.on('ReciveMessageThread',messages=>{
    this.meesageThreadSource.next(messages);
  })
  this.hubConnection.on('NewMessage',message=>{
    this.meesageThread$.pipe(take(1)).subscribe(messages=>{
      this.meesageThreadSource.next([...messages,message])
    })
  })

  this.hubConnection.on('UpdatedGroup',(group:Group)=>{
    if(group.connections.some(x=>x.username===otherUsername)){
      this.meesageThread$.pipe(take(1)).subscribe(messages=>{
        messages.forEach(message=>{
          if(!message.dateRead){
            message.dateRead= new Date(Date.now())
          }
        })
        this.meesageThreadSource.next([...messages])
      })
    }

  })
}
private startConnection(): void {
  this.hubConnection?.start().catch(error => console.error(error));
}

stopHubConnection(){
  if(this.hubConnection){
    this.hubConnection.stop();
  }

}


  getMessages(pageNumber,pageSize,container){
    let params= this.getPaginationHeaders(pageNumber,pageSize);
    params= params.append('Container',container);
    return this.getPaginatedResult<Messages[]>(this.baseUrl+'Messages',params);


  }



  private getPaginatedResult<T>(url, params) {
    const paginatedResult:PaginatedResult<Messages[]> = new PaginatedResult<Messages[]>();

    return this.http.get<Messages[]>(url,{ observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult;
      })

    );
  }

  getMessageThread(username : String){
    return this.http.get<Messages[]>(this.baseUrl+'messages/thread/' + username);
  }


  // sendMessage(username:string , content:string): Promise<void>{

  //   return  this.hubConnection.start().then(()=>this.hubConnection.invoke('SendMessage',{recipientsUsername: username, content:content}))
  //   .catch(error=>console.log(error));

  //  }

  sendMessage(username: string, content: string): Promise<void> {
    if (this.hubConnection) {
      return this.hubConnection.invoke('SendMessage', { recipientsUsername: username, content: content })
        .catch(error => console.error(error));
    } else {
      return Promise.reject('Hub connection is not initialized');
    }
  }

 private getPaginationHeaders(pageNumber: number, pageSize:number){
  let params= new HttpParams();
  if (pageNumber !== undefined) {
    params=params.append('pageNumber',pageNumber.toString());

  }
  if (pageSize !== undefined) {
    params= params.append('pageSize', pageSize.toString());

  }
   return params;

 }




 deleteMessage(id:number){
  return this.http.delete(this.baseUrl+'Messages/'+id)
 }
}


