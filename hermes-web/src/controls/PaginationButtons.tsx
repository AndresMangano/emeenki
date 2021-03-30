import React from 'react';
import {Pagination, PaginationItem, PaginationLink } from 'reactstrap';

type PaginationProps = {
    previousPage:() => void;
    nextPage:() => void;
    firstPage:() => void;
    lastPage:() => void;
}

export function PaginationButtons ({ firstPage, previousPage, nextPage, lastPage }: PaginationProps) {
    
    return (
        <Pagination className="app-pagination">
            <PaginationItem className="app-pagination-item">
                <PaginationLink className="app-pagination-item-link" onClick={firstPage} first href="#" />
            </PaginationItem>
            <PaginationItem>
                <PaginationLink onClick={previousPage} previous href="#" />
            </PaginationItem>
            <PaginationItem>
                <PaginationLink onClick={nextPage} next href="#" />
            </PaginationItem>
            <PaginationItem>
                <PaginationLink onClick={lastPage} last href="#"/>
            </PaginationItem>
        </Pagination>
    );
} 

/*
            <PaginationItem>
                <PaginationLink href="#">
                    {paginationPages}
                </PaginationLink>
            </PaginationItem>
*/