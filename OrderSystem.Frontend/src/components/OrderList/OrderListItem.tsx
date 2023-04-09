import React from 'react';
import { Order } from '../../api/models/Orders/OrdersResponse';

interface OrderListItemProps {
    order: Order
}

function OrderListItem(props: OrderListItemProps) {
    console.log(props.order.date);
    
    return (
        <button className="row text-center w-100 m-0">
            <div className="col">{props.order.id}</div>
            <div className="col">{props.order.number}</div>
            
            <div className="col">{props.order.providerName}</div>
        </button>
    );
}

export default OrderListItem;