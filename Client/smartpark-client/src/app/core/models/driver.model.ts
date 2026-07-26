export interface Driver {
  id: number;
  firstName: string;
  lastName: string;
  phone?: string;
  licensePlate: string;
  recordStatus: number;
  createdAt: string;
}

/** מודל לשליחה ביצירה/עדכון — ללא שדות שהשרת ממלא בעצמו */
export type DriverInput = Omit<Driver, 'id' | 'recordStatus' | 'createdAt'> & {
  id?: number;
};
