import React from 'react';
import { Order } from '../../api/models/Orders/OrdersResponse';

interface OrderListItemProps {
    order: Order
}

function OrderListItem(props: OrderListItemProps) {
    return (
        <button className="grid text-center">
            <div className="g-col-4">{props.order.id}</div>
            <div className="g-col-4">{props.order.number}</div>
            <div className="g-col-4">{props.order.date.toLocaleDateString('sv')}</div>
            <div className="g-col-4">{props.order.providerName}</div>
        </button>
    );
}

export default OrderListItem;