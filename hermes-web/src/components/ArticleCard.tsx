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
        url: string;
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
};

// Custom relative time formatter:
// 1m ago (minimum), Xh ago, Xd ago, Xw ago, X month(s) ago
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
    lockedSentences
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
        // ArticleDTO shape like in TranslateView/mapArticle:
        // articleData.title: ISentence[]
        // articleData.text:  ISentence[]
        const titleArr = (articleData as any).title || [];
        const textArr = (articleData as any).text || [];

        // Helper to check if a sentence is locked
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

    // Decide label based on context:
    // - If we have explicit totalSentences & lockedSentences and NO link
    //   → Text Storage mode → just show "N sentences".
    // - Otherwise (article lists with link) → show locked ratio.
    let progressText: string;
    if (total > 0) {
        if (!link && totalSentences != null && lockedSentences != null) {
            progressText = `${total} sentences`;
        } else {
            progressText = `${locked} / ${total} translations locked`;
        }
    } else {
        progressText = 'No sentence data';
    }

    const imageContent = (
        <div
            className="HArticleList-image d-flex align-items-end"
            style={{
                backgroundImage: `linear-gradient(
                    to bottom,
                    rgba(0, 0, 0, 0) 0%,
                    rgba(0, 0, 0, 0.3) 35%,
                    rgba(0, 0, 0, 0.7) 70%,
                    rgba(0, 0, 0, 1) 100%
                ), url(${photoURL})`,                backgroundSize: 'cover',
                backgroundPosition: 'center',
                height: '270px',
                cursor: link ? 'pointer' : 'default'
            }}
        >
            <div className="w-100 text-center text-white p-2">
                <h5 className="mb-0">
                    <strong>{title}</strong>
                </h5>
            </div>
        </div>
    );

    return (
        <Card className='HArticleList'>
            {/* Image area: clicking navigates to translate if link is provided */}
            {link ? (
                <Link
                    to={link.url + articleID}
                    style={{ textDecoration: 'none', color: 'inherit' }}
                >
                    {imageContent}
                </Link>
            ) : (
                imageContent
            )}

            {/* Info container below the image */}
            <CardBody className='pt-3'>
                <Row className='align-items-center mb-2'>
                    <Col xs={8}>
                        <span
                            className="badge badge-pill badge-light text-dark font-weight-bold mr-2"
                            style={{ border: '1px solid #ccc' }}
                        >
                            {languageID}
                        </span>
                        <small className='font-weight-bold text-dark'>
                            {formatRelativeTime(created)}
                        </small>
                    </Col>
                    <Col xs={4} className='text-right'>
                        {enableArchive && (
                            <Button
                                size='sm'
                                color='danger'
                                className='p-1 rounded-circle d-inline-flex align-items-center justify-content-center'
                                style={{ width: '32px', height: '32px' }}
                                onClick={() => onArchive(articleID)}
                                title="Archive"
                            >
                                {/* Archive icon (no text) - white on red for strong contrast */}
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

                {/* Locked / total sentences info */}
                <Row className='mb-2'>
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

                {/* Add to room button (if enabled) */}
                {enableAddToRoom && (
                    <Row className='mt-3'>
                        <Col className='text-right'>
                            <Button
                                size='sm'
                                color='light'
                                style={{ color: '#000', fontWeight: 700, border: '1px solid #ccc' }}
                                onClick={() => onAddToRoom && onAddToRoom(articleID)}
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
