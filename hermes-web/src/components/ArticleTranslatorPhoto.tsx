import React from 'react';

type ArticleTranslatorPhotoProps = {
    photoUrl: string;
}
export function ArticleTranslatorPhoto({ photoUrl }: ArticleTranslatorPhotoProps) {
    return (
        <img
            src={photoUrl}
            alt=' '
            style={{
                borderRadius: '1rem',
                height: '16rem'
            }}
        />
    );
}