import React, { useReducer } from 'react';
import { Form, FormGroup, Button, Input } from 'reactstrap';

export type ProfilePostFormProps = {
    onSubmit: (post: string, parentUserPostID:string|null) => void;
    parentUserPostID: string|null;
    showSenderName?: string;
}
export function ProfilePostForm({ onSubmit, parentUserPostID, showSenderName }: ProfilePostFormProps) {
    const [{ post }, dispatch] = useReducer(reducer, {
        post: showSenderName === undefined ? '' : '@' + showSenderName + ' ',
    });
    
    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmit(post, parentUserPostID);
        dispatch({ _type: 'RESET' });
    };

    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'CHANGE_POST', post: event.currentTarget.value });
    }

    return (
        <Form onSubmit={handleSubmit}>
            <FormGroup>
                <Input type='textarea' name='post' placeholder='Leave your comment' value={post} onChange={handleInputChange} />
            </FormGroup>
            <Button color='primary'>Post</Button>
        </Form>
    );
}

type State = {
    post: string;
}
type Action =
| { _type: 'CHANGE_POST', post: string }
| { _type: 'RESET' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_POST': return { ...state, post: action.post };
        case 'RESET': return { ...state, post: '' };
    }
}

/*

<>
            { !showSenderName &&
            <Form onSubmit={handleSubmit}>
                <FormGroup>
                    <Input type='textarea' name='post' placeholder='Leave your comment' value={post} onChange={handleInputChange} />
                </FormGroup>
                <Button color='primary'>Post</Button>
            </Form>
            }
            { showSenderName &&
            <Form onSubmit={handleSubmitReply}>
            <FormGroup>
                <Input type='textarea' name='replyPost' placeholder='Leave your comment' value={replyPostConstruction} onChange={handleInputChange} />
            </FormGroup>
            <Button color='primary'>Post</Button>
            </Form>
            }
        </>

        -----

    type State = {
    post: string;
    replyPost: string;
}
type Action =
| { _type: 'CHANGE_POST', post: string }
| { _type: 'CHANGE_REPLY', replyPost: string}
| { _type: 'SUBMIT' }
| { _type: 'SUBMIT_REPLY'}
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_POST': return { ...state, post: action.post };
        case 'CHANGE_REPLY': return { ...state, replyPost: action.replyPost}
        case 'SUBMIT': return { ...state, post: '' };
        case 'SUBMIT_REPLY': return { ...state, replyPost: ''}
    }
}

----

function handleSubmitReply(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmit(post, parentUserPostID) 
        dispatch({ _type: 'SUBMIT_REPLY' });
    };

*/