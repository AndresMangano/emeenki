import React, { useState, useReducer } from 'react';
import { CardBody, Card, CardImg, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { UserAPI } from '../api/UserAPI';

type RegisterProfileDataProps = {
    onError: (error: any) => void;
    onSubmit: () => void;
    email: string,
    signInType: 'google' | 'password',
    password: string,
    languages: {
        languageID: string;
        description: string;
    }[]
}
export function RegisterProfileData({ onError, onSubmit, email, signInType, password, languages }: RegisterProfileDataProps) {
    const [{ userID, profilePhotoURL, languageID }, dispatch] = useReducer(reducer, {
        userID: '',
        profilePhotoURL: '',
        languageID: languages[0].languageID
    });

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        if (signInType === 'google'){
            console.log("Register with "+email);
            UserAPI.registerWithGoogle({
                userID: userID,
                googleEmail: email,
                profilePhotoURL: profilePhotoURL,
                languageID: languageID
            })
            .then(() => onSubmit())
            .catch(onError);
        } else if (signInType === 'password'){
            UserAPI.registerWithPassword({
                userID: userID,
                password: password,
                profilePhotoURL: profilePhotoURL,
                languageID: languageID
            })
            .then(() => onSubmit())
            .catch(onError);
        }
        dispatch({ _type: 'SUBMIT' });
    };
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'userID': dispatch({ _type: 'CHANGE_USER_ID', userID: event.currentTarget.value }); break;
            case 'profilePhotoURL': dispatch({ _type: 'CHANGE_PROFILE_PHOTO_URL', profilePhotoURL: event.currentTarget.value }); break;
            case 'languageID': dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value }); break;
        }
    }

    return (
        <Card>
            { profilePhotoURL !== '' &&
                <CardImg top width='100%' src={profilePhotoURL} alt='Profile Photo'/>
            }
            <CardBody>
                <Form onSubmit={handleSubmit}>
                    <FormGroup>
                        <Label for='userID'>Username</Label>
                        <Input type='text' name='userID' id='userID' value={userID} onChange={handleInputChange} required/>
                    </FormGroup>
                    <FormGroup>
                        <Label for='profilePhotoURL'>Photo URL</Label>
                        <Input type='url' name='profilePhotoURL' id='profilePhotoURL' value={profilePhotoURL} onChange={handleInputChange}/>
                    </FormGroup>
                    <FormGroup>
                        <Label for='languageID'>Native Language</Label>
                        <Input type='select' name='languageID' id='languageID' value={languageID} onChange={handleInputChange} required>
                            { languages.map((e, index) =>
                                <option key={index} value={e.languageID}>{e.description}</option>
                            )}
                        </Input>
                    </FormGroup>
                    <Button color='primary'>Register</Button>
                </Form>
            </CardBody>
        </Card>
    );
}

type State = {
    userID: string;
    profilePhotoURL: string;
    languageID: string;
}
type Action =
| { _type: 'CHANGE_USER_ID', userID: string }
| { _type: 'CHANGE_PROFILE_PHOTO_URL', profilePhotoURL: string }
| { _type: 'CHANGE_LANGUAGE_ID', languageID: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_USER_ID': return { ...state, userID: action.userID };
        case 'CHANGE_PROFILE_PHOTO_URL': return { ...state, profilePhotoURL: action.profilePhotoURL };
        case 'CHANGE_LANGUAGE_ID': return { ...state, languageID: action.languageID };
        case 'SUBMIT': return { ...state, userID: '', profilePhotoURL: '' };
    }
}