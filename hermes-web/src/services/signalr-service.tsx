import React, { ReactNode, useContext, useEffect, useState } from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { queryCache } from "react-query";

export interface ISignalRService
{
    connect(): Promise<void>;
    joinGroup(group: string): Promise<void>;
    leaveGroup(group: string): Promise<void>;
    listen(method: string, action: (...args: any[]) => void): void;
    removeListeners(method: string, action: (...args: any[]) => void): void;
}
export class SignalRService implements ISignalRService {
    private connection: HubConnection;
    private groups: {
        [key: string]: number;
    }
    constructor(url: string) {
        this.connection = new HubConnectionBuilder()
            .withUrl(url)
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();
        this.groups = {};
    }
    async connect() {
        try {
            await this.connection.start();
            console.log("SignalR connected");
        } catch (err) {
            console.log(err);
        }
    }
    async joinGroup(group: string) {
        try {
            if (this.groups[group] === undefined) {
                await this.connection.invoke('joinGroup', group);
                console.log(`SignalR => Joining group ${group}`);
            }
            this.groups[group]++;
        } catch (err) {
            console.log(err);
        }
    }
    async leaveGroup(group: string) {
        try {
            this.groups[group]--;
            if (this.groups[group] <= 0) {
                await this.connection.invoke('leaveGroup', group);
                console.log(`SignalR => Leaving group ${group}`);
                delete this.groups[group];
            }
        } catch (err) {
            console.log(err);
        }
    }
    listen(method: string, action: (...args: any[]) => void) {
        try {
            console.log(`SignalR => Listening ${method} signals`);
            this.connection.on(method, action);
        } catch (err) {
            console.log(err);
        }
    }
    removeListeners(method: string) {
        try {
            this.connection.off(method);
            console.log(`SignalR => Stopped listening ${method} signals`);
        } catch (err) {
            console.log(err);
        }
    }
}

const SignalRContext = React.createContext<ISignalRService|undefined>(undefined);

export function SignalRProvider(props: { children?: ReactNode }) {
    const signalRUrl = process.env.REACT_APP_HERMES_API_URL + '/api/hubs/queries';
    const [signalRService, setSignalRService] = useState<ISignalRService>();
    
    useEffect(() => {
        const service = new SignalRService(signalRUrl);
        service.connect()
        .then(() => {
            service.listen('article-updated', (articleID) => {
                console.log("SignalR => article-updated signal received");
                queryCache.invalidateQueries('ARTICLES');
                queryCache.invalidateQueries(['ARTICLE', articleID]);
                queryCache.invalidateQueries('ACTIVITY_FEED');
            });
            service.listen('article-template-updated', (articleTemplateID) => {
                console.log("SignalR => article-template-updated signal received");
                queryCache.invalidateQueries('ARTICLE_TEMPLATES');
                queryCache.invalidateQueries(['ARTICLE_TEMPLATE', articleTemplateID]);
            });
            service.listen('room-updated', (roomID) => {
                console.log("SignalR => room-updated signal received");
                queryCache.invalidateQueries('ROOMS');
                queryCache.invalidateQueries(['ROOM', roomID]);
                queryCache.invalidateQueries(['ROOM_USERS', roomID]);
                queryCache.invalidateQueries(['ROOM_PENDING_USERS', roomID]);
            });
            service.listen('user-updated', (userID) => {
                console.log("SignalR => user-updated signal received");
                queryCache.invalidateQueries('USERS');
                queryCache.invalidateQueries('RANKING');
                queryCache.invalidateQueries(['USER', userID]);
                queryCache.invalidateQueries(['USER_DETAILS', userID]);
                queryCache.invalidateQueries(['USER_POSTS', userID]);
                queryCache.invalidateQueries(['ACTIVITY_FEED', userID]);
            });
            service.listen('forum-post-updated', (languageID) => {
                console.log("SignalR => forum-post-updated signal received");
                queryCache.invalidateQueries(['FORUM_FEED', languageID]);
            });
            service.listen('forum-post-updated', (forumPostID) => {
                console.log("SignalR => forum-post-updated signal received");
                queryCache.invalidateQueries(['FORUM_COMMENTS', forumPostID])
            });
            setSignalRService(service);
            
        });

        return function cleanup() {
            service.removeListeners('article-updated');
            service.removeListeners('article-template-updated');
            service.removeListeners('room-updated');
            service.removeListeners('user-updated');
            service.removeListeners('forum-post-updated');
        }
    }, [])

    return (
        <SignalRContext.Provider value={signalRService}>
            { props.children }
        </SignalRContext.Provider>
    );
}

export function useSignalR(...groups: string[]) {
    var [groupsJoined, setGroupsJoined] = useState<boolean>(false);
    var service = useContext(SignalRContext);

    useEffect(() => {
        if (service !== undefined) {
            Promise.all(groups.map(group => service ? service.joinGroup(group) : Promise.resolve()))
            .then(() => setGroupsJoined(true));
        }
        return function cleanup() {
            Promise.all(groups.map(group => service ? service.leaveGroup(group) : Promise.resolve()))
            .then(() => setGroupsJoined(false));
        }
    }, [service])

    return groupsJoined;
}

/* señal: 

forum-post-updated

grupos:
forumPosts
forumPost:${forumPostID} */