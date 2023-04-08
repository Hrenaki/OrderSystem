import React from "react";
import { useDispatch, useSelector } from "react-redux";

import { logout, UserState } from "../../common/UserState";
import OrderList from "../OrderList/OrderList";
import OrderFilterPanel from "../FilterPanel/OrderFilterPanel";

const MainPanel: React.FC = () => {
    const dispatch = useDispatch();

    const user = useSelector<UserState, UserState>((x) => x);
    console.log(user);

    return (
        <div className='container text-center row justify-content-between'>
            <div className='col-3 bg-light border rounded-4 p-3'>
                <OrderFilterPanel onClick={_ => console.log("submited")} />
            </div>
            <div className='col-8 bg-light border rounded-4 p-3'>
                <OrderList />
            </div>
        </div>
    );
}

export default MainPanel;