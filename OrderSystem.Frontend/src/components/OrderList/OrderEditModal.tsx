import { useEffect, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import Order from "../../api/models/common/Order";
import Modal from "../common/Modal";
import OrderForm, { OrderFormValues } from "./OrderForm";
import OrderEditItemTable from "./OrderEditItemTable";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderItem from "../../api/models/common/OrderItem";

export interface OrderEditModalProps {
    order: Order | null,
    show: boolean,
    providers: Provider[],
    setOrder: (order: Order) => void,
    discardChanges: (orderId: number) => void,
    onClose: () => void,
    onSavedClose: () => void
}

export default function OrderEditModal(props: OrderEditModalProps) {
    const [order, setOrder] = useState(props.order);
    const [orderItems, setOrderItems] = useState(props.order?.items);

    useEffect(() => {
        setOrder(props.order);
        setOrderItems(props.order?.items);
    }, [props.order]);

    async function Save() {
        await OrderSystemAPI.PostOrderAsync(order!);
        //props.setOrder({...order!, draft: false});
        props.onSavedClose();
        props.onClose();
    }

    function onClose() {
        props.setOrder(order!);
        props.onClose();
    }

    function onFormValuesChange(values: OrderFormValues) {
        setOrder({...order!, draft: true, ...values});
    }

    function onOrderItemsChange(items: OrderItem[] | undefined) {
        setOrder({...props.order!, items, draft: true});
        setOrderItems(items);
    }

    const modalBody = props.order !== null ?
        <>
            <OrderForm order={order!} providers={props.providers} onChange={onFormValuesChange}/>
            <OrderEditItemTable order={order!} items={orderItems} setOrderItems={onOrderItemsChange}/>
        </>
        : '';

    return (
        <Modal className="modal" isShown={props.show} onClose={onClose}>
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Edit order</h5>
                    <button type="button" className="btn-close" onClick={() => onClose()}></button>
                </div>

                <div className="modal-body">
                    {modalBody}
                </div>

                <div className="modal-footer">
                    <button type="button" className="btn btn-danger" onClick={() => props.discardChanges(props.order!.id)}>Discard changes</button>
                    <button type="button" className="btn btn-success" onClick={() => Save()}>Save</button>
                </div>
            </div>
        </Modal>
    );
}