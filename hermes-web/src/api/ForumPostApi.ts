import axios from 'axios';
import { getHeaders } from './Headers';

const DUMMY_FORUM_POST: ForumPostDTO = {
    forumPostID: '1',
    userID: 'LongNameeeeee',
    profilePhoto: 'https://images.livemint.com/img/2020/06/16/600x338/FED1_1592323197070_1592323212036.jpg',
    title: 'VeryLongTitleeeeee eeeeeeeeeeeeeeeeeeeeeeee eeeeeeeeeeeeeeee eeeeeeee eeeeeeeeeeeeeeeee eeeeeeeeee?',
    text: 'I am testing this forum. Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp  Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp  Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp  Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp  Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp  Asadasdas doaspdj apsodj aopdj poasdj oapsdj aospdj aopsdasodp ',
    languageID: 'ENG',
    nativeLanguageID: 'SPA',
    timestamp: new Date,
}

export class ForumPostApi
{
    static async get(forumPostID: string): Promise<ForumPostDTO>
    {
        //return axios.get<ForumPostDTO>(`${process.env.REACT_APP_HERMES_API_URL}api/forum/get/${forumPostID}`, getHeaders());
        return DUMMY_FORUM_POST;
    }
    static async query (languageID: string|null): Promise<ForumPostFeedDTO[]>
    {
        //return axios.get<ForumPostFeedDTO[]>(`${process.env.REACT_APP_HERMES_APIURL}api/forum/getPostFeed`, {
        //    ...getHeaders(),
        //    params: {
        //        languageID
        //    }
        //})
        return [
            {
                forumPostID: '1',
                userID: 'ShortName',
                profilePhoto:'https://images.livemint.com/img/2020/06/16/600x338/FED1_1592323197070_1592323212036.jpg',
                title: 'ShortTitleee eeeee',
                languageID: 'ENG',
                created: new Date,
            },
            {
                forumPostID: '2',
                userID: 'LongNameeeeee',
                profilePhoto:'https://images.livemint.com/img/2020/06/16/600x338/FED1_1592323197070_1592323212036.jpg',
                title: 'LongTitleeeeeeeeeeeeeee eeeeeeeeeeeeee e eeeeeee e e e e e ee ',
                languageID: 'ENG',
                created: new Date,
            },
            {
                forumPostID: '3',
                userID: 'VeryLongNameeeeeeeeeeeeee',
                profilePhoto:'https://images.livemint.com/img/2020/06/16/600x338/FED1_1592323197070_1592323212036.jpg',
                title: 'VeryLongTitleeeeee eeeeeeeeeeeeeeeeeeeeeeee eeeeeeeeeeeeeeee eeeeeeee eeeeeeeeeeeeeeeee eeeeeeeeee?',
                languageID: 'ENG',
                created: new Date,
            }
        ]
    }
    static upload (command: ForumUploadCommand)
    {
        return axios.post<ForumUploadResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/forum/upload`, command, getHeaders());
    }
    static edit (command: ForumEditCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forum/edit`, command, getHeaders());
    }
    static delete (command: ForumPostDeleteCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forum/delete`, command, getHeaders());
    }
    static addForumComment (command: ForumCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}'api/forum/comment`, command, getHeaders());
    }
    static async getUserComments (forumPostID:string): Promise<CommentDTO[]>
    {
        //return axios.get<CommentDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}'api/forum/${forumPostID}/comments)` , getHeaders());
        return [
            {
                commentIndex: 1,
                userID: 'Comentador1',
                profilePhoto: 'https://i.imgur.com/ipAslnw.png',
                comment: 'Probando el comentario',
                nativeLanguageID: 'SPA',
                timestamp: new Date,
            },
            {
                commentIndex: 2,
                userID: 'Jwerp',
                profilePhoto: 'https://i.imgur.com/ipAslnw.png',
                comment: 'Probando un comentario mas largo para ver como queda el comentario largo en los comentarios',
                nativeLanguageID: 'SPA',
                timestamp: new Date,
            },
            {
                
                commentIndex: 3,
                userID: '3rdUser',
                profilePhoto: 'https://i.imgur.com/ipAslnw.png',
                comment: 'asd aad221 12 as dasd asd asd asda s asd aad221 12 as dasd asd asd asda s asd aad221 12 as dasd asd asd asda s asd aad221 12 as dasd asd asd asda s asd aad221 12 as dasd asd asd asda s',
                nativeLanguageID: 'ENG',
                timestamp: new Date,
            },
        ]
    }
    static deleteForumComment(command: DeleteForumCommentCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/forum/deleteComment`, command, getHeaders());
    }
}

export type ForumPostDTO = {
    forumPostID: string;
    userID: string;
    profilePhoto: string;
    title: string;
    text: string;
    languageID: string;
    nativeLanguageID: string;
    timestamp: Date;
}

export type ForumPostFeedDTO = {
    forumPostID: string;
    profilePhoto: string;
    userID: string;
    title: string;
    languageID: string;
    created: Date;
}

export type CommentDTO = {
    commentIndex: number;
    userID: string;
    profilePhoto: string;
    comment: string;
    nativeLanguageID: string;
    timestamp: Date;
}

export type ForumUploadCommand = {
    title: string;
    text: string;
    languageID: string;
}

export type ForumEditCommand = {
    forumPostID: string;
    text: string;
}

export type ForumUploadResult = {
    forumPostID: string;
}

export type ForumPostDeleteCommand = {
    forumPostID: string;
}

export type ForumCommentCommand = {
    commentIndex: number|null;
    comment: string;
}

export type DeleteForumCommentCommand = {
    commentIndex: number;
} 