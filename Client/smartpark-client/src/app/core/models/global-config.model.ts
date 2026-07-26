export interface GlobalConfig {
  id: number;
  parkingLotName: string;
  address: string;
  screenTitle: string;
  totalSpots: number;
  floors: number;
  /** פורמט TimeSpan של .NET, למשל "08:00:00" */
  openingTime: string;
  closingTime: string;
  pricePerHourBusiness: number;
  pricePerHourRegular: number;
  businessHoursStart: string;
  businessHoursEnd: string;
  currency: string;
  maxParkingHours: number;
  licensePlateRecognitionEnabled: boolean;
}
