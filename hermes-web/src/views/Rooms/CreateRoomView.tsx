import React from 'react';
import { Col, Row, Container } from 'reactstrap';
import { CreateRoomForm } from '../../components/CreateRoomForm';
import { RoomAPI } from '../../api/RoomAPI';
import { PageHeader } from '../../components/PageHeader';
import { useLanguagesQuery } from '../../services/queries-service';
import { RouteComponentProps } from 'react-router-dom';

type CreateRoomViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function CreateRoomView({ onError, history }: CreateRoomViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { data: languagesData } = useLanguagesQuery();
    
    function handleCreateRoom(roomID:string, language1ID:string, language2ID:string, usersLimit:number, restricted:boolean) {
        RoomAPI.open({
            roomID: roomID,
            languages: [language1ID, language2ID],
            usersLimit: usersLimit,
            restricted: restricted,
            userID
        })
        .then(result => history.push('/rooms'))
        .catch(onError);
    }

    return(
        <Container>
            <PageHeader>Create Room</PageHeader>
            <Row className='justify-content-center'>
                <Col md={4}>
                    { (languagesData) &&
                        <CreateRoomForm
                            languages={languagesData}
                            onCreateRoom={handleCreateRoom}
                            onCancel={() => history.goBack()}/>
                    }
                </Col>
            </Row>
        </Container>
    );
}