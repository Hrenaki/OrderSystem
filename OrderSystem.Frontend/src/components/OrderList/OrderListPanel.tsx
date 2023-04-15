import { useEffect, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderList from "./OrderList";
import OrderCreateModal from "./OrderCreateModal";
import CreateOrderRequest from "../../api/models/Order/CreateOrderRequest";
import Order from "../../api/models/common/Order";
import OrderEditModal from "./OrderEditModal";

export interface OrderListPanelProps {
    providers: Provider[],
    orders: Order[],
    error: string,
    refreshOrders: () => void
};

export default function OrderListPanel(props: OrderListPanelProps) {
    const [createOrderModalShow, setCreateOrderModalShow] = useState(false);
    
    const [editOrderModalShow, setEditOrderModalShow] = useState(false);
    const [edittingOrder, setEdittingOrder] = useState({id: -1, number: '', date: new Date(), providerId: -1, providerName: ''});

    async function createOrder(e: CreateOrderRequest) {
        const response = await OrderSystemAPI.CreateOrderAsync(e);
        if(response.success)
            props.refreshOrders();
        return response;
    }

    async function onOrderSelect(id: number) {
        const order = await OrderSystemAPI.GetOrderAsync(id);
        setEdittingOrder(order);
        setEditOrderModalShow(true);
    }

    var containerBody = null;
    if(props.error !== '')
        containerBody = props.error;
    else if(props.orders.length < 1)
        containerBody = 'No orders';
    else containerBody = <OrderList orders={props.orders} onOrderSelect={onOrderSelect}/>;

    return (
        <div>
            <div className="row justify-content-between m-0 mb-3">
			    <h3 className="col-auto">Orders</h3>
			    <button className="col-auto btn btn-primary" onClick={() => setCreateOrderModalShow(true)}>Add order</button>
		    </div>

            <div className="container text-center p-0">
                {containerBody}
            </div>

            <OrderCreateModal show={createOrderModalShow} providers={props.providers} onSubmit={createOrder} onClose={() => setCreateOrderModalShow(false)}/>
            <OrderEditModal show={editOrderModalShow} providers={props.providers} order={edittingOrder} onClose={() => setEditOrderModalShow(false)}/>
        </div>
    );
}