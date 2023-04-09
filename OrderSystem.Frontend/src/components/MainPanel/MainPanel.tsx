import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";

import { logout, UserState } from "../../common/UserState";
import OrderFilterPanel from "../FilterPanel/OrderFilterPanel";
import { Provider } from "../../api/models/Providers/ProvidersResponse";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import OrderListPanel from "../OrderList/OrderListPanel";

const MainPanel: React.FC = () => {
    const dispatch = useDispatch();
    const user = useSelector<UserState, UserState>((x) => x);

    const [providers, setProviders] = useState(Array<Provider>(0));
    useEffect(() =>
    {
        async function getProviders() {
            const response = await OrderSystemAPI.GetProvidersAsync();
            setProviders(response.providers);
        }

        getProviders();
    }, []);

    return (
        <div className='container text-center row justify-content-between'>
            <div className='col-3 bg-light border rounded-4 p-3'>
                <OrderFilterPanel providers={providers} onClick={_ => console.log("submited")} />
            </div>
            <div className='col-8 bg-light border rounded-4 p-3'>
                <OrderListPanel providers={providers} />
            </div>
        </div>
    );
}

export default MainPanel;