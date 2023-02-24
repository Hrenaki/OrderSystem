import React, { useState } from "react";
import 'bootstrap/dist/css/bootstrap.css';

interface NavButtonProps {
    name: string,
    isActive: boolean,
    onClick: any,
    style: any
}

const NavButton = (props: NavButtonProps) : JSX.Element => {
    const btnClassName = "nav-link" + (props.isActive ? ' active' : '');
    return (
        <button className={btnClassName} data-mdb-toggle="pill" onClick={props.onClick}>{props.name}</button>
    );
}

export default NavButton;