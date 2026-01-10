import React, { useState } from 'react';
import { Container, Nav, NavItem, NavLink, TabContent, TabPane } from 'reactstrap';
import { UploadForm } from '../../components/UploadForm';
import { UploadVideoForm } from '../../components/UploadVideoForm';
import { RouteComponentProps } from 'react-router-dom';
import { PageHeader } from '../../components/PageHeader';

type UploadViewProps = RouteComponentProps & {
    onError: (error: any) => void;
}

export function UploadView(props: UploadViewProps) {
    const [activeTab, setActiveTab] = useState<'text' | 'video'>('text');

    return (
        <Container>
            <PageHeader>UPLOAD CONTENT</PageHeader>
            <Container>
                <Nav tabs>
                    <NavItem>
                        <NavLink
                            className={activeTab === 'text' ? 'active' : ''}
                            onClick={() => setActiveTab('text')}
                            style={{ cursor: 'pointer' }}
                        >
                            Upload Text
                        </NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink
                            className={activeTab === 'video' ? 'active' : ''}
                            onClick={() => setActiveTab('video')}
                            style={{ cursor: 'pointer' }}
                        >
                            Upload YouTube Video
                        </NavLink>
                    </NavItem>
                </Nav>
                <TabContent activeTab={activeTab} className="mt-4">
                    <TabPane tabId="text">
                        <UploadForm {...props} />
                    </TabPane>
                    <TabPane tabId="video">
                        <UploadVideoForm {...props} />
                    </TabPane>
                </TabContent>
            </Container>
        </Container>
    );
}