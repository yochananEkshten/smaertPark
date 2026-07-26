import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VehicleEntryService } from '../../core/services/vehicle-entry.service';
import { ToastService } from '../../core/services/toast.service';
import { VehicleEntry } from '../../core/models/parking.model';

@Component({
  selector: 'sp-vehicle-entries',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="head">
      <div>
        <h1>כניסות ויציאות</h1>
        <p>רישום כניסת רכב חדש וניהול רכבים שנמצאים כרגע בחניון</p>
      </div>
    </div>

    <div class="card entry-form">
      <h3>כניסת רכב</h3>
      <form (ngSubmit)="submitEntry()" class="entry-form__row">
        <div class="field">
          <label for="plate">מספר רישוי</label>
          <input id="plate" name="plate" class="mono" [(ngModel)]="plateNumber" placeholder="12-345-67" required />
        </div>
        <div class="field">
          <label for="driverId">מזהה נהג (אופציונלי)</label>
          <input id="driverId" name="driverId" type="number" [(ngModel)]="driverId" placeholder="לא חובה" />
        </div>
        <button type="submit" class="btn-primary" [disabled]="submitting() || !plateNumber">
          {{ submitting() ? 'רושם...' : 'רשום כניסה' }}
        </button>
      </form>
    </div>

    <div class="card table-card">
      <div class="table-card__head">
        <h3>רכבים בחניון כרגע ({{ activeEntries().length }})</h3>
        <button class="btn-ghost" (click)="reload()">רענן</button>
      </div>

      @if (loading()) {
        <p>טוען...</p>
      } @else if (!activeEntries().length) {
        <p>אין כרגע רכבים בחניון.</p>
      } @else {
        <table>
          <thead>
            <tr>
              <th>לוחית רישוי</th>
              <th>שעת כניסה</th>
              <th>מקום חניה</th>
              <th>מזוהה אוטומטית</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            @for (entry of activeEntries(); track entry.id) {
              <tr>
                <td class="mono">{{ entry.plateNumber }}</td>
                <td>{{ entry.entryTime | date:'dd/MM/yyyy HH:mm' }}</td>
                <td>{{ entry.spotId ?? '—' }}</td>
                <td>{{ entry.autoDetected ? 'כן' : 'לא' }}</td>
                <td>
                  <button class="btn-danger" [disabled]="exitingId() === entry.id" (click)="exitVehicle(entry)">
                    {{ exitingId() === entry.id ? 'מבצע יציאה...' : 'יציאה' }}
                  </button>
                </td>
              </tr>
            }
          </tbody>
        </table>
      }
    </div>
  `,
  styles: [`
    .head { margin-bottom: 18px; }
    .entry-form { margin-bottom: 20px; }
    .entry-form h3 { margin-bottom: 14px; }
    .entry-form__row { display: flex; gap: 14px; align-items: flex-end; flex-wrap: wrap; }
    .field { min-width: 180px; flex: 1; }
    .table-card__head { display: flex; justify-content: space-between; align-items: center; margin-bottom: 14px; }
  `]
})
export class VehicleEntriesComponent implements OnInit {
  private entryService = inject(VehicleEntryService);
  private toast = inject(ToastService);

  loading = signal(true);
  submitting = signal(false);
  exitingId = signal<number | null>(null);
  activeEntries = signal<VehicleEntry[]>([]);

  plateNumber = '';
  driverId: number | null = null;

  ngOnInit(): void {
    this.reload();
  }

  reload(): void {
    this.loading.set(true);
    this.entryService.getActive().subscribe({
      next: entries => {
        this.activeEntries.set(entries);
        this.loading.set(false);
      },
      error: () => {
        this.toast.error('שגיאה בטעינת הרכבים הפעילים');
        this.loading.set(false);
      }
    });
  }

  submitEntry(): void {
    if (!this.plateNumber) return;
    this.submitting.set(true);
    this.entryService.entry({
      licensePlate: this.plateNumber,
      driverId: this.driverId || null
    }).subscribe({
      next: () => {
        this.toast.success(`כניסת רכב ${this.plateNumber} נרשמה בהצלחה`);
        this.plateNumber = '';
        this.driverId = null;
        this.submitting.set(false);
        this.reload();
      },
      error: () => {
        this.toast.error('שגיאה ברישום הכניסה');
        this.submitting.set(false);
      }
    });
  }

  exitVehicle(entry: VehicleEntry): void {
    this.exitingId.set(entry.id);
    this.entryService.exit(entry.id).subscribe({
      next: payment => {
        this.toast.success(`רכב ${entry.plateNumber} יצא · חיוב: ${payment}`);
        this.exitingId.set(null);
        this.reload();
      },
      error: () => {
        this.toast.error('שגיאה ברישום היציאה');
        this.exitingId.set(null);
      }
    });
  }
}
