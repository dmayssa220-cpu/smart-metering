import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent {
  constructor(public auth: AuthService) {}

  logout(): void {
    this.auth.logout();
  }
}
