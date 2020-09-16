import * as React from 'react';

import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import Modal from '@material-ui/core/Modal';
import Backdrop from '@material-ui/core/Backdrop';
import { Paper } from '@material-ui/core';
import { CustomFade } from './CustomFade';
import { AlertType } from '@Palavyr-Types';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        modal: {
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            outlineWidth: "0px",
        },
        paper: {
            borderRadius: "10px",
            boxShadow: theme.shadows[5],
            padding: theme.spacing(2, 4, 3),
            border: '8px solid blue',
            outlineWidth: "0px",
        },
        div: {
            height: "100%",

        }
    }),
);

export interface ICustomAlert {
    setAlert: (val: boolean) => void
    alertState: boolean;
    alert: AlertType;
}

export const CustomAlert = ({ alertState, setAlert, alert }: ICustomAlert) => {
    const classes = useStyles();

    return (
        <>
            <Modal
                aria-labelledby="alertdialog"
                aria-describedby="alertToShowUnallowedStateChange"
                className={classes.modal}
                open={alertState}
                onClose={() => setAlert(false)}
                closeAfterTransition
                BackdropComponent={Backdrop}
                BackdropProps={{
                    timeout: 100,
                }}
            >
                <CustomFade in={alertState}>
                    <Paper className={classes.paper} elevation={3} square variant="elevation">
                        <div className={classes.div}>
                            <h2>{alert.title}</h2>
                            <p>
                                {alert.message}
                            </p>
                        </div>
                    </Paper>
                </CustomFade>
            </Modal>
        </>
    );
}
