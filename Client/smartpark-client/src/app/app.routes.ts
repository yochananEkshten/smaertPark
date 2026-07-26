import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  {
    path: 'spots',
    loadComponent: () => import('./features/parking-spots/parking-spots.component').then(m => m.ParkingSpotsComponent)
  },
  {
    path: 'entries',
    loadComponent: () => import('./features/vehicle-entries/vehicle-entries.component').then(m => m.VehicleEntriesComponent)
  },
  {
    path: 'drivers',
    loadComponent: () => import('./features/drivers/drivers.component').then(m => m.DriversComponent)
  },
  {
    path: 'settings',
    loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent)
  },
  { path: '**', redirectTo: 'dashboard' }
];
