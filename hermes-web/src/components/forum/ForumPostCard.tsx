import moment from 'moment'
import React from 'react'
import { Link } from 'react-router-dom'
import { Badge, Col, Container, Row } from 'reactstrap'

type ForumCardProps = {
    onClick?:() => void;
    userID: string,
    profilePhoto: string,
    title: string;
    languageID: string;
    timestamp: Date; 
    lastCommentUserID: string;
    lastCommentTimestamp: Date;
}

export function ForumPostCard ({userID, profilePhoto, title, languageID, timestamp, lastCommentUserID, lastCommentTimestamp, onClick}: ForumCardProps ) {
    return (
        <Container fluid>
            <Row className='app-forum-post-card'>
                <Col md={3} className='app-forum-post-card-col d-flex flex-column align-self-center'>
                    <img
                        className='d-flex align-self-center'
                        src={profilePhoto === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhoto}
                        alt='profile photo'
                        style={{ width: '47px', height: '47px', borderRadius: '50%', objectFit:'cover', marginRight:'5px'}}
                    />
                    <Link className="app-forum-post-card-col-user d-flex align-self-center" to={`/profile/${userID}`}><strong>{userID}</strong></Link>
                    <span className='d-flex align-self-center'>Posted {moment.utc(timestamp).fromNow()}</span>               
                </Col>
                <Col onClick={onClick} md={5} className='app-forum-post-card-col d-flex align-items-center'>
                    <span className='app-forum-post-card-col-title'>
                        {title}
                    </span>
                </Col>
                <Col className='app-forum-post-card-col d-flex align-items-center justify-content-center' md={1}>
                    <Badge>
                        {languageID}
                    </Badge>  
                </Col>
                {   lastCommentUserID !== null &&
                <Col className='d-flex flex-column align-self-center' md={3}> 
                    
                    <Link className="app-forum-post-card-user d-flex align-self-center" to={`/profile/${lastCommentUserID}`}><strong>{lastCommentUserID}</strong></Link>
                    <span className='d-flex align-self-center'>Commented {moment.utc(lastCommentTimestamp).fromNow()}</span>   
                    
                </Col>
                }       
            </Row>  
        </Container>
    )
}