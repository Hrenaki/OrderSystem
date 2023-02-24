import React, { useState } from "react";
import 'bootstrap/dist/css/bootstrap.css';

import LoginPanel from "./LoginPanel";
import RegisterPanel from "./RegisterPanel";
import NavButton from "../common/NavButton";

interface LoginRegisterPanelProps {
    backgroundColor: string
}

function LoginRegisterPanel(props: LoginRegisterPanelProps) {
    const [isLoginTab, setLoginTab] = useState(true);
    const style = {
        backgroundColor: props.backgroundColor
    };

    return (
        <div className="w-100 h-100 d-flex flex-column justify-content-start align-items-center">
            <div style={{height: '30%', width: '50%'}}></div>
            <div className="w-50 m-2 d-flex flex-column justify-content-center">
                <ul className="nav nav-pills nav-justified mb-3" role="tablist">
                    <li className="nav-item" role="presentation">
                        <NavButton name={'Login'} onClick={() => setLoginTab(true)} isActive={isLoginTab} style={style} />
                    </li>
                    <li className="nav-item" role="presentation">
                        <NavButton name={'Register'} onClick={() => setLoginTab(false)} isActive={!isLoginTab} style={style} />
                    </li>
                </ul>

                <div className="tab-content">
                    {isLoginTab ? <LoginPanel /> : <RegisterPanel />}
                </div>
            </div>
        </div>
    );
}

export default LoginRegisterPanel;