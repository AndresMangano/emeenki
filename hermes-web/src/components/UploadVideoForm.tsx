import React, { useReducer } from 'react';
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { ArticleTemplateAPI } from '../api/ArticleTemplateAPI';
import { useLanguagesQuery, useTopicsQuery } from '../services/queries-service';
import { RouteComponentProps } from 'react-router-dom';

type UploadVideoFormProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function UploadVideoForm({ onError, history }: UploadVideoFormProps) {
    const { data: languagesData } = useLanguagesQuery();
    const { data: topicsData } = useTopicsQuery();
    const [{ languageID, topicID, youtubeURL, isLoading }, dispatch] = useReducer(reducer, {
        languageID: 'ENG',
        topicID: 'OTH',
        youtubeURL: '',
        isLoading: false
    });
    
    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        dispatch({ _type: 'SET_LOADING', isLoading: true });
        
        ArticleTemplateAPI.uploadVideo({ languageID, topicID, youtubeURL })
            .then((response) => {
                const articleTemplateID = response.data.articleTemplateID;
                history.push(`/video-translate/${articleTemplateID}`);
            })
            .catch((error) => {
                dispatch({ _type: 'SET_LOADING', isLoading: false });
                onError(error);
            });
    }
    
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID': 
                dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value }); 
                break;
            case 'topicID': 
                dispatch({ _type: 'CHANGE_TOPIC_ID', topicID: event.currentTarget.value }); 
                break;
            case 'youtubeURL': 
                dispatch({ _type: 'CHANGE_YOUTUBE_URL', youtubeURL: event.currentTarget.value }); 
                break;
        }
    }
    
    return (
        <Form onSubmit={handleSubmit}>
            <FormGroup>
                <Label for='languageID'>Video Language</Label>
                <Input 
                    type='select' 
                    name='languageID' 
                    id='languageID' 
                    value={languageID} 
                    onChange={handleInputChange}
                    disabled={isLoading}
                >
                    {languagesData &&
                        languagesData.map((e, index) =>
                            <option key={index} value={e.languageID}>{e.description}</option>
                        )}
                </Input>
            </FormGroup>
            <FormGroup>
                <Label for='topicID'>Topic</Label>
                <Input 
                    type='select' 
                    name='topicID' 
                    id='topicID' 
                    value={topicID} 
                    onChange={handleInputChange}
                    disabled={isLoading}
                >
                    {topicsData &&
                        topicsData.map((e, index) =>
                            <option key={index} value={e.topicID}>{e.name}</option>
                        )}
                </Input>
            </FormGroup>
            <FormGroup>
                <Label size='lg' for='youtubeURL'>YouTube URL</Label>
                <Input 
                    bsSize='lg' 
                    type='url' 
                    name='youtubeURL' 
                    id='youtubeURL' 
                    value={youtubeURL} 
                    onChange={handleInputChange} 
                    placeholder='https://www.youtube.com/watch?v=...'
                    required
                    disabled={isLoading}
                />
            </FormGroup>
            <FormGroup>
                <Button color='primary' disabled={isLoading}>
                    {isLoading ? 'Processing...' : 'Upload Video'}
                </Button>
                <Button color='secondary' onClick={() => history.goBack()} disabled={isLoading}>
                    Cancel
                </Button>
            </FormGroup>
        </Form>
    );
}

type State = {
    languageID: string;
    topicID: string;
    youtubeURL: string;
    isLoading: boolean;
}

type Action =
    | { _type: 'CHANGE_LANGUAGE_ID', languageID: string }
    | { _type: 'CHANGE_TOPIC_ID', topicID: string }
    | { _type: 'CHANGE_YOUTUBE_URL', youtubeURL: string }
    | { _type: 'SET_LOADING', isLoading: boolean }

function reducer(state: State, action: Action): State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': 
            return { ...state, languageID: action.languageID };
        case 'CHANGE_TOPIC_ID': 
            return { ...state, topicID: action.topicID };
        case 'CHANGE_YOUTUBE_URL': 
            return { ...state, youtubeURL: action.youtubeURL };
        case 'SET_LOADING':
            return { ...state, isLoading: action.isLoading };
    }
}




