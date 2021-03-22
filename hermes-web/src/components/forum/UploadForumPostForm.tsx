import { ENGINE_METHOD_ALL } from 'constants';
import React, { useReducer } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
import { ForumPostApi } from '../../api/ForumPostApi';
import { useLanguagesQuery } from '../../services/queries-service';

type createPostFormProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function UploadForumPostForm ({onError, history}: createPostFormProps) {

    const { data: languagesData } = useLanguagesQuery();

    const [{title, text, languageID }, dispatch] = useReducer(reducer, {
        title: '',
        text: '',
        languageID: 'ENG',
    });

function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    ForumPostApi.upload({title, text, languageID })
    .then(() => history.push(`/forum/`))
    .catch(onError);
}

function handleInputChange (event: React.ChangeEvent<HTMLInputElement>) {
    switch (event.currentTarget.name) {
        case 'title': dispatch({ _type: 'CHANGE_TITLE', title: event.currentTarget.value}); break;
        case 'text': dispatch({ _type: 'CHANGE_TEXT', text: event.currentTarget.value}); break;
        case 'languageID': dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value}); break;
    }
}

return(
    <Form onSubmit={handleSubmit} className="app-upload-forum-post-form">
        <FormGroup className="app-upload-forum-post-form-title">
            <Label for="title">Title</Label>
            <Input type='text' name='title' id='title' value={title} onChange={handleInputChange} required/>
        </FormGroup>
        <FormGroup className="app-upload-forum-post-form-text">
            <Label for="text">Text</Label>
            <Input type='textarea' name='text' id='text' value={text} onChange={handleInputChange} required/>
        </FormGroup>
        <Row>
            <Col>
                <Row className="app-upload-forum-post-form-language">
                    <Label for="languageID">Language</Label>
                    <Input type='select' name='languageID' id='languageID' value={languageID} onChange={handleInputChange}>
                        {
                        languagesData !== undefined &&
                            languagesData.map((languages, index) =>
                            <option key={index} value={languages.languageID}>{languages.description}</option>

                        )}   
                    </Input>
                </Row>
            </Col>
        </Row> 
        <FormGroup>
            <Button color='primary'>Upload</Button>
            <Button className="ml-2" color='secondary' onClick={() => history.goBack()}>Cancel</Button>
        </FormGroup>     
    </Form>
)
}

type State = {
    title: string;
    text: string;
    languageID: string;
}

type Action = 
| { _type: 'CHANGE_TITLE', title: string }
| { _type: 'CHANGE_TEXT', text: string}
| { _type: 'CHANGE_LANGUAGE_ID', languageID:string}
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch(action._type) {
        case 'CHANGE_TITLE': return { ...state, title: action.title };
        case 'CHANGE_TEXT': return { ...state, text: action.text};
        case 'CHANGE_LANGUAGE_ID': return { ...state, languageID: action.languageID}
        case 'SUBMIT': return {... state, title:'', text:'', languageID:''}
    }
}