import moment from 'moment'
import React, { useReducer } from 'react'
import { Link } from 'react-router-dom'
import { Button, Col, Container, Form, Input, Row } from 'reactstrap'
import { LanguageFlag } from '../LanguageFlag'

type ForumThreadProps = {
    onDelete:(forumPostID: string) => void;
    onSubmitEdit:(forumPostID: string, text: string) => void;
    forumPostID: string;
    profilePhoto: string,
    userID: string;
    title: string;
    text: string;
    languageID: string;
    timestamp: Date;
}

export function ForumThread ({profilePhoto, userID, languageID, title, text, timestamp, onDelete, onSubmitEdit, forumPostID }: ForumThreadProps) {
    
    const [{isEditBoxOpen, editedText }, dispatch] = useReducer(reducer, {
        isEditBoxOpen: false,
        editedText: text,
    });

    const userLoggedID = localStorage.getItem("hermes.userID") || "";

    function handleOnSubmitEdit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmitEdit(forumPostID, editedText);
        dispatch({ _type: 'TOGGLE_EDIT_BOX'});
    }

    function handleEditBoxToggle () {
        dispatch({ _type: 'TOGGLE_EDIT_BOX'});
    }

    function handleInputChange (event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'CHANGE_EDITED_TEXT', editedText: event.currentTarget.value})
    }

    return (
       <Container className= 'app-forum-threadpage'>
            <Row className='d-flex align-items-center border-bottom border-grey'>
                <Col md={12} className='app-forum-threadpage-title d-flex justify-content-center'>
                    <p>
                        {title}
                    </p>
                </Col>
            </Row>
            <Row className="d-flex justify-content-center mt-5">
                <Col md={3} className='d-flex flex-column align-self-center'>
                    <img
                        className='d-flex align-self-center'
                        src={profilePhoto === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhoto}
                        alt='profile photo'
                        style={{ width: '71px', height: '71px', borderRadius: '50%', objectFit:'cover', marginRight:'5px'}}
                    />
                    <Link className="app-forum-threadpage-user d-flex align-self-center" to={`/profile/${userID}`}><strong>{userID}</strong></Link>
                    <td className='d-flex align-self-center'><LanguageFlag languageID={languageID} size="16px"/></td>   
                </Col>
                <Col md={9}className="app-forum-threadpage-text">
                    { isEditBoxOpen === false &&
                        <p>
                            {text}
                        </p>
                    }
                    {   isEditBoxOpen === true &&
                        <Form onSubmit={handleOnSubmitEdit}>
                            <Input bsSize="lg" type='textarea' name='edit' id='edit' value={editedText} onChange={handleInputChange} />
                            <Button color="primary">Edit</Button>
                            <Button color="danger" onClick={handleEditBoxToggle}>Cancel</Button>
                        </Form>
                    }
                </Col>
            </Row>
            <Row className='mt-4'>
                <Col md={3}  className='d-flex justify-content-center'>
                    <p>Posted {`${moment.utc(timestamp).fromNow()}`}</p>
                </Col>
                { (userID === userLoggedID) && (isEditBoxOpen === false ) && 
                    <Col className='d-flex justify-content-end'> 
                        <Button className='mr-4' color='primary' onClick={() => onDelete(forumPostID)}>Delete</Button>
                        <Button color='secondary' onClick={handleEditBoxToggle}>Edit</Button>
                    </Col>
                }
            </Row>
            <hr />
        </Container>
    )
}

type State = {
    isEditBoxOpen: boolean;
    editedText: string;
}

type Action = 
| { _type:'TOGGLE_EDIT_BOX'}
| { _type: 'CHANGE_EDITED_TEXT', editedText: string;}
| { _type: 'RESET'}

function reducer(state: State, action: Action) : State {
    switch(action._type) {
        case 'TOGGLE_EDIT_BOX': return {...state, isEditBoxOpen: !state.isEditBoxOpen};
        case 'CHANGE_EDITED_TEXT': return { ...state, editedText: action.editedText}
        case 'RESET': return {...state, editedText: ''}
    }
}