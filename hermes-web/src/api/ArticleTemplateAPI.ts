import axios from 'axios';
import { getHeaders } from './Headers';

export class ArticleTemplateAPI
{
    static get(articleTemplateID:string)
    {
        return axios.get<ArticleTemplateDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/articleTemplate/get/${articleTemplateID}`, getHeaders());
    }
    static query(archived: boolean, languageID:string|null, topicID: string|null)
    {
        return axios.get<ArticleTemplateListDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/articleTemplate/query/${archived}`, { 
            ...getHeaders(),
            params: {
                languageID,
                topicID
            }            
        });
    }
    static upload(command:ArticleTemplateUploadCommand)
    {
        return axios.post<ArticleTemplateUploadResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/articleTemplate/upload`, command, getHeaders());
    }
    static delete(command:ArticleTemplateDeleteCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/articleTemplate/delete`, command, getHeaders());
    }
}

export type ArticleTemplateDTO = {
    version: number;
    articleTemplateID: string;
    deleted: boolean;
    languageID: string;
    title: string[];
    text: string[];
    source: string;
    photoURL: string;
    timestamp: Date;
}
export type ArticleTemplateListDTO = {
    articleTemplateID: string;
    title: string;
    created: Date;
    languageID: string;
    photoURL: string;
}

export type ArticleTemplateUploadCommand = {
    languageID: string;
    topicID: string;
    title: string;
    text: string;
    source: string;
    photoURL: string;
}
export type ArticleTemplateUploadResult = {
    articleTemplateID: string;
}
export type ArticleTemplateDeleteCommand = {
    articleTemplateID: string;
    userID: string;
}