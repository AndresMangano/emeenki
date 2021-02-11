import React, { ReactNode } from 'react';
import { Card, CardBody, Row, Col, Badge, Media, Toast, ToastHeader, ToastBody } from 'reactstrap';
import moment from 'moment';
import { Link } from 'react-router-dom';

type ActivityCardProps = {
    profilePhoto: ReactNode;
    userID: string;
    title: string;
    points: number;
    timestamp: Date;
    count: number;
}
export function ActivityCard({ profilePhoto, userID, title, points, timestamp, count }: ActivityCardProps) {
    return (
        <Toast className="ActivityBox">
            <div className="toast-header ActivityHeader">
                <h4><Badge color='primary'>+{points} XP</Badge></h4>
                <span className="ml-auto ActivityPhoto">{profilePhoto}</span>
                <Link className="mr-auto mt-2" to={`/profile/${userID}`}><strong>{userID}</strong></Link>
                <small>{moment.utc(timestamp).fromNow()}</small>
            </div>
            <ToastBody>
                <p className="text-center">{transformText(count)} - <u>{title}</u></p>
            </ToastBody>
        </Toast>
    );
}

function transformText(count: number): string {
    return (count === 1)
            ? `Added ${count} translation to the article` 
        : (count > 1)
            ? `Added ${count} translations to the article`
            : ''
}