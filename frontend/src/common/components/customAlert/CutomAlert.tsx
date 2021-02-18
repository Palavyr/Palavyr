import * as React from 'react';

import { makeStyles, Theme } from '@material-ui/core/styles';
import Modal from '@material-ui/core/Modal';
import Backdrop from '@material-ui/core/Backdrop';
import { Paper } from '@material-ui/core';
import { CustomFade } from './CustomFade';
import { AlertType } from '@Palavyr-Types';
import { Link } from 'react-router-dom';


const useStyles = makeStyles((theme: Theme) => ({
    modal: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        outlineWidth: "0px",
        zIndex: 99999
    },
    paper: {
        borderRadius: "10px",
        boxShadow: theme.shadows[5],
        padding: theme.spacing(2, 4, 3),
        border: '2px solid black',
        outlineWidth: "0px",
        zIndex: 99999
    },
    div: {
        height: "100%",

    }
}));

export interface ICustomAlert {
    setAlert: (val: boolean) => void
    alertState: boolean;
    alert: AlertType;
}

export const CustomAlert = ({ alertState, setAlert, alert }: ICustomAlert) => {
    const classes = useStyles({ alertState });

    return (
        alertState ? <Modal
            aria-labelledby="alertdialog"
            aria-describedby="alertToShowUnallowedStateChange"
            className={classes.modal}
            open={alertState}
            onClose={() => setAlert(false)}
            closeAfterTransition
            BackdropComponent={Backdrop}
            BackdropProps={{
                style: {zIndex: 999999},
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
                    {alert.link && <Link to={alert.link}>{alert.linktext}</Link>}
                </Paper>
            </CustomFade>
        </Modal>
        : null
    );
}
