import React, { useMemo } from 'react';
import { Container, Col, Row } from 'reactstrap';
import { RankingList } from '../../components/RankingList';
import { ActivityFeed } from '../../components/activity/ActivityFeed';
import { PageHeader } from '../../components/PageHeader';
import { useSignalR } from '../../services/signalr-service';
import { useActivityFeedQuery, useRankingQuery } from '../../services/queries-service';
import { ActivityCard } from '../../components/activity/ActivityCard';
import { MediumUserImage } from '../../controls/MediumUserImage';

export function RankingView() {
    useSignalR('articles', 'users');
    const { data: activityData } = useActivityFeedQuery();
    const { data: rankingData } = useRankingQuery();

    type GroupedActivity = {
        title: string;
        userID: string;
        profilePhotoURL: string;
        count: number;
        timestamp: Date;
        points: number;
    };

    const activityCounter = useMemo<GroupedActivity[]>(() => {
        return activityData === undefined
            ?   []
            :   activityData.reduce<GroupedActivity[]>((prev, curr) => {
                    const index = prev.findIndex(a => a.title === curr.title && a.userID === curr.userID);
                    if (index === -1) {
                        prev.push({
                            title: curr.title,
                            userID: curr.userID,
                            count: 1,
                            profilePhotoURL: curr.profilePhotoURL,
                            timestamp: curr.timestamp,
                            points: curr.points,
                        });
                    } else {
                        prev[index].count++;
                        prev[index].points += curr.points;
                        prev[index].timestamp = curr.timestamp > prev[index].timestamp ? curr.timestamp : prev[index].timestamp;
                    }
                
                    return prev;
                }, []);
    }, [activityData]);

    return (
        <>
            <Container>
                <PageHeader>Ranking</PageHeader>
            </Container>
            <Container fluid>
                <Row>
                    <Col md={4}>
                        { (rankingData) && <RankingList ranking={rankingData}/> }
                    </Col>
                    <Col md={2}/>
                    <Col md={6}>
                        { (activityData) &&
                            <ActivityFeed>
                                { activityCounter.map((activity, index) =>
                                    <ActivityCard key={index} {...activity}
                                        profilePhoto={
                                            <MediumUserImage profilePhotoURL={activity.profilePhotoURL} />
                                        }
                                    />
                                )}
                            </ActivityFeed>
                        }
                    </Col>
                </Row>
            </Container>
        </>
    );
}