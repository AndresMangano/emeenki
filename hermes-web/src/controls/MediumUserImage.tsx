import React from 'react';

type MediumUserImageProps = {
    profilePhotoURL: string;
}
export function MediumUserImage({ profilePhotoURL }: MediumUserImageProps) {
    return (
        <img
            className='rounded mr-2'
            src={profilePhotoURL === "" ? "https://i.imgur.com/ipAslnw.png" : profilePhotoURL}
            alt='profile photo'
            style={{ width: '50px' }}
        />
    );
}