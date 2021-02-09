import React from 'react';
import { Link } from 'react-router-dom';

type TranslationProps = {
    onHovered: (sentencePos: number, hovered: boolean) => void;
    onUpvoted: (sentencePos: number) => void;
    inText: boolean;
    sentencePos: number;
    hovered: boolean | null;
    originalText: string;
    translation: string | null;
    translateUrl: string;
    textColor: string;
}
export function Translation({ onHovered, onUpvoted, inText, sentencePos, hovered, originalText, translation, translateUrl, textColor }: TranslationProps) {
    return (
        <Link
            id={`${(inText ? 'text' : 'title')}-translation-${sentencePos}`}
            onMouseLeave={() => onHovered(sentencePos, false)}
            onMouseEnter={() => onHovered(sentencePos, true)}
            style={{ fontSize: inText ? '.9rem' : '1.5rem', cursor: 'pointer' }} 
            className={`HSentence${hovered ? '-hovered' : ''} ${textColor}`}
            to={translateUrl}>
                { (translation !== null)
                    ?   translation + '. '
                    :   originalText.replace(/\S/g, '-') + '. '
                }
        </Link>
    );
}