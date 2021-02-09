import React from 'react';
import { Row, Col, Card, CardBody, CardHeader, CardFooter, Button } from 'reactstrap';
import { LanguageFlag } from './LanguageFlag';

type RoomUsersListProps = {
    fnAcceptUser: (userID:string) => void;
    fnDeclineUser: (userID:string) => void;
    fnExpelUser: (userID:string) => void;
    users: {
        username: string;
        photoURL: string;
        nativeLanguageID: string;
        rights: string;
    }[];
    status: string;
    permission: string;
}
export function RoomUsersList(props: RoomUsersListProps)
{
    return (
        <Row className='justify-content-center'>
            {
                props.users.map((user, index) =>
                    <Col key={index} md={3} className='mb-5'>
                        <Card outline color='primary' className='h-100 d-flex flex-column justify-content-between'>
                            <CardHeader className={`bg-${props.status === 'active' ? 'info' : 'warning'} text-center`} tag="h4">
                                <Row>
                                    <Col md={4}>
                                        <LanguageFlag languageID={user.nativeLanguageID} size="24px"/>
                                    </Col>
                                    <Col md={8}>
                                        {user.username}
                                    </Col>
                                </Row>
                            </CardHeader>
                            <CardBody className='HRoomUser-card'
                                style={{
                                    background: `url("${user.photoURL}") center center`
                                }}/>
                            <CardFooter className={`bg-${props.status === 'active' ? 'info' : 'warning'}`}>
                                    <Row className='justify-content-center text-center'>
                                        {
                                            props.status === 'active' &&
                                                <>
                                                    <Col md={5}>
                                                        <span>{user.rights.toUpperCase()}</span>
                                                    </Col>
                                                    <Col md={7}>
                                                        {
                                                            (props.status === 'active' && props.permission === 'admin') &&
                                                                <Button color='danger' onClick={() => props.fnExpelUser(user.username)}>Expel</Button>
                                                        }
                                                    </Col>
                                                </>
                                        }
                                        {
                                            props.status === 'pending' &&
                                                <>
                                                    <Col md={6}>
                                                        {
                                                            props.permission === 'admin' &&
                                                                <Button color='success' onClick={() => props.fnAcceptUser(user.username)}>Accept</Button>
                                                        }
                                                    </Col>
                                                    <Col md={6}>
                                                        {
                                                            props.permission === 'admin' &&
                                                                <Button color='danger' onClick={() => props.fnDeclineUser(user.username)}>Decline</Button>
                                                        }
                                                    </Col>
                                                </>
                                        }
                                    </Row>
                            </CardFooter>
                        </Card>
                    </Col>
            )}
        </Row>
    );
}