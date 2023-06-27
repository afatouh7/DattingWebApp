import { Component, OnInit,Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

@Output() cancelRegister= new EventEmitter();

  registerForm: FormGroup;
 validationErrors:string[]=[];
  constructor(private accountService:AccountService, private toastr:ToastrService) { }

  ngOnInit(): void {
  this.initializeForm();
  }

  initializeForm(){
    this.registerForm= new FormGroup({
      gender: new FormControl('male'),
      username: new FormControl('',Validators.required),
      dateOfBirth: new FormControl('',Validators.required),
      city: new FormControl('',Validators.required),
      country: new FormControl('',Validators.required),
      KnownAs: new FormControl('',Validators.required),

      password: new FormControl('',[Validators.required, Validators.minLength(4),Validators.maxLength(8)]),
      confirmPassword: new FormControl('',[Validators.required,this.matchValue('password')])
    })
    this.registerForm.controls['password'].valueChanges.subscribe(()=>{
      this.registerForm.controls['confirmPassword'].updateValueAndValidity();
    })
  }

  matchValue(matchto: string):ValidatorFn{
    return(control:AbstractControl)=>{
      return control?.value ===  control?.parent?.controls[matchto].value ? null:{isMatching: true}
    }
  }

  register(){

     this.accountService.register(this.registerForm.value).subscribe(response=>{
      console.log(response);
      this.cancel();
     },error=>{

      this.validationErrors=error;
      this.toastr.error(error.error);


     })

  }
  cancel(){
    console.log('cancelled');
    this.cancelRegister.emit(false);

  }
}
