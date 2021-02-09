import React, { useReducer } from 'react';
import { Media, Button, Modal, ModalHeader, Container, Row, Col, ModalBody } from 'reactstrap';
import moment from 'moment';
import { ArticleCommentForm } from './ArticleCommentForm';

type ArticleCommentProps = {
    onSubmit: (comment: string, parentCommentPos:number|null) => void;
    onDelete: (commentIndex: number, childCommentPos: number|null) => void;
    actualUserID: string;
    userID: string;
    profilePhotoURL: string;
    commentIndex: number;
    comment: string;
    timestamp: Date;
    replies: {
        profilePhotoURL: string;
        userID: string;
        timestamp: Date;
        comment: string;
        commentIndex: number;
        childCommentIndex: number|null;
    }[]
}
export function ArticleComment({onDelete, onSubmit, actualUserID, userID, profilePhotoURL, commentIndex, comment, timestamp, replies}: ArticleCommentProps) {

    const userLoggedID = localStorage.getItem("hermes.userID") || "";
    const [{isModalCommentRepliesOpen, openedArticleCommentForm}, dispatch] = useReducer(reducer, {
        isModalCommentRepliesOpen: false,
        openedArticleCommentForm: null,
    });

    function handleCommentRepliesToggle() {
        dispatch ({ _type: 'TOGGLE_COMMENT_REPLIES_MODAL'});
    }

    function handleCommentReplyReplies(articleCommentFormIndex: number) {
        dispatch ({ _type: 'TOGGLE_COMMENT_REPLY_REPLIES', articleCommentFormIndex: openedArticleCommentForm === articleCommentFormIndex ? null : articleCommentFormIndex});
    }

    function handleReplySubmit(comment: string, parentCommesPos: number|null) {
        onSubmit(comment, parentCommesPos);
        dispatch({_type: 'TOGGLE_COMMENT_REPLY_REPLIES', articleCommentFormIndex: null})
    }

    return (
        <>
            <Media className='m-3' heading>
                <Media className="PostsCommentsUpperPart">
                    <img className="PostsCommentsPhoto" src={profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhotoURL} alt=''/>
                    <big className='PostsCommentsUserData'>{`${userID}`}</big>
                    <big className='PostsCommentsTimeStamp'>{`${moment.utc(timestamp).fromNow()}`}</big>

                </Media>
                <Media body>
                    <p className='PostsCommentsText'>{ comment }</p>    
                </Media>
                <Media className="d-flex flex-row justify-content-end" bottom>
                    { (actualUserID === userID) &&
                        <Button className="d-flex ml-auto" size='md' color='transparent' onClick={() => onDelete(commentIndex, null)}>Delete</Button>
                    }
                    <Button size='md' color='transparent' onClick={handleCommentRepliesToggle}><b>Reply</b></Button>
                </Media>
            </Media>
            <hr />
            {  
                isModalCommentRepliesOpen && 
                    <Modal isOpen={true} toggle={handleCommentRepliesToggle}>
                        <ModalHeader>
                            Replies
                        </ModalHeader>
                        <ModalBody>
                            <Container>
                                <Media className='m-3' heading>
                                    <Media className="PostsCommentsUpperPart">
                                        <img className="PostsCommentsPhoto" src={profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhotoURL} alt=''/>
                                        <big className='PostsCommentsUserData mr-auto'>{`${userID}`}</big>
                                        <big className='PostsCommentsTimeStamp'>{`${moment.utc(timestamp).fromNow()}`}</big>
                                    </Media>
                                    <Media body>
                                        <p className='PostsCommentsText'>{comment}</p>    
                                    </Media>
                                </Media>
                                <Row>
                                    <Col>
                                        <ArticleCommentForm 
                                            onSubmit={onSubmit} 
                                            parentCommentPos={commentIndex}
                                        />
                                    </Col>
                                </Row>
                                {
                                    replies.map((r) =>
                                        <Media key={r.childCommentIndex!} className='m-3' heading>
                                            <Media className="PostsCommentsUpperPart">
                                                <img className="PostsCommentsPhoto" src={r.profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : r.profilePhotoURL} alt=''/>
                                                <big className='PostsCommentsUserData mr-auto'>{`${r.userID}`}</big>
                                                <big className='PostsCommentsTimeStamp'>{`${moment.utc(r.timestamp).fromNow()}`}</big>
                                            </Media>
                                            <Media body>
                                                <p className='PostsCommentsText'>{r.comment}</p>    
                                            </Media>
                                            <Media className='app-modal-reply-id'>
                                                 { (r.userID === userLoggedID) &&
                                                    <Button className="d-flex ml-auto" size='md' color='transparent' onClick={() => onDelete(commentIndex, r.childCommentIndex)}><b>Delete</b></Button>
                                                 }
                                                 { (r.userID !== userLoggedID) &&
                                                 <Button className="d-flex ml-auto" size="md" color="transparent" onClick={() => handleCommentReplyReplies(r.childCommentIndex!)}>Reply</Button>
                                                 }
                                            </Media>
                                                {
                                                    (r.childCommentIndex === openedArticleCommentForm) &&
                                                        <Row>
                                                            <Col>
                                                                <ArticleCommentForm 
                                                                    onSubmit={handleReplySubmit} 
                                                                    parentCommentPos={commentIndex}
                                                                    showSenderName={r.userID}
                                                                />
                                                            </Col>
                                                        </Row>
                                                }
                                            <hr />
                                        </Media>)
                                }
                            </Container>
                        </ModalBody>
                    </Modal>
            }
        </>
    );
}

type State = {
    isModalCommentRepliesOpen: boolean;
    openedArticleCommentForm: number|null;
}

type Action = 
| {_type: 'TOGGLE_COMMENT_REPLIES_MODAL'} 
| {_type: 'TOGGLE_COMMENT_REPLY_REPLIES', articleCommentFormIndex: number|null}
function reducer(state: State, action:Action) : State {
    switch (action._type) {
        case 'TOGGLE_COMMENT_REPLIES_MODAL': return {...state, isModalCommentRepliesOpen: !state.isModalCommentRepliesOpen };
        case 'TOGGLE_COMMENT_REPLY_REPLIES': return {... state, openedArticleCommentForm: action.articleCommentFormIndex};
    }
}