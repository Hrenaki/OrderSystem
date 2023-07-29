import Order from "../common/Order"

export interface OrdersRequest {
    dateFrom: Date | undefined,
    dateTo: Date | undefined,
    providerIds: number[] | undefined
}

export default interface OrdersResponse {
    orders: Order[],
    error: string
}