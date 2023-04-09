import { useEffect, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import Order from "../../api/models/common/Order";
import Modal from "../common/Modal";
import OrderForm, { OrderFormValues } from "./OrderForm";

export interface OrderEditModalProps {
    order: Order | null,
    show: boolean,
    providers: Provider[],
    onClose: () => void
}

export default function OrderEditModal(props: OrderEditModalProps) {
    const initialValues = {number: '', date: new Date(), providerId: -1};
    const [formValues, setFormValues] = useState(initialValues);
    const [orderItems, setOrderItems] = useState(props.order?.items);

    function onFormValuesChange(e: OrderFormValues) {
        setFormValues(e);
    }

    useEffect(() =>
    {
        const order = props.order;
        if(order !== null)
            onFormValuesChange({number: order!.number, date: new Date(order!.date), providerId: order!.providerId});
    }, [props.order]);
    
    useEffect(() =>
    {
        const providerId = props.providers.length > 0 ? props.providers[0].id : -1;
        onFormValuesChange({...formValues, providerId});
    }, [props.providers]);

    return (
        <Modal className="modal" isShown={props.show} onClose={props.onClose}>
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Edit order</h5>
                    <button type="button" className="btn-close" onClick={() => props.onClose()}></button>
                </div>

                <div className="modal-body">
                    <OrderForm values={formValues} providers={props.providers} onChange={onFormValuesChange}/>
                    <div className="row justify-content-around">
	                    <h4 className="col-4">Order items</h4>
	                    <button className="btn btn-primary col-3">Add item</button>
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
                                <tr>
                                	<th scope="row">123</th>
                                	<td>1,000</td>
                                	<td>3213</td>
                                	<td><button id="1-item-edit-btn" className="btn btn-outline-primary w-auto">Edit</button></td>
                                	<td><button id="1-item-delete-btn" className="btn btn-outline-danger w-auto">Delete</button></td>
                                </tr>
			                </tbody>
		                </table>
                    </div>
                </div>

                <div className="modal-footer">
                    <button type="button" className="btn btn-secondary" onClick={() => props.onClose()}>Close</button>
                    <button type="button" className="btn btn-primary">Save</button>
                </div>
            </div>
        </Modal>
    );
}