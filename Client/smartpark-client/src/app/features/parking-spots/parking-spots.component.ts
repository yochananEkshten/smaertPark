import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParkingSpotService } from '../../core/services/parking-spot.service';
import { ParkingSpot } from '../../core/models/parking.model';
import { SpotStatus, SpotStatusLabel } from '../../core/models/enums';

@Component({
  selector: 'sp-parking-spots',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="head">
      <div>
        <h1>מפת חניה</h1>
        <p>תצוגה חיה של כל מקומות החניה לפי קומה</p>
      </div>
      <div class="legend">
        <span class="legend__item"><i class="dot available"></i> פנוי</span>
        <span class="legend__item"><i class="dot occupied"></i> תפוס</span>
        <span class="legend__item"><i class="dot disabled"></i> מושבת</span>
      </div>
    </div>

    @if (loading()) {
      <p>טוען מפה...</p>
    } @else {
      <div class="floor-tabs">
        @for (floor of floors(); track floor) {
          <button
            class="floor-tab"
            [class.active]="selectedFloor() === floor"
            (click)="selectedFloor.set(floor)">
            קומה {{ floor }}
          </button>
        }
      </div>

      <div class="grid">
        @for (spot of spotsOnFloor(); track spot.id) {
          <div class="spot" [class]="statusClass(spot.statusId)" [title]="tooltip(spot)">
            <span class="spot__number mono">{{ spot.spotNumber }}</span>
            <span class="spot__status">{{ statusLabel(spot.statusId) }}</span>
            @if (spot.vehicle?.plateNumber) {
              <span class="spot__plate mono">{{ spot.vehicle?.plateNumber }}</span>
            }
          </div>
        }
        @if (!spotsOnFloor().length) {
          <p>אין מקומות חניה מוגדרים בקומה זו.</p>
        }
      </div>
    }
  `,
  styles: [`
    .head { display: flex; justify-content: space-between; align-items: flex-end; margin-bottom: 18px; flex-wrap: wrap; gap: 12px; }
    .legend { display: flex; gap: 16px; }
    .legend__item { display: flex; align-items: center; gap: 6px; font-size: 13px; color: var(--text-dim); }
    .dot { width: 10px; height: 10px; border-radius: 50%; display: inline-block; }
    .dot.available { background: var(--green); }
    .dot.occupied { background: var(--red); }
    .dot.disabled { background: var(--text-dim); }

    .floor-tabs { display: flex; gap: 8px; margin-bottom: 18px; }
    .floor-tab {
      background: var(--surface);
      border: 1px solid var(--border);
      color: var(--text-dim);
      padding: 8px 16px;
      border-radius: 8px;
      font-size: 13px;
    }
    .floor-tab.active { background: var(--amber); color: #14181F; border-color: var(--amber); }

    .grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
      gap: 12px;
    }
    .spot {
      position: relative;
      border-radius: 10px;
      padding: 14px 12px;
      display: flex;
      flex-direction: column;
      gap: 4px;
      border: 2px solid transparent;
      background: var(--surface);
      overflow: hidden;
    }
    .spot::before {
      content: '';
      position: absolute;
      inset-inline-start: 0;
      top: 0;
      bottom: 0;
      width: 5px;
    }
    .spot.available { border-color: rgba(43,182,115,0.35); }
    .spot.available::before { background: var(--green); }
    .spot.occupied { border-color: rgba(228,87,46,0.35); }
    .spot.occupied::before { background: var(--red); }
    .spot.disabled { border-color: rgba(138,147,166,0.25); opacity: 0.6; }
    .spot.disabled::before { background: var(--text-dim); }

    .spot__number { font-size: 18px; font-weight: 700; }
    .spot__status { font-size: 12px; color: var(--text-dim); }
    .spot__plate { font-size: 12px; color: var(--amber); }
  `]
})
export class ParkingSpotsComponent implements OnInit {
  private spotService = inject(ParkingSpotService);

  loading = signal(true);
  spots = signal<ParkingSpot[]>([]);
  selectedFloor = signal<number | null>(null);

  floors = computed(() => {
    const set = new Set(this.spots().map(s => s.floor));
    return Array.from(set).sort((a, b) => a - b);
  });

  spotsOnFloor = computed(() => {
    const floor = this.selectedFloor();
    return this.spots()
      .filter(s => floor === null || s.floor === floor)
      .sort((a, b) => a.spotNumber.localeCompare(b.spotNumber));
  });

  statusClass = (status: SpotStatus) =>
    status === SpotStatus.Available ? 'available' : status === SpotStatus.Occupied ? 'occupied' : 'disabled';

  statusLabel = (status: SpotStatus) => SpotStatusLabel[status];

  tooltip(spot: ParkingSpot): string {
    return `${spot.spotNumber} · קומה ${spot.floor} · ${this.statusLabel(spot.statusId)}`;
  }

  ngOnInit(): void {
    this.spotService.getAll().subscribe({
      next: spots => {
        this.spots.set(spots);
        const firstFloor = Array.from(new Set(spots.map(s => s.floor))).sort((a, b) => a - b)[0];
        this.selectedFloor.set(firstFloor ?? null);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}
