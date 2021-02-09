import React, { useReducer, useEffect, useMemo } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Modal, ModalHeader, ModalBody, Container, Row, Col, CardBody, Card, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { PageHeader } from '../../components/PageHeader';
import { UserAPI } from '../../api/UserAPI';
import { LanguageFlag } from '../../components/LanguageFlag';
import { UserPhoto } from '../../components/UserPhoto';
import { faCamera } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import classnames  from 'classnames';
import { ActivityFeed } from '../../components/activity/ActivityFeed';
import { ProfilePostsPanel } from '../../components/ProfilePostsPanel';
import { RouteComponentProps } from 'react-router-dom';
import { useSignalR } from '../../services/signalr-service';
import { useUserActivityFeedQuery, useLanguagesQuery, useUserDetailsQuery, useUserPostsQuery } from '../../services/queries-service';
import { ActivityCard } from '../../components/activity/ActivityCard';
import { MediumUserImage } from '../../controls/MediumUserImage';
import { count } from 'console';

type ProfileViewProps = RouteComponentProps<{userID:string}> & {
    onError: (error: any) => void;
}
export function ProfileView({ onError, match, history }: ProfileViewProps) {
    const userID = localStorage.getItem("hermes.userID") || '';
    const { userID: profileUserID } = match.params;

    useSignalR('articles', `user:${profileUserID}`);
    const [{ profilePhotoURL, nativeLanguageID, isPhotoModalOpen, activeTab }, dispatch] = useReducer(reducer, {
        profilePhotoURL: '',
        nativeLanguageID: 'ENG',
        isPhotoModalOpen: false,
        activeTab: '1'
    });
    const { data: activityFeedData } = useUserActivityFeedQuery(profileUserID);
    const { data: languagesData } = useLanguagesQuery();
    const { data: userDetailsData } = useUserDetailsQuery(profileUserID);
    const { data: userPostsData } = useUserPostsQuery(profileUserID);

    type GroupedActivity = {
        title: string;
        userID: string;
        profilePhotoURL: string;
        count: number;
        timestamp: Date;
        points: number;
    };

    const activityCounter = useMemo<GroupedActivity[]>(() => {
        return activityFeedData === undefined
            ?   []
            :   activityFeedData.reduce<GroupedActivity[]>((prev, curr) => {
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
    }, [activityFeedData]);

    function handlePhotoModalToggle() {
        dispatch({ _type: 'TOGGLE_PHOTO_MODAL' });
    }
    function handleSubmitPhoto(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        if (profilePhotoURL !== undefined) {
            UserAPI.changeProfilePhoto({
                userID: profileUserID,
                profilePhotoURL
            })
            .then(() => dispatch({ _type: 'TOGGLE_PHOTO_MODAL' }))
            .catch(onError);
        }
    }
    function handleLanguageChange(event: React.ChangeEvent<HTMLInputElement>) {
        UserAPI.changeLanguage ({
            userID: profileUserID,
            nativeLanguageID: event.currentTarget.value
        })
        .catch(onError);
    }
    function handleProfilePhotoChange(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'CHANGE_PROFILE_PHOTO_URL', profilePhotoURL: event.currentTarget.value });
    }
    function handleTabSelect(tabId: '1'|'2') {
        dispatch({ _type: 'SELECT_TAB', tabId });
    }
    function handleAddPost(text: string, parentUserPostID: string|null) {
        UserAPI.addPost({
            userID: profileUserID,
            text, 
            parentUserPostID
        })
        .catch(onError);
    }
    function handleDeletePost(userPostID: string) {
        UserAPI.deleteUserPost({
            userID: profileUserID,
            userPostID,
            childUserPostID: null
        })
        .catch(onError);
    }

    useEffect(() => {
        if (userDetailsData !== undefined) {
            dispatch({ _type: 'LOAD_USER_DETAILS',
                nativeLanguageID: userDetailsData.nativeLanguageID,
                profilePhotoURL: userDetailsData.profilePhotoURL
            });
        }
    }, [userDetailsData]);
    
    return (
        <Container>
            { userDetailsData !== undefined &&
                <>
                    <Row className="UserHeader">
                        <Col>
                            <ul>
                                <li>{profileUserID}</li>
                                <li>
                                    <LanguageFlag languageID={userDetailsData.nativeLanguageID} size='24px' />
                                </li>
                            </ul>
                            <UserPhoto  
                                profilePhotoURL={profilePhotoURL}
                                photoHeight={225}
                                photoWidth={225}
                            />
                            <li className="Modal">
                                { (userID === profileUserID) && 
                                    <FontAwesomeIcon className="CameraIcon" icon={faCamera} onClick={(handlePhotoModalToggle)} size="2x"/>   
                                }
                                <Modal isOpen={isPhotoModalOpen} fade={false} toggle={handlePhotoModalToggle}>
                                    <ModalHeader toggle={handlePhotoModalToggle}><strong>User Photo</strong></ModalHeader>
                                        <ModalBody>
                                            <Card className="ChangePhoto">
                                                <CardBody>
                                                    <Form onSubmit={handleSubmitPhoto}>
                                                        <FormGroup>
                                                            <Label for='profilePhotoURL'>Photo URL</Label>
                                                            <Input type='url' name='profilePhotoURL' id='profilePhotoURL' value={profilePhotoURL} onChange={handleProfilePhotoChange}/>
                                                        </FormGroup>
                                                        <Button color='primary'>Update</Button>
                                                    </Form>
                                                </CardBody>
                                            </Card>
                                        </ModalBody>
                                </Modal>
                            </li>    
                        </Col>
                        
                        <Col>
                            { userDetailsData !== undefined &&
                                <>
                                    <ul>
                                        <li>
                                            <h1><b>{userDetailsData.points} EXP</b></h1>
                                        </li>
                                    </ul>
                                </>
                            }
                        </Col>
                    </Row>
                    <div className="ProfileTabs">
                        <Nav tabs>
                            <NavItem>
                                <NavLink
                                    className={classnames({ active: activeTab === '1' })}
                                    onClick={() => handleTabSelect('1')}
                                >
                                    Activity
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                { (userID === profileUserID) &&
                                    <NavLink
                                        className={classnames({ active: activeTab === '2' })}
                                        onClick={() => handleTabSelect('2')}
                                    >
                                        Configuration
                                    </NavLink>
                                }
                            </NavItem>
                        </Nav>
                    </div>
                    <TabContent activeTab={activeTab}>
                        <TabPane tabId="1">
                            <Row>
                                <Col className="ProfilePostsBorder" md={6}>
                                    { (userPostsData) &&
                                        <ProfilePostsPanel
                                            userID={userID}
                                            profileUserID={profileUserID}
                                            onDelete={handleDeletePost}
                                            onSubmit={handleAddPost}
                                            posts={userPostsData}
                                        />
                                    }
                                </Col>
                                <Col className="ProfileActivityBorder" md={6}>
                                    { activityFeedData && 
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
                        </TabPane>
                        <TabPane tabId="2">
                            <Row>
                                <Col className="ProfileInput" md={{size: 12}}>
                                    <ul>
                                        <li>
                                            <Card className="CardLanguage">
                                                <CardBody>
                                                    <Form>
                                                        <FormGroup>
                                                            <Label><strong>Native Language</strong></Label>
                                                            <Input type='select' value={nativeLanguageID} onChange={handleLanguageChange}>
                                                                { (languagesData) &&
                                                                    languagesData.map((e, index) => 
                                                                        <option key={index} value={e.languageID}>{e.description}</option>
                                                                )}
                                                            </Input>
                                                        </FormGroup>
                                                    </Form>
                                                </CardBody>
                                            </Card>
                                        </li>
                                    </ul>
                                </Col>
                            </Row>
                        </TabPane>
                    </TabContent>
                </>
            }
        </Container>
    );
}

type State = {
    profilePhotoURL: string|undefined;
    nativeLanguageID: string;
    isPhotoModalOpen: boolean;
    activeTab: '1'|'2';
}
type Action =
| { _type: 'LOAD_USER_DETAILS', nativeLanguageID: string; profilePhotoURL: string; }
| { _type: 'CHANGE_PROFILE_PHOTO_URL', profilePhotoURL: string }
| { _type: 'SELECT_TAB', tabId: '1'|'2' }
| { _type: 'TOGGLE_PHOTO_MODAL' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'LOAD_USER_DETAILS': return { ...state, profilePhotoURL: action.profilePhotoURL, nativeLanguageID: action.nativeLanguageID };
        case 'CHANGE_PROFILE_PHOTO_URL': return { ...state, profilePhotoURL: action.profilePhotoURL };
        case 'SELECT_TAB': return { ...state, activeTab: action.tabId };
        case 'TOGGLE_PHOTO_MODAL': return { ...state, isPhotoModalOpen: !state.isPhotoModalOpen };
    }
}

/* type GroupedActivity = {
    title: string;
    userID: string;
    count: number;
};

const result: GroupedActivity[] = activityFeed.reduce<GroupedActivity[]>((prev, curr) => {
    const index = prev.findIndex(a => a.title === curr.title && a.userID === curr.userID);
    if (index === -1) {
        prev.push({
            title: curr.title,
            userID: curr.userID,
            count: 1
        });
    } else {
        prev[index].count++;
    }

    return prev;
}, []);
*/