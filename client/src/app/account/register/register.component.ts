import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, finalize, map, switchMap, take } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errors: string [] | null = null;

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  complexPassword = "(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$"

  registerForm = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email], [this.validateEmailNotTaken()]],
    password: ['', Validators.required, Validators.pattern(this.complexPassword)]
  })

  onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl('/shop'),
      error: error => this.errors = error.errors
    })
  }

  //AsyncValidatorFn = customisable validator function
  validateEmailNotTaken(): AsyncValidatorFn {

    //returns an abstract control which is the base class of a control
    return (control: AbstractControl) => {
      return control.valueChanges.pipe(
        //debounce to take a longer time before API requests, so they dont happen on every single key stroke but wait a second first
        debounceTime(1000),
        //only take the last api request that was emitted
        take(1),
        //returns an observable for the function provided in the switchMap
        switchMap(() => {
          return this.accountService.checkEmailExists(control.value).pipe(
              map(result => result ? {emailExists: true} : null),
              //updating the field to touched via the control so the user can see the email is taken without clicking out of the field
              finalize(() => control.markAsTouched())
            )
          }
        })

      )
    }
  

}
