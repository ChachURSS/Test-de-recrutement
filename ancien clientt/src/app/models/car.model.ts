import { Garage } from "./garage.model";

export interface Car {
  id: number;
  brand: string;
  model: string;
  garageId?: number;
  garage?: Garage;
  color: string;
}
