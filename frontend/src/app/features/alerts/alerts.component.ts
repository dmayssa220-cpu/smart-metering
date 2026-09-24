import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, Alert } from '../../core/services/api.service';

@Component({
  selector: 'app-alerts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.scss'
})
export class AlertsComponent implements OnInit {
  alerts: Alert[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.api.getAlerts(false).subscribe({
      next: (a) => (this.alerts = a),
      error: () => (this.alerts = [])
    });
  }

  acknowledge(id: string): void {
    this.api.acknowledgeAlert(id).subscribe(() => this.load());
  }
}
