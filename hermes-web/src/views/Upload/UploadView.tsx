import React from 'react';
import { Container } from 'reactstrap';
import { UploadForm } from '../../components/UploadForm';
import { RouteComponentProps } from 'react-router-dom';
import { PageHeader } from '../../components/PageHeader';

type UploadViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}
export function UploadView(props: UploadViewProps) {
    return (
        <Container>
            <PageHeader>UPLOAD TEXT</PageHeader>
            <Container>
                <UploadForm {...props}/>
            </Container>
        </Container>
    );
}