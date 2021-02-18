import React from 'react';
import { Link } from 'react-router-dom';
import { Row, Badge, Table } from 'reactstrap';
import { LanguageFlag } from './LanguageFlag';

type RankingListProps = {
    ranking: {
        userID: string;
        points: number;
        profilePhotoURL: string;
        nativeLanguageID: string;
    }[]
}
export function RankingList({ ranking }: RankingListProps) {
    return (
        <div>
            <Row>
                <Table hover>
                    <tbody>
                        { ranking.map((e, index) =>
                            <tr key={index} className='appBrandFont'>
                                <td>{index + 1}</td>
                                <td>
                                    <img
                                        className='rounded'
                                        src={e.profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : e.profilePhotoURL}
                                        alt='profile photo'
                                        style={{ maxWidth: '3rem', maxHeight: '2.2rem' }}
                                    />
                                </td>
                                <td><LanguageFlag languageID={e.nativeLanguageID} size="24px"/></td>
                                <td><Link to={`/profile/${e.userID}`}><h4><strong>{e.userID}</strong></h4></Link></td>
                                <td><h5><Badge color='primary'>{e.points} XP</Badge></h5></td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            </Row>
        </div>
    );
}