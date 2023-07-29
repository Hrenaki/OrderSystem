import React, { useState } from 'react';
import { useDispatch } from "react-redux";

import 'bootstrap/dist/css/bootstrap.css';

import LoginRequest from '../../api/models/Login/LoginRequest';
import OrderSystemAPI from '../../api/OrderSystemAPI';
import { login } from '../../common/UserState';

enum LoginStatus {
    Ok,
    InvalidCredentials,
    InternalError,
    Initial
}

const LoginPanel: React.FC = () => {
    const dispatch = useDispatch();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [loginStatus, setLoginStatus] = useState(LoginStatus.Initial);
    
    const onUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setLoginStatus(LoginStatus.Initial);
        setUsername(e.currentTarget.value);
    };

    const onPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setLoginStatus(LoginStatus.Initial);
        setPassword(e.currentTarget.value);
    }

    const loginButtonClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();

        const request: LoginRequest = {
            username: username,
            password: password
        };

        try {
            const response = await OrderSystemAPI.LoginAsync(request);
            const {accessToken } = response;
            dispatch(login(accessToken));
        }
        catch(error) {
            const err = error as Error
			setLoginStatus(err === undefined || err.message !== '401' ? LoginStatus.InternalError : LoginStatus.InvalidCredentials);
        }
    }

    const errorSpanText = loginStatus === LoginStatus.InvalidCredentials ? 'Invalid username or password' : 'Something went wrong';
    return (
        <div>
            <form>
                <div className="form-floating mb-3">
                    <span style={{color: 'red'}}>{loginStatus !== LoginStatus.Initial ? errorSpanText : "\u200b"}</span>
                </div>
                <div className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingUsername" placeholder="Username" onChange={onUsernameChange} />
                    <label htmlFor="floatingUsername">Username</label>
                </div>
                <div className="form-floating mb-3">
                    <input type="password" className="form-control" id="floatingPassword" placeholder="Password" onChange={onPasswordChange} />
                    <label htmlFor="floatingPassword">Password</label>
                </div>
                <button type="submit" className="btn btn-primary btn-block" onClick={loginButtonClick}>Sign in</button>
            </form>
        </div>
    );
}

export default LoginPanel;