import './App.scss';
import React, { useReducer } from 'react';
import { Switch, Route } from 'react-router-dom';
import { UploadView } from './views/Upload/UploadView';
import { TranslateView } from './views/Translate/TranslateView';
import { VideoTranslateView } from './views/Translate/VideoTranslateView';
import { HNavbar } from './components/MainNavbar';
import { Container } from 'reactstrap';
import { LoginView } from './views/LogIn/LoginView';
import { RootView } from './views/Root/RootView';
import { ArticleTemplatesIndex } from './views/ArticleTemplates/ArticleTemplatesView';
import { RoomsIndex } from './views/Rooms/Index';
import { ProfileView } from './views/Profile/ProfileView';
import { RankingView } from './views/Ranking/RankingView';
import { Footer } from './components/AppFooter';
import { ErrorModal } from './components/ErrorModal';
import { UploadForumPostView } from './views/Upload/UploadForumPostView';
import { ForumView } from './views/Forum/ForumView';
import { ThreadPageView } from './views/Forum/ThreadPageView';
import { PublicView } from './views/Public/PublicView';

export function App() {
    const [{ errorIsOpen, error }, dispatch] = useReducer(reducer, {
        errorIsOpen: false,
        error: ''
    });

    function handleError(error: any) {
        dispatch({ _type: 'ERROR_RAISED', error: error });
    }

    return (
        <div className="App">
            <HNavbar items={[
                { label: 'FORUM', url: '/forum', admin: false, logguedIn: false},
                { label: 'RANKING', url: '/ranking', admin: false, logguedIn: false },
                { label: 'TEXT STORAGE', url: '/templates/active', admin: true, logguedIn: true },
                { label: 'ROOMS', url: '/rooms', admin: false, logguedIn: true },
                { label: 'PUBLIC', url: '/public', admin: false, logguedIn: false }
            ]}/>
            <Container className='App-MainPanel' fluid>
                <Switch>
                    <Route exact path='/' render={(props) => {
                        if (localStorage.getItem("hermes.userID") === null) {
                            return <LoginView {...props} onError={handleError}/>
                        } else {
                            props.history.push('/rooms');
                            return undefined;
                        }}}/>
                    <Route path='/templates/:archived/:roomID?' render={(props) => <ArticleTemplatesIndex {...props} onError={handleError}/>} />
                    <Route path='/rooms' render={(props) => <RoomsIndex {...props} onError={handleError}/>}/>
                    <Route path='/upload' render={(props) => <UploadView {...props} onError={handleError}/>}/>
                    <Route path='/video-translate/:articleTemplateID' render={(props) => <VideoTranslateView {...props} onError={handleError}/>}/>
                    <Route path='/translate/:articleID/:inText?/:sentencePos?/:translationPos?/:comments?' render={(props) => <TranslateView {...props} onError={handleError}/>}/>
                    <Route path='/root' render={(props) => <RootView {...props} onError={handleError}/>}/>
                    <Route path='/profile/:userID' render={(props) => <ProfileView {...props} onError={handleError}/>}/>
                    <Route path='/ranking' render={() => <RankingView/>}/>
                    <Route exact path='/forum' render={(props) => <ForumView {...props} onError={handleError}/>}/>
                    <Route path='/forum/:forumPostID' render={(props) => <ThreadPageView {...props} onError={handleError}/>}/>
                    <Route path='/uploadForumPost' render={(props) => <UploadForumPostView {...props} onError={handleError}/>}/>
                    <Route path='/public' render={(props) => <PublicView {...props} onError={handleError}/>}/>
                </Switch>
            </Container>
            <Footer />
            <ErrorModal
                isOpen={errorIsOpen}
                onAccept={() => dispatch({ _type: 'ERROR_CLOSED'})}
                error={error}
            />
        </div>
    );
}

type State = {
    errorIsOpen: boolean;
    error: any;
}
type Action =
| { _type: 'ERROR_RAISED', error: any; }
| { _type: 'ERROR_CLOSED' }
function reducer(state: State, event: Action) : State {
    switch (event._type) {
        case 'ERROR_RAISED': return { errorIsOpen: true, error: event.error };
        case 'ERROR_CLOSED': return { errorIsOpen: false, error: '' };
    }
}
