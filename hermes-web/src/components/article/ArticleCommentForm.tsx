import React, { useReducer } from 'react';
import { Form, FormGroup, Button, Input } from 'reactstrap';

type ArticleCommentFormProps = {
    onSubmit: (comment: string, parentCommentPos:number|null) => void;
    parentCommentPos: number|null;
    showSenderName?: string;
}
export function ArticleCommentForm({ onSubmit, parentCommentPos, showSenderName  }: ArticleCommentFormProps) {
    const [{ comment }, dispatch] = useReducer(reducer, {
        comment: showSenderName === undefined ? '' : '@' + showSenderName + ' ',
    });

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmit(comment, parentCommentPos);
        dispatch({ _type: 'RESET' } );
    }
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'CHANGE_COMMENT', comment: event.currentTarget.value });
    }
    

    return (
        <Form onSubmit={handleSubmit}>
            <FormGroup>
                <Input type='textarea' name='comment' placeholder='Comment' value={comment} onChange={handleInputChange} />
            </FormGroup>
            <Button color='primary'>Post</Button>
        </Form>
    );
}

type State = {
    comment: string;
}
type Action =
| { _type: 'CHANGE_COMMENT', comment: string }
| { _type: 'RESET' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_COMMENT': return { ...state, comment: action.comment };
        case 'RESET': return { ...state, comment: '' };
    }
}