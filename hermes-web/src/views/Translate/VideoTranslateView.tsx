// src/views/Translate/VideoTranslateView.tsx
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

import {
    CaptionSentencePart,
    CaptionSentenceUI,
    CaptionPartTimed,
    groupCaptionsIntoSentences,
    getActiveWordIndexForPart
} from '../../helpers/captionHelpers';

import {
    useYouTubePlayerTime,
    getYouTubeVideoId
} from '../../helpers/videoPlayerHelpers';

import { formatTimestamp } from '../../helpers/timeHelpers';

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

// NEW: add "off"
type TrackMode = 'off' | 'word' | 'sentence';

export function VideoTranslateView({ onError, history, match }: VideoTranslateViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { articleTemplateID } = match.params;

    useSignalR('articleTemplate:' + articleTemplateID);
    const { data: articleTemplateData } = useArticleTemplateQuery(articleTemplateID);
    const videoArticle: IVideoArticle | undefined = useMemo(function () {
        return mapVideoArticle(articleTemplateData);
    }, [articleTemplateData]);

    // Group raw caption chunks into logical sentences + timed parts
    const grouped = useMemo(function () {
        if (!videoArticle) {
            return {
                sentences: [] as CaptionSentenceUI[],
                flatParts: [] as CaptionPartTimed[]
            };
        }
        return groupCaptionsIntoSentences(videoArticle.text);
    }, [videoArticle]);

    const captionSentences = grouped.sentences;
    const flatParts = grouped.flatParts;

    // Custom hook: player ref + currentTimeMs + react-youtube handlers
    const {
        playerRef,
        currentTimeMs,
        handlePlayerReady,
        handlePlayerStateChange
    } = useYouTubePlayerTime(200);

    // default still "word"
    const [trackMode, setTrackMode] = useState<TrackMode>('word');

    // Find the single active part for the current time
    const activePart: CaptionPartTimed | null = useMemo(
        function () {
            if (!flatParts || flatParts.length === 0) return null;

            const now = currentTimeMs;
            let bestIndex = -1;

            for (let i = 0; i < flatParts.length; i++) {
                const p = flatParts[i];
                if (p.startTimeMs == null) continue;

                const start = p.startTimeMs;
                const end = p.endTimeMs != null ? p.endTimeMs : start + 4000;

                if (now >= start && now < end) {
                    bestIndex = i;
                    break;
                }
                if (now >= start) {
                    bestIndex = i;
                }
            }

            if (bestIndex === -1) return null;
            return flatParts[bestIndex];
        },
        [currentTimeMs, flatParts]
    );

    const activeSentenceRef = useRef<HTMLDivElement | null>(null);

    // Auto-scroll only when tracking is ON (word or sentence)
    useEffect(
        function () {
            if (trackMode === 'off') return;
            if (activeSentenceRef.current) {
                activeSentenceRef.current.scrollIntoView({
                    behavior: 'smooth',
                    block: 'center'
                });
            }
        },
        [activePart, trackMode]
    );

    const handleSentenceClick = (startTimeMs?: number) => {
        if (!playerRef.current || startTimeMs == null) return;
        playerRef.current.seekTo(startTimeMs / 1000, true);
        playerRef.current.playVideo();
    };

    // Seek based on a word within a specific part
    const handleWordClick = (
        part: CaptionSentencePart,
        wordIndex: number,
        wordCount: number
    ) => {
        if (!playerRef.current) return;
        if (part.startTimeMs == null || part.endTimeMs == null || wordCount <= 0) return;

        const start = part.startTimeMs;
        const end = part.endTimeMs;
        if (end <= start) return;

        let fraction = wordCount === 1 ? 0.0 : (wordIndex + 0.5) / wordCount;
        if (fraction < 0) fraction = 0;
        if (fraction > 1) fraction = 1;

        const targetMs = start + fraction * (end - start);
        const targetSeconds = targetMs / 1000;

        try {
            playerRef.current.seekTo(targetSeconds, true);
            playerRef.current.playVideo();
        } catch {
            // ignore
        }
    };

    return (
        <Container fluid>
            {videoArticle !== undefined && (
                <>
                    <Row className="mb-4">
                        <Col md={12}>
                            <h2>
                                {videoArticle.title
                                    .map(function (s) {
                                        return s.originalText;
                                    })
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
                                <span
                                    style={{
                                        marginRight: '0.5rem',
                                        fontWeight: 'bold'
                                    }}
                                >
                                    Track mode:
                                </span>
                                <ButtonGroup size="sm">
                                    <Button
                                        color={trackMode === 'off' ? 'primary' : 'secondary'}
                                        onClick={() => setTrackMode('off')}
                                    >
                                        Off
                                    </Button>
                                    <Button
                                        color={trackMode === 'word' ? 'primary' : 'secondary'}
                                        onClick={() => setTrackMode('word')}
                                    >
                                        Word
                                    </Button>
                                    <Button
                                        color={
                                            trackMode === 'sentence'
                                                ? 'primary'
                                                : 'secondary'
                                        }
                                        onClick={() => setTrackMode('sentence')}
                                    >
                                        Sentence
                                    </Button>
                                </ButtonGroup>
                            </div>
                        </Col>
                    </Row>

                    <Row>
                        <Col md={6}>
                            <h4>Original Captions</h4>
                            <div
                                className="app-video-captions"
                                style={{
                                    padding: '1rem'
                                }}
                            >
                                {captionSentences.map(function (sentence) {
                                    const isSentenceActive =
                                        activePart != null &&
                                        activePart.sentenceIndex ===
                                            sentence.sentenceIndex;

                                    const firstPart =
                                        sentence.parts.length > 0
                                            ? sentence.parts[0]
                                            : null;
                                    const sentenceStartTimeMs = firstPart
                                        ? firstPart.startTimeMs
                                        : undefined;

                                    const containerBackground =
                                        trackMode === 'sentence' && isSentenceActive
                                            ? '#fff3cd'
                                            : 'transparent';
                                    const containerBorder =
                                        trackMode === 'sentence' && isSentenceActive
                                            ? '1px solid #ffc107'
                                            : '1px solid transparent';

                                    return (
                                        <div
                                            key={sentence.sentenceIndex}
                                            ref={isSentenceActive ? activeSentenceRef : null}
                                            onClick={function () {
                                                handleSentenceClick(sentenceStartTimeMs);
                                            }}
                                            className="app-video-sentence"
                                            style={{
                                                cursor:
                                                    sentenceStartTimeMs !== undefined
                                                        ? 'pointer'
                                                        : 'default',
                                                backgroundColor: containerBackground,
                                                border: containerBorder,
                                                transition: 'background-color 0.15s ease'
                                            }}
                                        >
                                            {sentence.parts.map(function (part, partIndex) {
                                                const partStartMs = part.startTimeMs;

                                                // Original chunk for this part
                                                const originalChunk =
                                                    videoArticle &&
                                                    videoArticle.text[part.chunkIndex];
                                                const originalStartMs = originalChunk
                                                    ? originalChunk.startTimeMs
                                                    : undefined;

                                                // Only show timestamp if this part's start time
                                                // is exactly the original chunk's startTimeMs
                                                const showTimestamp =
                                                    typeof originalStartMs === 'number' &&
                                                    partStartMs === originalStartMs;

                                                const isActivePart =
                                                    activePart != null &&
                                                    activePart.sentenceIndex ===
                                                        sentence.sentenceIndex &&
                                                    activePart.partIndexInSentence ===
                                                        partIndex;

                                                const words =
                                                    part.text && part.text.length > 0
                                                        ? part.text.split(/\s+/)
                                                        : [];
                                                let activeWordIndex: number | null = null;

                                                if (
                                                    trackMode === 'word' &&
                                                    isActivePart &&
                                                    words.length > 0 &&
                                                    part.startTimeMs != null &&
                                                    part.endTimeMs != null
                                                ) {
                                                    activeWordIndex = getActiveWordIndexForPart(
                                                        part,
                                                        currentTimeMs,
                                                        words.length
                                                    );
                                                }

                                                return (
                                                    <span
                                                        key={partIndex}
                                                        className="app-video-sentence-part"
                                                        style={{ marginRight: '0.25rem' }}
                                                    >
                                                        {showTimestamp && (
                                                            <span
                                                                className="app-video-timestamp"
                                                                onClick={function (e) {
                                                                    e.stopPropagation();
                                                                    handleSentenceClick(
                                                                        partStartMs
                                                                    );
                                                                }}
                                                                style={{
                                                                    fontSize: '0.85rem',
                                                                    color: '#6c757d',
                                                                    marginRight: '0.25rem'
                                                                }}
                                                            >
                                                                ({formatTimestamp(partStartMs)})
                                                            </span>
                                                        )}

                                                        {trackMode === 'word' ? (
                                                            <span className="app-video-text">
                                                                {words.map(function (w, wi) {
                                                                    const isCurrentWord =
                                                                        isActivePart &&
                                                                        activeWordIndex !==
                                                                            null &&
                                                                        wi ===
                                                                            activeWordIndex;

                                                                    return (
                                                                        <span
                                                                            key={wi}
                                                                            onClick={function (
                                                                                e
                                                                            ) {
                                                                                e.stopPropagation();
                                                                                handleWordClick(
                                                                                    part,
                                                                                    wi,
                                                                                    words.length
                                                                                );
                                                                            }}
                                                                            style={{
                                                                                cursor:
                                                                                    partStartMs !==
                                                                                    undefined
                                                                                        ? 'pointer'
                                                                                        : 'default',
                                                                                backgroundColor:
                                                                                    isCurrentWord
                                                                                        ? '#ffe58a'
                                                                                        : 'transparent'
                                                                            }}
                                                                        >
                                                                            {w}
                                                                            {wi <
                                                                            words.length - 1
                                                                                ? ' '
                                                                                : ''}
                                                                        </span>
                                                                    );
                                                                })}
                                                            </span>
                                                        ) : (
                                                            <span className="app-video-text">
                                                                {part.text}
                                                            </span>
                                                        )}
                                                    </span>
                                                );
                                            })}
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

function mapVideoArticle(
    articleTemplate: ArticleTemplateDTO | undefined
): IVideoArticle | undefined {
    if (articleTemplate === undefined) {
        return undefined;
    }

    return {
        articleTemplateID: articleTemplate.articleTemplateID,
        videoURL: articleTemplate.videoURL,
        photoUrl: articleTemplate.photoURL,
        title: articleTemplate.title.map(function (e, index) {
            return {
                sentencePos: index,
                originalText: e.text,
                startTimeMs: e.startTimeMs,
                endTimeMs: e.endTimeMs
            };
        }),
        text: articleTemplate.text.map(function (e, index) {
            return {
                sentencePos: index,
                originalText: e.text,
                startTimeMs: e.startTimeMs,
                endTimeMs: e.endTimeMs
            };
        })
    };
}
