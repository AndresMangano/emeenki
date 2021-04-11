import React from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';

type TutorialModalProps= {
    onClose:() => void;
    show: boolean;
    videoLink: string;
}

export function TutorialModal ({onClose, show, videoLink }: TutorialModalProps) {
    return (
        <Modal isOpen={show} toggle={onClose} className='app-tutorial-modal'>
            <ModalHeader className='app-tutorial-modal-header'><strong>Tutorial: For begginers</strong></ModalHeader>
                <ModalBody className='app-tutorial-modal-body'>
                    <iframe src="https://streamable.com/e/qrejzy" frameBorder="0" width="100%" height="100%" allowFullScreen></iframe>
                </ModalBody>
        </Modal> 
    )
}

// https://thumbs.gfycat.com/MeaslyEthicalCottontail-mobile.mp4