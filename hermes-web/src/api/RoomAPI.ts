import axios from 'axios';
import { getHeaders } from './Headers';

export class RoomAPI
{
    static get(roomID:string)
    {
        return axios.get<RoomDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/get/${roomID}`, getHeaders());
    }
    static getUsers(roomID:string)
    {
        return axios.get<RoomUsersQuery[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/get/${roomID}/users`, getHeaders());
    }
    static getPendingUsers(roomID:string)
    {
        return axios.get<RoomUsersQuery[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/get/${roomID}/pendingusers`, getHeaders());
    }
    static query(filter:string, userID:string, language1:string, language2:string)
    {
        return axios.get<RoomDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/query/${filter}/${userID}/${language1}/${language2}`, getHeaders());
    }
    static open(command:RoomOpenCommand)
    {
        return axios.post<RoomOpenResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/open`, command, getHeaders());
    }
    static close(command:RoomCloseCommand)
    {
        return axios.post<RoomCloseCommand>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/close`, command, getHeaders());
    }
    static join(command:RoomJoinCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/join`, command, getHeaders());
    }
    static acceptUser(command:RoomAcceptUserCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/acceptUser`, command, getHeaders());
    }
    static rejectUser(command:RoomRejectUserCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/rejectUser`, command, getHeaders());
    }
    static left(command:RoomLeftCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/left`, command, getHeaders());
    }
    static expelUser(command:RoomExpelUserCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/expelUser`, command, getHeaders());
    }
    static changeUsersLimit(command:RoomChangeUsersLimitCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/changeUsersLimit`, command, getHeaders());
    }
    static restrict(command:RoomRestrictCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/restrict`, command, getHeaders());
    }
    static unrestrict(command:RoomUnrestrictCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/unrestrict`, command, getHeaders());
    }
    static inviteUser(command:RoomInviteUserCommand)
    {
        return axios.post<RoomInviteUserResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/room/inviteUser`, command, getHeaders());
    }
    static joinWithToken(command:RoomJoinWithTokenCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/room/joinWithToken`, command, getHeaders());
    }
}

export type RoomDTO = {
    version: number;
    roomID: string;
    closed: boolean;
    languageID1: string;
    languageID2: string;
    users: RoomUserDTO[];
    usersQueue: string[];
    restricted: boolean;
    usersLimit: number;
}
export type RoomUserDTO = {
    userID: string;
    permission: string;
}

export type RoomUsersQuery = {
    username: string;
    nativeLanguageID: string;
    photoURL: string;
    rights: string;
}

export type RoomOpenCommand = {
    roomID: string;
    languages: string[];
    usersLimit: number;
    restricted: boolean;
    userID: string;
}
export type RoomOpenResult = {
    roomID: string;
}
export type RoomCloseCommand = {
    roomID: string;
    userID: string;
}
export type RoomJoinCommand = {
    roomID: string;
    userID: string;
}
export type RoomAcceptUserCommand = {
    roomID: string;
    roomUserID: string;
    permission: string;
    userID: string;
}
export type RoomRejectUserCommand = {
    roomID: string;
    roomUserID: string;
    userID: string;
}
export type RoomLeftCommand = {
    roomID: string;
    userID: string;
}
export type RoomExpelUserCommand = {
    roomID: string;
    roomUserID: string;
    userID: string;
}
export type RoomChangeUsersLimitCommand = {
    roomID: string;
    newLimit: number;
    userID: string;
}
export type RoomRestrictCommand = {
    roomID: string;
    userID: string;
}
export type RoomUnrestrictCommand = {
    roomID: string;
    userID: string;
}
export type RoomInviteUserCommand = {
    roomID: string;
    userID: string;
}
export type RoomInviteUserResult = {
    token: string;
}
export type RoomJoinWithTokenCommand = {
    roomID: string;
    token: string;
    userID: string;
}