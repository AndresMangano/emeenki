import React, { useMemo } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Row, Container } from 'reactstrap';
import { RoomsFilter } from '../../components/RoomsFilter';
import { RoomsList, RoomItemProps } from '../../components/RoomsList';
import { RoomAPI } from '../../api/RoomAPI';
import { PageHeader } from '../../components/PageHeader';
import { useLanguagesQuery, useRoomsQuery } from '../../services/queries-service';
import { useSignalR } from '../../services/signalr-service';

type RoomsViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function RoomsView({ onError, history, location }: RoomsViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { filter, languages } = useMemo(() => {
        let query = new URLSearchParams(location.search);
        let languages = query.getAll("languages");
        return {
            filter: query.get("filter") || 'all',
            languages: languages.length != 2 ? ['ENG', 'SPA'] : languages
        };
    }, [location.search]);

    useSignalR('rooms');
    const { data: languagesData } = useLanguagesQuery();
    const { data: roomsData } = useRoomsQuery(filter, userID, languages[0], languages[1]);

    const rooms: RoomItemProps[] = useMemo(() => {
        if (roomsData !== undefined) {
            // Hide PUBLIC rooms from the UI
            const nonPublicRooms = roomsData.filter(r => !r.roomID.startsWith('PUBLIC_'));

            return nonPublicRooms.map(e => ({
                roomID: e.roomID,
                languageID1: e.languageID1,
                languageID2: e.languageID2,
                membership: e.users.some(x => x.userID === userID)
                    ? 'active'
                    : e.usersQueue.some(x => x === userID)
                        ? 'pending'
                        : 'inactive',
                users: e.users.length,
                usersLimit: e.usersLimit,
                restricted: e.restricted,
                userID: userID,
                onError: onError,
                onJoinRoom: (roomID: string, userID: string) => {
                    RoomAPI.join({ roomID, userID })
                    .then(() => history.push(`/rooms/${e.roomID}/articles/active`))
                    .catch(onError);
                },
                onLeaveRoom: (roomID: string, userID: string) => {
                    RoomAPI.left({ roomID, userID })
                    .catch(onError);
                }
            }));
        }
        return [];
    }, [roomsData, userID, onError, history]);

    return(
        <Container>
            <PageHeader>Rooms</PageHeader>
            <Row className='mb-4'>
                { (languagesData) && 
                    <RoomsFilter
                        filter={filter === 'mine' ? 'mine' : 'all'}
                        languageFilter={languages}
                        languages={languagesData}
                        onFiltersChange={(f, l1, l2) => history.push(`/rooms?filter=${f}&languages=${l1}&languages=${l2}`)}
                    />
                }
            </Row>
            <hr/>
            <Row className='justify-content-center'>
                <RoomsList rooms={rooms}/> 
            </Row>
        </Container>
    );
}
