import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'sp-toast',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="toast-stack">
      @for (m of toast.messages(); track m.id) {
        <div class="toast" [class]="m.kind" (click)="toast.dismiss(m.id)">
          {{ m.text }}
        </div>
      }
    </div>
  `,
  styles: [`
    .toast-stack {
      position: fixed;
      bottom: 24px;
      inset-inline-start: 24px;
      display: flex;
      flex-direction: column;
      gap: 10px;
      z-index: 1000;
    }
    .toast {
      min-width: 220px;
      max-width: 360px;
      padding: 12px 16px;
      border-radius: 10px;
      font-size: 14px;
      font-weight: 600;
      color: #14181F;
      background: #EAEAEA;
      box-shadow: 0 8px 24px rgba(0,0,0,0.35);
      cursor: pointer;
      animation: rise 0.18s ease-out;
    }
    .toast.success { background: #2BB673; color: #06140E; }
    .toast.error { background: #E4572E; color: #1A0803; }
    .toast.info { background: #3E4C63; color: #F4F6F8; }
    @keyframes rise {
      from { transform: translateY(8px); opacity: 0; }
      to { transform: translateY(0); opacity: 1; }
    }
  `]
})
export class ToastComponent {
  toast = inject(ToastService);
}
