import { useEffect, useState } from "react";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import Order from "../../api/models/common/Order";

export interface OrderFormValues {
    id: number,
    number: string,
    date: Date,
    providerId: number
}

export interface OrderFormProps {
    providers: Provider[],
    order: Order,
    onChange: (values: Order) => void
}

export default function OrderForm(props: OrderFormProps) {
    const [number, setNumber] = useState(props.order.number);
    const [date, setDate] = useState(props.order.date);
    const [providerId, setProviderId] = useState(props.order.providerId);

    useEffect(() => {
        setNumber(props.order.number);
        setDate(props.order.date);
        setProviderId(props.order.providerId);
    }, [props.order]);

    function onNumberChange(e: React.ChangeEvent<HTMLInputElement>) {
        setNumber(e.currentTarget.value);
        props.onChange({...props.order, number: e.currentTarget.value});
    }

    function onDateChange(e: React.ChangeEvent<HTMLInputElement>) {
        setDate(e.currentTarget.valueAsDate!);
        props.onChange({...props.order, date: e.currentTarget.valueAsDate!});
    }

    function onProviderIdChange(e: React.ChangeEvent<HTMLSelectElement>) {
        const newProviderId = Number.parseInt(e.currentTarget.value);
        setProviderId(newProviderId);
        props.onChange({...props.order, providerId: newProviderId, providerName: props.providers.find(p => p.id === newProviderId)!.name});
    }

    const providerSelectListItems = props.providers.map(p => (
        <option key={p.id} value={p.id}>{p.name}</option>
    ));

    return (
        <form>
            <div className="mb-3 row justify-content-center">
                <label className="col-sm-3 col-form-label" htmlFor="Number">Number</label>
                <div className="col-sm-5">
                    <input type="text"
                        onChange={onNumberChange}
                        className="form-control"
                        value={number}/>
                </div>
            </div>

            <div className="mb-3 row justify-content-center">
                <label className="col-sm-3 col-form-label" htmlFor="Date">Date</label>
                <div className="col-sm-5">
                    <input type="date"
                        onChange={onDateChange}
                        className="form-control"
                        value={date.toLocaleDateString('sv')}/>
                </div>
            </div>

            <div className="mb-3 row justify-content-center">
                <label className="col-sm-3 col-form-label" htmlFor="ProviderId">Provider name</label>
                <div className="col-sm-5">
                    <select className="form-select"
                        onChange={onProviderIdChange}
                        value={providerId}>
                        {providerSelectListItems}
                    </select>
                </div>
            </div>
        </form>
    );
}