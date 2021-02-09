import React, { ReactNode, Children } from 'react';
import { Link } from 'react-router-dom';
import { Card, CardBody,  Row, Col, CardFooter, Button } from 'reactstrap';
import { DefaultSpinner } from './DefaultSpinner';
type ArticleListProps = {
    children?: ReactNode;
}
export function ArticlesList({ children, }: ArticleListProps) {
    return(
        <Row className='justify-content-center'>
            { Children.map(children, (article, index) => 
                <Col className="mb-3" key={index} md={4}>{ article }</Col>
            )}
        </Row>
    );
}