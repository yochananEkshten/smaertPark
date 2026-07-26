import { SpotStatus, VehicleStatus } from './enums';
import { Driver } from './driver.model';

export interface VehicleEntry {
  id: number;
  plateNumber: string;
  entryTime: string;
  exitTime?: string | null;
  autoDetected: boolean;
  spotId?: number | null;
  status: VehicleStatus;
  driverId?: number | null;
  driver?: Driver | null;
}

export interface ParkingSpot {
  id: number;
  spotNumber: string;
  floor: number;
  statusId: SpotStatus;
  vehicleId?: number | null;
  vehicle?: VehicleEntry | null;
  updatedAt: string;
}

export interface VehicleEntryRequest {
  licensePlate: string;
  driverId?: number | null;
}
