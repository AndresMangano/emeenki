import React, { ReactNode } from 'react';
import { Col, Container, Row } from 'reactstrap';

type forumCommentsPanelProps = {
    children: ReactNode;
}

export function ForumCommentsPanel ({children}: forumCommentsPanelProps) {
    return (
        <Container className='mt-4' fluid>
            <Row>
                <Col md={10}>{ children }</Col>
            </Row>
        </Container>
    )
}