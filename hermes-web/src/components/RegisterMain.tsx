import React, { useReducer } from 'react';
import { CardBody, Card, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { GoogleSignIn } from './GoogleSignIn';

type RegisterMainProps = {
    onError: (error: string) => void;
    onGoogleSignIn: (email: string) => void;
    onPasswordSignIn: (email: string, password: string) => void;
}

export function RegisterMain({ onError, onGoogleSignIn, onPasswordSignIn }: RegisterMainProps) {
    const [{ email, password }, dispatch] = useReducer(reducer, {
        email: '',
        password: ''
    });

    function handleGoogleSubmit(email: string) {
        onGoogleSignIn(email);
    }
    function handlePasswordSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onPasswordSignIn(email, password);
    };
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'email': dispatch({ _type: 'CHANGE_EMAIL', email: event.currentTarget.value }); break;
            case 'password': dispatch({ _type: 'CHANGE_PASSWORD', password: event.currentTarget.value }); break;
        }
    }

    return (
        <Card>
            <CardBody>
                <Form onSubmit={handlePasswordSubmit}>
                    <FormGroup>
                        <Label for='email'>Email</Label>
                        <Input type='email' name='email' id='email' value={email} onChange={handleInputChange} required/>
                    </FormGroup>
                    <FormGroup>
                        <Label for='password'>Password</Label>
                        <Input type='password' name='password' id='password' value={password} onChange={handleInputChange} required/>
                    </FormGroup>
                    <Button color='primary'>Register</Button>
                </Form>
            </CardBody>
            <CardBody className="GoogleButton">
                <GoogleSignIn
                    onSuccess={handleGoogleSubmit}
                />
            </CardBody>
        </Card>
    );
}

type State = {
    email: string;
    password: string;
}
type Action =
| { _type: 'CHANGE_EMAIL', email: string }
| { _type: 'CHANGE_PASSWORD', password: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_EMAIL': return { ...state, email: action.email };
        case 'CHANGE_PASSWORD': return { ...state, password: action.password };
        case 'SUBMIT': return { ...state, email: '', password: '' };
    }
}