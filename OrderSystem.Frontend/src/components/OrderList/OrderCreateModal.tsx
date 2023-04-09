import { useEffect, useRef, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import Modal from "../common/Modal";
import CreateOrderRequest from "../../api/models/Order/CreateOrderRequest";
import CreateOrderResponse from "../../api/models/Order/CreateOrderResponse";

export interface CreateOrderModalProps {
    show: boolean,
    providers: Provider[],
    onSubmit: (e: CreateOrderRequest) => Promise<CreateOrderResponse>,
    onClose: () => void
}

export default function OrderCreateModal(props: CreateOrderModalProps) {
    const [number, setNumber] = useState('');
    const [date, setDate] = useState(new Date().toLocaleDateString('sv'));
    const [providerId, setProviderId] = useState(-1);
    const [error, setError] = useState('');
    
    const formRef = useRef(null);

    useEffect(() =>
    {
        setProviderId(props.providers.length > 0 ? props.providers[0].id : -1);
    }, [props.providers]);

    function onNumberChange(e: React.ChangeEvent<HTMLInputElement>) {
        setNumber(e.currentTarget.value);
    }

    function onDateChange(e: React.ChangeEvent<HTMLInputElement>) {
        setDate(e.currentTarget.value);
    }

    function onProviderIdChange(e: React.ChangeEvent<HTMLSelectElement>) {
        console.log(Number.parseInt(e.currentTarget.value));
        setProviderId(Number.parseInt(e.currentTarget.value));
    }

    async function onSubmit() {
        var response = await props.onSubmit({number, date: new Date(date), providerId});
        if(response.success)
            props.onClose();
        else setError(response.message);
    }

    const providerSelectListItems = props.providers.map(p => (
        <option key={p.id} value={p.id}>{p.name}</option>
    ));

    return (
        <Modal className="modal" isShown={props.show} onClose={props.onClose}>
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title">Create order</h5>
                    <button type="button" className="btn-close" onClick={() => props.onClose()}></button>
                </div>
                <div className="modal-body">
                    <form id="createOrderForm" ref={formRef}>
                        <div className="mb-3 row justify-content-center">
                            <label className="col-sm-3 col-form-label" htmlFor="Number">Number</label>
                            <div className="col-sm-5">
                                <input type="text"
                                    onChange={onNumberChange}
                                    className="form-control"
                                    data-val="true"
                                    data-val-required="Number isn't set"
                                    id="Number"
                                    name="Number"
                                    value={number}/>
                            </div>
                        </div>

                        <div className="mb-3 row justify-content-center">
                            <label className="col-sm-3 col-form-label" htmlFor="Date">Date</label>
                            <div className="col-sm-5">
                                <input type="date"
                                    onChange={onDateChange}
                                    className="form-control"
                                    data-val="true"
                                    data-val-required="Date isn't set"
                                    id="Date"
                                    name="Date"
                                    value={date}/>
                            </div>
                        </div>

                        <div className="mb-3 row justify-content-center">
                            <label className="col-sm-3 col-form-label" htmlFor="ProviderId">Provider name</label>
                            <div className="col-sm-5">
                                <select className="form-select"
                                    onChange={onProviderIdChange}
                                    data-val="true"
                                    data-val-required="ProviderId isn't set"
                                    id="ProviderId"
                                    name="ProviderId">
                                    {providerSelectListItems}
                                </select>
                            </div>
                        </div>
                    </form>
                </div>

                <div className="modal-footer">
                    <button type="button" className="btn btn-secondary" onClick={() => props.onClose()}>Close</button>
                    <button type="button" className="btn btn-primary" onClick={() => onSubmit()}>Create</button>
                </div>
            </div>
        </Modal>
    );
}