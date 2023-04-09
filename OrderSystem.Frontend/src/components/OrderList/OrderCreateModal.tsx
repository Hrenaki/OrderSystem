import { useEffect, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import Modal from "../common/Modal";
import CreateOrderRequest from "../../api/models/Order/CreateOrderRequest";
import CreateOrderResponse from "../../api/models/Order/CreateOrderResponse";
import OrderForm, { OrderFormValues } from "./OrderForm";

import '../../styles/Modal.css';

export interface CreateOrderModalProps {
    show: boolean,
    providers: Provider[],
    onSubmit: (e: CreateOrderRequest) => Promise<CreateOrderResponse>,
    onClose: () => void
}

export default function OrderCreateModal(props: CreateOrderModalProps) {
    const [formValues, setFormValues] = useState({number: '', date: new Date(), providerId: -1});
    const [error, setError] = useState('');

    function onFormValuesChange(e: OrderFormValues) {
        setFormValues(e);
    }
    
    useEffect(() =>
    {
        const providerId = props.providers.length > 0 ? props.providers[0].id : -1;
        onFormValuesChange({...formValues, providerId});
    }, [props.providers]);

    async function onSubmit() {
        var response = await props.onSubmit(formValues);
        if(response.success)
            props.onClose();
        else setError(response.message);
    }

    return (
        <Modal className="modal" isShown={props.show} onClose={props.onClose}>
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Create order</h5>
                    <button type="button" className="btn-close" onClick={() => props.onClose()}></button>
                </div>
                <div className="modal-body">
                    <OrderForm values={formValues} providers={props.providers} onChange={onFormValuesChange}/>
                </div>

                <div className="modal-footer">
                    <button type="button" className="btn btn-secondary" onClick={() => props.onClose()}>Close</button>
                    <button type="button" className="btn btn-primary" onClick={() => onSubmit()}>Create</button>
                </div>
            </div>
        </Modal>
    );
}