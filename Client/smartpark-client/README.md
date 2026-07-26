# SmartPark — Angular Client

קליינט Angular 18 (standalone components) עבור ה-API של פרויקט smartPark (.NET 8).

## התאמה ל-API

הקליינט בנוי בדיוק מול ה-Controllers, ה-Models וה-enums מהפרויקט שהעלית:

| Controller (.NET)        | Service (Angular)             | מסך                  |
|---------------------------|--------------------------------|-----------------------|
| `DriverController`        | `driver.service.ts`            | `/drivers`            |
| `ParkingSpotController`    | `parking-spot.service.ts`      | `/spots`              |
| `VehicleEntryController`   | `vehicle-entry.service.ts`     | `/entries`            |
| `GlobalConfigController`   | `global-config.service.ts`     | `/settings`           |

`SpotStatus`, `VehicleStatus`, `Role` תואמים ל-`Models/enums.cs`.

## הרצה

```bash
npm install
npm start
```

האפליקציה תרוץ על `http://localhost:4200` — בדיוק הכתובת שמוגדרת ב-`Program.cs` בפרויקט השרת תחת `AddCors("AllowAngular")`.

## חיבור לשרת

כתובת ה-API מוגדרת ב-`src/environments/environment.development.ts`:

```ts
apiUrl: 'https://localhost:7020/api'
```

זו הכתובת שמוגדרת ב-`Properties/launchSettings.json` (פרופיל `https`). אם אתה מריץ את השרת בפורט אחר, עדכן כאן.

> **הערה:** אם השרת רץ עם תעודת SSL פיתוח (dev cert) שלא מאושרת בדפדפן, ייתכן שתקבל שגיאת CORS/רשת. פתח את `https://localhost:7020/swagger` פעם אחת בדפדפן ואשר את אזהרת התעודה, או הרץ את השרת בפרופיל `http` ועדכן את `apiUrl` ל-`http://localhost:5099/api`.

## מבנה הפרויקט

```
src/app/
  core/
    models/       # ממשקי TypeScript תואמים למודלים ב-C#
    services/      # HttpClient services מול כל Controller
  features/
    dashboard/     # לוח בקרה — KPIs ואחוז תפוסה
    parking-spots/ # מפת חניה חיה לפי קומה
    vehicle-entries/ # רישום כניסה + טבלת רכבים פעילים + יציאה
    drivers/       # CRUD מלא לנהגים
    settings/      # טופס GlobalConfig
  shared/
    components/    # navbar, toast
```

## הערות

- הבנייה נעשית עם standalone components (ללא NgModules), Angular 18.
- טפסים עם `FormsModule` (ngModel) — פשוט ומספיק לגודל הפרויקט הנוכחי.
- שדות `TimeSpan` מה-.NET (למשל `"08:00:00"`) מומרים אוטומטית לפורמט `HH:mm` לצורך `<input type="time">` ובחזרה.
- אין כרגע מסך התחברות (Login) — ה-Controllers הקיימים בשרת לא כוללים Authentication. אם תוסיף `AuthController` בשרת, אפשר להוסיף מסך Login + Guard בקלות.
