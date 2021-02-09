import React, { useReducer } from 'react';
import { Media, Button, Modal, ModalHeader, ModalBody, Container, Row, Col } from 'reactstrap';
import { ProfilePostForm } from './ProfilePostForm';
import moment from 'moment';

type ProfilePostProps = {
    onSubmit: (post: string, parentUserPostID:string|null) => void;
    onDelete: (userPostID: string, childUserPostID: string|null) => void;
    senderUserID: string;
    profilePhotoURL: string;
    userPostID: string;
    text: string;
    timestamp: Date;
    showDelete: boolean;
    replies: {
        userPostID: string;
        senderUserID: string;
        text: string;
        timestamp: Date;
        profilePhotoURL: string;
        childUserPostID: string|null;
    }[];
}

export function ProfilePost({ onSubmit, onDelete, senderUserID, profilePhotoURL, userPostID, text, timestamp, showDelete, replies }: ProfilePostProps) {
    
    const userID = localStorage.getItem("hermes.userID") || '';
    const [{isModalPostRepliesOpen, openedProfilePostForm}, dispatch] = useReducer(reducer, {
        isModalPostRepliesOpen: false,
        openedProfilePostForm: null,
    });  
    function handlePostRepliesToggle() {
        dispatch({ _type: 'TOGGLE_POST_REPLIES_MODAL' });
    }

    function handlePostReplyRepliesToggle(profilePostFormIndex: string) {
        dispatch({ _type: 'TOGGLE_POST_REPLY_REPLIES', profilePostFormIndex: openedProfilePostForm === profilePostFormIndex ? null : profilePostFormIndex })
    }

    function handleReplySubmit(post: string, parentUserPostID:string|null) {
        onSubmit(post, parentUserPostID);
        dispatch({ _type: 'TOGGLE_POST_REPLY_REPLIES', profilePostFormIndex: null });
    }

    return (
        <>
            <Media className='m-3' heading>
                <Media className="PostsCommentsUpperPart">
                    <img className="PostsCommentsPhoto" src={profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhotoURL} alt=''/>
                    <big className='PostsCommentsUserData'>{`${senderUserID}`}</big>
                    <big className='mt-1 PostsCommentsTimeStamp'>{`${moment.utc(timestamp).fromNow()}`}</big>
                </Media>
                <Media body>
                    <p className='PostsCommentsText'>{ text }</p>    
                </Media>
                <Media className="d-flex flex-row justify-content-end" bottom>
                    { showDelete &&
                            <Button className="ml-auto" size='md' color='transparent' onClick={() => onDelete(userPostID, null)}><b>Delete</b></Button>
                        }
                    <Button size='md' color='transparent' onClick={handlePostRepliesToggle}><b>Reply</b></Button>
                </Media>
            </Media>
            {  
                isModalPostRepliesOpen && 
                    <Modal isOpen={true} toggle={handlePostRepliesToggle}>
                        <ModalHeader>
                            Replies
                        </ModalHeader>
                        <ModalBody>
                            <Container>
                                <Media className='m-3' heading>
                                    <Media className="PostsCommentsUpperPart">
                                        <img className="PostsCommentsPhoto" src={profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhotoURL} alt=''/>
                                        <big className='PostsCommentsUserData mr-auto'>{`${senderUserID}`}</big>
                                        <big className='PostsCommentsTimeStamp'>{`${moment.utc(timestamp).fromNow()}`}</big>
                                    </Media>
                                    <Media body>
                                        <p className='PostsCommentsText'>{text}</p>    
                                    </Media>
                                </Media>
                                <Row>
                                    <Col>
                                        <ProfilePostForm 
                                            onSubmit={onSubmit} 
                                            parentUserPostID={userPostID}
                                        />
                                    </Col>
                                </Row>
                                {
                                    replies.map((r) =>
                                        <Media key={r.childUserPostID!} className='m-3' heading>
                                            <Media className="PostsCommentsUpperPart">
                                                <img className="PostsCommentsPhoto" src={r.profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : r.profilePhotoURL} alt=''/>
                                                <big className='PostsCommentsUserData mr-auto'>{`${r.senderUserID}`}</big>
                                                <big className='PostsCommentsTimeStamp'>{`${moment.utc(r.timestamp).fromNow()}`}</big>
                                            </Media>
                                            <Media body>
                                                <p className='PostsCommentsText'>{r.text}</p>    
                                            </Media>
                                            <Media className='app-modal-reply-id'>
                                                { (r.senderUserID === userID) && 
                                                    <Button className="d-flex ml-auto" size='md' color='transparent' onClick={() => onDelete(userPostID, r.childUserPostID)}><b>Delete</b></Button>
                                                }    
                                                { (r.senderUserID !== userID) &&
                                                 <Button className="d-flex ml-auto" size="md" color="transparent" onClick={() => handlePostReplyRepliesToggle(r.childUserPostID!)}>Reply</Button>
                                                 }
                                            </Media>
                                                {
                                                    (r.childUserPostID === openedProfilePostForm) && 
                                                    
                                                        <ProfilePostForm 
                                                            onSubmit={handleReplySubmit}
                                                            parentUserPostID={userPostID}
                                                            showSenderName={r.senderUserID}
                                                        /> 
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
    isModalPostRepliesOpen: boolean;
    openedProfilePostForm: string|null;
}
type Action =
| { _type: 'TOGGLE_POST_REPLIES_MODAL' }
| { _type: 'TOGGLE_POST_REPLY_REPLIES', profilePostFormIndex: string|null}
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'TOGGLE_POST_REPLIES_MODAL': return { ...state, isModalPostRepliesOpen: !state.isModalPostRepliesOpen };
        case 'TOGGLE_POST_REPLY_REPLIES': return { ...state, openedProfilePostForm: action.profilePostFormIndex };
    }
}