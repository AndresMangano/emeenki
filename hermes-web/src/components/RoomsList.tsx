import React, { useState, useEffect } from 'react';
import { Container, Row, Card, CardBody, Col, Button, CardHeader, Badge } from 'reactstrap';
import { Link } from 'react-router-dom';

export type RoomItemProps = {
    onJoinRoom: (roomID:string, userID:string) => void;
    onLeaveRoom: (roomID:string, userID:string) => void;
    roomID: string;
    languageID1: string;
    languageID2: string;
    users: number;
    usersLimit: number;
    restricted: boolean;
    membership: string;
    userID: string;
}
function RoomItem({
    onJoinRoom,
    onLeaveRoom,
    roomID,
    languageID1,
    languageID2,
    users,
    usersLimit,
    restricted,
    membership,
    userID
}: RoomItemProps) {
    return (
        <Card className='HRoomItem mb-4' color={getColor(membership, restricted)}>
            { (membership === 'active') &&
                <Link to={`/rooms/${roomID}/articles/active`}>
                    <CardHeader className={`HRoomItem-header bg-${getColor(membership, restricted)} text-light text-center`}>
                        <h4 className='text-white'>{roomID}</h4>
                    </CardHeader>
                </Link>
            }
            { (membership !== 'active') &&
                <CardHeader className={`HRoomItem-header bg-${getColor(membership, restricted)} text-light text-center`}>
                    <h4 className='text-white'>{roomID}</h4>
                </CardHeader>
            }
            <CardBody className='text-center'>
                <Badge className='HRoomItem-badge text-center p-3'>{`${languageID1} - ${languageID2}`}</Badge>
                <Row className='justify-content-center m-3'>
                    { (membership === 'active') && <Button color='danger' onClick={() => onLeaveRoom(roomID, userID)}>Leave</Button> }
                    { (membership !== 'active') && <Button color='primary' disabled={membership !== 'inactive'} onClick={() => onJoinRoom(roomID, userID)}>Join</Button> }
                </Row>
                <Row className='justify-content-center'>{`${users} / ${usersLimit}`}</Row>
            </CardBody>
        </Card>
    );
}

type RoomListProps = {
    rooms: RoomItemProps[];
}
export function RoomsList({ rooms }: RoomListProps) {
    return (
        <Container>
            <Row className='justify-content-center'>
                { rooms.map((room, index) => 
                    <Col key={index} md={{ size: 3 }}>
                        <RoomItem {...room}/>
                    </Col>
                )}
            </Row>
        </Container> 
    );
}

function getColor(membership:string, restricted:boolean) {
    return membership === 'active'
        ?   'success'
        :   membership === 'inactive' && restricted
            ?   'danger'
            :   'info';
}