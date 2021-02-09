import React from 'react';
import { Link } from 'react-router-dom';

type SentenceProps = {
    isTitle: boolean;
    originalText: string;
    translateUrl: string;
    textColor: string;
    isSelected: boolean;
    hovered: boolean;
}
export function Sentence({ isTitle, originalText, translateUrl, textColor, isSelected, hovered }: SentenceProps) {
    return (
        <Link className={`HSentence${hovered ? '-hovered' : ''} ${textColor}`} to={translateUrl} style={{ cursor: 'pointer' }}>
            { (isSelected)
                ?   <div className='mt-2 mb-2 bg-info' style={{
                        fontSize: isTitle ? '1.5rem' : '1rem',
                        fontWeight: 'bold',
                        boxShadow: '0 0 2rem 0 rgba(0,0,0,0.75)',
                        padding: '.8rem',
                        borderRadius: '.5rem',
                        color: 'black'}}
                    >
                        { originalText + '. ' }
                    </div>
                :   <span style={{ fontSize: isTitle ? '1.5rem' : '.9rem' }}>{ originalText + '. ' }</span>
            }
        </Link>
    );
}