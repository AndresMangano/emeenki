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
                <Col md={8}>{ form }</Col>
            </Row>
            <Row>
                <Col md={10}>{ children }</Col>
            </Row>
        </Container>
    );
}