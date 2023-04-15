import { useEffect, useState } from "react";
import OrderItem from "../../api/models/common/OrderItem";
import Order from "../../api/models/common/Order";

export interface OrderEditItemTableProps {
    order: Order,
    items: OrderItem[] | undefined,
    setOrderItems: (items: OrderItem[]) => void
}

export default function OrderEditItemTable(props: OrderEditItemTableProps) {
    const [newOrderCount, setNewOrderCount] = useState(0);

    const AddOrderItem = () => {
        const orderItemName = props.order.number + `_order-item-${newOrderCount + 1}`;
        const item = {id: -1 * (newOrderCount + 1), orderId: props.order.id, name: orderItemName, quantity: 1, unit: "unit"};
        setNewOrderCount(newOrderCount + 1);

        if(props.items === undefined)
            props.setOrderItems([item]);
        else props.setOrderItems([...props.items!, item].sort((a, b) => a.name.localeCompare(b.name)));
    };

    const EditOrderItem = (item: OrderItem) => {
        const items = props.items!.filter(i => i.id !== item.id);
        props.setOrderItems([...items, item].sort((a, b) => a.name.localeCompare(b.name)));
    };

    const DeleteOrderItem = (item: OrderItem) => {
        if(props.items !== undefined)
            props.setOrderItems(props.items.filter(i => i.id !== item.id));
    };

    const items = props.items?.map(item => (
        <tr key={item.id}>
            <th scope="row">{item.name}</th>
            <td>{item.quantity}</td>
            <td>{item.unit}</td>
            <td><button id="1-item-edit-btn" className="btn btn-outline-primary w-auto">Edit</button></td>
            <td><button id="1-item-delete-btn" className="btn btn-outline-danger w-auto" onClick={() => DeleteOrderItem(item)}>Delete</button></td>
        </tr>
    ));

    return (
        <div>
            <div className="row justify-content-around">
                <h4 className="col-4">Order items</h4>
                <button className="btn btn-primary col-3" onClick={() => AddOrderItem()}>Add item</button>
            </div>

            <div className="table-responsive-lg">
                <table id="order-items-table" className="table table-hover">
                    <thead>
                        <tr>
                            <th scope="col">Name</th>
                            <th scope="col">Quantity</th>
                            <th scope="col">Unit</th>
                            <th scope="col"></th>
                            <th scope="col"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {items}
                    </tbody>
                </table>
            </div>
        </div>
    );
}