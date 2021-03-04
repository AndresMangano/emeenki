import React, { useReducer } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Col, Container, Row } from 'reactstrap';
import { ForumPostApi } from '../../api/ForumPostApi';
import { CommentForm } from '../../components/CommentForm';
import { ForumComment } from '../../components/forum/ForumComment';
import { ForumCommentForm } from '../../components/forum/ForumCommentForm';
import { ForumCommentsPanel } from '../../components/forum/ForumCommentsPanel';
import { ForumThread } from '../../components/forum/ForumThread';
import { useForumQuery, useUsersComments } from '../../services/queries-service';

type ThreadPageViewProps = RouteComponentProps<{forumPostID:string}> & {
    onError: (error: any) => void;
}

export function ThreadPageView({ onError, match, history }: ThreadPageViewProps) {

    const { forumPostID } = match.params;

    const { data: threadData } = useForumQuery(forumPostID);
    const { data: commentsData } = useUsersComments (forumPostID);

    function handleDeleteComment (commentIndex: number) {
        ForumPostApi.deleteForumComment({
            commentIndex
        })
        .catch(onError);
    }
    
    function handleAddComment (comment: string, commentIndex: number|null) {
        ForumPostApi.addForumComment({
            comment,
            commentIndex
        })
        .catch(onError);
    }
    

    return (
        <Container>
            <Row>
                <Col>
                    { threadData &&
                        <ForumThread
                            userID={threadData.userID}
                            profilePhoto={threadData.profilePhoto}
                            title={threadData.title}
                            text={threadData.text}
                            timestamp={threadData.timestamp}
                            nativeLanguageID={threadData.nativeLanguageID}
                        />
                    }
                </Col>
            </Row>
            <Row>
                <Col>
                    <ForumCommentForm 
                        onSubmit={handleAddComment}
                        commentIndex={null!}
                    />
                </Col>
            </Row>
            <Row>
               <Col>
                    <ForumCommentsPanel>
                        {
                            commentsData !== undefined &&
                            commentsData.map((comments) => 
                                <ForumComment key={comments.userID}
                                    onSubmit={handleAddComment}
                                    userID={comments.userID}
                                    profilePhoto={comments.profilePhoto}
                                    commentIndex={comments.commentIndex}
                                    comment={comments.comment}
                                    timestamp={comments.timestamp}
                                    onDelete={handleDeleteComment}
                                />
                        )}
                        
                    </ForumCommentsPanel>
               </Col>         
            </Row>
        </Container>
    )
}