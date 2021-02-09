import React, { useEffect, useReducer } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { PageHeader } from '../../components/PageHeader';
import { RoomAPI } from '../../api/RoomAPI';
import { Container } from 'reactstrap';

type RoomUserInvitationViewProps = RouteComponentProps<{roomID: string; token: string; username: string; }>;
export function RoomUserInvitationView({ history, match }: RoomUserInvitationViewProps) {
    const { roomID, token, username } = match.params;
    const [{ validToken }, dispatch] = useReducer(reducer, {
        validToken: undefined
    });

    useEffect(() => {
        RoomAPI.joinWithToken({
            roomID,
            token,
            userID: username
        })
        .then(() => dispatch({ _type: 'VALIDATE_TOKEN' }))
        .catch(() => dispatch({ _type: 'INVALIDATE_TOKEN' }));
    }, [roomID, token, username]);

    return (
        <Container>
            <PageHeader
                color={validToken === true ? 'success' : validToken === false ? 'danger' : undefined}>
                { (validToken === undefined) && <>Validating Token</> }
                { (validToken === false) && <>Invalid Token</> }
                { (validToken) && <>Welcome to {roomID}</> }
            </PageHeader>
        </Container>
    );
}

type State = {
    validToken: boolean|undefined;
}
type Action =
| { _type: 'VALIDATE_TOKEN' }
| { _type: 'INVALIDATE_TOKEN' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'VALIDATE_TOKEN': return { ...state, validToken: true };
        case 'INVALIDATE_TOKEN': return { ...state, validToken: false };
    }
}