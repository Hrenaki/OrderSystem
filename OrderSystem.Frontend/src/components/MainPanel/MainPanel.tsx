import React from "react";
import { useDispatch, useSelector } from "react-redux";

import { logout, UserState } from "../../common/UserState";

const MainPanel: React.FC = () => {
    const dispatch = useDispatch();

    const user = useSelector<UserState, UserState>((x) => x);
    console.log(user);

    return (
        <div>
            <div>Hello, {user.username}!</div>
            <button onClick={() => dispatch(logout())}>Logout</button>
        </div>
    );
}

export default MainPanel;