import moment from 'moment'
import React from 'react'
import { Link } from 'react-router-dom'
import { Col, Container, Media, Row } from 'reactstrap'
import { LanguageFlag } from '../LanguageFlag'

type ForumThreadProps = {
    profilePhoto: string,
    userID: string;
    title: string;
    text: string;
    nativeLanguageID: string;
    timestamp: Date;
}

export function ForumThread ({profilePhoto, userID, nativeLanguageID, title, text, timestamp}: ForumThreadProps) {
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
                    <td className='d-flex align-self-center'><LanguageFlag languageID={nativeLanguageID} size="16px"/></td>
                </Col>
                <Col md={9}className="app-forum-threadpage-text">
                    <p>
                        {text}
                    </p>
                </Col>
            </Row>
            <Row className='mt-4'>
                <Col>
                   <p className='d-flex justify-content-end'>Posted {`${moment.utc(timestamp).fromNow()}`}</p>
                </Col>
            </Row>
            <hr />
        </Container>
    )
}