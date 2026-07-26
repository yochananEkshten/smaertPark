import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DriverService } from '../../core/services/driver.service';
import { ToastService } from '../../core/services/toast.service';
import { Driver } from '../../core/models/driver.model';

type DriverForm = Omit<Driver, 'id' | 'recordStatus' | 'createdAt'> & { id: number | null };

const EMPTY_FORM: DriverForm = { id: null, firstName: '', lastName: '', phone: '', licensePlate: '' };

@Component({
  selector: 'sp-drivers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="head">
      <div>
        <h1>נהגים</h1>
        <p>ניהול רשימת הנהגים הרשומים במערכת</p>
      </div>
      <button class="btn-primary" (click)="openNew()">נהג חדש +</button>
    </div>

    @if (showForm()) {
      <div class="card form-card">
        <h3>{{ form.id ? 'עריכת נהג' : 'נהג חדש' }}</h3>
        <form (ngSubmit)="save()" class="form-grid">
          <div class="field">
            <label for="firstName">שם פרטי</label>
            <input id="firstName" name="firstName" [(ngModel)]="form.firstName" required maxlength="45" />
          </div>
          <div class="field">
            <label for="lastName">שם משפחה</label>
            <input id="lastName" name="lastName" [(ngModel)]="form.lastName" required maxlength="45" />
          </div>
          <div class="field">
            <label for="phone">טלפון</label>
            <input id="phone" name="phone" [(ngModel)]="form.phone" maxlength="20" />
          </div>
          <div class="field">
            <label for="licensePlate">לוחית רישוי</label>
            <input id="licensePlate" name="licensePlate" class="mono" [(ngModel)]="form.licensePlate" required maxlength="20" placeholder="12-345-67" />
          </div>
          <div class="form-actions">
            <button type="submit" class="btn-primary" [disabled]="saving()">
              {{ saving() ? 'שומר...' : 'שמור' }}
            </button>
            <button type="button" class="btn-ghost" (click)="closeForm()">ביטול</button>
          </div>
        </form>
      </div>
    }

    <div class="card table-card">
      @if (loading()) {
        <p>טוען נהגים...</p>
      } @else if (!drivers().length) {
        <p>אין נהגים רשומים עדיין.</p>
      } @else {
        <table>
          <thead>
            <tr>
              <th>שם</th>
              <th>טלפון</th>
              <th>לוחית רישוי</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            @for (d of drivers(); track d.id) {
              <tr>
                <td>{{ d.firstName }} {{ d.lastName }}</td>
                <td>{{ d.phone || '—' }}</td>
                <td class="mono">{{ d.licensePlate }}</td>
                <td class="row-actions">
                  <button class="btn-ghost" (click)="openEdit(d)">עריכה</button>
                  <button class="btn-danger" [disabled]="deletingId() === d.id" (click)="remove(d)">
                    {{ deletingId() === d.id ? 'מוחק...' : 'מחיקה' }}
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
    .head { display: flex; justify-content: space-between; align-items: flex-end; margin-bottom: 18px; flex-wrap: wrap; gap: 12px; }
    .form-card { margin-bottom: 20px; }
    .form-card h3 { margin-bottom: 14px; }
    .form-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 14px; }
    .form-actions { grid-column: 1 / -1; display: flex; gap: 10px; margin-top: 4px; }
    .row-actions { display: flex; gap: 8px; justify-content: flex-end; }
    @media (max-width: 600px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class DriversComponent implements OnInit {
  private driverService = inject(DriverService);
  private toast = inject(ToastService);

  loading = signal(true);
  saving = signal(false);
  deletingId = signal<number | null>(null);
  showForm = signal(false);
  drivers = signal<Driver[]>([]);

  form: DriverForm = { ...EMPTY_FORM };
  private editingOriginal: Driver | null = null;

  ngOnInit(): void {
    this.reload();
  }

  reload(): void {
    this.loading.set(true);
    this.driverService.getAll().subscribe({
      next: drivers => {
        this.drivers.set(drivers);
        this.loading.set(false);
      },
      error: () => {
        this.toast.error('שגיאה בטעינת רשימת הנהגים');
        this.loading.set(false);
      }
    });
  }

  openNew(): void {
    this.editingOriginal = null;
    this.form = { ...EMPTY_FORM };
    this.showForm.set(true);
  }

  openEdit(driver: Driver): void {
    this.editingOriginal = driver;
    this.form = {
      id: driver.id,
      firstName: driver.firstName,
      lastName: driver.lastName,
      phone: driver.phone,
      licensePlate: driver.licensePlate
    };
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
  }

  save(): void {
    this.saving.set(true);
    const payload = {
      firstName: this.form.firstName,
      lastName: this.form.lastName,
      phone: this.form.phone,
      licensePlate: this.form.licensePlate
    };
  
    const onSuccess = () => {
      this.toast.success(this.form.id ? 'הנהג עודכן בהצלחה' : 'הנהג נוסף בהצלחה');
      this.saving.set(false);
      this.showForm.set(false);
      this.reload();
    };
    const onError = () => {
      this.toast.error('שגיאה בשמירת הנהג');
      this.saving.set(false);
    };
  
    if (this.form.id && this.editingOriginal) {
      this.driverService
        .update({ ...this.editingOriginal, ...payload, id: this.form.id })
        .subscribe({ next: onSuccess, error: onError });
    } else {
      this.driverService.add(payload).subscribe({ next: onSuccess, error: onError });
    }
  }

  remove(driver: Driver): void {
    if (!confirm(`למחוק את ${driver.firstName} ${driver.lastName}?`)) return;
    this.deletingId.set(driver.id);
    this.driverService.delete(driver.id).subscribe({
      next: () => {
        this.toast.success('הנהג נמחק');
        this.deletingId.set(null);
        this.reload();
      },
      error: () => {
        this.toast.error('שגיאה במחיקת הנהג');
        this.deletingId.set(null);
      }
    });
  }
}
