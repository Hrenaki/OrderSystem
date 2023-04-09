import React, { useEffect, useState } from "react";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import { Order } from "../../api/models/Orders/OrdersResponse";
import Modal from "../common/Modal";
import { Provider } from "../../api/models/Providers/ProvidersResponse";

import '../../styles/Modal.css';

export interface OrderListProps {
    orders: Order[]
}

function OrderList(props: OrderListProps) {
    const listItems = props.orders.map((order, index) => (
        <tr key={order.id}>
            <th scope="row">{index + 1}</th>
            <td>{order.number}</td>
            <td>{order.providerName}</td>
            <td>{new Date(order.date.toString()).toLocaleDateString('sv')}</td>
        </tr>
    ));

    return (
        <table className="table">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Number</th>
                    <th scope="col">Provider</th>
                    <th scope="col">Date</th>
                    </tr>
            </thead>
            <tbody>
                {listItems}
            </tbody>
        </table>
    );
}

export default OrderList;