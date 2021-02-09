import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import './index.scss';
import { App } from './App';
import * as serviceWorker from './serviceWorker';
import { SignalRProvider } from './services/signalr-service';

const basename = document.getElementsByTagName("base")[0].getAttribute("href");

ReactDOM.render(
    <BrowserRouter basename={basename === null ? undefined : basename}>
        <SignalRProvider>
            <App />
        </SignalRProvider>
    </BrowserRouter>, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
