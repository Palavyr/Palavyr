import React from 'react'
import { CustomAlert } from './customAlert/CutomAlert'
import { AlertMessage } from './SaveOrCancel'


export interface IPalavyrAlert {
    alertState: boolean;
    setAlertState: any;
    useAlert?: boolean;
    alertMessage?: AlertMessage;
}

export const PalavyrAlert = ({ alertState, useAlert, setAlertState, alertMessage }: IPalavyrAlert) => {
    return (
        <>
            {
                alertState && useAlert && <CustomAlert setAlert={setAlertState} alertState={alertState} alert={alertMessage ?? { title: "Save Successful", message: "" }} />
            }
        </>
    )
}