import React, { useState, useReducer } from 'react';
import { Form, FormGroup, Label, Input, Button, Card, CardBody } from 'reactstrap';
import { UserAPI } from '../api/UserAPI';
import { GoogleSignIn } from './GoogleSignIn';

type LoginFormProps = {
    onError: (error: any) => void;
    onSubmit?: (userID: string) => void;
}
export function LogInForm({ onSubmit, onError }: LoginFormProps) {
    const [{ userID, password }, dispatch] = useReducer(reducer, {
        userID: '',
        password: ''
    });

    function handleLoginWithPassword(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        UserAPI.logInWithPassword({ userID, password })
        .then(() => {
            onSubmit && onSubmit(userID);
            dispatch({ _type: 'SUBMIT' });
        })
        .catch(onError);
    };
    function handleLoginWithGoogle(email: string) {
        UserAPI.logInWithGoogle()
        .then(result => {
            onSubmit && onSubmit(result.userID);
        })
        .catch(onError);
    }
    function handleInputChange(event: React.FormEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'userID': dispatch({ _type: 'CHANGE_USER_ID', userID: event.currentTarget.value }); break;
            case 'password': dispatch({ _type: 'CHANGE_PASSWORD', password: event.currentTarget.value }); break;
        }
    }

    return (
        <Card>
            <CardBody>
                <Form onSubmit={handleLoginWithPassword}>
                    <FormGroup>
                        <Label for='userID'>Username</Label>
                        <Input type='text' name='userID' id='userID' value={userID} onChange={handleInputChange} required/>
                    </FormGroup>
                    <FormGroup>
                        <Label for='password'>Password</Label>
                        <Input type='password' name='password' id='password' value={password} onChange={handleInputChange} required/>
                    </FormGroup>
                    <Button color='primary'>Log In</Button>
                </Form>
            </CardBody>
            <CardBody className="GoogleButton">
                <GoogleSignIn
                    onSuccess={handleLoginWithGoogle}
                    buttonText='Log In'
                />
            </CardBody>
        </Card>
    );
}

type State = {
    userID: string;
    password: string;
}
type Action =
| { _type: 'CHANGE_USER_ID', userID: string }
| { _type: 'CHANGE_PASSWORD', password: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_USER_ID': return { ...state, userID: action.userID };
        case 'CHANGE_PASSWORD': return { ...state, password: action.password };
        case 'SUBMIT': return { ...state, userID: '', password: '' };
    }
}