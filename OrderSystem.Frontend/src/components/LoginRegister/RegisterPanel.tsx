import React, { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import 'bootstrap/dist/css/bootstrap.css';

import LoginRequest from "../../api/models/Login/LoginRequest";
import OrderSystemAPI from "../../api/OrderSystemAPI";
import { login } from '../../common/UserState';

enum RegisterStatus {
    Initial,
    UserExists,
    InternalError
}

enum PasswordsMatch {
    Initial,
    Match,
    DontMatch
}

const RegisterPanel: React.FC = () => {
    const dispatch = useDispatch();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");
    const [registerStatus, setRegisterStatus] = useState(RegisterStatus.Initial);
    const [passwordsMatch, setPasswordMatch] = useState(PasswordsMatch.Initial);

    const onUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setRegisterStatus(RegisterStatus.Initial);
        setUsername(e.currentTarget.value);
    }

    const onPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setRegisterStatus(RegisterStatus.Initial);
        setPassword(e.currentTarget.value);
    }

    const onPasswordBlur = (e: React.FocusEvent<HTMLInputElement>) => {
        setPasswordMatch(e.currentTarget.value !== repeatPassword ? PasswordsMatch.DontMatch : PasswordsMatch.Match);
    }

    const onRepeatPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPasswordMatch(e.currentTarget.value !== password ? PasswordsMatch.DontMatch : PasswordsMatch.Match);
        setRepeatPassword(e.currentTarget.value);
    }

    const registerButtonClick = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        e.preventDefault();

        if(password !== repeatPassword)
        {
            setPasswordMatch(PasswordsMatch.DontMatch);
            return;
        }

        const request: LoginRequest = {
            username: username,
            password: password
        }

        try {
            const response = await OrderSystemAPI.RegisterAsync(request);
            const {accessToken } = response;
            dispatch(login(accessToken));
        }
        catch(error) {
            const err = error as Error
			setRegisterStatus(err === undefined || err.message !== '401' ? RegisterStatus.InternalError : RegisterStatus.UserExists);
        }
    }

    const errorSpanText = registerStatus === RegisterStatus.UserExists ? 'User with such username exists' : 'Something went wrong';
    return (
        <div>
            <form>
                <div className="form-floating mb-3">
                    <span style={{color: 'red'}}>{registerStatus !== RegisterStatus.Initial ? errorSpanText : "\u200b"}</span>
                </div>

                <div className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingUsername" placeholder="Username" onChange={onUsernameChange} />
                    <label htmlFor="floatingUsername">Username</label>
                </div>

                <div className="form-floating mb-3">
                    <input type="password" className="form-control" id="floatingPassword" placeholder="Password" onChange={onPasswordChange} onBlur={onPasswordBlur} />
                    <label htmlFor="floatingPassword">Password</label>
                </div>

                <div className="form-floating mb-3">
                    <input type="password" className={`form-control${passwordsMatch === PasswordsMatch.DontMatch ? ' is-invalid' : ''}`} id="floatingRepeatPassword" placeholder="Repeat Password" onChange={onRepeatPasswordChange} />
                    <label htmlFor="floatingRepeatPassword">Repeat password</label>
                </div>

                <button type="submit" className="btn btn-primary btn-block mb-3" onClick={registerButtonClick}>Register</button>
            </form>
        </div>
    );
}

export default RegisterPanel;