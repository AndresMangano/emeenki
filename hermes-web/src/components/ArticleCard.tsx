import React from 'react';
import { CardBody, CardFooter, Row, Col, Button, Card } from 'reactstrap';
import moment from 'moment';
import { Link } from 'react-router-dom';

type ArticleCardProps = {
    onAddToRoom?: (articleID: string) => void;
    onArchive: (articleID: string) => void;
    link?: {
        label: string;
        url: string;
    }
    enableAddToRoom: boolean;
    enableArchive: boolean;
    title: string;
    photoURL: string;
    articleID: string;
    languageID: string;
    created: Date;
}
export function ArticleCard({ onAddToRoom, onArchive, link, enableAddToRoom, enableArchive, title, photoURL, articleID, languageID, created }: ArticleCardProps) {
    return (
        <Card
            className='HArticleList'
            style={{
                background: `linear-gradient(rgba(255, 255, 255, 0), rgba(0, 0, 0, 1) 99%), url(${photoURL})`,
                backgroundSize: 'cover'
            }}>
            <CardBody className='HArticleList-item d-flex flex-column justify-content-between text-center'>
                <CardBody></CardBody>
                <CardFooter>
                    <Row>
                        <h6 className='text-white'>
                            <strong>{title}</strong>
                        </h6>
                    </Row>
                    <Row>
                        <Col md={5}>
                            <span className="badge badge-pill badge-dark">{languageID}</span>
                            <small className='text-white'> {moment.utc(created).fromNow()}</small>
                        </Col>
                        <Col md={7}>
                            <Row className='justify-content-center text-center'>
                                { (link !== undefined) && 
                                    <Col md={4}>
                                        <Button tag={Link} size='sm' color='primary' to={link !== undefined ? link.url + articleID : ''}>
                                            {link !== undefined ? link.label : ''}
                                        </Button>
                                    </Col>
                                }
                                { enableAddToRoom &&
                                    <Col md={4}>
                                        <Button size='sm' color='primary' onClick={() => onAddToRoom && onAddToRoom(articleID)}>Add to room</Button>
                                    </Col>
                                }
                                { enableArchive &&
                                    <Col md={4}>
                                        <Button size='sm' color='danger' onClick={() => onArchive(articleID)}>Archive</Button>
                                    </Col>
                                }
                            </Row>
                        </Col>
                    </Row>
                </CardFooter>
            </CardBody>
        </Card>
    )
}