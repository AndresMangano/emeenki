import React, { useMemo } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { PageHeader } from '../../components/PageHeader';
import { Row, Col, Container } from 'reactstrap';
import { RoomUsersList } from '../../components/RoomUsersList';
import { RoomAPI } from '../../api/RoomAPI';
import { RoomUserSettings } from '../../components/RoomUserSettings';
import { useSignalR } from '../../services/signalr-service';
import { useRoomQuery, useRoomUsersQuery, useRoomPendingUsersQuery } from '../../services/queries-service';

type RoomUsersViewProps = RouteComponentProps<{ roomID: string, status: 'pending'|'active'}>  & {
    onError: (error: any) => void;
}
export function RoomUsersView({ onError, history, match }: RoomUsersViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { roomID, status } = match.params;

    useSignalR('users', `room:${roomID}`);
    const { data: roomData } = useRoomQuery(roomID);
    const { data: roomUsersData } = useRoomUsersQuery(roomID);
    const { data: pendingUsersData } = useRoomPendingUsersQuery(roomID);
    const rights: string|undefined = useMemo(() => {
        if (roomData !== undefined) {
            let roomUser = roomData.users.find(x => x.userID === userID);
            if(roomUser !== undefined){
                return roomUser.permission;
            }
        }
        return undefined;
    }, [roomData]);
    const usersLimit: number|undefined = useMemo(() => {
        if (roomData !== undefined) {
            return roomData.usersLimit;
        }
        return undefined;
    }, [roomData]);
    const users = useMemo(() => {
        if (status === 'active' && roomUsersData !== undefined) {
            return roomUsersData;
        } else if (status === 'pending' && pendingUsersData !== undefined) {
            return pendingUsersData;
        }
        return [];
    }, [status, roomUsersData, pendingUsersData]);

    function handleUpdateUsersLimit(newLimit: number) {
        RoomAPI.changeUsersLimit({ roomID, newLimit, userID })
        .catch(onError);
    }
    function handleAcceptUser(roomUserID: string) {
        RoomAPI.acceptUser({
            roomID,
            roomUserID,
            permission: 'user',
            userID
        })
        .then(response => history.push(`/rooms/${roomID}/articles`))
        .catch(onError);
    }
    function handleDeclineUser(roomUserID: string) {
        RoomAPI.rejectUser({ roomID, roomUserID, userID })
        .then(response => history.push(`/rooms/${roomID}/articles`))
        .catch(onError);
    }
    function handleExpelUser(roomUserID: string) {
        RoomAPI.expelUser({ roomID, roomUserID, userID })
        .then(response => history.push(`/rooms/${roomID}/users/active`))
        .catch(onError);
    }

    return (
        <Container>
            <PageHeader>{roomID} users</PageHeader>
            { (rights === 'admin') &&
                <Row className='justify-content-center text-center mb-5'>
                    <Col md={3}>
                        <Link className={`btn btn-primary ${status === 'active' ? 'active' : ''}`}
                            to={`/rooms/${roomID}/users/active`}>Room Users
                        </Link>
                    </Col>
                    <Col md={3}>
                        <Link className={`btn btn-primary ${status === 'pending' ? 'active' : ''}`}
                            to={`/rooms/${roomID}/users/pending`}>Pending Users
                        </Link>
                    </Col>
                    <Col md={4}>
                        { (usersLimit !== undefined) &&
                            <RoomUserSettings
                                onError={onError}
                                roomID={roomID}
                                usersLimit={usersLimit}
                                onUpdateUsersLimit={handleUpdateUsersLimit}
                            />
                        }
                    </Col>
                </Row>
            }
            { (rights !== undefined) && 
                <RoomUsersList
                    status={status}
                    users={users}
                    permission={rights}
                    fnAcceptUser={handleAcceptUser}
                    fnDeclineUser={handleDeclineUser}
                    fnExpelUser={handleExpelUser}
                />
            }
        </Container>
    );
}