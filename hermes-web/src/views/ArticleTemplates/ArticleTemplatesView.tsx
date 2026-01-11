// src/views/ArticleTemplates/ArticleTemplatesView.tsx
import React, { useReducer, useMemo } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { Container, Row, Col, Button, Input, Label } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { ArticlesList } from '../../components/ArticleList';
import { ArticleTemplateAPI } from '../../api/ArticleTemplateAPI';
import { useSignalR } from '../../services/signalr-service';
import {
    useArticleTemplatesQuery,
    useLanguagesQuery,
    useTopicsQuery,
    useArticleTemplateQuery
} from '../../services/queries-service';
import { ArticleCard } from '../../components/ArticleCard';
import { ArticleAPI } from '../../api/ArticleAPI';

type ArticleTemplatesViewProps = RouteComponentProps<{ roomID?: string; archived: string;}> & {
    onError: (error: any) => void;
};

type ArticleTypeFilter = 'all' | 'text' | 'video';

export function ArticleTemplatesIndex ({ onError, history, match }: ArticleTemplatesViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const rights = localStorage.getItem('hermes.rights') || '';
    const archived = match.params.archived === 'archived';
    const { roomID } = match.params;

    useSignalR('articleTemplates');
    const [{ languageID, topicID, articleType }, dispatch] = useReducer(reducer, {
        languageID: 'ENG',
        topicID: '',
        articleType: 'all' as ArticleTypeFilter
    });

    const { data: languagesData } = useLanguagesQuery();
    const { data: topicsData } = useTopicsQuery();
    const { data: articleTemplatesData } = useArticleTemplatesQuery( archived, languageID, topicID);

    // map topicID → topic name
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

    const articleTemplates = useMemo(() => {
        if (articleTemplatesData !== undefined) {
            let mapped = articleTemplatesData.map((e: any) => ({
                title: e.title,
                photoURL: e.photoURL,
                ID: e.articleTemplateID,
                languageID: e.languageID,
                created: e.created,
                isVideo: e.isVideo ? true : false,
                videoURL: e.videoURL,
                topicID: e.topicID,
                topicName: topicsById[e.topicID] || e.topicID
            }));

            if (articleType === 'video') {
                mapped = mapped.filter(t => t.isVideo);
            } else if (articleType === 'text') {
                mapped = mapped.filter(t => !t.isVideo);
            }

            return mapped;
        }
        return [];
    }, [articleTemplatesData, articleType, topicsById]);

    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID':
                dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value });
                break;
            case 'topicID':
                dispatch({ _type: 'CHANGE_TOPIC_ID', topicID: event.currentTarget.value });
                break;
            case 'articleType':
                dispatch({
                    _type: 'CHANGE_ARTICLE_TYPE',
                    articleType: event.currentTarget.value as ArticleTypeFilter
                });
                break;
        }
    }

    function handleArchive(articleTemplateID: string) {
        ArticleTemplateAPI.delete({ articleTemplateID, userID })
            .catch(onError);
    }

    function handleAddToRoom(articleTemplateID: string) {
        if (roomID !== undefined) {
            ArticleAPI.takeTemplate({
                articleTemplateID: articleTemplateID,
                roomID: roomID,
                userID: userID
            })
            .then(function () {
                if (roomID.startsWith('PUBLIC_')) {
                    const parts = roomID.split('_'); // e.g. ["PUBLIC", "ENG", "SPA"]
                    const lang1 = parts[1];
                    const lang2 = parts[2];

                    if (lang1 === 'ENG' && lang2 === 'SPA') {
                        history.push('/public');
                    } else if (lang1 && lang2) {
                        history.push('/public?languages=' + lang1 + '&languages=' + lang2);
                    } else {
                        history.push('/public');
                    }
                } else {
                    history.push('/rooms/' + roomID + '/articles/active');
                }
            })
            .catch(onError);
        }
    }
    
    return (
        <>
            <Container>
                <PageHeader subtitle={roomID !== undefined ? 'Add template to room ' + roomID : undefined}>
                    Text Storage
                </PageHeader>
                <Row className='justify-content-center align-items-end text-center mb-4'>
                    <Col md={{ size: 2 }}>
                        <Link className='btn btn-success' to='/upload'>Upload Text</Link>
                    </Col>
                    <Col md={{ size: 3}}>
                        <Label>Language</Label>
                        <Input type='select' name='languageID' value={languageID} onChange={handleInputChange}>
                            <option value=''>All</option>
                            { (languagesData) &&
                                languagesData.map(function (e: any, index: number) {
                                    return (
                                        <option key={index} value={e.languageID}>{e.description}</option>
                                    );
                                })
                            }
                        </Input>
                    </Col>
                    <Col md={{ size: 3}}>
                        <Label>Topic</Label>
                        <Input type='select' name='topicID' id='topicID' value={topicID} onChange={handleInputChange}>
                            <option value=''>All</option>
                            { (topicsData) &&
                                topicsData.map(function (e: any, index: number) {
                                    return (
                                        <option key={index} value={e.topicID}>{e.name}</option>
                                    );
                                })
                            }
                        </Input>
                    </Col>
                    <Col md={{ size: 2 }}>
                        <Label>Type</Label>
                        <Input
                            type='select'
                            name='articleType'
                            value={articleType}
                            onChange={handleInputChange}
                        >
                            <option value='all'>All</option>
                            <option value='text'>Text only</option>
                            <option value='video'>Video only</option>
                        </Input>
                    </Col>
                    <Col md={{ size: 2 }}>
                        { !archived &&
                            <Button tag={Link} color='danger' to='/templates/archived'>Archive</Button>
                        }
                        { archived &&
                            <Button tag={Link} color='primary' to='/templates/active'>Active</Button>
                        }
                    </Col>
                </Row>
            </Container>
            <Container fluid>
                <ArticlesList key={languageID}>
                    { articleTemplates.map(function (articleTemplate, index) {
                        return (
                            <TemplateArticleCard
                                key={index}
                                template={articleTemplate}
                                onArchive={handleArchive}
                                onAddToRoom={handleAddToRoom}
                                enableAddToRoom={roomID !== undefined}
                                enableArchive={rights === 'admin'}
                            />
                        );
                    })}
                </ArticlesList>
            </Container>
        </>
    );
}

type State = {
    languageID: string;
    topicID: string;
    articleType: ArticleTypeFilter;
};

type Action =
    | { _type: 'CHANGE_LANGUAGE_ID', languageID: string }
    | { _type: 'CHANGE_TOPIC_ID', topicID: string }
    | { _type: 'CHANGE_ARTICLE_TYPE', articleType: ArticleTypeFilter };

function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': return { ...state, languageID: action.languageID };
        case 'CHANGE_TOPIC_ID': return { ...state, topicID: action.topicID };
        case 'CHANGE_ARTICLE_TYPE': return { ...state, articleType: action.articleType };
    }
}

/**
 * Wrapper card used only in Text Storage to fetch the template
 * and compute the total number of sentences.
 */
type TemplateCardData = {
    ID: string;
    title: string;
    photoURL: string;
    languageID: string;
    created: Date;
    isVideo: boolean;
    videoURL?: string;
    topicID: string;
    topicName: string;
};

type TemplateArticleCardProps = {
    template: TemplateCardData;
    onArchive: (articleTemplateID: string) => void;
    onAddToRoom: (articleTemplateID: string) => void;
    enableAddToRoom: boolean;
    enableArchive: boolean;
};

function TemplateArticleCard({
    template,
    onArchive,
    onAddToRoom,
    enableAddToRoom,
    enableArchive
}: TemplateArticleCardProps) {
    // Load full template to compute total sentences
    const { data: templateData } = useArticleTemplateQuery(template.ID);

    let totalSentences: number | undefined = undefined;

    if (templateData) {
        const titleArr = (templateData as any).title || [];
        const textArr = (templateData as any).text || [];
        totalSentences = titleArr.length + textArr.length;
    }

    return (
        <ArticleCard
            title={template.title}
            photoURL={template.photoURL}
            articleID={template.ID}
            languageID={template.languageID}
            created={template.created}
            onArchive={onArchive}
            onAddToRoom={onAddToRoom}
            enableAddToRoom={enableAddToRoom}
            enableArchive={enableArchive}
            isVideo={template.isVideo}
            videoURL={template.videoURL}
            topicName={template.topicName}   // <-- HERE
            totalSentences={totalSentences}
            lockedSentences={0}
        />
    );
}
