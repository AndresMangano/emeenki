import React, { useReducer } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { ArticleTranslatorPhoto } from './ArticleTranslatorPhoto';
import { Sentence } from './Sentence';
import { Translation } from './Translation';
import { CommentForm } from './CommentForm';
import { DetailedTranslationForm } from './DetailedTranslationForm';
import { LOCK_THRESHOLD } from '../utils/util';

type ArticleTranslatorProps = {
    onSubmitTranslation: (inText: boolean, sentencePos: number, translation: string) => void;
    onSubmitComment: (inText: boolean, sentencePos: number, translationPos: number, comment: string) => void;
    onUpvote: (inText: boolean, sentencePos: number, translationPos: number, redirect: boolean) => void;
    onDownvote: (inText: boolean, sentencePos: number, translationPos: number) => void;
    buildTranslateUrl: (inText: boolean, sentencePos: number) => string;
    buildCommentsUrl: (inText: boolean, sentencePos: number, translationPos: number) => string;
    rootUrl: string;
    userID: string;
    photoUrl: string;
    title: {
        sentencePos: number;
        originalText: string;
        translationHistory: {
            translationPos: number;
            translation: string;
            userID: string;
            profilePhotoURL: string;
            nativeLanguageID: string;
            timestamp: Date;
            upvotes: string[];
            upvotesCount: number;
            downvotesCount: number;
            commentsCount: number;
        }[];
    }[];
    text: {
        sentencePos: number;
        originalText: string;
        translationHistory: {
            translationPos: number;
            translation: string;
            userID: string;
            profilePhotoURL: string;
            nativeLanguageID: string;
            timestamp: Date;
            upvotes: string[];
            upvotesCount: number;
            downvotesCount: number;
            commentsCount: number;
        }[];
    }[][];
    selected: {
        inText: boolean;
        sentencePos: number;
        translationPos?: number;
    } | null;
    comments: {
        profilePhotoUrl: string;
        userID: string;
        timestamp: Date;
        comment: string;
    }[] | null;
};

export function ArticleTranslator({
    onSubmitTranslation,
    onSubmitComment,
    onUpvote,
    onDownvote,
    buildTranslateUrl,
    buildCommentsUrl,
    rootUrl,
    userID,
    photoUrl,
    title,
    text,
    selected,
    comments
}: ArticleTranslatorProps) {
    const [{ hovered }, dispatch] = useReducer(reducer, {
        hovered: null
    });

    return (
        <Container fluid style={{ fontFamily: 'Roboto Mono, monospace', lineHeight: 1 }}>
            <Row className='align-items-center mb-3'>
                <Col md={6}>
                    <div className="display-5 mb-4 text-center font-weight-bold">
                        {title.map((s, sIndex) => (
                            <Sentence
                                key={sIndex}
                                isTitle={true}
                                originalText={s.originalText}
                                translateUrl={buildTranslateUrl(false, s.sentencePos)}
                                textColor={getTextColor(s, userID)}
                                hovered={
                                    hovered !== null &&
                                    !hovered.inText &&
                                    hovered.sentencePos === s.sentencePos
                                }
                                isSelected={
                                    selected !== null &&
                                    !selected.inText &&
                                    selected.sentencePos == s.sentencePos
                                }
                            />
                        ))}
                    </div>
                </Col>
                <Col md={6}>
                    <div className="display-5 mb-4 text-center font-weight-bold">
                        {title.map((s, sIndex) => {
                            const isSelectedTitle =
                                selected !== null &&
                                !selected.inText &&
                                selected.sentencePos === s.sentencePos;

                            // Locked for this user if top translation meets locked criteria and is not theirs
                            const lockedForCurrentUser = isLockedForUser(s, userID);

                            return (
                                <React.Fragment key={sIndex}>
                                    {isSelectedTitle ? (
                                        <DetailedTranslationForm
                                            readOnly={lockedForCurrentUser}
                                            onSubmit={translation =>
                                                onSubmitTranslation(false, s.sentencePos, translation)
                                            }
                                            onUpvote={translationPos =>
                                                onUpvote(false, s.sentencePos, translationPos, true)
                                            }
                                            onDownvote={translationPos =>
                                                onDownvote(false, s.sentencePos, translationPos)
                                            }
                                            buildCommentsUrl={translationPos =>
                                                buildCommentsUrl(false, s.sentencePos, translationPos)
                                            }
                                            cancelUrl={rootUrl}
                                            history={s.translationHistory}
                                        />
                                    ) : (
                                        <Translation
                                            onHovered={(sentencePos, hoveredFlag) =>
                                                dispatch(
                                                    hoveredFlag
                                                        ? {
                                                              _type: 'SELECT',
                                                              inText: false,
                                                              sentencePos
                                                          }
                                                        : { _type: 'UNSELECT' }
                                                )
                                            }
                                            onUpvoted={sentencePos =>
                                                onUpvote(
                                                    false,
                                                    sentencePos,
                                                    s.translationHistory[0].translationPos,
                                                    false
                                                )
                                            }
                                            inText={false}
                                            sentencePos={s.sentencePos}
                                            hovered={
                                                hovered === null
                                                    ? null
                                                    : !hovered.inText &&
                                                      hovered.sentencePos === s.sentencePos
                                            }
                                            originalText={s.originalText}
                                            translation={
                                                s.translationHistory.length > 0
                                                    ? s.translationHistory[0].translation
                                                    : null
                                            }
                                            translateUrl={buildTranslateUrl(false, s.sentencePos)}
                                            textColor={getTextColor(s, userID)}
                                        />
                                    )}
                                </React.Fragment>
                            );
                        })}
                    </div>
                </Col>
            </Row>
            <Row className='align-items-center text-center mb-3'>
                <Col md={{ size: 6, offset: 3 }}>
                    <ArticleTranslatorPhoto photoUrl={photoUrl} />
                </Col>
            </Row>
            <>
                {text.map((g, gIndex) => (
                    <Row key={gIndex} className='mb-3'>
                        <Col md={6}>
                            <div className="articleText">
                                {g.map((s, sIndex) => (
                                    <React.Fragment key={sIndex}>
                                        <Sentence
                                            isTitle={false}
                                            originalText={s.originalText}
                                            translateUrl={buildTranslateUrl(true, s.sentencePos)}
                                            textColor={getTextColor(s, userID)}
                                            hovered={
                                                hovered !== null &&
                                                hovered.inText &&
                                                hovered.sentencePos === s.sentencePos
                                            }
                                            isSelected={
                                                selected !== null &&
                                                selected.inText &&
                                                selected.sentencePos == s.sentencePos
                                            }
                                        />
                                    </React.Fragment>
                                ))}
                            </div>
                        </Col>
                        <Col md={6}>
                            <div className="articleText">
                                {g.map((s, sIndex) => {
                                    const isSelectedText =
                                        selected !== null &&
                                        selected.inText &&
                                        selected.sentencePos === s.sentencePos;

                                    const lockedForCurrentUser = isLockedForUser(s, userID);

                                    return (
                                        <React.Fragment key={sIndex}>
                                            {isSelectedText ? (
                                                <DetailedTranslationForm
                                                    readOnly={lockedForCurrentUser}
                                                    onSubmit={translation =>
                                                        onSubmitTranslation(true, s.sentencePos, translation)
                                                    }
                                                    onUpvote={translationPos =>
                                                        onUpvote(true, s.sentencePos, translationPos, true)
                                                    }
                                                    onDownvote={translationPos =>
                                                        onDownvote(true, s.sentencePos, translationPos)
                                                    }
                                                    buildCommentsUrl={translationPos =>
                                                        buildCommentsUrl(true, s.sentencePos, translationPos)
                                                    }
                                                    cancelUrl={rootUrl}
                                                    history={s.translationHistory}
                                                />
                                            ) : (
                                                <Translation
                                                    onHovered={(sentencePos, hoveredFlag) =>
                                                        dispatch(
                                                            hoveredFlag
                                                                ? {
                                                                      _type: 'SELECT',
                                                                      inText: true,
                                                                      sentencePos
                                                                  }
                                                                : { _type: 'UNSELECT' }
                                                        )
                                                    }
                                                    onUpvoted={sentencePos =>
                                                        onUpvote(
                                                            true,
                                                            sentencePos,
                                                            s.translationHistory[0].translationPos,
                                                            false
                                                        )
                                                    }
                                                    inText={true}
                                                    sentencePos={s.sentencePos}
                                                    hovered={
                                                        hovered == null
                                                            ? null
                                                            : hovered.inText &&
                                                              hovered.sentencePos === s.sentencePos
                                                    }
                                                    originalText={s.originalText}
                                                    translation={
                                                        s.translationHistory.length > 0
                                                            ? s.translationHistory[0].translation
                                                            : null
                                                    }
                                                    translateUrl={buildTranslateUrl(true, s.sentencePos)}
                                                    textColor={getTextColor(s, userID)}
                                                />
                                            )}
                                        </React.Fragment>
                                    );
                                })}
                            </div>
                        </Col>
                    </Row>
                ))}
                {comments !== null && selected !== null && (
                    <CommentForm
                        onSubmit={comment => {
                            selected !== null &&
                                selected.translationPos !== undefined &&
                                onSubmitComment(
                                    selected.inText,
                                    selected.sentencePos,
                                    selected.translationPos,
                                    comment
                                );
                        }}
                        closeUrl={buildTranslateUrl(selected.inText, selected.sentencePos)}
                        comments={comments}
                    />
                )}
            </>
        </Container>
    );
}

type State = {
    hovered: {
        inText: boolean;
        sentencePos: number;
    } | null;
};
type Action =
    | { _type: 'SELECT'; inText: boolean; sentencePos: number }
    | { _type: 'UNSELECT' };

function reducer(state: State, action: Action): State {
    switch (action._type) {
        case 'SELECT':
            return { ...state, hovered: { inText: action.inText, sentencePos: action.sentencePos } };
        case 'UNSELECT':
            return { ...state, hovered: null };
    }
}

// Locked rule for COLOR
function isLocked(text: { translationHistory: { upvotesCount: number }[] }): boolean {
    if (!text.translationHistory || text.translationHistory.length === 0) return false;
    const top = text.translationHistory[0];
    return top.upvotesCount >= LOCK_THRESHOLD;
}

// Locked rule for EDITING: locked AND authored by someone else
function isLockedForUser(
    text: {
        translationHistory: {
            upvotesCount: number;
            userID: string;
        }[];
    },
    userID: string
): boolean {
    if (!text.translationHistory || text.translationHistory.length === 0) return false;
    const top = text.translationHistory[0];
    // re-use same lock rule + ensure it's not the author's own translation
    return isLocked(text) && top.userID !== userID;
}

function getTextColor(
    text: {
        translationHistory: {
            userID: string;
            upvotes: string[];
            upvotesCount: number;
        }[];
    },
    userID: string
): string {
    // Empty → "empty" color
    if (text.translationHistory.length === 0) return 'translationColor-notTranslated';

    const top = text.translationHistory[0];
    const likedByUser = top.upvotes.includes(userID);
    const locked = isLocked(text);

    // Locked & liked by me → special class for blue text + green-ish background
    if (locked && likedByUser) {
        return 'translationColor-lockedLiked';
    }

    // Locked but not liked by me → plain locked blue
    if (locked) {
        return 'translationColor-locked';
    }

    let count = 0;
    for (let i = 1; i < text.translationHistory.length; i++) {
        count += text.translationHistory[i].userID === userID ? 1 : 0;
    }

    if (top.userID === userID) return 'translationColor-translatedByMe';
    else if (likedByUser) return 'translationColor-likedByMe';
    else if (count > 0) return 'translationColor-replacedByOthers';
    else return 'translationColor-translatedByOthers';
}
