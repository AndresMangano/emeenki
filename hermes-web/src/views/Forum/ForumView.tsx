import React, { useEffect, useMemo, useReducer } from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { Col, Container, Input, Label, Row } from 'reactstrap';
import { ForumPostCard } from '../../components/forum/ForumPostCard';
import { ForumPostsFeed } from '../../components/forum/ForumPostsFeed';
import { PageHeader } from '../../components/PageHeader';
import { PaginationButtons } from '../../controls/PaginationButtons';
import { useForumFeedQuery, useLanguagesQuery } from '../../services/queries-service';

type ForumViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function ForumView ({onError, match, history}: ForumViewProps) {

    const itemsPerPage = 7;

    const [{ languageID, currentPage }, dispatch] = useReducer(reducer, {
        languageID: '',
        currentPage: 0,
    });

    const { data: languagesData } = useLanguagesQuery();
    const { data: forumFeedData } = useForumFeedQuery(languageID);

    const currentItems = useMemo(() => { 
        return forumFeedData !== undefined
            ?   forumFeedData.slice(currentPage * itemsPerPage, currentPage * itemsPerPage + itemsPerPage)
            : 0
    },
    [forumFeedData, currentPage] );

    const totalPages = useMemo(() => { 
        return forumFeedData !== undefined
            ?   Math.ceil(forumFeedData.length / itemsPerPage)
            :   0
    },
    [forumFeedData] );
    

    
    
    //const currentItems = (forumFeedData !== undefined) && forumFeedData.slice(indexOfFirstItem, indexOfLastItem);
    //const isLastPage = (currentItems !== false ) && currentItems.length !== itemsPerPage; // || endIndex === items.length 
    
    // const totalPages = 
    
   
   
   
    // const itemsArray = items.length 


   // const endIndex = indexOfLastItem + itemsPerPage;







    /* const pages = [];

    for(let i=1; i<=Math.ceil(data.length/itemsPerPage); i++ ) {
        pages.push(i);
    }

    const pageNumbers = pages.map((number => {

        if(number < maxPageNumberLimit+1 && number>minPageNumberLimit) {
        return (
            <ul className='page-numbers'>
                <li key={number} 
                    id='pageID' 
                    value={number} 
                    onClick={() => handleTurnPage}
                    className={currentPage == number ? "active" : 'null' }>
                    {number}
                </li>
            </ul>
        );
        }else{
            return null;
        }
    })); 
    
    function handleTurnPage (event: React.ChangeEvent<HTMLLIElement>) {
        switch (event.currentTarget.id) {
            case 'pageID': dispatch( {_type: 'CHANGE_CURRENT_PAGE', currentPage: event.currentTarget.value})
        }
    }

    */


    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'CHANGE_LANGUAGE_ID', languageID: event.currentTarget.value });    
    }

    function handleFirstPageButton () {
        dispatch({ _type: 'CHANGE_CURRENT_PAGE', currentPage: 0})
    }
    function handleNextButton () {
        dispatch({ _type: 'CHANGE_CURRENT_PAGE', currentPage: totalPages -1 === currentPage ? currentPage : currentPage +1 })  // NEEDS FIXING      
    }

    function handlePreviousButton () {
        dispatch({ _type: 'CHANGE_CURRENT_PAGE', currentPage: currentPage == 0 ? currentPage : currentPage - 1 })
    }

    function handleLastPageButton () {
        dispatch({ _type: 'CHANGE_CURRENT_PAGE', currentPage: totalPages -1 })
    }


    return (
        <>
            <Container>
                <PageHeader>FORUM</PageHeader>
                <Row className='justify-content-end align-items-end text-center mb-4' style={{width: '95%'}}>
                    <Col md={{ size: 2 }}>
                        <Label>Language</Label>
                        <Input type='select' name='languageID' id='languageID' value={languageID} onChange={handleInputChange}>
                            <option value=''>All</option>
                            { (languagesData) &&
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
                <Row>
                    <Col>
                        <ForumPostsFeed>
                            {
                                currentItems &&
                                currentItems.map((forum) =>
                                        <ForumPostCard onClick={() => history.push(`/forum/${forum.id}`)} key={forum.id}
                                            userID={forum.userID}
                                            profilePhoto={forum.profilePhotoURL}
                                            title={forum.title}
                                            languageID={forum.languageID}
                                            timestamp={forum.timestamp}
                                            latestCommentUserID={forum.latestCommentUserID}
                                            latestCommentTimestamp={forum.latestCommentTimestamp}
                                        />
                            )}
                        </ForumPostsFeed>
                    </Col>
                </Row>
                <Row>
                    <Col className='d-flex justify-content-center mt-5'>
                        <PaginationButtons firstPage={handleFirstPageButton} previousPage={handlePreviousButton} nextPage={handleNextButton} lastPage={handleLastPageButton} />
                    </Col>
                </Row>
            </Container>
            <hr />
        </>
    )
}

type State = {
    languageID: string;
    currentPage: number;  
}

type Action = 
| { _type: 'CHANGE_LANGUAGE_ID', languageID: string}
| { _type: 'CHANGE_CURRENT_PAGE', currentPage: number}

function reducer(state: State, action: Action ) : State {
    switch (action._type) {
        case 'CHANGE_LANGUAGE_ID': return {...state, languageID: action.languageID}
        case 'CHANGE_CURRENT_PAGE': return { ...state, currentPage: action.currentPage}
    }
}
