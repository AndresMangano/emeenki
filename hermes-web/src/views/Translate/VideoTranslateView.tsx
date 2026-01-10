import React, { useMemo } from 'react';
import { Card, CardBody, CardText, Col, Container, Row } from 'reactstrap';
import { RouteComponentProps } from 'react-router-dom';
import { ArticleTranslator } from '../../components/ArticleTranslator';
import { ArticleAPI } from '../../api/ArticleAPI';
import { ArticleCommentsPanel } from '../../components/article/ArticleCommentsPanel';
import { useArticleTemplateQuery } from '../../services/queries-service';
import { useSignalR } from '../../services/signalr-service';
import { ArticleCommentForm } from '../../components/article/ArticleCommentForm';
import { ArticleComment } from '../../components/article/ArticleComment';
import { ArticleTemplateDTO } from '../../api/ArticleTemplateAPI';

interface ISentence {
    sentencePos: number;
    originalText: string;
    startTimeMs?: number;
    endTimeMs?: number;
}

interface IVideoArticle {
    articleTemplateID: string;
    videoURL: string;
    photoUrl: string;
    title: ISentence[];
    text: ISentence[];
}

type VideoTranslateViewProps = RouteComponentProps<{ articleTemplateID: string }> & {
    onError: (error: any) => void;
};

export function VideoTranslateView({ onError, history, match }: VideoTranslateViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { articleTemplateID } = match.params;

    useSignalR(`articleTemplate:${articleTemplateID}`);
    const { data: articleTemplateData } = useArticleTemplateQuery(articleTemplateID);
    const videoArticle: IVideoArticle | undefined = useMemo(() => mapVideoArticle(articleTemplateData), [articleTemplateData]);

    const getYouTubeVideoId = (url: string): string => {
        const regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#&?]*).*/;
        const match = url.match(regExp);
        return (match && match[7].length === 11) ? match[7] : '';
    };

    console.log('Video Article Text', videoArticle)

    return (
        <Container fluid>
            {videoArticle !== undefined && (
                <>
                    <Row className="mb-4">
                        <Col md={12}>
                            <h2>{videoArticle.title.map(s => s.originalText).join(' ')}</h2>
                        </Col>
                    </Row>
                    <Row className="mb-4">
                        <Col md={12}>
                            <div style={{ position: 'relative', paddingBottom: '56.25%', height: 0, overflow: 'hidden' }}>
                                <iframe
                                    style={{
                                        position: 'absolute',
                                        top: 0,
                                        left: 0,
                                        width: '100%',
                                        height: '100%'
                                    }}
                                    src={`https://www.youtube.com/embed/${getYouTubeVideoId(videoArticle.videoURL)}`}
                                    frameBorder="0"
                                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                                    allowFullScreen
                                    title="YouTube video"
                                />
                            </div>
                        </Col>
                    </Row>
                    <Row>
                        <Col md={6}>
                            <h4>Original Captions</h4>
                            <div style={{ maxHeight: '600px', overflowY: 'auto', padding: '1rem', border: '1px solid #ddd', borderRadius: '0.5rem' }}>
                                {GroupParagraphs(videoArticle.text).map((paragraph, pIndex) => (
                                    <div key={pIndex} style={{ marginBottom: '1.5rem' }}>
                                        {paragraph.map((sentence, sIndex) => (
                                            <div 
                                                key={`${pIndex}-${sIndex}`}
                                                style={{ 
                                                    marginBottom: '0.5rem',
                                                    padding: '0.5rem',
                                                    backgroundColor: '#f8f9fa',
                                                    borderRadius: '0.25rem'
                                                }}
                                            >
                                                <span style={{ fontSize: '0.85rem', color: '#6c757d', marginRight: '0.5rem' }}>
                                                    {formatTimestamp(sentence.startTimeMs)}
                                                </span>
                                                <span>{sentence.originalText}</span>
                                            </div>
                                        ))}
                                    </div>
                                ))}
                            </div>
                        </Col>
                        <Col md={6}>
                            <h4>Translations</h4>
                            <div style={{ padding: '1rem' }}>
                                <p className="text-muted">Translation feature coming soon...</p>
                            </div>
                        </Col>
                    </Row>
                    <hr />
                    <Row className="mt-4">
                        <Col md={12}>
                            <Card className="app-translation-color-card">
                                <CardBody className="app-translation-color-card-body">
                                    <CardText className="app-translation-color-card-body-your">
                                        ■ Your Translation 
                                    </CardText>
                                    <CardText className="app-translation-color-card-body-otherLiked">
                                        ■ Liked Translation
                                    </CardText>
                                    <CardText className="app-translation-color-card-body-activity">
                                        ■ Activity in your Translation
                                    </CardText>
                                    <CardText className="app-translation-color-card-body-other">
                                        ■ Other User&apos;s Translation
                                    </CardText>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </>
            )}
        </Container>
    );
}

function GroupParagraphs(sentences: ISentence[]): ISentence[][] {
    let result: ISentence[][] = [];
    result[0] = [];
    let actual = 0;

    sentences.forEach((s, index) => {
        if (s.originalText.charCodeAt(0) === 10) {
            actual++;
            result[actual] = [];
            result[actual].push(s);
        } else {
            result[actual].push(s);
        }
    });

    return result;
}

function mapVideoArticle(articleTemplate: ArticleTemplateDTO | undefined): IVideoArticle | undefined {
    return articleTemplate === undefined
        ? undefined
        : {
              articleTemplateID: articleTemplate.articleTemplateID,
              videoURL: articleTemplate.videoURL,
              photoUrl: articleTemplate.photoURL,
              title: articleTemplate.title.map((e, index) => ({
                  sentencePos: index,
                  originalText: e.text,
                  startTimeMs: e.startTimeMs,
                  endTimeMs: e.endTimeMs
              })),
              text: articleTemplate.text.map((e, index) => ({
                  sentencePos: index,
                  originalText: e.text,
                  startTimeMs: e.startTimeMs,
                  endTimeMs: e.endTimeMs
              }))
          };
}

function formatTimestamp(ms?: number): string {
    if (ms === undefined) return '00:00';
    const seconds = Math.floor(ms / 1000);
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${remainingSeconds.toString().padStart(2, '0')}`;
}




