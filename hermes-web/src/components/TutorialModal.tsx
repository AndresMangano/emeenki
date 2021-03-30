import React from 'react';
import { Modal, ModalBody, ModalHeader } from 'reactstrap';

type TutorialModalProps= {
    onClose:() => void;
    videoLink: string;
    show: boolean;
}

export function TutorialModal ({onClose, show, videoLink}: TutorialModalProps) {
    return (
        <Modal isOpen={show} toggle={onClose} className='app-tutorial-modal'>
            <ModalHeader className='app-tutorial-modal-header'>Tutorial: For begginers</ModalHeader>
                <ModalBody className='app-tutorial-modal-body'>
                        <iframe src={videoLink} title='Emeenki Tutorial' width='100%' height='350'  frameBorder='0' allowFullScreen/>
                </ModalBody>
        </Modal> 
    )
}

// https://thumbs.gfycat.com/MeaslyEthicalCottontail-mobile.mp4