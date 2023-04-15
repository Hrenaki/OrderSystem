import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import { logout, UserState } from "../../common/UserState";
import OrderFilterPanel from "../FilterPanel/OrderFilterPanel";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderListPanel from "../OrderList/OrderListPanel";
import Order from "../../api/models/common/Order";
import { OrdersRequest } from "../../api/models/Orders/OrdersResponse";

const MainPanel: React.FC = () => {
    const dispatch = useDispatch();
    const user = useSelector<UserState, UserState>((x) => x);

    const [providers, setProviders] = useState(Array<Provider>(0));
    const [orders, setOrders] = useState(Array<Order>(0));
    const [error, setError] = useState("");

    async function getOrders(request: OrdersRequest) {
        console.log(request);

        try {
            const response = await OrderSystemAPI.GetOrdersAsync(request);

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
    const getAllOrders = () => getOrders({dateTo: undefined, dateFrom: undefined, providerIds: []});

    useEffect(() =>
    {
        async function getProviders() {
            const response = await OrderSystemAPI.GetProvidersAsync();
            setProviders(response.providers);
        }

        getProviders();
        getAllOrders();
    }, []);

    function setOrder(order: Order) {
        var tempOrders = orders.filter(o => o.id !== order.id);
        setOrders([...tempOrders, order].sort((a, b) => a.number.localeCompare(b.number)));
    }

    const sortedOrders = orders.sort((a, b) => a.number.localeCompare(b.number));
    return (
        <div className='container text-center row justify-content-between'>
            <div className='col-3 bg-light border rounded-4 p-3'>
                <OrderFilterPanel providers={providers} onClick={getOrders} />
            </div>
            <div className='col-8 bg-light border rounded-4 p-3'>
                <OrderListPanel providers={providers} orders={sortedOrders} error={error} setOrder={setOrder} refreshOrders={getAllOrders} />
            </div>
        </div>
    );
}

export default MainPanel;