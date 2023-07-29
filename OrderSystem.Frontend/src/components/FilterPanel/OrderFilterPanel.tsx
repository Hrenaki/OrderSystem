import { OrdersRequest } from "../../api/models/Orders/OrdersResponse";
import { useRef, useState } from "react";
import MultiSelectList, { SelectListOption } from "../common/MultiSelectList";
import { Provider } from "../../api/models/Providers/ProvidersResponse";

export interface OrderFilterPanelProps {
    providers: Provider[],
    onClick: (request: OrdersRequest) => void
};

function OrderFilterPanel (props: OrderFilterPanelProps) {
    const date = new Date();
    const [dateTo, setDateTo] = useState(date.toLocaleDateString('sv'));

    date.setMonth(date.getMonth() - 1);
    const [dateFrom, setDateFrom] = useState(date.toLocaleDateString('sv'));

    const [providerListIsVisible, setProviderListIsVisible] = useState(false);
    const[selectedProviderIds, setSelectedProviderIds] = useState(Array<number>(0));

    const OnDateFromChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDateFrom(e.currentTarget.value);
    }

    const OnDateToChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDateTo(e.currentTarget.value);
    }

    const OnProviderSelect = (e: SelectListOption) => {
        var provider = props.providers.find(p => p.name === e.value)!;
        if(selectedProviderIds.some(id => id === provider.id))
            setSelectedProviderIds(selectedProviderIds.filter(id => id !== provider.id).sort());
        else setSelectedProviderIds([...selectedProviderIds, provider.id].sort())
    }

    const OnApplyFitersClick = () => {
        var request = {dateFrom: new Date(dateFrom), dateTo: new Date(dateTo), providerIds: selectedProviderIds};
        props.onClick(request);
    }

    return (
        <form className='p-1'>
            <div className="input-group mb-3">
                <span className="input-group-text" id="spanFrom">From</span>
                <input type="date" className="form-control" value={dateFrom} onChange={OnDateFromChange}/>
            </div>
            <div className="input-group mb-3">
                <span className="input-group-text" id="spanTo">To</span>
                <input type="date" className="form-control" value={dateTo} onChange={OnDateToChange}/>
            </div>

            <button className="btn btn-outline-primary w-100 mb-3" type="button" onClick={() => setProviderListIsVisible(!providerListIsVisible)}>Providers</button>
            {providerListIsVisible ?
                <MultiSelectList options={props.providers.map(p => {
                            return {key: p.id.toString(), value: p.name};
                        })}
                    onSelect={OnProviderSelect}/> :
                ""
            }
            <button className="btn btn-primary w-100" type="button" onClick={() => OnApplyFitersClick()}>Apply filters</button>
        </form>
    )
};

export default OrderFilterPanel;