import React, { useState, useReducer } from 'react';
import { Card, CardBody, Form, Input, FormGroup, Label, Button, Row, Col, Media } from 'reactstrap';
import { Link } from 'react-router-dom';
import { TranslationHistory } from './TranslationHistory';
import { LanguageFlag } from './LanguageFlag';

type DetailedTranslationFormProps = {
    onSubmit: (translation: string) => void;
    onUpvote: (translationPos: number) => void;
    onDownvote: (translationPos: number) => void;
    buildCommentsUrl: (translationPos: number) => string;
    cancelUrl: string;
    history: {
        translationPos: number;
        translation: string;
        userID: string;
        profilePhotoURL: string;
        nativeLanguageID: string;
        timestamp: Date;
        upvotesCount: number;
        downvotesCount: number;
        commentsCount: number;
    }[];
}
export function DetailedTranslationForm({ onSubmit, onUpvote, onDownvote, buildCommentsUrl, cancelUrl, history }: DetailedTranslationFormProps) {
    const [{ newTranslation, translation }, dispatch] = useReducer(reducer, {
        newTranslation: history.length === 0,
        translation: history.length === 0 ? '' : history[0].translation
    });

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmit(translation);
        dispatch({ _type: 'SUBMIT' });
    }
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'translation': dispatch({ _type: 'CHANGE_TRANSLATION', translation: event.currentTarget.value }); break;
        }
    }

    return (
        <Card className="TranslationBox">
            <CardBody>
                <Form onSubmit={handleSubmit}>
                    <FormGroup>
                        <Media className="d-flex flex-row justify-content-between" heading>
                        { (!newTranslation && history.length > 0) &&
                                <Button
                                    active={true}
                                    className='LikeButton oi oi-thumb-up mr-2'
                                    color="success"
                                    onClick={() => onUpvote(history[0].translationPos)}
                                >
                                    {history[0].upvotesCount}
                                </Button>
                            }
                        <Label className="mr-auto TranslationNumber" for='translationPos'><b>Translation #</b></Label>
                        { (history.length > 0 && !newTranslation) &&
                            <>
                                <Media>
                                    <img
                                        className='TranslationPhoto'
                                        src={history[0].profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : history[0].profilePhotoURL}
                                        alt='profile photo'
                                    /> 
                                    <Link to={`/profile/${history[0].userID}`} className="mr-auto UserNameTranslation">{history[0].userID}</Link>
                                    <LanguageFlag languageID={history[0].nativeLanguageID} size='24px'/>
                                </Media>
                                <Media/>
                                <p className="h6 ml-auto">Minutes ago</p>
                                <Media/>
                            </>
                        }
                        </Media>
                        <Input
                            type='textarea'
                            name='translation'
                            rows={3}
                            value={translation}
                            onChange={handleInputChange}
                            style={!newTranslation ? { backgroundColor: '#8cffa6'} : {}}
                        />
                    </FormGroup>
                    <Row form>
                        <Col className= "d-flex justify-content-around" md={11}>
                            <Button className="SendButton mr-1 ml-4 rounded-pill p-2.9" color="primary">Submit</Button>
                            <Button className="rounded-pill" tag={Link} color='danger' to={cancelUrl}>Cancel</Button>
                            { history.length !== 0 && <Button className="ml-auto rounded-pill" color='dark' id='historyToggler'>History</Button>}
                        </Col>
                    </Row>
                </Form>
                <>
                    { history.length !== 0 &&
                        <TranslationHistory
                            onUpvote={onUpvote}
                            onDownvote={onDownvote}
                            buildCommentsUrl={buildCommentsUrl}
                            history={history}
                        /> 
                    }
                </>
            </CardBody>
        </Card>
    );
}

type State = {
    newTranslation: boolean;
    translation: string;
}
type Action =
| { _type: 'CHANGE_TRANSLATION', translation: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_TRANSLATION': return { ...state, newTranslation: true, translation: action.translation };
        case 'SUBMIT': return { ...state, newTranslation: false, translation: '' };
    }
}