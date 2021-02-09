import React from 'react';

type UserPhotoProps = {
    profilePhotoURL: string | undefined;
    photoHeight: number;
    photoWidth: number;
}
export function UserPhoto({ profilePhotoURL, photoHeight, photoWidth }: UserPhotoProps) {
    return (
        <> 
            { (profilePhotoURL !== undefined) && 
                <img
                    src={profilePhotoURL}
                    alt=' '
                    style={{
                        borderRadius: '50%',
                        objectFit: 'cover',
                        height: `${photoHeight}px`,
                        width: `${photoWidth}px`
                    }}
                />
            } 
        </>   
    );
}