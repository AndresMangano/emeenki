import React, { useState, useReducer } from 'react';
import { Form, FormGroup, Label, Input, Button, Row, Col } from 'reactstrap';
import { ArticleTemplateAPI } from '../api/ArticleTemplateAPI';
import { useLanguagesQuery, useTopicsQuery } from '../services/queries-service';
import { RouteComponentProps } from 'react-router-dom';

type UploadFormProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function UploadForm({ onError, history }: UploadFormProps) {
    const userID = localStorage.getItem("hermes.userID") || '';
    const { data: languagesData } = useLanguagesQuery();
    const { data: topicsData  } = useTopicsQuery();
    const [{ languageID, topicID, title, text, source, photoURL }, dispatch] = useReducer(reducer, {
        languageID: 'ENG',
        topicID: 'OTH',
        title: '',
        text: '',
        source: '',
        photoURL: ''
    });
    
    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        ArticleTemplateAPI.upload({ languageID, topicID, title, text, source, photoURL, userID })
        .then(() => history.push(`/templates/active`))
        .catch(onError);
    }
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID': dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value }); break;
            case 'topicID': dispatch({ _type: 'CHANGE_TOPIC_ID', topicID: event.currentTarget.value }); break;
            case 'title': dispatch({ _type: 'CHANGE_TITLE', title: event.currentTarget.value }); break;
            case 'text': dispatch({ _type: 'CHANGE_TEXT', text: event.currentTarget.value }); break;
            case 'photoURL': dispatch({ _type: 'CHANGE_PHOTO_URL', photoURL: event.currentTarget.value }); break;
            case 'source': dispatch({ _type: 'CHANGE_SOURCE', source: event.currentTarget.value }); break;
        }
    }
    
    return(
        <Form onSubmit={handleSubmit}>
            <Row className='mb-3'>
                <Col md={6}>
                    { (photoURL !== '') &&
                        <img src={photoURL} alt='Article Photo' style={{ width: '20rem', borderRadius: '1rem'}}/> 
                    }
                </Col>
                <Col md={6}>
                    <Row>
                        <Label for='languageID'>Article Language</Label>
                        <Input type='select' name='languageID' id='languageID' value={languageID} onChange={handleInputChange}>
                            { (languagesData) &&
                                languagesData.map((e, index) =>
                                    <option key={index} value={e.languageID}>{e.description}</option>
                            )}
                        </Input>
                    </Row>
                    <Row>
                        <Label for='topicID'>Topic</Label>
                        <Input type='select' name='topicID' id='topicID' value={topicID} onChange={handleInputChange}>
                            { (topicsData) &&
                                topicsData.map((e, index) =>
                                <option key={index} value={e.topicID}>{e.name}</option>
                            )}
                        </Input>
                    </Row>
                </Col>
            </Row>
            <FormGroup>
                <Label for='photoURL'>Photo URL</Label>
                <Input type='url' name='photoURL' id='photoURL' value={photoURL} onChange={handleInputChange}/>
            </FormGroup>
            <FormGroup>
                <Label size='lg' for='title'>Title</Label>
                <Input bsSize='lg' type='text' name='title' id='title' value={title} onChange={handleInputChange} required/>
            </FormGroup>
            <FormGroup>
                <Label size='lg' for='text'>Text</Label>
                <Input type='textarea' name='text' id='text' rows={8} value={text} onChange={handleInputChange} required/>
            </FormGroup>
            <FormGroup>
                <Label for='source'>Source</Label>
                <Input type='text' name='source' id='source' value={source} onChange={handleInputChange} required/>
            </FormGroup>
            <FormGroup>
                <Button color='primary'>Upload</Button>
                <Button color='secondary' onClick={() => history.goBack()}>Cancel</Button>
            </FormGroup>
        </Form>
    );
}

type State = {
    languageID: string;
    topicID: string;
    title: string;
    text: string;
    source: string;
    photoURL: string;
}
type Action =
| { _type: 'CHANGE_LANGUAGE_ID', languageID: string }
| { _type: 'CHANGE_TOPIC_ID', topicID: string }
| { _type: 'CHANGE_TITLE', title: string }
| { _type: 'CHANGE_TEXT', text: string }
| { _type: 'CHANGE_SOURCE', source: string }
| { _type: 'CHANGE_PHOTO_URL', photoURL: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': return { ...state, languageID: action.languageID };
        case 'CHANGE_TOPIC_ID': return { ...state, topicID: action.topicID};
        case 'CHANGE_TITLE': return { ...state, title: action.title };
        case 'CHANGE_TEXT': return { ...state, text: action.text };
        case 'CHANGE_SOURCE': return { ...state, source: action.source };
        case 'CHANGE_PHOTO_URL': return { ...state, photoURL: action.photoURL };
        case 'SUBMIT': return { ...state, languageID: '', topicID:'', title: '', text: '', source: '', photoURL: '' };
    }
}