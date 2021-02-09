import React from 'react';

type LanguageFlagProps = {
    languageID: string;
    size: '16px'|'24px';
}
export function LanguageFlag({ languageID, size }: LanguageFlagProps) {
    return (
        <div className="language-flag">
            { languageID === 'DUT' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/dutch.png`} alt="German" height={size}/> }
            { languageID === 'ENG' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/english.png`} alt="German" height={size}/> }
            { languageID === 'FRE' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/french.png`} alt="German" height={size}/> }
            { languageID === 'GER' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/german.png`} alt="German" height={size}/> }
            { languageID === 'ITA' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/italian.png`} alt="German" height={size}/> }
            { languageID === 'POR' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/portuguese.png`} alt="German" height={size}/> }
            { languageID === 'RUS' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/russian.png`} alt="German" height={size}/> }
            { languageID === 'SPA' && <img src={process.env.PUBLIC_URL+`/img/flags/${size}/spanish.png`} alt="German" height={size}/> }
        </div>
    );
}