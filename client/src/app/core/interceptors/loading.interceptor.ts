import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    //takes off the loading interceptor when we are querying if the email already exists when the user signs up
    //this is to prevent the loading spinner appearing every time someone presses a character in the email field
    if(!request.url.includes('emailExists') ||
    request.method === 'POST' && request.url.includes('orders') ||
    request.method === 'DELETE') {
      return next.handle(request);
    }

    this.busyService.busy();
    return next.handle(request).pipe(
      delay(1000),
      finalize(() => this.busyService.idle())
    );
  };
}
