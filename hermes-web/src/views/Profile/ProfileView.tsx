import React, { useReducer, useEffect, useMemo } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Modal, ModalHeader, ModalBody, Container, Row, Col, CardBody, Card, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { UserAPI } from '../../api/UserAPI';
import { LanguageFlag } from '../../components/LanguageFlag';
import { UserPhoto } from '../../components/UserPhoto';
import { faCamera, faEdit, faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import classnames  from 'classnames';
import { ActivityFeed } from '../../components/activity/ActivityFeed';
import { ProfilePostsPanel } from '../../components/ProfilePostsPanel';
import { RouteComponentProps } from 'react-router-dom';
import { useSignalR } from '../../services/signalr-service';
import { useUserActivityFeedQuery, useLanguagesQuery, useUserDetailsQuery, useUserPostsQuery } from '../../services/queries-service';
import { ActivityCard } from '../../components/activity/ActivityCard';
import { MediumUserImage } from '../../controls/MediumUserImage';

type ProfileViewProps = RouteComponentProps<{userID:string}> & {
    onError: (error: any) => void;
}
export function ProfileView({ onError, match, history }: ProfileViewProps) {
    const userID = localStorage.getItem("hermes.userID") || '';
    const { userID: profileUserID } = match.params;

    useSignalR('articles', `user:${profileUserID}`);
    const [{ profilePhotoURL, nativeLanguageID, isPhotoModalOpen, activeTab, isDescriptionBoxOpen, description, country, currentPassword, newPassword, repeatPassword, signInType }, dispatch] = useReducer(reducer, {
        profilePhotoURL: '',
        nativeLanguageID: 'ENG',
        isPhotoModalOpen: false,
        isDescriptionBoxOpen: false,
        description: '',
        country: '',
        currentPassword: '',
        newPassword: '',
        repeatPassword: '',
        activeTab: '1',
        signInType: 'password'||'google'
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

    function handleDescriptionBoxToggle() {
        dispatch ({ _type: 'TOGGLE_DESCRIPTION_BOX'});
    }
    function handleSubmitDescription(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        UserAPI.updateDescription({
            userID: profileUserID,
            description: description === '' ? null : description
        })
        .then(() => dispatch({ _type: 'TOGGLE_DESCRIPTION_BOX' }))
        .catch(onError);
    }
    function handleChangeProfileDescription(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'UPDATE_PROFILE_DESCRIPTION', description: event.currentTarget.value})
    }
    function closeDescriptionBox() {
        dispatch({ _type: 'CLOSE_DESCRIPTION_BOX'})
    }
    function handleChangeProfileCountry(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'UPDATE_PROFILE_COUNTRY', country: event.currentTarget.value})
    }
    function handleUserDetailsChange(event: React.ChangeEvent<HTMLFormElement>) {
        event.preventDefault();
        UserAPI.updateCountry({
            userID: profileUserID,
            country: country
        })
        .catch(onError);
    }
    function handleChangeCurrentPassword(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'INSERT_CURRENT_PASSWORD', currentPassword: event.currentTarget.value})
    }
    function handleChangeNewPassword(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'INSERT_NEW_PASSWORD', newPassword: event.currentTarget.value})
    }
    function handleChangeRepeatPassword(event: React.ChangeEvent<HTMLInputElement>) {
        dispatch({ _type: 'INSERT_REPEAT_PASSWORD', repeatPassword: event.currentTarget.value})
    }
    function handleSubmitNewPassword(event: React.ChangeEvent<HTMLFormElement>) {
        event.preventDefault();
        if ( newPassword === repeatPassword && newPassword !== null && newPassword.length > 0 ) {
            UserAPI.changePassword({
                actualPassword: currentPassword,
                newPassword: newPassword
            })
            .then(() => dispatch({ _type: 'RESET' }))
            .catch(onError);
        }
    }
    useEffect(() => {
        if (userDetailsData !== undefined) {
            dispatch({ _type: 'LOAD_USER_DETAILS',
                nativeLanguageID: userDetailsData.nativeLanguageID,
                profilePhotoURL: userDetailsData.profilePhotoURL,
                description: userDetailsData.description!,
                country: userDetailsData.country
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
                        <Col className="d-flex flex-row justify-content-center UserCountry">
                            <p>{userDetailsData.country}</p>
                            <FontAwesomeIcon className="MapmarkerIcon" icon={faMapMarkerAlt} size="2x"/>
                        </Col>    
                        </Col>
                            { (userID === profileUserID && description === null) &&
                                   <Col>
                                        <Button size="lg" color="transparent" onClick={handleDescriptionBoxToggle}>ADD DESCRIPTION</Button>
                                    </Col>        
                            }
                            { isDescriptionBoxOpen === false &&
                            
                                <Col className="UserDescription" md={6}>
                                    <p>{userDetailsData.description}</p>
                                </Col>
                            }
                            { (userID === profileUserID && description !== null && isDescriptionBoxOpen === false) &&
                                <Col>
                                    <FontAwesomeIcon className="EditIcon" icon={faEdit} onClick={handleDescriptionBoxToggle} size="lg"/>
                                </Col>
                            }
                            { isDescriptionBoxOpen &&
                            
                                <Col md={6}>
                                    <Form onSubmit={handleSubmitDescription}>
                                        <Input type="textarea" name="description" id="description" value={description!} onChange={handleChangeProfileDescription}/>
                                        <Button size="lg" color="transparent">SUBMIT</Button> 
                                        <Button size="lg" color="transparent" onClick={closeDescriptionBox}>CANCEL</Button>
                                    </Form>
                                </Col>
                            }
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
                    <TabContent className="ConfigurationTab" activeTab={activeTab}>
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
                                <Col md={6}>
                                    <Card className="ConfigurationCard">
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
                                    <Card className="ConfigurationCard">
                                        <CardBody>
                                            <Form onSubmit={handleUserDetailsChange}>
                                                    <FormGroup>
                                                        <Label><strong>Country</strong></Label>
                                                        <Input type='text' name='country' value={country} onChange={handleChangeProfileCountry}/>
                                                    </FormGroup>
                                                    <Button size="md" color="primary">Update</Button>
                                                </Form>
                                        </CardBody>
                                    </Card>
                                </Col>
                                {(userDetailsData.signInType === 'password') &&
                                <Col md={6}>
                                    <Card Card className="ConfigurationCard">
                                        <CardBody>
                                            <Form onSubmit={handleSubmitNewPassword}>
                                                <FormGroup>
                                                    <Label><strong>Current Password</strong></Label>
                                                    <Input type='password' name='currentPassword' value={currentPassword} onChange={handleChangeCurrentPassword}/>    
                                                </FormGroup>
                                                <FormGroup>
                                                    <Label><strong>New Password</strong></Label>
                                                    <Input type='password' name='newPassword' value={newPassword} onChange={handleChangeNewPassword}/>    
                                                </FormGroup>
                                                <FormGroup>
                                                    <Label><strong>Repeat New Password</strong></Label>
                                                    <Input type='password' name='repeatPassword' value={repeatPassword} onChange={handleChangeRepeatPassword}/>    
                                                </FormGroup>
                                                {
                                                    (newPassword !== repeatPassword) &&
                                                        <>
                                                            <p className="text-danger"><strong>Passwords don't match</strong></p>
                                                            <Button size="md" color="primary" disabled>Update</Button>
                                                        </>
                                                }
                                                { 
                                                    (newPassword === repeatPassword) &&
                                                        <Button size="md" color="primary">Update</Button>
                                                }
                                            </Form>
                                        </CardBody>
                                    </Card>  
                                </Col>
                                }      
                                {
                                (userDetailsData.signInType === 'google') &&
                                <Col className="d-flex align-items-center ml-5">
                                    <h4><strong>YOU ARE LOGGED IN WITH A GOOGLE ACCOUNT</strong></h4>
                                </Col>
                                }             
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
    isDescriptionBoxOpen: boolean;
    description: string|null;
    country: string;
    currentPassword: string;
    newPassword: string;
    repeatPassword: string;
    signInType: 'password'|'google';
    activeTab: '1'|'2';
}
type Action =
| { _type: 'LOAD_USER_DETAILS', nativeLanguageID: string; profilePhotoURL: string; description: string, country: string }
| { _type: 'CHANGE_PROFILE_PHOTO_URL', profilePhotoURL: string }
| { _type: 'SELECT_TAB', tabId: '1'|'2' }
| { _type: 'TOGGLE_PHOTO_MODAL' }
| { _type: 'UPDATE_PROFILE_DESCRIPTION', description: string|null }
| { _type: 'TOGGLE_DESCRIPTION_BOX'}
| { _type: 'CLOSE_DESCRIPTION_BOX' }
| { _type: 'UPDATE_PROFILE_COUNTRY', country: string}
| { _type: 'INSERT_CURRENT_PASSWORD', currentPassword: string}
| { _type: 'INSERT_NEW_PASSWORD', newPassword: string}
| { _type: 'INSERT_REPEAT_PASSWORD', repeatPassword: string}
| { _type: 'SELECT_SIGNINTYPE_GOOGLE'}
| { _type: 'SELECT_SIGNINTYPE_PASSWORD'}
| { _type: 'RESET' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'LOAD_USER_DETAILS': return { ...state, profilePhotoURL: action.profilePhotoURL, nativeLanguageID: action.nativeLanguageID, description: action.description, country: action.country };
        case 'CHANGE_PROFILE_PHOTO_URL': return { ...state, profilePhotoURL: action.profilePhotoURL };
        case 'SELECT_TAB': return { ...state, activeTab: action.tabId };
        case 'TOGGLE_PHOTO_MODAL': return { ...state, isPhotoModalOpen: !state.isPhotoModalOpen };
        case 'TOGGLE_DESCRIPTION_BOX': return {...state, isDescriptionBoxOpen: !state.isDescriptionBoxOpen} ;
        case 'UPDATE_PROFILE_DESCRIPTION': return { ...state, description: action.description};
        case 'CLOSE_DESCRIPTION_BOX': return {...state, isDescriptionBoxOpen: false};
        case 'UPDATE_PROFILE_COUNTRY': return { ...state, country: action.country}
        case 'INSERT_CURRENT_PASSWORD': return { ...state, currentPassword: action.currentPassword}
        case 'INSERT_NEW_PASSWORD': return { ...state, newPassword: action.newPassword}
        case 'INSERT_REPEAT_PASSWORD': return { ...state, repeatPassword: action.repeatPassword}
        case 'RESET': return { ...state, currentPassword: '', newPassword: '', repeatPassword: '' } 
        case 'SELECT_SIGNINTYPE_GOOGLE': return { ...state, signInType: 'google'}
        case 'SELECT_SIGNINTYPE_PASSWORD': return { ...state, signInType: 'password'}
    }
}