import React, { useEffect, useState, useMemo } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { Container, Row, Col, Button } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { ArticlesList } from '../../components/ArticleList';
import { ArticleAPI } from '../../api/ArticleAPI';
import { RoomAPI } from '../../api/RoomAPI';
import { useSignalR } from '../../services/signalr-service';
import { useArticlesQuery, useRoomQuery } from '../../services/queries-service';
import { ArticleCard } from '../../components/ArticleCard';

type RoomArticlesViewProps = RouteComponentProps<{ roomID: string; status: string }> & {
    onError: (error: any) => void;
}
export function RoomArticlesView({ onError, history, match }: RoomArticlesViewProps) {
    const userID: string = localStorage.getItem('hermes.userID') || '';
    const rights: string = localStorage.getItem('hermes.rights') || '';
    const { roomID } = match.params;
    const archived = match.params.status === 'archived';
    
    useSignalR('articles', `room:${roomID}`);
    const { data: articlesData } = useArticlesQuery(roomID, archived);
    const { data: roomData } = useRoomQuery(roomID);
    const roomRights: string|undefined = useMemo(() => {
        if (roomData !== undefined) {
            let roomUser = roomData.users.find(x => x.userID === userID);
            if(roomUser !== undefined) {
                return roomUser.permission;
            }
        }
        return undefined;
    }, [roomData]);
    const articles = useMemo(() => {
        if (articlesData !== undefined) {
            return articlesData.map(e => ({
                title: e.title,
                photoURL: e.photoURL,
                ID: e.articleID,
                languageID: e.originalLanguageID,
                created: e.created
            }));
        }
        return [];
    }, [articlesData]);

    function handleCloseRoom() {
        RoomAPI.close({ roomID, userID })
        .then(() => history.push('/rooms'))
        .catch(onError);
    }
    function handleArchive(articleID: string) {
        ArticleAPI.archive({
            articleID: articleID,
            userID: userID
        })
        .catch(onError);
    }

    return (
        <>
            <Container>
                <PageHeader>{roomID}</PageHeader>
                { (roomRights !== undefined) &&
                    <>
                        <Row className='justify-content-center text-center mb-5'>
                            { (roomRights === 'admin') &&
                                <Col md={2}>
                                    <Link className='btn btn-success' to={`/templates/active/${roomID}`}>Add Text</Link>
                                </Col>
                            }
                            <Col md={2}>
                                <Link className='btn btn-primary' to={`/rooms/${roomID}/users/active`}>Users</Link>
                            </Col>
                            { (roomRights === 'admin') &&
                                <Col md={2}>
                                    <Button color='danger' onClick={handleCloseRoom}>Close Room</Button>
                                </Col>
                            }
                            { !archived &&
                                <Col md={2}>
                                    <Button tag={Link} color='primary' to={`/rooms/${roomID}/articles/archived`}>Archive</Button>
                                </Col>
                            }
                            { archived &&
                                <Col md={2}>
                                    <Button tag={Link} color='primary' to={`/rooms/${roomID}/articles/active`}>Active</Button>
                                </Col>
                            }
                        </Row>
                    </> 
                }
            </Container>
            <Container fluid>  
                <ArticlesList>
                    { articles.map((article, index) =>
                        <ArticleCard key={index} {...article}
                            articleID={article.ID}
                            link={{
                                url: '/translate/',
                                label: 'Translate' 
                            }}
                            onArchive={handleArchive}
                            enableArchive={rights === 'admin'}
                            enableAddToRoom={false}
                        />
                    )}
                </ArticlesList>
            </Container>
        </>
    );
}