export interface Order {
    id: number,
    number: string,
    date: Date,
    providerName: string
}

export interface OrdersRequest {
    dateFrom: Date | undefined,
    dateTo: Date | undefined,
    providerIds: number[] | undefined
}

export default interface OrdersResponse {
    orders: Order[],
    error: string
}