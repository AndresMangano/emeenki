import React from 'react';
import { faTwitter, faYoutube} from '@fortawesome/free-brands-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

export function SocialMediaWeb() {
     return (
        <div className='SocialMediaWeb'>  
            <a href="https://www.twitter.com/emeenki"> 
                <FontAwesomeIcon icon={faTwitter} className="FaTwitterWeb" size="3x"/>
            </a>
            <a href="https://www.youtube.com/channel/UCI3sS6YAX5bpM71zYDRiceg/featured"> 
                <FontAwesomeIcon icon={faYoutube} className="FaYoutubeWeb" size="3x"/>
            </a>
        </div>
    );
}