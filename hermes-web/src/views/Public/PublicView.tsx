// src/views/Public/PublicView.tsx
import React, { useMemo, useState } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { Container, Row, Col, Input, Label, Button } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { ArticlesList } from '../../components/ArticleList';
import { ArticleAPI } from '../../api/ArticleAPI';
import { useSignalR } from '../../services/signalr-service';
import { useArticlesQuery, useRoomQuery, useLanguagesQuery, useTopicsQuery } from '../../services/queries-service';
import { ArticleCard } from '../../components/ArticleCard';

type PublicViewProps = RouteComponentProps<{}> & {
    onError: (error: any) => void;
};

type ArticleTypeFilter = 'all' | 'text' | 'video';

export function PublicView({ onError, history, location }: PublicViewProps) {
    const userID: string = localStorage.getItem('hermes.userID') || '';

    const languages = useMemo(() => {
        const query = new URLSearchParams(location.search);
        const langs = query.getAll('languages');
        return langs.length !== 2 ? ['ENG', 'SPA'] : langs;
    }, [location.search]);

    const languageID1 = languages[0];
    const languageID2 = languages[1];

    const roomID = useMemo(
        () => `PUBLIC_${languageID1}_${languageID2}`,
        [languageID1, languageID2]
    );

    useSignalR('articles', `room:${roomID}`);

    const { data: languagesData } = useLanguagesQuery();
    const { data: articlesData } = useArticlesQuery(roomID, false);
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

    const languageName1 = useMemo(() => {
        if (!languagesData) return languageID1;
        const lang = languagesData.find((l: any) => l.languageID === languageID1);
        return lang ? (lang.description || lang.languageID) : languageID1;
    }, [languagesData, languageID1]);

    const languageName2 = useMemo(() => {
        if (!languagesData) return languageID2;
        const lang = languagesData.find((l: any) => l.languageID === languageID2);
        return lang ? (lang.description || lang.languageID) : languageID2;
    }, [languagesData, languageID2]);

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

    function navigateForPair(l1: string, l2: string) {
        if (l1 === 'ENG' && l2 === 'SPA') {
            history.push('/public');
        } else {
            history.push(`/public?languages=${l1}&languages=${l2}`);
        }
    }

    function handleArchive(articleID: string) {
        ArticleAPI.archive({
            articleID: articleID,
            userID: userID
        }).catch(onError);
    }

    function handleLanguage1Change(event: React.ChangeEvent<HTMLInputElement>) {
        const newL1 = event.currentTarget.value;
        navigateForPair(newL1, languageID2);
    }

    function handleLanguage2Change(event: React.ChangeEvent<HTMLInputElement>) {
        const newL2 = event.currentTarget.value;
        navigateForPair(languageID1, newL2);
    }

    function handleSwapLanguages() {
        navigateForPair(languageID2, languageID1);
    }

    return (
        <>
            <Container>
                <PageHeader>
                    Public {languageName1} → {languageName2}
                </PageHeader>

                {languagesData && (
                    <Row className='mb-4 justify-content-center align-items-center'>
                        <Col md={3}>
                            <Label htmlFor='languageID1'>From</Label>
                            <Input
                                type='select'
                                id='languageID1'
                                name='languageID1'
                                value={languageID1}
                                onChange={handleLanguage1Change}
                            >
                                {languagesData.map((lang: any, index: number) => (
                                    <option key={index} value={lang.languageID}>
                                        {lang.description || lang.languageID}
                                    </option>
                                ))}
                            </Input>
                        </Col>

                        <Col md="auto" className='text-center' style={{ marginTop: '1.6rem' }}>
                            <Button color='secondary' onClick={handleSwapLanguages}>
                                ⇄
                            </Button>
                        </Col>

                        <Col md={3}>
                            <Label htmlFor='languageID2'>To</Label>
                            <Input
                                type='select'
                                id='languageID2'
                                name='languageID2'
                                value={languageID2}
                                onChange={handleLanguage2Change}
                            >
                                {languagesData.map((lang: any, index: number) => (
                                    <option key={index} value={lang.languageID}>
                                        {lang.description || lang.languageID}
                                    </option>
                                ))}
                            </Input>
                        </Col>
                    </Row>
                )}

                {/* Article type filter */}
                <Row className='mb-4 justify-content-center'>
                    <Col md={3}>
                        <Label htmlFor='articleTypeFilter'>Article Type</Label>
                        <Input
                            type='select'
                            id='articleTypeFilter'
                            name='articleTypeFilter'
                            value={articleTypeFilter}
                            onChange={e => setArticleTypeFilter(e.currentTarget.value as ArticleTypeFilter)}
                        >
                            <option value='all'>All</option>
                            <option value='text'>Text only</option>
                            <option value='video'>Video only</option>
                        </Input>
                    </Col>
                </Row>

                {roomRights === 'admin' && (
                    <Row className='justify-content-center text-center mb-5'>
                        <Col md={2}>
                            <Link className='btn btn-success' to={`/templates/active/${roomID}`}>
                                Add Text
                            </Link>
                        </Col>
                    </Row>
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
                                topicName={article.topicName}  // <-- HERE
                                link={{
                                    url: linkBase,
                                    label: isVideo ? 'Watch & translate' : 'Translate'
                                }}
                                onArchive={handleArchive}
                                enableArchive={roomRights === 'admin'}
                                enableAddToRoom={false}
                            />
                        );
                    })}
                </ArticlesList>
            </Container>
        </>
    );
}
