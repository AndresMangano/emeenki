import React, { FormEvent, useState, useEffect } from 'react';
import { Col, Row, ButtonGroup, Button, Input, Container } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LanguageDTO } from '../api/StaticAPI';

type RoomsFilterProps = {
    onFiltersChange: (filter: 'all'|'mine', language1: string, language2: string) => void;
    languages: LanguageDTO[];
    filter: 'all'|'mine';
    languageFilter: string[];
}
export function RoomsFilter({ onFiltersChange, languages, filter, languageFilter }: RoomsFilterProps) {
    const isAdmin = localStorage.getItem("hermes.rights") === "admin";
    const areLanguagesEqual = languageFilter[0] === languageFilter[1];

    function handleInputChange(event: React.ChangeEvent<HTMLInputElement>) {
        switch (event.currentTarget.name) {
            case 'languageID1': onFiltersChange(filter, event.currentTarget.value, languageFilter[1]); break;
            case 'languageID2': onFiltersChange(filter, languageFilter[0], event.currentTarget.value); break;
        }
    }

    return (
        <Container>
            <Row className='mb-3'>
                <Col className='text-center' md={{ size: 2, offset: 5 }}>
                    <ButtonGroup>
                        <Button color='primary' active={filter !== 'mine'} onClick={() => onFiltersChange('all', languageFilter[0], languageFilter[1])}>ALL</Button>
                        <Button color='success' active={filter === 'mine'} onClick={() => onFiltersChange('mine', languageFilter[0], languageFilter[1])}>MINE</Button>
                    </ButtonGroup>
                </Col>
            </Row>
            <Row>
                <Col md={{ size: 3, offset: 2 }}>
                    <Input type='select' name='languageID1' value={languageFilter[0]} invalid={areLanguagesEqual} onChange={handleInputChange}>
                        { languages.map((e, index) => 
                            <option key={index}value={e.languageID}>{e.description}</option>
                        )}
                    </Input>
                </Col>
                <Col className='text-center' md={2}>
                    { isAdmin &&
                        <Link className='btn btn-primary' to='/rooms/create'>Create Room</Link>
                    }
                </Col>
                <Col md={{ size: 3 }}>
                    <Input type='select' name='languageID2' value={languageFilter[1]} invalid={areLanguagesEqual} onChange={handleInputChange}>
                        { languages.map((e, index) =>
                            <option key={index} value={e.languageID}>{e.description}</option>
                        )}
                    </Input>
                </Col>
            </Row>
        </Container>
    );
}