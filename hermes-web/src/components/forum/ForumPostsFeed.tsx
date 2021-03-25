import React, {ReactNode, Children} from 'react';
import { Container, Row, Col } from 'reactstrap';

type forumFeedProps = {
    children?: ReactNode;
}

export function ForumPostsFeed({children}: forumFeedProps) {
    return (
        <Container>
                    <Row>
                        <Col md={11}>{ children }</Col>
                    </Row>
                    <hr />
        </Container>
    );
}