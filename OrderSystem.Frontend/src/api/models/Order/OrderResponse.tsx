import Order from "../common/Order";

export default interface OrderResponse {
    order: Order,
    error: string
}