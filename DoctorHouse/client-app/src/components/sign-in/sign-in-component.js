import React, { useState } from 'react';
import '../../styles/common.css';
import LoginForm from './index.js';
import { home } from '../common/routes-directions';
import { withRouter } from 'react-router';

const SignInComponent = (props) => {
    const [signIn, setSignIn] = useState({});
    const [ready, setReady] = useState(false);

    const submit = values => {
        console.log("Value submit", values);
        setSignIn(values); //окремий стейт.
        setReady(true);
    };

    if (ready) {
        props.history.push(home);
        //console.log('Validation login', signIn);
    }

    return (
        <LoginForm submit={submit} />
    )
};

export default withRouter(SignInComponent);