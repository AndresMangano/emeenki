import React from 'react';
import { RouteComponentProps, Switch, Route } from 'react-router-dom';
import { RoomsView } from './RoomsView';
import { CreateRoomView } from './CreateRoomView';
import { RoomArticlesView } from './RoomArticlesView';
import { RoomUsersView } from './RoomUsersView';
import { RoomUserInvitationView } from './RoomUserInvitationView';

type RoomsIndexProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function RoomsIndex({ onError }: RoomsIndexProps) {
    return(
        <Switch>
            <Route exact path='/rooms' render={(props) => <RoomsView {...props} onError={onError} />} />
            <Route exact path='/rooms/create' render={(props) => <CreateRoomView {...props} onError={onError} />} />
            <Route exact path='/rooms/:roomID/articles/:status' render={(props) => <RoomArticlesView {...props} onError={onError} />} />
            <Route exact path='/rooms/:roomID/users/:status' render={(props) => <RoomUsersView {...props} onError={onError} />} />
            <Route exact path='/rooms/:roomID/token/:token/:username' component={RoomUserInvitationView}/>
        </Switch>
    );
}