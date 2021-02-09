import React, { ReactNode, Children } from 'react';
import { Container, Row, Col } from 'reactstrap';

type ActivityFeedProps = {
    children?: ReactNode;
}
export function ActivityFeed({ children }: ActivityFeedProps) {
    return (
        <div>
            <h1 className='display-6 mb-4'>Last day activity</h1>
            <Container fluid>
                { Children.map(children, (activity, index) =>
                    <Row key={index} className='mb-3'>
                        <Col md={12}>{ activity }</Col>
                    </Row>
                )}
            </Container>
        </div>
    );
}