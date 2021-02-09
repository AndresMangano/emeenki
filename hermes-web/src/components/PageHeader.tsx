import React, { ReactNode } from 'react';
import { Row, Badge } from 'reactstrap';

type PageHeaderProps = {
    children?: ReactNode;
    subtitle?: string;
    color?: string;
    badge?: string;
}
export function PageHeader({ children, subtitle, color, badge }: PageHeaderProps) {
    return (
        <>
            <Row>
                <h1 className={`appBrandFont display-3 ${color !== undefined ? 'text-' + color : ''}`}>{children}</h1>
            </Row>
            <Row className={subtitle === undefined ? 'mb-5' : ''}>
                <h4>
                    <Badge color='dark'>{badge}</Badge>
                </h4>
            </Row>
            <>
                { (subtitle !== undefined) &&
                    <Row className='mb-5'>
                        <h4 className={`display-5 ${color !== undefined ? 'text-' + color : ''}`}>{subtitle}</h4>
                    </Row> 
                }
            </>
        </>
    );
}