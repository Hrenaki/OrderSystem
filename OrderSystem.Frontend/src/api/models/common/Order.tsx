import OrderItem from "./OrderItem";

export default interface Order {
    id: number,
    number: string,
    date: Date,
    providerId: number,
    providerName: string,
    items?: OrderItem[]
}