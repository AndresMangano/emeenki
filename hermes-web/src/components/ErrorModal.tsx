import React from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button } from 'reactstrap';

type ErrorModalProps = {
    onAccept: () => void;
    isOpen: boolean;
    error: any;
}
export function ErrorModal({ isOpen, onAccept, error }: ErrorModalProps) {
    return(
        <Modal isOpen={isOpen}>
            <ModalHeader><h5 className='appBrandFont'>Error</h5></ModalHeader>
            <ModalBody>{error.response ? error.response.data.Error : JSON.stringify(error)}</ModalBody>
            <ModalFooter>
                <Button color="primary" onClick={onAccept}>OK</Button>
            </ModalFooter>
        </Modal>
    );
}