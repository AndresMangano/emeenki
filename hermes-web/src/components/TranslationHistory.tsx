import React from 'react';
import { UncontrolledCollapse, Button, Container, Row, Col, Media } from 'reactstrap';
import { TextDiffer } from '../helpers/TextDiff';
import { Link } from 'react-router-dom';
import moment from 'moment';
import { LanguageFlag } from './LanguageFlag';

type TranslationHistoryProps = {
    onUpvote: (translationPos: number) => void;
    onDownvote: (translationPos: number) => void;
    buildCommentsUrl: (translationPos: number) => string;
    history: {
        translationPos: number;
        translation: string;
        userID: string;
        timestamp: Date;
        upvotesCount: number;
        downvotesCount: number;
        commentsCount: number;
        profilePhotoURL: string;
        nativeLanguageID: string;
    }[]
}
export function TranslationHistory({ onUpvote, onDownvote, buildCommentsUrl, history }: TranslationHistoryProps) {
    return(
        <UncontrolledCollapse className='mt-3 border-top border-left border-right border-dark' toggler='#historyToggler'>
            <ul className='articleTranslator-detailedForm-history list-unstyled'>
                { history.map((f, index) =>
                    <li key={index} className='mt-2'>
                        <Container
                            style={{
                                borderBottomStyle: 'solid',
                                borderBottomWidth: '1px',
                                borderBottomColor: 'black'
                            }}
                        >
                            <Media className="d-flex flex-row justify-content-between" heading>
                                    <Button
                                        size='sm'
                                        color='success'
                                        active={true}
                                        className='LikeButton oi oi-thumb-up'
                                        onClick={() => onUpvote(f.translationPos)}
                                    >
                                        {f.upvotesCount}
                                    </Button>
                                    <b><u><Link
                                        className='text-black pl-2 mt-2 ml-1'
                                        to={buildCommentsUrl(f.translationPos)}
                                        style={{ fontSize: '.7rem'}}
                                    >
                                        View Comments({f.commentsCount})
                                    </Link></u></b>
                                    <img
                                        className='d-flex ml-auto TranslationHistoryPhoto'
                                        src={f.profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : f.profilePhotoURL}
                                        alt='profile photo'
                                    />
                                    <Link to={`/profile/${f.userID}`}className="UserNameHistoryTranslation">{f.userID}</Link>
                                    <LanguageFlag languageID={f.nativeLanguageID} size='16px'/>
                                    <small className="d-flex ml-auto h6">{moment.utc(f.timestamp).fromNow()}</small>
                            </Media>
                            <Row className='pt-3'>
                                <p className='text-left' style={{ fontSize: '1.1rem'}}>
                                    { (index < 2 && history.length > 1)
                                        ?   (index === 0
                                                ? new TextDiffer(history[1].translation, f.translation)
                                                : new TextDiffer(history[0].translation, f.translation)
                                            ).getDiff().map((e, index, parts) =>
                                                <>
                                                    <small
                                                        key={index}
                                                        style={{
                                                            padding: '2px',
                                                            color: `${e.status === 2 ? '#FF4136' : 'inherited'}`,
                                                            backgroundColor: `${e.status === 2 ? '#FF4136' : e.status === 0 ? '#d8d509' : ''}`
                                                        }}
                                                    >
                                                        {e.text + (index + 1 < parts.length && parts[index + 1].status === 0 ? ' ' : '')}
                                                    </small>
                                                    <>{(index + 1 < parts.length && parts[index + 1].status !== 0 ? ' ' : '')}</>
                                                </>
                                            )
                                        : <small>{f.translation}</small>
                                    }
                                </p>
                            </Row>
                        </Container>
                    </li>
                )}
            </ul>
        </UncontrolledCollapse>
    );
}