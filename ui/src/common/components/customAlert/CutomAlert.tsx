import * as React from "react";

import { makeStyles, Theme } from "@material-ui/core/styles";
import Modal from "@material-ui/core/Modal";
import Backdrop from "@material-ui/core/Backdrop";
import { Paper, Typography } from "@material-ui/core";
import { CustomFade } from "./CustomFade";
import { AlertType } from "@Palavyr-Types";
import { Link } from "react-router-dom";

const useStyles = makeStyles((theme: Theme) => ({
    modal: {
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        outlineWidth: "0px",
    },
    paper: {
        borderRadius: "7px",
        boxShadow: theme.shadows[5],
        padding: theme.spacing(2, 4, 3),
        outlineWidth: "0px",
        backgroundColor: theme.palette.primary.light,
    },
    div: {
        height: "100%",
    },
    link: {
        textDecoration: "none",
        color: theme.palette.success.light,
        "&:hover": {
            color: theme.palette.success.dark
            // boxShadow: theme.shadows[8]
        }
    }
}));

export interface ICustomAlert {
    setAlert: (val: boolean) => void;
    alertState: boolean;
    alert: AlertType;
}

export const CustomAlert = ({ alertState, setAlert, alert }: ICustomAlert) => {
    const cls = useStyles({ alertState });

    return alertState ? (
        <Modal
            aria-labelledby="alertdialog"
            aria-describedby="alertToShowUnallowedStateChange"
            className={cls.modal}
            open={alertState}
            onClose={() => setAlert(false)}
            closeAfterTransition
            BackdropComponent={Backdrop}
            BackdropProps={{
                timeout: 100,
            }}
        >
            <CustomFade in={alertState}>
                <Paper className={cls.paper} elevation={3} square variant="elevation">
                    <div className={cls.div}>
                        <Typography paragraph variant="h4">{alert.title}</Typography>
                        <Typography paragraph variant="body1">{alert.message}</Typography>
                    </div>
                    {alert.link && (
                        <Typography variant="h6" className={cls.link}>
                            <Link className={cls.link} to={alert.link}>{alert.linktext}</Link>
                        </Typography>
                    )}
                </Paper>
            </CustomFade>
        </Modal>
    ) : null;
};
