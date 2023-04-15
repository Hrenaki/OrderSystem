import OrderItem from "./OrderItem";

export default interface Order {
    id: number,
    number: string,
    date: Date,
    providerId: number,
    providerName: string,
    items?: OrderItem[],
    draft: boolean
}

export const Order = {
    id: -1,
    number: '',
    date: new Date(),
    providerId: -1,
    providerName: ''
} as Order;