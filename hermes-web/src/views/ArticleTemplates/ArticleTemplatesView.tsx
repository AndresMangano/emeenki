import React, { useReducer, useMemo } from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';
import { Container, Row, Col, Button, Input, Label } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { ArticlesList } from '../../components/ArticleList';
import { ArticleTemplateAPI } from '../../api/ArticleTemplateAPI';
import { useSignalR } from '../../services/signalr-service';
import { useArticleTemplatesQuery, useLanguagesQuery, useTopicsQuery } from '../../services/queries-service';
import { ArticleCard } from '../../components/ArticleCard';
import { ArticleAPI } from '../../api/ArticleAPI';

type ArticleTemplatesViewProps = RouteComponentProps<{ roomID?: string; archived: string;}> & {
    onError: (error: any) => void;
}
export function ArticleTemplatesIndex ({ onError, history, match }: ArticleTemplatesViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const rights = localStorage.getItem('hermes.rights') || '';
    const archived = match.params.archived === 'archived';
    const { roomID } = match.params;

    useSignalR('articleTemplates');
    const [{ languageID, topicID }, dispatch] = useReducer(reducer, {
        languageID: 'ENG',
        topicID: ''
    });
    const { data: languagesData } = useLanguagesQuery();
    const { data: topicsData } = useTopicsQuery();
    const { data: articleTemplatesData } = useArticleTemplatesQuery( archived, languageID, topicID);
    const articleTemplates = useMemo(() => {
        if (articleTemplatesData !== undefined) {
            return articleTemplatesData.map(e => ({
                title: e.title,
                photoURL: e.photoURL,
                ID: e.articleTemplateID,
                languageID: e.languageID,
                created: e.created 
            }));
        }
        return [];
    }, [articleTemplatesData])

    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID': dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value }); break;
            case 'topicID': dispatch( { _type: 'CHANGE_TOPIC_ID', topicID: event.currentTarget.value}); break;
        }
    }
    function handleArchive(articleTemplateID: string) {
        ArticleTemplateAPI.delete({ articleTemplateID, userID })
        .catch(onError)
    }
    function handleAddToRoom(articleTemplateID: string) {
        if (roomID !== undefined) {
            ArticleAPI.takeTemplate({
                articleTemplateID,
                roomID,
                userID
            })
            .then(() => history.push(`/rooms/${roomID}/articles/active`))
            .catch(onError);
        }
    }
    
    return (
        <>
            <Container>
                <PageHeader subtitle={roomID !== undefined ? `Add template to room ${roomID}` : undefined}>
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
                                languagesData.map((e, index) =>
                                    <option key={index} value={e.languageID}>{e.description}</option>
                            )}
                        </Input>
                    </Col>
                    <Col md={{ size: 3}}>
                        <Label>Topic</Label>
                        <Input type='select' name='topicID' id='topicID' value={topicID} onChange={handleInputChange}>
                            <option value=''>All</option>
                            { (topicsData) &&
                                topicsData.map((e, index) =>
                                <option key={index} value={e.topicID}>{e.name}</option>
                            )}
                        </Input>
                    </Col>
                    <Col md={{ size: 2 }}>
                        { !archived &&
                            <Button tag={Link} color='danger' to={`/templates/archived`}>Archive</Button>
                        }
                        { archived &&
                            <Button tag={Link} color='primary' to={`/templates/active`}>Active</Button>
                        }
                    </Col>
                </Row>
            </Container>
            <Container fluid>
                <ArticlesList key={languageID}>
                    { articleTemplates.map((articleTemplate, index) =>
                        <ArticleCard key={index} {...articleTemplate}
                            articleID={articleTemplate.ID}
                            onArchive={handleArchive}
                            onAddToRoom={handleAddToRoom}
                            enableAddToRoom={roomID !== undefined}
                            enableArchive={rights === 'admin'}
                        />
                    )}
                </ArticlesList>
            </Container>
        </>
    );
}

type State = {
    languageID: string;
    topicID: string;
}
type Action =
| { _type: 'CHANGE_LANGUAGE_ID', languageID: string }
| { _type: 'CHANGE_TOPIC_ID', topicID: string }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': return { ...state, languageID: action.languageID };
        case 'CHANGE_TOPIC_ID': return { ...state, topicID: action.topicID };
    }
}