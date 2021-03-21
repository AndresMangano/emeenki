import React, { useReducer } from 'react';
import { Card, CardHeader, CardBody, Form, Row, Button, Input, Label, InputGroup, InputGroupAddon, FormText } from 'reactstrap';
import { RoomAPI } from '../api/RoomAPI';

type RoomUserSettingsProps = {
    onError: (message: string) => void;
    onUpdateUsersLimit: (usersLimit: number) => void;
    roomID: string;
    usersLimit: number;
}
export function RoomUserSettings({ onError, onUpdateUsersLimit, roomID, usersLimit }: RoomUserSettingsProps) {
    const [{ userID, invitationLink, usersLimitField }, dispatch] = useReducer(reducer, {
        userID: '',
        invitationLink: '',
        usersLimitField: usersLimit
    });

    function handleInviteUser() {
        RoomAPI.inviteUser({ roomID, userID })
        .then(response => dispatch({ _type: 'CHANGE_INVITATION_LINK', invitationLink: window.location.host + `/rooms/${roomID}/token/${response.data.token}/${userID}`}))
        .catch(onError);
    }

    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'userID': dispatch({ _type: 'CHANGE_USER_ID', userID: event.currentTarget.value }); break;
            case 'usersLimit': dispatch({ _type: 'CHANGE_USERS_LIMIT', usersLimit: event.currentTarget.valueAsNumber }); break;
        }
    }

    return (
        <Card outline color='primary' className='h-100 d-flex flex-column justify-content-between'>
            <CardHeader className={`bg-info text-center`} tag="h4">
                Room Settings
            </CardHeader>
            <CardBody>
                <Form>
                    <Row form>
                        <Label>Invite</Label>
                        <InputGroup>
                            <InputGroupAddon addonType='prepend'>
                                <Button onClick={handleInviteUser}>Renew Token</Button>
                            </InputGroupAddon>
                            <Input placeholder='Username' name='userID' value={userID} onChange={handleInputChange}/>
                            <InputGroupAddon addonType='append'>
                                <Button disabled={invitationLink === ''} className='oi oi-external-link' onClick={() => navigator.clipboard.writeText(invitationLink)}/>
                            </InputGroupAddon>
                            <FormText>{invitationLink}</FormText>
                        </InputGroup>
                        <FormText>Current link expires in ...</FormText>
                    </Row>
                    <Row form>
                        <Label>Users Limit</Label>
                        <InputGroup>
                            <Input
                                name='usersLimit'
                                type='number'
                                value={usersLimitField}
                                onChange={handleInputChange}
                            />
                                { (usersLimit !== usersLimitField) &&
                                    <InputGroupAddon addonType='append'>
                                        <Button color='info' onClick={() => onUpdateUsersLimit(usersLimitField)}>Update</Button>
                                    </InputGroupAddon>
                                }
                        </InputGroup>
                    </Row>
                </Form>
            </CardBody>
        </Card>
    );
}

type State = {
    userID: string;
    invitationLink: string;
    usersLimitField: number;
}
type Action =
| { _type: 'CHANGE_USER_ID', userID: string }
| { _type: 'CHANGE_INVITATION_LINK', invitationLink: string }
| { _type: 'CHANGE_USERS_LIMIT', usersLimit: number }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_USER_ID': return { ...state, userID: action.userID };
        case 'CHANGE_INVITATION_LINK': return { ...state, invitationLink: action.invitationLink };
        case 'CHANGE_USERS_LIMIT': return { ...state, usersLimitField: action.usersLimit };
        case 'SUBMIT': return { ...state, userID: '', invitationLink: '' };
    }
}