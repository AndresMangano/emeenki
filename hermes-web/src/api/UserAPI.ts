import axios from 'axios';
import { getHeaders } from './Headers';

export class UserAPI
{
    static get(userID: string)
    {
        return axios.get<UserDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/user/${userID}`, getHeaders());
    }
    static getUserDetails(userID: string)
    {
        return axios.get<UserDetailsDTO>(`${process.env.REACT_APP_HERMES_API_URL}/api/user/${userID}/details`, getHeaders());
    }
    static query()
    {
        return axios.get<UserDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/user/query`, getHeaders());
    }
    static registerWithPassword(command:UserRegisterWithPasswordCommand)
    {
        return axios.post<UserRegisterResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/tokens/registerWithPassword`, command);
    }
    static registerWithGoogle(command:UserRegisterWithGoogleCommand)
    {
        return axios.post<UserRegisterResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/tokens/registerWithGoogle`, command, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('hermes.googleToken' || '')}`
            }
        }); 
    }
    static async logInWithPassword(command:UserLoginWithPasswordCommand)
    {
        return axios.post<UserLogInResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/tokens/loginWithPassword`, command)
            .then(result => {
                localStorage.setItem('hermes.userID', result.data.userID);
                localStorage.setItem('hermes.profilePhotoURL', result.data.profilePhotoURL);
                localStorage.setItem('hermes.rights', result.data.rights);
                localStorage.setItem('hermes.token', result.data.token);
                if(result.data.userID === 'root'){
                    window.location.href = '/root';
                } else if(result.data.rights == 'admin'){
                    window.location.href ='/rooms';
                } else {
                    window.location.href = '/rooms';
                }
            });
    }
    static async logInWithGoogle()
    {
        return axios.post<UserLogInResult>(`${process.env.REACT_APP_HERMES_API_URL}/api/tokens/loginWithGoogle`, undefined, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('hermes.googleToken' || '')}`
            }
        })
            .then(result => {
                localStorage.setItem('hermes.userID', result.data.userID);
                localStorage.setItem('hermes.profilePhotoURL', result.data.profilePhotoURL);
                localStorage.setItem('hermes.rights', result.data.rights);
                localStorage.setItem('hermes.token', result.data.token);
                if(result.data.userID === 'root'){
                    window.location.href = '/root';
                } else if(result.data.rights == 'admin'){
                    window.location.href ='/rooms';
                } else {
                    window.location.href = '/rooms';
                }
                return result.data;
            });
    }
    static logOut()
    {
        localStorage.removeItem('hermes.userID');
        localStorage.removeItem('hermes.profilePhotoURL');
        localStorage.removeItem('hermes.rights');
        localStorage.removeItem('hermes.token');
        localStorage.removeItem('hermes.googleToken');
        window.location.href = '/';
    }
    static setRights(command:UserSetRightsCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/setRights`, command, getHeaders());
    }
    static ban(command:UserBanCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/ban`, command, getHeaders());
    }
    static unban(command:UserUnbanCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/unban`, command, getHeaders());
    }
    static delete(command:UserDeleteCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/delete`, command, getHeaders());
    }
    static async changeProfilePhoto(command: UserChangeProfilePhotoCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/changeProfilePhoto`, command, getHeaders()).then(() => {
            localStorage.setItem('hermes.profilePhotoURL', command.profilePhotoURL);
        });
    }
    static changeLanguage(command: UserChangeLanguageCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/changeLanguage`, command, getHeaders());
    }
    static updateDescription(command: UserUpdateDescriptionCommand)
    {
       return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/updateDescription`, command, getHeaders()); 
    }
    static updateCountry (command: UserUpdateCountryCommand)
    {
       return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/updateCountry`, command, getHeaders()); 
    }
    static changePassword (command: ChangePasswordCommand)
    {
       return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/changePassword`, command, getHeaders()); 
    }
    static getRanking()
    {
        return axios.get<UserRankingDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/user/ranking`, getHeaders());
    }
    static addPost(command: UserAddPostCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/addPost`, command, getHeaders());
    }
    static getUserPosts(userID: string)
    {
        return axios.get<UserPostDTO[]>(`${process.env.REACT_APP_HERMES_API_URL}/api/user/${userID}/posts`, getHeaders());
    }
    static deleteUserPost(command: DeleteUserPostCommand)
    {
        return axios.post(`${process.env.REACT_APP_HERMES_API_URL}/api/user/deletePost`, command, getHeaders());
    }
    
}

export type UserDTO = {
    userID: string;
    rights: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
}
export type UserDetailsDTO = {
    userID: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
    description?: string|null;
    country: string;
    points: number;
    signInType: string;
}
export type UserRankingDTO = {
    userID: string;
    profilePhotoURL: string;
    nativeLanguageID: string;
    points: number;
}
export type UserPostDTO = {
    userPostID: string;
    childUserPostID: string|null;
    text: string;
    senderUserID: string;
    timestamp: Date;
    profilePhotoURL: string;
}

// Commands
export type UserRegisterWithPasswordCommand = {
    userID: string;
    password: string;
    profilePhotoURL: string;
    languageID: string;
    country: string;
}
export type UserRegisterWithGoogleCommand = {
    userID: string;
    googleEmail: string;
    profilePhotoURL: string;
    languageID: string;
    country: string;
}
export type UserRegisterResult = {
    userID: string;
}
export type UserLoginWithPasswordCommand = {
    userID: string;
    password: string;
} 
export type UserLogInResult = {
    userID: string;
    profilePhotoURL: string;
    rights: string;
    token: string;
}
export type UserSetRightsCommand = {
    userID: string;
    newRights: string;
}
export type UserDeleteCommand = {
    userID: string;
}
export type UserBanCommand = {
    userID: string;
    reason: string;
    adminUserID: string;
}
export type UserUnbanCommand = {
    userID: string;
    adminUserID: string;
}
export type UserUpdateProfileCommand = {
    password: string;
    languageID: string;
    profilePhotoID: string;
}
export type UserChangeProfilePhotoCommand = {
    userID: string;
    profilePhotoURL: string;
}
export type UserChangeLanguageCommand = {
    userID: string;
    nativeLanguageID: string;
}
export type UserUpdateDescriptionCommand = {
    userID: string;
    description: string|null;
}
export type UserUpdateCountryCommand = {
    userID: string;
    country: string;
}
export type UserAddPostCommand = {
    userID: string;
    text: string;
    parentUserPostID: string|null;
}
export type DeleteUserPostCommand = {
    userID: string;
    userPostID: string;
    childUserPostID: string|null;
}

export type ChangePasswordCommand = {
    actualPassword: string;
    newPassword: string;
}