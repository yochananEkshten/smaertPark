export enum Role {
  Admin = 1,
  Employee = 2
}

export enum SpotStatus {
  Available = 1,
  Occupied = 2,
  Disabled = 3
}

export enum VehicleStatus {
  Inside = 1,
  Exited = 2
}

export const SpotStatusLabel: Record<SpotStatus, string> = {
  [SpotStatus.Available]: 'פנוי',
  [SpotStatus.Occupied]: 'תפוס',
  [SpotStatus.Disabled]: 'מושבת'
};

export const VehicleStatusLabel: Record<VehicleStatus, string> = {
  [VehicleStatus.Inside]: 'בחניון',
  [VehicleStatus.Exited]: 'יצא'
};
