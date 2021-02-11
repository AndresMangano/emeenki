import React, { useMemo } from 'react';
import { Card, CardBody, Col, Container, Row } from 'reactstrap';
import { RouteComponentProps } from 'react-router-dom';
import { ArticleTranslator } from '../../components/ArticleTranslator';
import { ArticleAPI, ArticleDTO } from '../../api/ArticleAPI';
import { ArticleCommentsPanel } from '../../components/article/ArticleCommentsPanel';
import { useArticleQuery } from '../../services/queries-service';
import { useSignalR } from '../../services/signalr-service';
import { ArticleCommentForm } from '../../components/article/ArticleCommentForm';
import { ArticleComment } from '../../components/article/ArticleComment';

interface ISentence
{
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
        comments: IComment[];
    }[];
}
interface IComment
{
    profilePhotoUrl: string;
    userID: string;
    timestamp: Date;
    comment: string;
}
interface IArticleComment
{
    profilePhotoURL: string;
    userID: string;
    timestamp: Date;
    comment: string;
    commentIndex: number;
    childCommentIndex: number|null;
}
interface IArticle
{
    articleID: string;
    photoUrl: string;
    title: ISentence[];
    text: ISentence[];
    comments: IArticleComment[];
}

type TranslateViewProps = RouteComponentProps<{ articleID: string, inText?: string, sentencePos?: string, translationPos?: string, comments?: string }> & {
    onError: (error: any) => void;
}
export function TranslateView({ onError, history, match }: TranslateViewProps) {
    const userID = localStorage.getItem('hermes.userID') || '';
    const { articleID } = match.params;
    const pInText = match.params.inText === 'true';
    const pSentencePos = match.params.sentencePos === undefined ? undefined : parseInt(match.params.sentencePos, 10);
    const pTranslationPos = match.params.translationPos === undefined ? undefined : parseInt(match.params.translationPos, 10);
    const comments = match.params.comments === 'true';

    useSignalR(`article:${articleID}`);
    const { data: articleData } = useArticleQuery(articleID);
    const article: IArticle|undefined = useMemo(() => mapArticle(articleData), [articleData]);

    function handleDeleteArticleComment(commentPos: number, childCommentPos: number|null) {
        if (article !== undefined) {
            ArticleAPI.deleteMainComment({
                articleID: article.articleID,
                commentPos,
                childCommentPos
            })
            .then(() => history.push(`/translate/${articleID}`))
            .catch(onError);
        }
    }
    function handleCommentArticle(comment: string) {
        if (article !== undefined) {
            ArticleAPI.commentMain({
                articleID: article.articleID,
                comment,
                parentCommentPos: null
            })
            .then(() => history.push(`/translate/${articleID}`))
            .catch(onError);
        }
    }

    function handleCommentRepliesArticle(comment: string, parentCommentPos: number|null) {
        if (article !== undefined) {
            ArticleAPI.commentMain({
                articleID: article.articleID,
                comment,
                parentCommentPos,
            })
            .then(() => history.push(`/translate/${articleID}`))
            .catch(onError);
        }
    }
    
    return (
        <Container fluid>
            { (article !== undefined) &&
                <>
                    <ArticleTranslator
                        onSubmitTranslation={(inText, sentencePos, translation) =>
                            SubmitTranslation(articleID, userID, inText, sentencePos, translation,
                                () => history.push(`/translate/${articleID}`),
                                onError
                            )}
                        onSubmitComment={(inText, sentencePos, translationPos, comment) =>
                            SubmitComment(articleID, inText, sentencePos, translationPos, comment, userID,
                                () => history.push(`/translate/${articleID}/${inText}/${sentencePos}/${translationPos}/true`),
                                onError
                            )}
                        onUpvote={(inText, sentencePos, translationPos, redirect) =>
                            UpvoteTranslation(articleID, inText, sentencePos, translationPos, userID,
                                !redirect
                                    ? () => history.push(`/translate/${articleID}`)
                                    : () => history.push(`/translate/${articleID}/${inText}/${sentencePos}/${translationPos}`),
                                onError
                            )}
                        onDownvote={(inText, sentencePos, translationPos) =>
                            DownvoteTranslation(articleID, inText, sentencePos, translationPos, userID,
                                () => history.push(`/translate/${articleID}/${inText}/${sentencePos}/${translationPos}`),
                                onError
                            )}
                        buildTranslateUrl={(inText, index) => `/translate/${articleID}/${inText}/${index}`}
                        buildCommentsUrl={(inText, sentencePos, translationPos) => `/translate/${articleID}/${inText}/${sentencePos}/${translationPos}/true`}
                        rootUrl={`/translate/${articleID}`}
                        userID={userID}
                        photoUrl={article.photoUrl}
                        title={article.title}
                        text={GroupParagraphs(article.text)}
                        selected={(pInText === undefined || pSentencePos === undefined)
                            ?   null
                            :   {
                                    inText: pInText,
                                    sentencePos: pSentencePos,
                                    translationPos:
                                        pTranslationPos === undefined
                                            ?   undefined
                                            :   pTranslationPos
                                }
                        }
                        comments={comments && pSentencePos !== undefined && pTranslationPos !== undefined
                            ?   GetComments(article, pInText, pSentencePos, pTranslationPos)
                            :   null
                        }
                    />
                    <hr />
                    <Row>
                        <Col md={6}>
                            <ArticleCommentsPanel
                                form={
                                    <ArticleCommentForm 
                                        onSubmit={handleCommentArticle} 
                                        parentCommentPos={null}
                                    />

                                }
                            >
                                { article.comments.filter(c => c.childCommentIndex === null).map((e, index) =>
                                    <React.Fragment key={index}>
                                        <ArticleComment {...e} 
                                            key={index}
                                            onSubmit={handleCommentRepliesArticle}
                                            actualUserID={userID}
                                            onDelete={handleDeleteArticleComment}
                                            replies={article.comments.filter(c => c.childCommentIndex !== null && c.commentIndex === e.commentIndex)}
                                        />
                                        <hr />
                                    </React.Fragment>
                                )}
                            </ArticleCommentsPanel>
                        </Col>
                        <Col md={6}>
                            <Card className="app-translation-color-card">
                                <CardBody className="app-translation-color-card-body">
                                    <p className="app-translation-color-card-body-empty">■ Empty Translation</p>
                                    <p className="app-translation-color-card-body-your">■ Your Translation or Liked Translation</p>
                                    <p className="app-translation-color-card-body-activity">■ Activity in your Translation</p>
                                    <p className="app-translation-color-card-body-other">■ Other User's Translation</p>
                                </CardBody>
                            </Card>
                        </Col>
                    </Row>
                </>
            }
        </Container>
    );
}

function GroupParagraphs(
    sentences: ISentence[]
) : ISentence[][]
{
    let result: ISentence[][] = [];
    result[0] = [];
    let actual = 0;

    sentences.forEach((s, index) => {
        if(s.originalText.charCodeAt(0) === 10){
            actual++;
            result[actual] = [];
            result[actual].push(s);
        } else {
            result[actual].push(s);
        }
    });

    return result;
}

function mapArticle(article: ArticleDTO|undefined) : IArticle|undefined
{
    return article === undefined
        ?   undefined
        :   {
                articleID: article.articleID,
                photoUrl: article.photoURL,
                title: article.title.map((e, index) => ({
                    sentencePos: e.sentenceIndex,
                    originalText: e.originalText,
                    translationHistory: e.translationHistory.map((th, thIndex) => ({
                        translationPos: th.translationIndex,
                        translation: th.translation,
                        userID: th.userID,
                        profilePhotoURL: th.profilePhotoURL,
                        nativeLanguageID: th.nativeLanguageID,
                        timestamp: th.timestamp,
                        upvotes: th.upvotes,
                        upvotesCount: th.upvotes.length,
                        downvotesCount: th.downvotes.length,
                        commentsCount: th.comments.length,
                        comments: th.comments.map((c, cIndex) => ({
                            profilePhotoUrl: c.profilePhotoURL,
                            userID: c.userID,
                            timestamp: c.timestamp,
                            comment: c.comment
                        }))
                    }))
                })),
                text: article.text.map((e, index) => ({
                    sentencePos: e.sentenceIndex,
                    originalText: e.originalText,
                    translationHistory: e.translationHistory.map((th, thIndex) => ({
                        translationPos: th.translationIndex,
                        translation: th.translation,
                        userID: th.userID,
                        profilePhotoURL: th.profilePhotoURL,
                        nativeLanguageID: th.nativeLanguageID,
                        timestamp: th.timestamp,
                        upvotes: th.upvotes,
                        upvotesCount: th.upvotes.length,
                        downvotesCount: th.downvotes.length,
                        commentsCount: th.comments.length,
                        comments: th.comments.map((c, cIndex) => ({
                            profilePhotoUrl: c.profilePhotoURL,
                            userID: c.userID,
                            timestamp: c.timestamp,
                            comment: c.comment
                        }))
                    }))
                })),
                comments: article.comments
            };
}

function GetComments(article: IArticle, inText: boolean, sentencePos: number, translationPos: number) : IComment[]
{
    if(inText){
        let sentence = article.text.find(t => t.sentencePos === sentencePos);
        let translation = sentence && sentence.translationHistory.find(th => th.translationPos == translationPos);
        return translation === undefined ? [] : translation.comments;
    } else {
        let sentence = article.title.find(t => t.sentencePos === sentencePos);
        let translation = sentence && sentence.translationHistory.find(th => th.translationPos == translationPos);
        return translation === undefined ? [] : translation.comments;
    }
}

function SubmitTranslation(
    articleID: string,
    userID: string,
    inText: boolean,
    sentencePos: number,
    translation: string,
    OnSubmit: () => void,
    onError: (error: any) => void)
{
    ArticleAPI.translate({
        articleID: articleID,
        inText: inText,
        sentencePos: sentencePos,
        translation: translation,
        userID: userID
    })
    .then(result => OnSubmit())
    .catch(onError);
}

function SubmitComment(
    articleID: string,
    inText: boolean,
    sentencePos: number,
    translationPos: number,
    comment: string,
    userID: string,
    OnSubmit: () => void,
    onError: (error: any) => void)
{
    ArticleAPI.comment({
        articleID: articleID,
        inText: inText,
        sentencePos: sentencePos,
        translationPos: translationPos,
        comment: comment,
        userID: userID
    })
    .then(result => OnSubmit())
    .catch(onError);
}

function UpvoteTranslation(
    articleID: string,
    inText: boolean,
    sentencePos: number,
    translationPos: number,
    userID: string,
    OnSubmit: () => void,
    onError: (error: any) => void)
{
    ArticleAPI.vote({
        articleID: articleID,
        inText:  inText,
        sentencePos: sentencePos,
        translationPos: translationPos,
        positive: true,
        userID: userID
    })
    .then(result => OnSubmit())
    .catch(onError);
}

function DownvoteTranslation(
    articleID: string,
    inText: boolean,
    sentencePos: number,
    translationPos: number,
    userID: string,
    OnSubmit: () => void,
    onError: (error: any) => void)
{
    ArticleAPI.vote({
        articleID: articleID,
        inText:  inText,
        sentencePos: sentencePos,
        translationPos: translationPos,
        positive: false,
        userID: userID
    })
    .then(result => OnSubmit())
    .catch(onError);
}