import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, Meter, Alert } from '../../core/services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  meters: Meter[] = [];
  alerts: Alert[] = [];
  loading = true;

  // Exemple de série horaire de consommation (à remplacer par les données réelles /readings)
  readonly consumptionSample = [42, 45, 41, 50, 58, 63, 60, 55, 48, 52, 57, 61];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getMeters().subscribe({
      next: (m) => (this.meters = m),
      error: () => (this.meters = []),
      complete: () => (this.loading = false)
    });
    this.api.getAlerts(true).subscribe({
      next: (a) => (this.alerts = a),
      error: () => (this.alerts = [])
    });
  }

  get activeCount(): number {
    return this.meters.filter((m) => m.status === 'Active').length;
  }

  get faultyCount(): number {
    return this.meters.filter((m) => m.status === 'Faulty').length;
  }

  barHeight(value: number): number {
    const max = Math.max(...this.consumptionSample);
    return Math.round((value / max) * 100);
  }
}
