import axios from 'axios';

export class StaticAPI
{
    static getLanguages()
    {
        return axios.get<LanguageDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/static/getLanguages`);
    }
    static getTopics()
    {
        return axios.get <TopicsDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/static/topics`)
    }
}

export type LanguageDTO = {
    languageID: string;
    description: string;
}

export type TopicsDTO = {
    topicID: string;
    name: string;
}