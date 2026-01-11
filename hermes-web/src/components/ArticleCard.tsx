import React from 'react';
import { CardBody, Row, Col, Button, Card, Progress } from 'reactstrap';
import moment from 'moment';
import { Link } from 'react-router-dom';
import { useArticleQuery } from '../services/queries-service';
import { LOCK_THRESHOLD } from '../utils/util';

type ArticleCardProps = {
    onAddToRoom?: (articleID: string) => void;
    onArchive: (articleID: string) => void;
    link?: {
        label: string; // kept for compatibility but not rendered
        url: string;   // base path, ArticleCard will append an ID
    };
    enableAddToRoom: boolean;
    enableArchive: boolean;
    title: string;
    photoURL: string;
    articleID: string;
    languageID: string;
    created: Date;

    // Optional overrides; if provided, they win over computed values
    totalSentences?: number;
    lockedSentences?: number;

    // Video-related (optional)
    isVideo?: boolean;
    videoURL?: string;

    // Optional: override the ID used in the URL (e.g. articleTemplateID for video-translate)
    targetIDForLink?: string;

    // NEW: topic to display in header area
    topicName?: string; // or topicID if you prefer, it's just a string label
};

// Custom relative time formatter
function formatRelativeTime(created: Date | string): string {
    const createdMoment = moment.utc(created);
    const now = moment.utc();

    let diffMinutes = now.diff(createdMoment, 'minutes');
    if (diffMinutes < 1) diffMinutes = 1; // minimum 1m

    if (diffMinutes < 60) {
        return `${diffMinutes}m ago`;
    }

    const diffHours = now.diff(createdMoment, 'hours');
    if (diffHours < 24) {
        return `${diffHours}h ago`;
    }

    const diffDays = now.diff(createdMoment, 'days');
    if (diffDays < 7) {
        return `${diffDays}d ago`;
    }

    const diffWeeks = now.diff(createdMoment, 'weeks');
    if (diffWeeks < 4) {
        return `${diffWeeks}w ago`;
    }

    const diffMonths = now.diff(createdMoment, 'months');
    if (diffMonths <= 1) {
        return '1 month ago';
    }
    return `${diffMonths} months ago`;
}

// Extract YouTube video ID from common URL formats
function extractYouTubeVideoId(url: string): string | null {
    try {
        // Handle plain IDs just in case
        if (/^[a-zA-Z0-9_-]{11}$/.test(url)) return url;

        const u = new URL(url);

        // https://www.youtube.com/watch?v=VIDEO_ID
        if (u.hostname.indexOf('youtube.com') !== -1) {
            const v = u.searchParams.get('v');
            if (v) return v;

            // https://www.youtube.com/embed/VIDEO_ID
            const parts = u.pathname.split('/');
            const embedIndex = parts.indexOf('embed');
            if (embedIndex >= 0 && parts[embedIndex + 1]) {
                return parts[embedIndex + 1];
            }
        }

        // https://youtu.be/VIDEO_ID
        if (u.hostname === 'youtu.be') {
            const id = u.pathname.replace('/', '');
            if (id) return id;
        }
    } catch {
        // ignore parse errors
    }
    return null;
}

function getYouTubeThumbnail(videoURL?: string, fallback?: string): string | undefined {
    if (!videoURL) return fallback;
    const id = extractYouTubeVideoId(videoURL);
    if (!id) return fallback;
    return 'https://img.youtube.com/vi/' + id + '/hqdefault.jpg';
}

export function ArticleCard({
    onAddToRoom,
    onArchive,
    link,
    enableAddToRoom,
    enableArchive,
    title,
    photoURL,
    articleID,
    languageID,
    created,
    totalSentences,
    lockedSentences,
    isVideo,
    videoURL,
    targetIDForLink,
    topicName
}: ArticleCardProps) {
    // Load full article to compute sentence/lock counts when overrides are not provided
    const { data: articleData } = useArticleQuery(articleID);

    let total = 0;
    let locked = 0;

    if (totalSentences != null && lockedSentences != null) {
        // If parent explicitly passes values, use those
        total = totalSentences;
        locked = lockedSentences;
    } else if (articleData) {
        // ArticleDTO shape:
        // articleData.title: ISentence[]
        // articleData.text:  ISentence[]
        const titleArr = (articleData as any).title || [];
        const textArr = (articleData as any).text || [];

        const isSentenceLocked = (sentence: any): boolean => {
            const hist = sentence.translationHistory || [];
            if (hist.length === 0) return false;
            const top = hist[0];
            const upvotes = (top.upvotes || []) as string[];
            return upvotes.length >= LOCK_THRESHOLD;
        };

        // Count title sentences
        for (let i = 0; i < titleArr.length; i++) {
            total++;
            if (isSentenceLocked(titleArr[i])) {
                locked++;
            }
        }

        // Count text sentences (flat array)
        for (let i = 0; i < textArr.length; i++) {
            total++;
            if (isSentenceLocked(textArr[i])) {
                locked++;
            }
        }
    }

    const completion = total > 0 ? Math.round((locked * 100) / total) : 0;

    // Decide label based on context
    let progressText: string;
    if (total > 0) {
        if (!link && totalSentences != null && lockedSentences != null) {
            progressText = total + ' sentences';
        } else {
            progressText = locked + ' / ' + total + ' translations locked';
        }
    } else {
        progressText = 'No sentence data';
    }

    const effectivePhotoURL = isVideo
        ? getYouTubeThumbnail(videoURL, photoURL) || photoURL
        : photoURL;

    const imageContent = (
        <div
            className="HArticleList-image d-flex align-items-end position-relative"
            style={{
                backgroundImage: `linear-gradient(
                    to bottom,
                    rgba(0, 0, 0, 0) 0%,
                    rgba(0, 0, 0, 0.3) 35%,
                    rgba(0, 0, 0, 0.7) 70%,
                    rgba(0, 0, 0, 1) 100%
                ), url(${effectivePhotoURL})`,
                backgroundSize: 'cover',
                backgroundPosition: 'center',
                height: '270px',
                cursor: link ? 'pointer' : 'default'
            }}
        >
            {isVideo && (
                <div
                    style={{
                        position: 'absolute',
                        top: 8,
                        left: 8,
                        display: 'flex',
                        alignItems: 'center',
                        gap: 6
                    }}
                >
                    <span className="badge badge-danger">VIDEO</span>
                    <span
                        style={{
                            width: 24,
                            height: 24,
                            borderRadius: '50%',
                            backgroundColor: 'rgba(0,0,0,0.6)',
                            display: 'inline-flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            color: '#fff',
                            fontSize: '0.8rem'
                        }}
                    >
                        ▶
                    </span>
                </div>
            )}
            <div className="w-100 text-center text-white p-2">
                <h5 className="mb-0">
                    <strong>{title}</strong>
                </h5>
            </div>
        </div>
    );

    const navID = targetIDForLink || articleID;
    const relativeTime = formatRelativeTime(created);

    return (
        <Card className='HArticleList'>
            {/* Image area: clicking navigates if link is provided */}
            {link ? (
                <Link
                    to={link.url + navID}
                    style={{ textDecoration: 'none', color: 'inherit' }}
                >
                    {imageContent}
                </Link>
            ) : (
                imageContent
            )}

            <CardBody className='pt-3'>
                {/* TOP ROW: Topic on left, archive on right */}
                <Row className='align-items-center mb-2'>
                    <Col xs={8}>
                        {topicName && (
                            <span
                                className="badge badge-pill badge-info text-white font-weight-bold"
                                style={{ border: '1px solid #007bff' }}
                            >
                                {topicName}
                            </span>
                        )}
                    </Col>
                    <Col xs={4} className='text-right'>
                        {enableArchive && (
                            <Button
                                size='sm'
                                color='danger'
                                className='p-1 rounded-circle d-inline-flex align-items-center justify-content-center'
                                style={{ width: '32px', height: '32px' }}
                                onClick={function () { onArchive(articleID); }}
                                title="Archive"
                            >
                                <span
                                    role="img"
                                    aria-label="Archive"
                                    style={{ fontSize: '1.2rem', color: '#FFF' }}
                                >
                                    🗄
                                </span>
                            </Button>
                        )}
                    </Col>
                </Row>

                {/* PROGRESS ROW */}
                <Row className='mb-1'>
                    <Col xs={12}>
                        <small className='d-block mb-1 text-dark font-weight-bold'>
                            {progressText}
                        </small>
                        <Progress
                            value={completion}
                            color='success'
                            style={{ height: '8px' }}
                        />
                    </Col>
                </Row>

                {/* NEW ROW: Language + relative time under the progress bar */}
                <Row className='mb-2'>
                    <Col xs={12}>
                        <span
                            className="badge badge-pill badge-light text-dark font-weight-bold mr-2"
                            style={{ border: '1px solid #ccc' }}
                        >
                            {languageID}
                        </span>
                        <small className='font-weight-bold text-dark'>
                            {relativeTime}
                        </small>
                    </Col>
                </Row>

                {/* Add to room button (if enabled) */}
                {enableAddToRoom && (
                    <Row className='mt-2'>
                        <Col className='text-right'>
                            <Button
                                size='sm'
                                color='light'
                                style={{ color: '#000', fontWeight: 700, border: '1px solid #ccc' }}
                                onClick={function () { if (onAddToRoom) onAddToRoom(articleID); }}
                            >
                                Add to room
                            </Button>
                        </Col>
                    </Row>
                )}
            </CardBody>
        </Card>
    );
}
