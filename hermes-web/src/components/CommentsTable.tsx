import React from 'react';
import { Media, Button } from 'reactstrap';
import moment from 'moment';
import { Link } from 'react-router-dom';

type CommentsTableProps = {
    comments: {
        profilePhotoUrl: string;
        userID: string;
        timestamp: Date;
        comment: string;
    }[]
}
export function CommentsTable({ comments }: CommentsTableProps) {
    return(
        <>
            { comments.map((comment, index) =>
                <li key={index} className="list-unstyled border-bottom">
                    <Media className='m-3' heading>
                        <Media className="PostsCommentsUpperPart">
                            <img className="PostsCommentsPhoto" src={comment.profilePhotoUrl === "" ? "https://i.imgur.com/ipAslnw.png" : comment.profilePhotoUrl} alt="Article"/>
                            <Link className='PostsCommentsUserData' to={`/profile/${comment.userID}`}>{`${comment.userID}`}</Link>
                            <big className='PostsCommentsTimeStamp'>{`${moment.utc(comment.timestamp).fromNow()}`}</big>
                        </Media>        
                        <Media body>
                            <p className='PostsCommentsText'>
                                {comment.comment}
                            </p>
                        </Media>
                    </Media>
                </li>
            )}
        </>
    );
}