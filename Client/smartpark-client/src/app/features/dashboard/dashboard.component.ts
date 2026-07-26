import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ParkingSpotService } from '../../core/services/parking-spot.service';
import { VehicleEntryService } from '../../core/services/vehicle-entry.service';
import { GlobalConfigService } from '../../core/services/global-config.service';
import { ParkingSpot, VehicleEntry } from '../../core/models/parking.model';
import { GlobalConfig } from '../../core/models/global-config.model';
import { SpotStatus } from '../../core/models/enums';

@Component({
  selector: 'sp-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="head">
      <div>
        <h1>{{ config()?.parkingLotName || 'SmartPark' }}</h1>
        <p>{{ config()?.address }}</p>
      </div>
      <a routerLink="/entries" class="btn-primary" style="text-decoration:none; display:inline-block;">כניסה חדשה +</a>
    </div>

    <div class="kpis">
      <div class="card kpi">
        <span class="kpi__label">סה"כ מקומות</span>
        <strong class="kpi__value">{{ totalSpots() }}</strong>
      </div>
      <div class="card kpi accent-green">
        <span class="kpi__label">פנויים כרגע</span>
        <strong class="kpi__value">{{ availableCount() }}</strong>
      </div>
      <div class="card kpi accent-red">
        <span class="kpi__label">תפוסים</span>
        <strong class="kpi__value">{{ occupiedCount() }}</strong>
      </div>
      <div class="card kpi accent-blue">
        <span class="kpi__label">רכבים בחניון</span>
        <strong class="kpi__value">{{ activeEntries().length }}</strong>
      </div>
    </div>

    <div class="card occupancy">
      <div class="occupancy__head">
        <h3>תפוסה</h3>
        <span class="mono">{{ occupancyPct() }}%</span>
      </div>
      <div class="occupancy__bar">
        <div class="occupancy__fill" [style.width.%]="occupancyPct()"></div>
      </div>
    </div>

    @if (loading()) {
      <p style="margin-top:20px;">טוען נתונים...</p>
    }
  `,
  styles: [`
    .head {
      display: flex;
      align-items: flex-end;
      justify-content: space-between;
      margin-bottom: 22px;
      flex-wrap: wrap;
      gap: 12px;
    }
    .kpis {
      display: grid;
      grid-template-columns: repeat(4, 1fr);
      gap: 16px;
      margin-bottom: 20px;
    }
    .kpi { display: flex; flex-direction: column; gap: 8px; border-top: 3px solid var(--border); }
    .kpi.accent-green { border-top-color: var(--green); }
    .kpi.accent-red { border-top-color: var(--red); }
    .kpi.accent-blue { border-top-color: var(--blue); }
    .kpi__label { font-size: 13px; color: var(--text-dim); font-weight: 600; }
    .kpi__value { font-family: var(--font-display); font-size: 34px; }
    .occupancy__head { display: flex; justify-content: space-between; align-items: center; margin-bottom: 12px; }
    .occupancy__bar { height: 12px; border-radius: 999px; background: var(--surface-2); overflow: hidden; }
    .occupancy__fill { height: 100%; background: linear-gradient(90deg, var(--green), var(--amber) 70%, var(--red)); transition: width 0.4s ease; }
    @media (max-width: 760px) {
      .kpis { grid-template-columns: repeat(2, 1fr); }
    }
  `]
})
export class DashboardComponent implements OnInit {
  private spotService = inject(ParkingSpotService);
  private entryService = inject(VehicleEntryService);
  private configService = inject(GlobalConfigService);

  loading = signal(true);
  spots = signal<ParkingSpot[]>([]);
  activeEntries = signal<VehicleEntry[]>([]);
  config = signal<GlobalConfig | null>(null);

  totalSpots = computed(() => this.spots().length);
  availableCount = computed(() => this.spots().filter(s => s.statusId === SpotStatus.Available).length);
  occupiedCount = computed(() => this.spots().filter(s => s.statusId === SpotStatus.Occupied).length);
  occupancyPct = computed(() => {
    const total = this.totalSpots();
    return total ? Math.round((this.occupiedCount() / total) * 100) : 0;
  });

  ngOnInit(): void {
    forkJoin({
      spots: this.spotService.getAll(),
      active: this.entryService.getActive(),
      config: this.configService.get()
    }).subscribe({
      next: ({ spots, active, config }) => {
        this.spots.set(spots);
        this.activeEntries.set(active);
        this.config.set(config);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}
