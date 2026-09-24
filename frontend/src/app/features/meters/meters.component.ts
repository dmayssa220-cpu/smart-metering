import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService, Meter } from '../../core/services/api.service';

@Component({
  selector: 'app-meters',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './meters.component.html',
  styleUrl: './meters.component.scss'
})
export class MetersComponent implements OnInit {
  meters: Meter[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getMeters().subscribe({
      next: (m) => (this.meters = m),
      error: () => (this.meters = [])
    });
  }
}
