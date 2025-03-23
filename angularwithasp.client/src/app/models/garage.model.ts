import { Car } from "./car.model";

export interface Garage {
    id: number;
    name: string;
    address: string;
    cars: Car[];
    city: string;}