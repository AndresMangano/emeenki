import React, { useEffect, useMemo, useRef, useState } from 'react';
import {
    Card,
    CardBody,
    CardText,
    Col,
    Container,
    Row,
    Button,
    ButtonGroup
} from 'reactstrap';
import { RouteComponentProps } from 'react-router-dom';
import YouTube from 'react-youtube';

import { useArticleTemplateQuery } from '../../services/queries-service';
import { useSignalR } from '../../services/signalr-service';
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

type TrackMode = 'word' | 'sentence';

export function VideoTranslateView({ onError, history, match }: VideoTranslateViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { articleTemplateID } = match.params;

    useSignalR('articleTemplate:' + articleTemplateID);
    const { data: articleTemplateData } = useArticleTemplateQuery(articleTemplateID);
    const videoArticle: IVideoArticle | undefined = useMemo(
        function () {
            return mapVideoArticle(articleTemplateData);
        },
        [articleTemplateData]
    );

    const playerRef = useRef<any | null>(null);
    const timeUpdateIntervalRef = useRef<number | null>(null);
    const [currentTimeMs, setCurrentTimeMs] = useState(0);
    const [trackMode, setTrackMode] = useState<TrackMode>('word');

    const activeSentencePos = useMemo(
        function () {
            if (!videoArticle || !videoArticle.text || !videoArticle.text.length) {
                return null;
            }

            const sentences = videoArticle.text;
            const now = currentTimeMs;

            for (let i = 0; i < sentences.length; i++) {
                const s = sentences[i];
                if (s.startTimeMs == null) continue;

                const start = s.startTimeMs;
                const next = sentences[i + 1];
                let end: number;

                if (s.endTimeMs != null) {
                    end = s.endTimeMs;
                } else if (next && next.startTimeMs != null) {
                    end = next.startTimeMs;
                } else {
                    end = start + 4000;
                }

                if (now >= start && now < end) {
                    return s.sentencePos;
                }
            }

            return null;
        },
        [currentTimeMs, videoArticle]
    );

    const activeSentenceRef = useRef<HTMLDivElement | null>(null);

    useEffect(
        function () {
            if (activeSentenceRef.current) {
                activeSentenceRef.current.scrollIntoView({
                    behavior: 'smooth',
                    block: 'center'
                });
            }
        },
        [activeSentencePos]
    );

    const startTimeTracking = () => {
        if (timeUpdateIntervalRef.current !== null) return;

        timeUpdateIntervalRef.current = window.setInterval(() => {
            if (playerRef.current && typeof playerRef.current.getCurrentTime === 'function') {
                const seconds: number = playerRef.current.getCurrentTime();
                setCurrentTimeMs(seconds * 1000);
            }
        }, 200);
    };

    const stopTimeTracking = () => {
        if (timeUpdateIntervalRef.current !== null) {
            window.clearInterval(timeUpdateIntervalRef.current);
            timeUpdateIntervalRef.current = null;
        }
    };

    useEffect(function () {
        return function () {
            stopTimeTracking();
        };
    }, []);

    const handlePlayerReady = (event: any) => {
        playerRef.current = event.target;
    };

    const handlePlayerStateChange = (event: any) => {
        const state = event.data;
        if (state === 1) {
            startTimeTracking();
        } else if (state === 0 || state === 2) {
            stopTimeTracking();
        }
    };

    const handleSentenceClick = (startTimeMs?: number) => {
        if (!playerRef.current || startTimeMs == null) return;
        playerRef.current.seekTo(startTimeMs / 1000, true);
        playerRef.current.playVideo();
    };

    const handleWordClick = (
        sentence: ISentence,
        nextSentence: ISentence | undefined,
        wordIndex: number,
        wordCount: number
    ) => {
        if (!playerRef.current) return;
        if (sentence.startTimeMs == null || wordCount <= 0) return;

        let start = sentence.startTimeMs;
        let end: number;

        if (sentence.endTimeMs != null) {
            end = sentence.endTimeMs;
        } else if (nextSentence && nextSentence.startTimeMs != null) {
            end = nextSentence.startTimeMs;
        } else {
            end = start + 4000;
        }

        if (end <= start) return;

        let fraction = wordCount === 1 ? 0.0 : (wordIndex + 0.5) / wordCount;
        fraction = Math.max(0, Math.min(fraction, 1));

        const targetMs = start + fraction * (end - start);
        const targetSeconds = targetMs / 1000;

        try {
            playerRef.current.seekTo(targetSeconds, true);
            playerRef.current.playVideo();
        } catch {
            // ignore
        }
    };

    const getYouTubeVideoId = (url: string): string => {
        const regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#&?]*).*/;
        const match = url.match(regExp);
        return match && match[7].length === 11 ? match[7] : '';
    };

    return (
        <Container fluid>
            {videoArticle !== undefined && (
                <>
                    <Row className="mb-4">
                        <Col md={12}>
                            <h2>
                                {videoArticle.title
                                    .map(s => s.originalText)
                                    .join(' ')}
                            </h2>
                        </Col>
                    </Row>

                    <Row className="mb-3">
                        <Col md={12}>
                            <div className="video-wrapper">
                                <YouTube
                                    videoId={getYouTubeVideoId(videoArticle.videoURL)}
                                    onReady={handlePlayerReady}
                                    onStateChange={handlePlayerStateChange}
                                    opts={{
                                        width: '100%',
                                        height: '100%',
                                        playerVars: {
                                            autoplay: 0,
                                            controls: 1,
                                            rel: 0
                                        }
                                    }}
                                    iframeClassName="video-iframe"
                                />
                            </div>
                        </Col>
                    </Row>

                    <Row className="mb-2">
                        <Col md={6}>
                            <div className="d-flex align-items-center">
                                <span style={{ marginRight: '0.5rem' }}>
                                    Caption sync mode:
                                </span>
                                <ButtonGroup size="sm">
                                    <Button
                                        color={trackMode === 'word' ? 'primary' : 'secondary'}
                                        onClick={() => setTrackMode('word')}
                                    >
                                        Word tracking
                                    </Button>
                                    <Button
                                        color={trackMode === 'sentence' ? 'primary' : 'secondary'}
                                        onClick={() => setTrackMode('sentence')}
                                    >
                                        Sentence tracking
                                    </Button>
                                </ButtonGroup>
                            </div>
                        </Col>
                    </Row>

                    <Row>
                        <Col md={6}>
                            <h4>Original Captions</h4>
                            <div
                                style={{
                                    maxHeight: '600px',
                                    overflowY: 'auto',
                                    padding: '1rem',
                                    border: '1px solid #ddd',
                                    borderRadius: '0.5rem'
                                }}
                            >
                                {videoArticle.text.map((sentence, index) => {
                                    const isActiveSentence =
                                        activeSentencePos !== null &&
                                        sentence.sentencePos === activeSentencePos;

                                    const nextSentence =
                                        videoArticle.text[sentence.sentencePos + 1];

                                    const words = sentence.originalText
                                        ? sentence.originalText.split(/\s+/)
                                        : [];

                                    let activeWordIndex: number | null = null;

                                    if (
                                        trackMode === 'word' &&
                                        isActiveSentence &&
                                        videoArticle
                                    ) {
                                        activeWordIndex = getActiveWordIndex(
                                            sentence,
                                            nextSentence,
                                            currentTimeMs,
                                            words.length
                                        );
                                    }

                                    const sentenceBackgroundColor =
                                        trackMode === 'sentence' && isActiveSentence
                                            ? '#fff3cd'
                                            : '#f8f9fa';

                                    const sentenceBorder =
                                        trackMode === 'sentence' && isActiveSentence
                                            ? '1px solid #ffc107'
                                            : '1px solid transparent';

                                    return (
                                        <div
                                            key={index}
                                            ref={
                                                isActiveSentence ? activeSentenceRef : null
                                            }
                                            onClick={() =>
                                                handleSentenceClick(sentence.startTimeMs)
                                            }
                                            style={{
                                                marginBottom: '0.5rem',
                                                padding: '0.5rem',
                                                borderRadius: '0.25rem',
                                                cursor:
                                                    sentence.startTimeMs !== undefined
                                                        ? 'pointer'
                                                        : 'default',
                                                backgroundColor: sentenceBackgroundColor,
                                                border: sentenceBorder,
                                                transition: 'background-color 0.15s ease'
                                            }}
                                        >
                                            <span
                                                style={{
                                                    fontSize: '0.85rem',
                                                    color: '#6c757d',
                                                    marginRight: '0.5rem'
                                                }}
                                            >
                                                {formatTimestamp(sentence.startTimeMs)}
                                            </span>

                                            {trackMode === 'word' ? (
                                                <span>
                                                    {words.map((w, wi) => {
                                                        const isCurrentWord =
                                                            isActiveSentence &&
                                                            activeWordIndex !== null &&
                                                            wi === activeWordIndex;
                                                        return (
                                                            <span
                                                                key={wi}
                                                                onClick={e => {
                                                                    e.stopPropagation();
                                                                    handleWordClick(
                                                                        sentence,
                                                                        nextSentence,
                                                                        wi,
                                                                        words.length
                                                                    );
                                                                }}
                                                                style={{
                                                                    cursor:
                                                                        sentence.startTimeMs !==
                                                                        undefined
                                                                            ? 'pointer'
                                                                            : 'default',
                                                                    backgroundColor:
                                                                        isCurrentWord
                                                                            ? '#ffe58a'
                                                                            : 'transparent',
                                                                    fontWeight:
                                                                        isCurrentWord
                                                                            ? 600
                                                                            : 400
                                                                }}
                                                            >
                                                                {w}
                                                                {wi < words.length - 1
                                                                    ? ' '
                                                                    : ''}
                                                            </span>
                                                        );
                                                    })}
                                                </span>
                                            ) : (
                                                <span>{sentence.originalText}</span>
                                            )}
                                        </div>
                                    );
                                })}
                            </div>
                        </Col>

                        <Col md={6}>
                            <h4>Human-verified captions</h4>
                            <div style={{ padding: '1rem' }}>
                                <p className="text-muted">
                                    Human-verified captions feature coming soon...
                                </p>
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

function mapVideoArticle(articleTemplate: ArticleTemplateDTO | undefined): IVideoArticle | undefined {
    if (articleTemplate === undefined) {
        return undefined;
    }

    return {
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
    return (
        minutes.toString().padStart(2, '0') +
        ':' +
        remainingSeconds.toString().padStart(2, '0')
    );
}

function getActiveWordIndex(
    sentence: ISentence,
    nextSentence: ISentence | undefined,
    currentTimeMs: number,
    wordCount: number
): number | null {
    if (wordCount === 0) return null;
    if (sentence.startTimeMs == null) return null;

    let start = sentence.startTimeMs;
    let end: number;

    if (sentence.endTimeMs != null) {
        end = sentence.endTimeMs;
    } else if (nextSentence && nextSentence.startTimeMs != null) {
        end = nextSentence.startTimeMs;
    } else {
        end = start + 4000;
    }

    if (end <= start) return null;

    if (currentTimeMs < start) return 0;
    if (currentTimeMs > end) return wordCount - 1;

    const totalDuration = end - start;
    const elapsed = currentTimeMs - start;

    let relative = elapsed / totalDuration;
    relative = Math.max(0, Math.min(relative, 1));

    let idx = Math.floor(relative * wordCount);
    return Math.min(idx, wordCount - 1);
}
