import React, { useState } from "react";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderListItem from "./OrderListItem";

async function OrderList() {
    const [orders, setOrders] = useState(Array(0));
    const [error, setError] = useState("");

    try {
        const response = await OrderSystemAPI.GetOrdersAsync();
        if(response.error !== null) {
            setError(response.error);
        }
        else {
            setOrders(response.orders);
            setError("");
        }
    }
    catch(error) {
        setError((error as Error)?.message);
    }

    const listItems = orders.map(order => (<OrderListItem order={order}/>));

    const containerBody = error === "" ? listItems : (<span>{error}</span>);
    return (
        <div className="container text-center">
            {containerBody}
        </div>
    );
}

export default OrderList;