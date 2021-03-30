import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Container } from 'reactstrap';
import { UploadForumPostForm } from '../../components/forum/UploadForumPostForm';
import { PageHeader } from '../../components/PageHeader';

type UploadForumPostViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function UploadForumPostView (props: UploadForumPostViewProps ) {
    return (
        <Container>
            <PageHeader>UPLOAD POST</PageHeader>
                <Container>
                    <UploadForumPostForm {... props}/>
                </Container>
        </Container>
    )
}