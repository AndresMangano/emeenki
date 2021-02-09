import React, { ReactNode } from 'react';
import { Container, Row, Col } from 'reactstrap';

type ArticleCommentsPanelProps = {
    form: ReactNode;
    children?: ReactNode;
}
export function ArticleCommentsPanel({ form, children }: ArticleCommentsPanelProps) {
    return (
        <Container fluid>
            <Row>
                <Col md={{ size: 5, offset: 1 }}>{ form }</Col>
            </Row>
            <Row>
                <Col md={{ size: 5, offset: 1}}>{ children }</Col>
            </Row>
        </Container>
    );
}