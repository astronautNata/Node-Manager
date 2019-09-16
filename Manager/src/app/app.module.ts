import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NetworkComponent } from '../network/network.component';
import { NetworkService } from 'src/_services/network.service';
import { NewNodeComponent } from 'src/newNode/newNode.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
   declarations: [
      AppComponent,
      NetworkComponent,
      NewNodeComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule
   ],
   providers: [
      NetworkService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
