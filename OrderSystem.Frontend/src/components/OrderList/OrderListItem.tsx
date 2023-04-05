import React from 'react';
import { Order } from '../../api/models/OrdersResponse';

interface OrderListItemProps {
    order: Order
}

function OrderListItem(props: OrderListItemProps) {
    return (
        <button className="row align-items-center">
            <div className="col">1</div>
            <div className="col">2</div>
            <div className="col">3</div>
        </button>
    );
}

export default OrderListItem;