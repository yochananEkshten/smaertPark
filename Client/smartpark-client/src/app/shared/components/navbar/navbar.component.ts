import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'sp-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
    <header class="nav">
      <div class="nav__brand">
        <span class="nav__mark">P</span>
        <div class="nav__title">
          <strong>SmartPark</strong>
          <small>מערכת ניהול חניון</small>
        </div>
      </div>
      <nav class="nav__links">
        <a routerLink="/dashboard" routerLinkActive="active">לוח בקרה</a>
        <a routerLink="/spots" routerLinkActive="active">מפת חניה</a>
        <a routerLink="/entries" routerLinkActive="active">כניסות ויציאות</a>
        <a routerLink="/drivers" routerLinkActive="active">נהגים</a>
        <a routerLink="/settings" routerLinkActive="active">הגדרות</a>
      </nav>
      <div class="nav__stripe" aria-hidden="true"></div>
    </header>
  `,
  styles: [`
    .nav {
      position: relative;
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 14px 28px 10px;
      background: linear-gradient(180deg, #171C25 0%, #14181F 100%);
      border-bottom: 1px solid #262C38;
    }
    .nav__brand { display: flex; align-items: center; gap: 12px; }
    .nav__mark {
      display: grid;
      place-items: center;
      width: 40px;
      height: 40px;
      border-radius: 8px;
      background: #F2A93B;
      color: #14181F;
      font-family: 'Space Grotesk', sans-serif;
      font-weight: 700;
      font-size: 20px;
    }
    .nav__title { display: flex; flex-direction: column; line-height: 1.2; }
    .nav__title strong {
      font-family: 'Space Grotesk', sans-serif;
      font-size: 18px;
      color: #F4F6F8;
      letter-spacing: 0.02em;
    }
    .nav__title small { color: #8A93A6; font-size: 12px; }
    .nav__links { display: flex; gap: 6px; }
    .nav__links a {
      color: #AEB6C4;
      text-decoration: none;
      font-size: 14px;
      font-weight: 600;
      padding: 8px 14px;
      border-radius: 8px;
      transition: background 0.15s ease, color 0.15s ease;
    }
    .nav__links a:hover { background: #1F2532; color: #F4F6F8; }
    .nav__links a.active { background: #F2A93B; color: #14181F; }
    .nav__stripe {
      position: absolute;
      bottom: -1px;
      left: 0;
      right: 0;
      height: 4px;
      background: repeating-linear-gradient(-45deg, #F2A93B 0 14px, #14181F 14px 28px);
      opacity: 0.9;
    }
    @media (max-width: 860px) {
      .nav { flex-wrap: wrap; gap: 10px; }
      .nav__links { flex-wrap: wrap; }
    }
  `]
})
export class NavbarComponent {}
