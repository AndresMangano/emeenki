import React, { useReducer } from 'react';
import { Modal, ModalHeader, ModalBody, Form, Input, FormGroup, Button, Row, Container, Col } from 'reactstrap';
import { Link } from 'react-router-dom';
import { CommentsTable } from './CommentsTable';

type CommentFormProps = {
    onSubmit: (comment: string) => void;
    closeUrl: string;
    comments: {
        profilePhotoUrl: string;
        userID: string;
        timestamp: Date;
        comment: string;
    }[]
}
export function CommentForm({ onSubmit, closeUrl, comments }: CommentFormProps) {
    const [state, dispatch] = useReducer(reducer, {
        comment: ''
    });

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        onSubmit(state.comment);
        dispatch({ _type: 'SUBMIT' });
    }
    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'comment': dispatch({ _type: 'CHANGE_COMMENT', comment: event.currentTarget.value }); break;
        }
    }

    return(
        <Modal isOpen={true}>
            <ModalHeader>Comments</ModalHeader>
            <ModalBody>
                <Container fluid>
                    <Row className='articleComments-history mb-2'>
                        <Col md={12}>
                            <Form onSubmit={handleSubmit}>
                                <FormGroup>
                                    <Input type='textarea' name='comment' placeholder='Comment' value={state.comment} onChange={handleInputChange}/>
                                </FormGroup>
                                <Button color='primary'>Submit</Button>
                                <Button tag={Link} color='dark' to={closeUrl}>Close</Button>
                            </Form>
                        </Col>
                    </Row>
                    <Row className='d-flex flex-column articleComments-table'>
                        <CommentsTable comments={comments}/>
                    </Row>
                </Container>
            </ModalBody>
        </Modal>
    );
}

type State = {
    comment: string;
}
type Action =
| { _type: 'CHANGE_COMMENT', comment: string }
| { _type: 'SUBMIT' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'CHANGE_COMMENT': return { ...state, comment: action.comment };
        case 'SUBMIT': return { ...state, comment: '' };
    }
}