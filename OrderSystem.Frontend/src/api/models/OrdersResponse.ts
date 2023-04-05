export interface Order {
    id: number,
    number: string,
    date: Date,
    providerName: string
}

export default interface OrdersResponse {
    orders: Order[],
    error: string
}