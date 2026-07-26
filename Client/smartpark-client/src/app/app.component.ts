import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { ToastComponent } from './shared/components/toast/toast.component';

@Component({
  selector: 'sp-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent, ToastComponent],
  template: `
    <sp-navbar></sp-navbar>
    <main class="page">
      <router-outlet></router-outlet>
    </main>
    <sp-toast></sp-toast>
  `,
  styles: [`
    .page {
      max-width: 1180px;
      margin: 0 auto;
      padding: 28px 24px 60px;
    }
  `]
})
export class AppComponent {}
