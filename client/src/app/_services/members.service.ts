import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, Pipe } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';




@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl=environment.apiUrl;
  members:Member[]=[];
  memberCache= new Map();
user: User;
userParams:UserParams;
  paginatedResult:PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private http :HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams= new UserParams(user);
    })
  }

  getUserParams(){
    return this.userParams;
  }

  setUserParams(params:UserParams){
    this.userParams= params;
  }


  restUserParams(){
    this.userParams= new UserParams(this.user);
    return this.userParams;
   }
  getMembers(UserParams: UserParams) {

  var response= this.memberCache.get(Object.values(UserParams).join('-'));
  if(response){
    return of(response);
  }
    let params = this.getPaginationHeaders(UserParams.pageNumber, UserParams.pageSize);
    if (UserParams.minAge !== undefined) {
    params = params.append('minAge', UserParams.minAge.toString());
  }
    if (UserParams.maxAge !== undefined) {
      params = params.append('maxAge', UserParams.maxAge.toString());
    }
    if (UserParams.gender !== undefined) {
    params = params.append('gender', UserParams.gender);
  }
  params = params.append('orderBy', UserParams.orderBy);
    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)
    .pipe(map(response=>{
      this.memberCache.set(Object.values(UserParams).join('-'),response);
    }))
  }


  getMember(username) {
     const member=this.members.find(x=>x.username ===username);
     if(member !== undefined)return of(member);
    // const member=[...this.memberCache.values()]
    // .reduce((arr, elem)=>arr.concat(elem.reset),[])
    // .find((member : Member)=>member.username==username);
    // console.log(member);

    return this.http.get<Member>(this.baseUrl+'USers/'+username );
  }


  updateMember(member :Member){
   return this.http.put(this.baseUrl +'USers',member).pipe(
    map(()=>{
      const index= this.members.indexOf(member);
      this.members[index]= member;
    })
   )

  }

  setMianPhoto(photoId:number){
    return this.http.put(this.baseUrl+'USers/set-main-photo/'+photoId,{});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl +'users/delet-photo/' +photoId);
  }
  addLike(username:string){
    return this.http.post(this.baseUrl+'likes/'+username,{})
  }


  getlike(predicate:string){
    //let params= this.getPaginationHeaders(pagenumber,pageSize);
    //params= params.append('predicate',predicate);
    return this.http.get<Partial<Member[]>>(this.baseUrl+'likes?predicate='+predicate)
  }




  private getPaginatedResult<T>(url, params) {
    const paginatedResult:PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

    return this.http.get<Member[]>(this.baseUrl + 'USers', { observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResult;
      })

    );
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

}
