import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GlobalConfigService } from '../../core/services/global-config.service';
import { ToastService } from '../../core/services/toast.service';
import { GlobalConfig } from '../../core/models/global-config.model';

@Component({
  selector: 'sp-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="head">
      <h1>הגדרות חניון</h1>
      <p>פרטי החניון, קיבולת, שעות פעילות ותעריפים</p>
    </div>

    @if (loading()) {
      <p>טוען הגדרות...</p>
    } @else if (config) {
      <form (ngSubmit)="save()" class="card">
        <h3>פרטי החניון</h3>
        <div class="form-grid">
          <div class="field">
            <label for="name">שם החניון</label>
            <input id="name" name="name" [(ngModel)]="config.parkingLotName" maxlength="100" />
          </div>
          <div class="field">
            <label for="address">כתובת</label>
            <input id="address" name="address" [(ngModel)]="config.address" maxlength="200" />
          </div>
          <div class="field full">
            <label for="screenTitle">כותרת במסך הכניסה</label>
            <input id="screenTitle" name="screenTitle" [(ngModel)]="config.screenTitle" maxlength="150" />
          </div>
        </div>

        <h3 class="section">קיבולת ושעות</h3>
        <div class="form-grid">
          <div class="field">
            <label for="totalSpots">סה"כ מקומות חניה</label>
            <input id="totalSpots" name="totalSpots" type="number" [(ngModel)]="config.totalSpots" />
          </div>
          <div class="field">
            <label for="floors">מספר קומות</label>
            <input id="floors" name="floors" type="number" [(ngModel)]="config.floors" />
          </div>
          <div class="field">
            <label for="openingTime">שעת פתיחה</label>
            <input id="openingTime" name="openingTime" type="time" [(ngModel)]="openingTimeInput" />
          </div>
          <div class="field">
            <label for="closingTime">שעת סגירה</label>
            <input id="closingTime" name="closingTime" type="time" [(ngModel)]="closingTimeInput" />
          </div>
        </div>

        <h3 class="section">תעריפים</h3>
        <div class="form-grid">
          <div class="field">
            <label for="priceReg">מחיר לשעה — רגיל</label>
            <input id="priceReg" name="priceReg" type="number" step="0.1" [(ngModel)]="config.pricePerHourRegular" />
          </div>
          <div class="field">
            <label for="priceBiz">מחיר לשעה — שעות עסקים</label>
            <input id="priceBiz" name="priceBiz" type="number" step="0.1" [(ngModel)]="config.pricePerHourBusiness" />
          </div>
          <div class="field">
            <label for="bizStart">תחילת שעות עסקים</label>
            <input id="bizStart" name="bizStart" type="time" [(ngModel)]="businessStartInput" />
          </div>
          <div class="field">
            <label for="bizEnd">סוף שעות עסקים</label>
            <input id="bizEnd" name="bizEnd" type="time" [(ngModel)]="businessEndInput" />
          </div>
          <div class="field">
            <label for="currency">מטבע</label>
            <input id="currency" name="currency" [(ngModel)]="config.currency" maxlength="10" />
          </div>
          <div class="field">
            <label for="maxHours">מקסימום שעות חניה</label>
            <input id="maxHours" name="maxHours" type="number" [(ngModel)]="config.maxParkingHours" />
          </div>
        </div>

        <label class="checkbox-row">
          <input type="checkbox" name="lpr" [(ngModel)]="config.licensePlateRecognitionEnabled" style="width:auto;" />
          זיהוי לוחית רישוי אוטומטי פעיל
        </label>

        <div class="form-actions">
          <button type="submit" class="btn-primary" [disabled]="saving()">
            {{ saving() ? 'שומר...' : 'שמור הגדרות' }}
          </button>
        </div>
      </form>
    }
  `,
  styles: [`
    .head { margin-bottom: 18px; }
    .section { margin-top: 22px; margin-bottom: 4px; }
    .form-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: 14px; margin-top: 14px; }
    .field.full { grid-column: 1 / -1; }
    .checkbox-row { display: flex; align-items: center; gap: 8px; margin-top: 18px; color: var(--text); font-size: 14px; font-weight: 600; }
    .form-actions { margin-top: 22px; }
    @media (max-width: 760px) { .form-grid { grid-template-columns: 1fr; } }
  `]
})
export class SettingsComponent implements OnInit {
  private configService = inject(GlobalConfigService);
  private toast = inject(ToastService);

  loading = signal(true);
  saving = signal(false);
  config: GlobalConfig | null = null;

  openingTimeInput = '';
  closingTimeInput = '';
  businessStartInput = '';
  businessEndInput = '';

  ngOnInit(): void {
    this.configService.get().subscribe({
      next: config => {
        this.config = config;
        this.openingTimeInput = this.toTimeInput(config.openingTime);
        this.closingTimeInput = this.toTimeInput(config.closingTime);
        this.businessStartInput = this.toTimeInput(config.businessHoursStart);
        this.businessEndInput = this.toTimeInput(config.businessHoursEnd);
        this.loading.set(false);
      },
      error: () => {
        this.toast.error('שגיאה בטעינת ההגדרות');
        this.loading.set(false);
      }
    });
  }

  private toTimeInput(timeSpan: string): string {
    // ".NET" TimeSpan מגיע כ-"HH:mm:ss" — לשדה <input type="time"> צריך "HH:mm"
    return timeSpan ? timeSpan.substring(0, 5) : '';
  }

  private toTimeSpan(timeInput: string): string {
    return timeInput ? `${timeInput}:00` : '00:00:00';
  }

  save(): void {
    if (!this.config) return;
    this.saving.set(true);
    const payload: GlobalConfig = {
      ...this.config,
      openingTime: this.toTimeSpan(this.openingTimeInput),
      closingTime: this.toTimeSpan(this.closingTimeInput),
      businessHoursStart: this.toTimeSpan(this.businessStartInput),
      businessHoursEnd: this.toTimeSpan(this.businessEndInput)
    };

    this.configService.update(payload).subscribe({
      next: () => {
        this.toast.success('ההגדרות נשמרו בהצלחה');
        this.saving.set(false);
      },
      error: () => {
        this.toast.error('שגיאה בשמירת ההגדרות');
        this.saving.set(false);
      }
    });
  }
}
