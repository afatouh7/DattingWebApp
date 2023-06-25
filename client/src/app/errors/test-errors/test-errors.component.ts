import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseUrl='https://localhost:44366/api/';
  validationErrors: string[]=[];
  constructor(private http:HttpClient){}

  ngOnInit(): void {
  //  throw new Error('Method not implemented.');
  }
  ge404Error(){
    this.http.get(this.baseUrl+'Buggy/not-found').subscribe(response=>{
      console.log(response);

    },error=>{
      console.log(error);

    })
  }
  ge400Error(){
    this.http.get(this.baseUrl+'Buggy/bad-request').subscribe(response=>{
      console.log(response);

    },error=>{
      console.log(error);

    })
  }
  ge500Error(){
    this.http.get(this.baseUrl+'Buggy/Server-error').subscribe(response=>{
      console.log(response);

    },error=>{
      console.log(error);

    })
  }
  ge401Error(){
    this.http.get(this.baseUrl+'Buggy/auth').subscribe(response=>{
      console.log(response);

    },error=>{
      console.log(error);

    })
  }
  ge404ValidationError(){
    this.http.post(this.baseUrl+'/api/Account/register',{}).subscribe(response=>{
      console.log(response);

    },error=>{
      console.log(error);
      this.validationErrors=error;

    })
  }

}
