import axios from 'axios';

export class StaticAPI
{
    static getLanguages()
    {
        return axios.get<LanguageDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/static/getLanguages`);
    }
}

export type LanguageDTO = {
    languageID: string;
    description: string;
}