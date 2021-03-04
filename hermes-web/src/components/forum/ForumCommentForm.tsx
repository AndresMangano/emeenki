import React, { useReducer } from 'react';
import { Button, Col, Container, Form, FormGroup, Input, Row } from 'reactstrap';

type ForumCommentFormProps = {
    onSubmit: (comment: string, commentIndex: number|null) => void;
    commentIndex: number|null;
    showSenderName?: string;
}

export function ForumCommentForm ({onSubmit, commentIndex, showSenderName}: ForumCommentFormProps) {
    const [{comment}, dispatch] = useReducer(reducer, {
        comment: showSenderName === undefined ? '' : '@' + showSenderName + ' '
    })

function handleSubmit (event: React.ChangeEvent<HTMLFormElement>) {
    event.preventDefault();
    onSubmit(comment, commentIndex);
    dispatch({ _type: 'RESET'});
}

function handleInputChange (event: React.ChangeEvent<HTMLInputElement>) {
    dispatch({ _type: 'CHANGE_COMMENT', comment: event.currentTarget.value});
}

    return (
        <Container fluid>
        <Row>
            <Col md={7}>
                <Form onSubmit={handleSubmit}>
                    <FormGroup>
                        <Input bsSize='lg' type='textarea' name='comment' placeholder='Leave your comment' value={comment} onChange={handleInputChange} />
                    </FormGroup>
                    <Button color='primary'>Reply</Button>
                </Form>
            </Col>
        </Row>
        </Container>
    )
}

type State = {
    comment: string;
}

type Action = 
| { _type: 'CHANGE_COMMENT', comment: string}
| { _type: 'RESET'}

function reducer (state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_COMMENT': return {... state, comment: action.comment}
        case 'RESET': return {... state, comment:''};
    }
}
