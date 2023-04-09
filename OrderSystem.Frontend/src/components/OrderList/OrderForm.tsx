import { Provider } from "../../api/models/Providers/ProvidersResponse";

export interface OrderFormValues {
    number: string,
    date: Date,
    providerId: number
}

export interface OrderFormProps {
    providers: Provider[],
    values: OrderFormValues,
    onChange: (values: OrderFormValues) => void
}

export default function OrderForm(props: OrderFormProps) {
    function onNumberChange(e: React.ChangeEvent<HTMLInputElement>) {
        props.onChange({...props.values, number: e.currentTarget.value});
    }

    function onDateChange(e: React.ChangeEvent<HTMLInputElement>) {
        props.onChange({...props.values, date: new Date(e.currentTarget.value)});
    }

    function onProviderIdChange(e: React.ChangeEvent<HTMLSelectElement>) {
        props.onChange({...props.values, providerId: Number.parseInt(e.currentTarget.value)});
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
                        data-val="true"
                        data-val-required="Number isn't set"
                        id="Number"
                        name="Number"
                        value={props.values.number}/>
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
                        value={props.values.date.toLocaleDateString('sv')}/>
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
    );
}