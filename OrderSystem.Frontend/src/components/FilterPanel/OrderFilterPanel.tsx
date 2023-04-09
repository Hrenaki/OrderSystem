import { OrdersRequest } from "../../api/models/Orders/OrdersResponse";
import { useState } from "react";
import MultiSelectList, { SelectListOption } from "../common/MultiSelectList";
import { Provider } from "../../api/models/Providers/ProvidersResponse";

export interface OrderFilterPanelProps {
    providers: Provider[],
    onClick: (request: OrdersRequest) => void;
};

function OrderFilterPanel (props: OrderFilterPanelProps) {
    const date = new Date();
    const [dateTo, setDateTo] = useState(date.toLocaleDateString('sv'));

    date.setMonth(date.getMonth() - 1);
    const [dateFrom, setDateFrom] = useState(date.toLocaleDateString('sv'));

    const [providerListIsVisible, setProviderListIsVisible] = useState(false);

    const OnDateFromChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDateFrom(e.currentTarget.value);
    }

    const OnDateToChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDateTo(e.currentTarget.value);
    }

    return (
        <form className='p-1'>
            <div className="input-group mb-3">
                <span className="input-group-text" id="spanFrom">From</span>
                <input type="date"
                    className="form-control"
                    placeholder="Start date"
                    aria-label="StartDate"
                    aria-describedby="spanFrom"
                    data-val="true"
                    data-val-required="The DateFrom field is required."
                    id="DateFrom"
                    name="DateFrom"
                    value={dateFrom} onChange={OnDateFromChange}>
                </input>
            </div>
            <div className="input-group mb-3">
                <span className="input-group-text" id="spanTo">To</span>
                <input type="date"
                    className="form-control"
                    placeholder="End date"
                    aria-label="EndDate"
                    aria-describedby="spanTo"
                    data-val="true"
                    data-val-required="The DateTo field is required."
                    id="DateTo"
                    name="DateTo"
                    value={dateTo} onChange={OnDateToChange}>
                </input>
            </div>

            <button className="btn btn-outline-primary w-100 mb-3" type="button" onClick={() => setProviderListIsVisible(!providerListIsVisible)}>Providers</button>
            {providerListIsVisible ? <MultiSelectList options={props.providers.map(p => {
                return {key: p.id.toString(), value: p.name};
            })} /> : "" }
        </form>
    )
};

export default OrderFilterPanel;