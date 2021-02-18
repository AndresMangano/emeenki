import React, { useReducer } from 'react';
import { Container, Col, Row, ButtonGroup, Button } from 'reactstrap';
import { LogInForm } from '../../components/LoginForm';
import { PageHeader } from '../../components/PageHeader';
import DiscordButton from '../../components/DiscordButton';
import { RegisterProfileData } from '../../components/RegisterProfileData';
import { RegisterMain } from '../../components/RegisterMain';
import { useLanguagesQuery } from '../../services/queries-service';
import { RouteComponentProps } from 'react-router-dom';

type LoginViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function LoginView({ onError, history }: LoginViewProps) {
    const [{ form, signInType, password, email }, dispatch] = useReducer(reducer, {
        form: 'login',
        signInType: 'password',
        password: '',
        email: ''
    });
    const { data: languagesData } = useLanguagesQuery();

    function handleGoogleSignIn(email: string) {
        dispatch({ _type: 'SELECT_GOOGLE_SIGN_IN', email });
    }
    function handlePasswordSignIn(email: string, password: string) {
        dispatch({ _type: 'SELECT_PASSWORD_SIGN_IN', email, password });
    }

    return (
        <Container>
            <PageHeader badge='v2.0'>
                Emeenki
            </PageHeader>
            <Row>
                <Col md={{ size: 4, offset: 4}}>
                    <ButtonGroup className="mb-1 mr-2">
                        <Button color="primary" onClick={() => dispatch({ _type: 'SWITCH_LOGIN' })} active={form === 'login'}>LogIn</Button>
                        <Button color="primary" onClick={() => dispatch({ _type: 'SWITCH_REGISTER'})} active={form === 'register'}>Register</Button>
                    </ButtonGroup>
                    <DiscordButton channel='https://discord.gg/yVqVhMAMM5'/>
                    { (form === 'register') &&
                        <RegisterMain
                            onError={onError}
                            onGoogleSignIn={handleGoogleSignIn}
                            onPasswordSignIn={handlePasswordSignIn}
                        />
                    }
                    { (form === 'profile' && languagesData !== undefined ) &&
                        <RegisterProfileData
                            onError={onError}
                            signInType={signInType}
                            onSubmit = {() => dispatch({ _type: 'SWITCH_LOGIN' })}                                   
                            email={email}
                            password={password}
                            languages={languagesData}
                        />
                    }
                    { (form === 'login') &&
                        <LogInForm onError={onError}/>
                    }
                </Col>
            </Row>
        </Container>
    );
}

type State = {
    form: 'login'|'register'|'profile';
    signInType: 'google'|'password';
    password: string;
    email: string;
}
type Action =
| { _type: 'SELECT_GOOGLE_SIGN_IN', email: string }
| { _type: 'SELECT_PASSWORD_SIGN_IN', email: string, password: string }
| { _type: 'SWITCH_LOGIN' }
| { _type: 'SWITCH_REGISTER' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'SELECT_GOOGLE_SIGN_IN': return { ...state, form: 'profile', signInType: 'google', email: action.email };
        case 'SELECT_PASSWORD_SIGN_IN': return { ...state, form: 'profile', signInType: 'password', email: action.email, password: action.password };
        case 'SWITCH_LOGIN': return { ...state, form: 'login' };
        case 'SWITCH_REGISTER': return { ...state, form: 'register' };
    }
}