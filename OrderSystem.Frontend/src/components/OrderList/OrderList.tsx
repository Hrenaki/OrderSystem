import React, { useEffect, useState } from "react";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderListItem from "./OrderListItem";
import { Order } from "../../api/models/Orders/OrdersResponse";

function OrderList() {
    const [orders, setOrders] = useState(Array<Order>(0));
    const [error, setError] = useState("");

    useEffect(() =>
    {
        async function getOrders() {
            try {
                const response = await OrderSystemAPI.GetOrdersAsync({
                    dateFrom: undefined,
                    dateTo: undefined,
                    providerIds: []
                });
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
        }
        
        getOrders();
    });

    const listItems = orders.map(order => (<OrderListItem order={order}/>));

    const containerBody = error === "" ? listItems : (<span>{error}</span>);
    return (
        <div>
            <div className="row justify-content-between m-0">
			    <h3 className="col-auto">Orders</h3>
			    <button className="col-auto btn btn-primary">Add order</button>
		    </div>
            <div className="container text-center">
                {containerBody}
            </div>
        </div>
    );
}

export default OrderList;