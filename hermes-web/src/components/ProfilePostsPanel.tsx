import React from 'react';
import { Container, Row, Col } from 'reactstrap';
import { ProfilePost } from './ProfilePost';
import { ProfilePostForm } from './ProfilePostForm';

type ProfilePostsPanelProps = {
    onSubmit: (text: string, parentUserPostID:string|null) => void;
    onDelete: (userPostID: string) => void;
    userID: string;
    profileUserID: string;
    posts: {
        userPostID: string;
        text: string;
        senderUserID: string;
        timestamp: Date;
        profilePhotoURL: string;
        childUserPostID: string|null;
    }[]
}
export function ProfilePostsPanel({ onSubmit, onDelete, userID, profileUserID, posts }: ProfilePostsPanelProps) {
    return (
        <Container fluid>
            <Row>
                <Col md={{ size: 11, offset: 1}}>
                    <ProfilePostForm 
                        onSubmit={onSubmit} 
                        parentUserPostID={null}             
                    />
                </Col>
            </Row>
            <Row>
                <Col md={{ size: 11, offset: 1}}>
                    { posts.filter(p => p.childUserPostID === null).map((e, index) =>
                        <React.Fragment key={index}>
                            <ProfilePost {...e}
                                key={index}
                                onSubmit={onSubmit}  
                                onDelete={onDelete}
                                replies={posts.filter(p => p.childUserPostID !== null && p.userPostID === e.userPostID)}
                                showDelete={
                                    e.senderUserID === userID ||
                                    profileUserID === userID 
                                }
                            />
                            <hr />
                        </React.Fragment>
                    )}
                </Col>
            </Row>
        </Container>
    );
}