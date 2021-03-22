import axios from 'axios';
import { getHeaders } from './Headers';

export class ForumPostApi
{
    static  getForumPost(forumPostID: string)
    {
        return axios.get<ForumPostDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/${forumPostID}`, getHeaders());
    }
    static query (languageID: string|null)
    {
        return axios.get<ForumPostDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost`, {
            ...getHeaders(),
            params: {
                languageID
            }
        })
    }
    static upload (command: ForumUploadCommand)
    {
        return axios.post<ForumUploadResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/create`, command, getHeaders());
    }
    static edit (command: ForumEditCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/edit`, command, getHeaders());
    }
    static delete (command: ForumPostDeleteCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/delete`, command, getHeaders());
    }
    static addForumComment (command: ForumCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/comment`, command, getHeaders());
    }
    static getPostComments (forumPostID:string)
    {
        return axios.get<CommentDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/${forumPostID}/comments` , getHeaders());
    }
    static deleteForumComment(command: DeleteForumCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forumPost/deleteComment`, command, getHeaders());
    }
}

export type ForumPostDTO = {
    id: string;
    title: string;
    text: string;
    languageID: string;
    userID: string;
    profilePhotoURL: string;
    timestamp: Date;
    modifiedOn: Date;
    latestCommentUserID: string;
    latestCommentTimestamp: Date;
}

export type CommentDTO = {
    id: string;
    forumPostID: string;
    text: string;
    userID: string;
    profilePhotoURL: string;
    timestamp: Date
}

export type ForumUploadCommand = {
    title: string;
    text: string;
    languageID: string;
}

export type ForumEditCommand = {
    forumPostID: string;
    title: string;
    text: string;
    languageID: string;
}

export type ForumUploadResult = {
    forumPostID: string;
}

export type ForumPostDeleteCommand = {
    forumPostID: string;
}

export type ForumCommentCommand = {
    forumPostID: string;
    text: string;
}

export type DeleteForumCommentCommand = {
    forumPostID: string;
    forumPostCommentID: string;
} 