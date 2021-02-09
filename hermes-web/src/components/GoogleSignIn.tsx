import React from 'react';
import { GoogleLogin } from 'react-google-login';

const clientId = process.env.REACT_APP_GOOGLE_CLIENT_ID;

type GoogleSignInProps = {
    onSuccess: (email: string) => void;
}
export function GoogleSignIn({ onSuccess }: GoogleSignInProps)  {
    function handleSuccess(result: any) {
        localStorage.setItem('hermes.googleToken', result.tokenId);    
        onSuccess(result.profileObj.email);
    }
    function handleFailure(result: any) {
        console.log(result.error);
    }
    return (
        <GoogleLogin
            clientId={clientId || ''}
            onSuccess={handleSuccess}
            onFailure={handleFailure}
            buttonText="Sign In"
            cookiePolicy={'single_host_origin'}
        />
    );
}
