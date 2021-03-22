import moment from 'moment'
import React from 'react'
import { Link } from 'react-router-dom'
import { Badge, Col, Row } from 'reactstrap'

type ForumCardProps = {
    onClick?:() => void;
    userID: string,
    profilePhoto: string,
    title: string;
    languageID: string;
    timestamp: Date; 
    latestCommentUserID: string;
    latestCommentTimestamp: Date;
}

export function ForumPostCard ({userID, profilePhoto, title, languageID, timestamp, latestCommentUserID, latestCommentTimestamp, onClick}: ForumCardProps ) {
    return (
        <Row className='app-forum-post-card'>
            <Col md={3} className='d-flex flex-column align-self-center mt-2 mb-2 border-right border-grey'>
                <img
                    className='d-flex align-self-center'
                    src={profilePhoto === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhoto}
                    alt='profile photo'
                    style={{ width: '47px', height: '47px', borderRadius: '50%', objectFit:'cover', marginRight:'5px'}}
                />
                <Link className="app-forum-post-card-user d-flex align-self-center" to={`/profile/${userID}`}><strong>{userID}</strong></Link>
            </Col>
            <Col onClick={onClick} md={6} className='app-forum-post-card-title d-flex align-self-center mt-3 mb-2 border-right border-grey'>
                <p>
                    {title}
                </p>
            </Col>
            <Col className='d-flex align-self-center' md={1}>
                <Badge>
                    {languageID}
                </Badge>
            </Col>
            <Col className='d-flex align-self-center' md={2}>
                {moment.utc(timestamp).fromNow()}
            </Col>
            <Col> 
                <Link className="app-forum-post-card-user d-flex align-self-center" to={`/profile/${userID}`}><strong>{latestCommentUserID}</strong></Link>
                {moment.utc(latestCommentTimestamp).fromNow()}   
            </Col>
        </Row>  
    )
}