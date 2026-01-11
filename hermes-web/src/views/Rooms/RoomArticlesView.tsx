// src/views/Rooms/RoomArticlesView.tsx
import React, { useEffect, useState, useMemo } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { Container, Row, Col, Button, Label, Input } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { ArticlesList } from '../../components/ArticleList';
import { ArticleAPI } from '../../api/ArticleAPI';
import { RoomAPI } from '../../api/RoomAPI';
import { useSignalR } from '../../services/signalr-service';
import { useArticlesQuery, useRoomQuery, useTopicsQuery } from '../../services/queries-service';
import { ArticleCard } from '../../components/ArticleCard';

type RoomArticlesViewProps = RouteComponentProps<{ roomID: string; status: string }> & {
    onError: (error: any) => void;
};

type ArticleTypeFilter = 'all' | 'text' | 'video';

export function RoomArticlesView({ onError, history, match }: RoomArticlesViewProps) {
    const userID: string = localStorage.getItem('hermes.userID') || '';
    const rights: string = localStorage.getItem('hermes.rights') || '';
    const { roomID } = match.params;
    const archived = match.params.status === 'archived';

    useSignalR('articles', `room:${roomID}`);
    const { data: articlesData } = useArticlesQuery(roomID, archived);
    const { data: roomData } = useRoomQuery(roomID);
    const { data: topicsData } = useTopicsQuery();

    const [articleTypeFilter, setArticleTypeFilter] = useState<ArticleTypeFilter>('all');

    const topicsById = useMemo(() => {
        const map: Record<string, string> = {};
        if (topicsData) {
            topicsData.forEach((t: any) => {
                if (t.topicID) {
                    map[t.topicID] = t.name || t.topicID;
                }
            });
        }
        return map;
    }, [topicsData]);

    const roomRights: string | undefined = useMemo(() => {
        if (roomData !== undefined) {
            const roomUser = roomData.users.find((x: any) => x.userID === userID);
            if (roomUser !== undefined) {
                return roomUser.permission;
            }
        }
        return undefined;
    }, [roomData, userID]);

    const articles = useMemo(() => {
        if (articlesData !== undefined) {
            let mapped = articlesData.map((e: any) => ({
                title: e.title,
                photoURL: e.photoURL,
                ID: e.articleID,
                languageID: e.originalLanguageID,
                created: e.created,
                isVideo: e.isVideo ? true : false,
                videoURL: e.videoURL,
                articleTemplateID: e.articleTemplateID,
                topicID: e.topicID,
                topicName: e.topicID ? (topicsById[e.topicID] || e.topicID) : undefined
            }));

            if (articleTypeFilter === 'video') {
                mapped = mapped.filter(a => a.isVideo);
            } else if (articleTypeFilter === 'text') {
                mapped = mapped.filter(a => !a.isVideo);
            }

            return mapped;
        }
        return [];
    }, [articlesData, articleTypeFilter, topicsById]);

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
                {roomRights !== undefined && (
                    <>
                        <Row className='justify-content-center text-center mb-4'>
                            {roomRights === 'admin' && (
                                <Col md={2}>
                                    <Link className='btn btn-success' to={`/templates/active/${roomID}`}>
                                        Add Text
                                    </Link>
                                </Col>
                            )}
                            <Col md={2}>
                                <Link
                                    className='btn btn-primary'
                                    to={`/rooms/${roomID}/users/active`}
                                >
                                    Users
                                </Link>
                            </Col>
                            {roomRights === 'admin' && (
                                <Col md={2}>
                                    <Button color='danger' onClick={handleCloseRoom}>
                                        Close Room
                                    </Button>
                                </Col>
                            )}
                            {!archived && (
                                <Col md={2}>
                                    <Button
                                        tag={Link}
                                        color='primary'
                                        to={`/rooms/${roomID}/articles/archived`}
                                    >
                                        Archive
                                    </Button>
                                </Col>
                            )}
                            {archived && (
                                <Col md={2}>
                                    <Button
                                        tag={Link}
                                        color='primary'
                                        to={`/rooms/${roomID}/articles/active`}
                                    >
                                        Active
                                    </Button>
                                </Col>
                            )}
                        </Row>

                        {/* Article type filter */}
                        <Row className='justify-content-center text-center mb-4'>
                            <Col md={3}>
                                <Label htmlFor='articleTypeFilter'>Article Type</Label>
                                <Input
                                    type='select'
                                    id='articleTypeFilter'
                                    name='articleTypeFilter'
                                    value={articleTypeFilter}
                                    onChange={e =>
                                        setArticleTypeFilter(
                                            e.currentTarget.value as ArticleTypeFilter
                                        )
                                    }
                                >
                                    <option value='all'>All</option>
                                    <option value='text'>Text only</option>
                                    <option value='video'>Video only</option>
                                </Input>
                            </Col>
                        </Row>
                    </>
                )}
            </Container>
            <Container fluid>
                <ArticlesList>
                    {articles.map((article, index) => {
                        const isVideo = article.isVideo === true;
                        const linkBase = isVideo ? '/video-translate/' : '/translate/';
                        const targetIDForLink =
                            isVideo && article.articleTemplateID
                                ? article.articleTemplateID
                                : article.ID;

                        return (
                            <ArticleCard
                                key={index}
                                title={article.title}
                                photoURL={article.photoURL}
                                articleID={article.ID}
                                languageID={article.languageID}
                                created={article.created}
                                isVideo={isVideo}
                                videoURL={article.videoURL}
                                targetIDForLink={targetIDForLink}
                                topicName={article.topicName}   // <-- HERE
                                link={{
                                    url: linkBase,
                                    label: isVideo ? 'Watch & translate' : 'Translate'
                                }}
                                onArchive={handleArchive}
                                enableArchive={rights === 'admin'}
                                enableAddToRoom={false}
                            />
                        );
                    })}
                </ArticlesList>
            </Container>
        </>
    );
}
