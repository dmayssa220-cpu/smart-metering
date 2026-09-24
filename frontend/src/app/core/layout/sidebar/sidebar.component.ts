import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  label: string;
  path: string;
  icon: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  readonly navItems: NavItem[] = [
    { label: 'Tableau de bord', path: '/dashboard', icon: 'grid' },
    { label: 'Compteurs', path: '/meters', icon: 'cpu' },
    { label: 'Alertes', path: '/alerts', icon: 'bell' }
  ];
}
