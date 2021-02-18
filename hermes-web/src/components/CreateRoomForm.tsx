import React, { useState, useEffect, useReducer } from 'react';
import { Form, FormGroup, Label, Input, Container, Row, Col, Button, CustomInput } from 'reactstrap';
import {  Link } from 'react-router-dom';
import { LanguageDTO } from '../api/StaticAPI';

type CreateRoomFormProps = {
    onCreateRoom: (roomID: string, language1ID: string, language2ID: string, usersLimit: number, restricted: boolean) => void;
    onCancel: () => void;
    languages: LanguageDTO[];
}
export function CreateRoomForm({ languages, onCreateRoom, onCancel }: CreateRoomFormProps) {
    const [{ roomID, languageID1, languageID2, usersLimit, restricted }, dispatch] = useReducer(reducer, {
        roomID: '',
        languageID1: 'ENG',
        languageID2: 'SPA',
        usersLimit: 0,
        restricted: false
    });

    function handleCreate(event: React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        onCreateRoom(roomID, languageID1, languageID2, usersLimit, restricted);
    }
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'roomID': dispatch({ _type: 'CHANGE_ROOM_ID', roomID: event.currentTarget.value }); break;
            case 'languageID1': dispatch({ _type: 'CHANGE_LANGUAGE_ID1', languageID: event.currentTarget.value }); break;
            case 'languageID2': dispatch({ _type: 'CHANGE_LANGUAGE_ID2', languageID: event.currentTarget.value }); break;
            case 'usersLimit': dispatch({ _type: 'CHANGE_USERS_LIMIT', usersLimit: event.currentTarget.valueAsNumber }); break;
            case 'restricted': dispatch({ _type: 'CHANGE_ACCESS', restricted: event.currentTarget.checked }); break;
        }
    }
    function renderLanguageOptions(filter:string) {
        return languages.filter(x => x.languageID !== filter).map((e, index) =>
            <option key={index} value={e.languageID}>{e.description}</option>
        );
    }

    return(
        <Form>
            <FormGroup>
                <Label for='roomID'>Room ID#</Label>
                <Input type='text' name='roomID' id='roomID' value={roomID} onChange={handleInputChange} />
            </FormGroup>
            <FormGroup>
                <Label>Languages</Label>
                <Row>
                    <Col md={6}>
                        <Input type='select' name='languageID1' value={languageID1} onChange={handleInputChange}>
                            {
                                renderLanguageOptions(languageID2)
                            }
                        </Input>
                    </Col>
                    <Col md={6}>
                        <Input type='select' name='languageID2' value={languageID2} onChange={handleInputChange}>
                            {
                                renderLanguageOptions(languageID1)
                            }
                        </Input>
                    </Col>
                </Row>
            </FormGroup>
            <Row>
                <Col md={8}>
                    <FormGroup>
                        <Label for='usersLimit'>Users Limit</Label>
                        <Input type='number' name='usersLimit' id='usersLimit' value={usersLimit} onChange={handleInputChange} />
                        <CustomInput type='switch' name='restricted' id='restricted' inline label='Restricted' checked={restricted} onChange={handleInputChange} />
                    </FormGroup>
                </Col>
            </Row>
            <Row className='justify-content-center'>
                <Button color='primary' onClick={handleCreate}>Create</Button>
                <Button color='secondary' onClick={onCancel}>Cancel</Button>
            </Row>
        </Form>
    );
}

type State = {
    roomID: string;
    languageID1: string;
    languageID2: string;
    usersLimit: number;
    restricted: boolean;
}
type Action = 
| { _type: 'CHANGE_ROOM_ID', roomID: string }
| { _type: 'CHANGE_LANGUAGE_ID1', languageID: string }
| { _type: 'CHANGE_LANGUAGE_ID2', languageID: string }
| { _type: 'CHANGE_USERS_LIMIT', usersLimit: number }
| { _type: 'CHANGE_ACCESS', restricted: boolean }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_ROOM_ID': return { ...state, roomID: action.roomID };
        case 'CHANGE_LANGUAGE_ID1': return { ...state, languageID1: action.languageID };
        case 'CHANGE_LANGUAGE_ID2': return { ...state, languageID2: action.languageID };
        case 'CHANGE_USERS_LIMIT': return { ...state, usersLimit: action.usersLimit };
        case 'CHANGE_ACCESS': return { ...state, restricted: action.restricted };
        case 'SUBMIT': return { ...state, roomID: '', languageID1: 'ENG', languageID2: 'SPA', usersLimit: 0, restricted: false };
    }
}