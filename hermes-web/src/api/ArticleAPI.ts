import axios from 'axios';
import { getHeaders } from './Headers';

export class ArticleAPI
{
    static get(articleID:string)
    {
        return axios.get<ArticleDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/article/get/${articleID}`, getHeaders());
    }
    static query(roomID:string, archived:boolean)
    {
        return axios.get<ArticleListDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/article/query/${roomID}/${archived}`, getHeaders());
    }
    static takeTemplate(command:ArticleTakeTemplateCommand)
    {
        return axios.post<ArticleTakeTemplateResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/article/takeTemplate`, command, getHeaders());
    }
    static translate(command:ArticleTranslateCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/translate`, command, getHeaders());
    }
    static vote(command:ArticleVoteCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/vote`, command, getHeaders());
    }
    static comment(command:ArticleCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/comment`, command, getHeaders());
    }
    static commentMain(command:ArticleCommentMainCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/commentMain`, command, getHeaders());
    }
    static archive(command:ArticleArchiveCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/archive`, command, getHeaders());
    }
    static getActivityFeed(userID?: string)
    {
        return axios.get<ActivityDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/article/activity`, {
            ...getHeaders(),
            params: {
                userID
            }
        });
    }
    static deleteMainComment(command: ArticleDeleteMainCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/article/deleteMainComment`, command, getHeaders());
    }
}

export type ArticleDTO = {
    version: number;
    articleID: string;
    archived: boolean;
    articleTemplateID: string;
    roomID: string;
    originalLanguageID: string;
    translationLanguageID: string;
    title: SentenceDTO[];
    text: SentenceDTO[];
    source: string;
    photoURL: string;
    comments: CommentDTO[];
    timestamp: Date;
}
export type SentenceDTO = {
    sentenceIndex: number;
    originalText: string;
    translationHistory: TranslationDTO[];
}
export type TranslationDTO = {
    translationIndex: number;
    translation: string;
    userID: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
    upvotes: string[];
    downvotes: string[];
    comments: TranslationCommentDTO[];
    timestamp: Date;
}
export type TranslationCommentDTO = {
    commentIndex: number;
    comment: string;
    userID: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
    timestamp: Date;
}
export type CommentDTO = {
    commentIndex: number;
    childCommentIndex: number|null;
    comment: string;
    userID: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
    timestamp: Date;
}

export type ArticleListDTO = {
    articleID: string;
    title: string;
    created: Date;
    originalLanguageID: string;
    translationLanguageID: string;
    photoURL: string;
    archived: boolean;
}
export type ActivityDTO = {
    profilePhotoURL: string;
    userID: string;
    event: string;
    title: string;
    points: number;
    timestamp: Date;
}

export type ArticleTakeTemplateCommand = {
    articleTemplateID: string;
    roomID: string;
    userID: string;
}
export type ArticleTakeTemplateResult = {
    articleID: string;
}
export type ArticleTranslateCommand = {
    articleID: string;
    inText: boolean;
    sentencePos: number;
    translation: string;
    userID: string;
}
export type ArticleVoteCommand = {
    articleID: string;
    inText: boolean;
    sentencePos: number;
    translationPos: number;
    positive: boolean;
    userID: string;
}
export type ArticleCommentCommand = {
    articleID: string;
    inText: boolean;
    sentencePos: number;
    translationPos: number;
    comment: string;
    userID: string;
}
export type ArticleArchiveCommand = {
    articleID: string;
    userID: string;
}
export type ArticleCommentMainCommand = {
    articleID: string;
    comment: string;
    parentCommentPos: number|null;
}
export type ArticleDeleteMainCommentCommand = {
    articleID: string;
    commentPos: number;
    childCommentPos: number|null;
}