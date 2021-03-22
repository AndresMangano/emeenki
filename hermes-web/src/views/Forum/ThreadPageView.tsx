import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Col, Container, Row } from 'reactstrap';
import { ForumPostApi } from '../../api/ForumPostApi'; 
import { ForumComment } from '../../components/forum/ForumComment';
import { ForumCommentForm } from '../../components/forum/ForumCommentForm';
import { ForumCommentsPanel } from '../../components/forum/ForumCommentsPanel';
import { ForumThread } from '../../components/forum/ForumThread';
import { useForumQuery, useUsersComments } from '../../services/queries-service';

type ThreadPageViewProps = RouteComponentProps<{forumPostID:string}> & {
    onError: (error: any) => void;
}

export function ThreadPageView({ onError, match, history }: ThreadPageViewProps) {

    const { forumPostID: commentPostiD } = match.params;
    const { forumPostID } = match.params;


    const { data: threadData } = useForumQuery(forumPostID);
    const { data: commentsData } = useUsersComments (commentPostiD);

   function handleSubmitEdit (forumPostID: string, text: string) {
       if (threadData !== undefined) {
        ForumPostApi.edit ({
            forumPostID,
            text,
            title: threadData.title,
            languageID: threadData.languageID
        })
       }
   }    

    function handleDeletePost (forumPostID: string) {
        ForumPostApi.delete({
            forumPostID
        })
        .then(() => history.push(`/forum`))
        .catch(onError);
    }
    
    function handleAddComment (text: string) {
        ForumPostApi.addForumComment({
            forumPostID: commentPostiD,
            text
        })
        .catch(onError);
    }

    function handleDeleteComment (forumPostCommentID: string) {
        ForumPostApi.deleteForumComment({
            forumPostID: commentPostiD,
            forumPostCommentID
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
                            forumPostID={threadData.id}
                            profilePhoto={threadData.profilePhotoURL}
                            title={threadData.title}
                            text={threadData.text}
                            timestamp={threadData.timestamp}
                            languageID={threadData.languageID}
                            onDelete={handleDeletePost}
                            onSubmitEdit={handleSubmitEdit}
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
                                    profilePhoto={comments.profilePhotoURL}
                                    commentIndex={comments.id}
                                    comment={comments.text}
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