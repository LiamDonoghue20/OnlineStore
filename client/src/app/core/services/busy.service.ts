import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCounter = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  busy() {
    this.busyRequestCounter++;
    this.spinnerService.show(undefined, {
      type: 'timer',
      bdColor: 'rgba(255,255,255,0.7)',
      color: '#333333'
    })
  }

  idle(){
    this.busyRequestCounter--;
    if(this.busyRequestCounter <= 0){
      this.busyRequestCounter = 0;
      this.spinnerService.hide();
    }
  }
}
