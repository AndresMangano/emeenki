import React, { useReducer } from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { Col, Container, Input, Label, Row } from 'reactstrap';
import { ForumPostCard } from '../../components/forum/ForumPostCard';
import { ForumPostsFeed } from '../../components/forum/ForumPostsFeed';
import { PageHeader } from '../../components/PageHeader';
import { useForumFeedQuery, useLanguagesQuery } from '../../services/queries-service';

type ForumViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function ForumView ({onError, match, history}: ForumViewProps) {

    const [{ languageID }, dispatch] = useReducer(reducer, {
        languageID: '',
    });

    const { data: languagesData } = useLanguagesQuery();
    const { data: forumFeedData } = useForumFeedQuery(languageID);

    function handleInputChange (event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID': dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value})
        }
    }

    return (
        <>
            <Container>
                <PageHeader>FORUM</PageHeader>
                <Row className='justify-content-end align-items-end text-center mb-4' style={{width: '95%'}}>
                    <Col md={{ size: 2 }}>
                            <Label for='languageID'>Language</Label>
                            <Input type='select' name='languageID' id='languageID' value={languageID} onChange={handleInputChange}>
                                 <option value=''>All</option>
                                 {
                                    languagesData !== undefined &&
                                     languagesData.map((languages, index) =>
                                        <option key={index} value={languages.languageID}>{languages.languageID}</option>

                                )}   
                
                        </Input>
                    </Col>
                    <Col md={{ size: 2}}>
                            <Link className='btn btn-success' to='/uploadForumPost'>Create Post</Link>
                    </Col>
                </Row>
            </Container>
            <Container>
                <ForumPostsFeed>
                    {
                        forumFeedData !== undefined &&
                            forumFeedData.map((forum) =>
                                <ForumPostCard onClick={() => history.push(`/forum/${forum.forumPostID}`)} key={forum.forumPostID}
                                    userID={forum.userID}
                                    profilePhoto={forum.profilePhoto}
                                    title={forum.title}
                                    languageID={forum.languageID}
                                    timestamp={forum.created}
                                />
                    )}
                </ForumPostsFeed>         
            </Container>
            <hr />
        </>
    )
}

type State = {
    languageID: string
}

type Action = 
| { _type: 'CHANGE_LANGUAGE_ID', languageID: string}

function reducer(state: State, action: Action ) : State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': return {...state, languageID: action.languageID}
    }
}

