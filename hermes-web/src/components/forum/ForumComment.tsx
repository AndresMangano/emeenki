import moment from 'moment';
import React, { useReducer } from 'react';
import { Link } from 'react-router-dom';
import { Button, Col, Media, Row } from 'reactstrap';
import { ForumCommentForm } from './ForumCommentForm';



type forumCommentType = {
    onSubmit: (comment: string, commentIndex: number|null) => void;
    onDelete: (commentIndex: number ) => void;
    userID: string;
    profilePhoto: string;
    commentIndex: number;
    comment: string;
    timestamp: Date;
}

export function ForumComment ({userID, profilePhoto, commentIndex, comment, timestamp, onSubmit, onDelete}: forumCommentType ) {
    const userLoggedID = localStorage.getItem("hermes.userID") || "";
    const [{ openedCommentForm }, dispatch] = useReducer (reducer, {
        openedCommentForm: null,
    });

    function handleSubmit (comment: string, commentIndex: number|null) {
        onSubmit(comment, commentIndex);
        dispatch({ _type: 'TOGGLE_COMMENT_REPLY_BOX', commentFormIndex: null})
    }

    function handleCommentReply (commentFormIndex: number) {
        dispatch({ _type: 'TOGGLE_COMMENT_REPLY_BOX', commentFormIndex: openedCommentForm === commentFormIndex ? null : commentFormIndex });
    }

    return (
        <>
        <Media className='m-3' heading>
            <Media className="PostsCommentsUpperPart">
                <img className="PostsCommentsPhoto" src={profilePhoto === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhoto} alt=''/>
                <Link className='PostsCommentsUserData' to={`/profile/${userID}`}>{`${userID}`}</Link>
                <big className='PostsCommentsTimeStamp'>{`${moment.utc(timestamp).fromNow()}`}</big>

            </Media>
            <Media body>
                <p className='PostsCommentsText'>{ comment }</p>    
            </Media>
            <Media className="d-flex flex-row justify-content-end" bottom>
                { (userLoggedID === userID) &&
                    <Button className="d-flex ml-auto" size='md' color='transparent' onClick={() => onDelete(commentIndex)}>Delete</Button>
                }
                <Button size='md' color='transparent' onClick={() => handleCommentReply(commentIndex)}><b>Reply</b></Button>
            </Media>
        </Media>

        {
            ( commentIndex === openedCommentForm) &&
                <Row>
                    <Col>
                        <ForumCommentForm 
                            onSubmit= {handleSubmit}
                            commentIndex={commentIndex}
                            showSenderName={userID}
                        />
                    </Col>
                </Row>
        }
        <hr />
    </>
    )
}

type State = {
    openedCommentForm: number|null;
}

type Action = 
| {_type: 'TOGGLE_COMMENT_REPLY_BOX', commentFormIndex: number|null}

function reducer (state: State, action: Action): State {
    switch (action._type) {
        case 'TOGGLE_COMMENT_REPLY_BOX': return {...state, openedCommentForm: action.commentFormIndex }
    }
}