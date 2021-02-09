import React from 'react';
import { Row, Col, Table, Button } from 'reactstrap';
import { UserAPI } from '../../api/UserAPI';
import { useUsersQuery } from '../../services/queries-service';
import { RouteComponentProps } from 'react-router-dom';
import { useSignalR } from '../../services/signalr-service';

type RootViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function RootView ({ onError }: RootViewProps) {
    useSignalR('users');
    const { data: usersData } = useUsersQuery();
    
    function handleGrant(userID: string, newRights:"admin"|"user") {
        UserAPI.setRights({ userID, newRights })
        .catch(onError);
    }

    return (
        <Row>
            <Col md={{ size: 4, offset: 4}}>
                <h1 className='display-4 text-center'>ROOT</h1>
                <Table hover>
                    <thead className='thead-dark'>
                        <tr>
                            <th>Photo</th>
                            <th>Username</th>
                            <th>Rights</th>
                            <th>Grant</th>
                        </tr>
                    </thead>
                    <tbody>
                        { (usersData) &&
                            usersData.filter((x) => x.userID !== "root").map((e, index) =>
                                <tr key={index}>
                                    <td>
                                        <img className="mr-3 rounded" src={e.profilePhotoURL} alt={e.userID} style={{ width: '3rem' }} />
                                    </td>
                                    <td className='align-middle'>{e.userID}</td>
                                    <td>{e.rights}</td>
                                    <td>
                                        { (e.userID !== "root" && e.rights === "user") &&
                                            <Button color="warning" onClick={() => handleGrant(e.userID, "admin")}>Admin</Button>
                                        }
                                        { (e.userID !== "root" && e.rights === "admin") &&
                                            <Button color="primary" onClick={() => handleGrant(e.userID, "user")}>User</Button>
                                        }
                                    </td>
                                </tr>
                            )
                        }
                    </tbody>
                </Table>
            </Col>
        </Row>
    );
}