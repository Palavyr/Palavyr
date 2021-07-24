import { makeStyles, Typography } from "@material-ui/core";
import React, { useContext, useEffect } from "react";
import { DashboardContext } from "../DashboardContext";
import { Align } from "../positioning/Align";
import Fade from "react-reveal/Fade";
import CloseIcon from "@material-ui/icons/Close";

const useStyles = makeStyles((theme) => ({
    container: {
        width: "100%",
        paddingTop: "0.8rem",
        paddingBottom: "0.8rem",
        backgroundColor: theme.palette.error.light,
    },
    panelArea: {
        width: "100%",
        backgroundOpacity: 0.6,
    },
    innerContainer: {
        display: "flex",
        justifyContent: "center",
        marginTop: "1rem",
    },
    close: {
        float: "right",
        marginRight: "2rem",
        display: "flex",
        justifyItems: "middle",
    },
    item: {
        padding: "0.3rem",
    },
}));

export const ErrorPanel = () => {
    const { panelErrors, setPanelErrors } = useContext(DashboardContext);
    const cls = useStyles();
    useEffect(() => {}, []);
    return panelErrors !== null ? (
        <Fade>
            <>
                <div className={cls.container}>
                    <Typography gutterBottom align="center" variant="h5">
                        Errors
                    </Typography>
                    <div className={cls.innerContainer}>
                        <Align direction="flex-start">
                            <div className={cls.panelArea}>
                                {panelErrors.message && (
                                    <Typography variant="body1" align="center">
                                        {panelErrors.message}
                                    </Typography>
                                )}
                                <ul>
                                    {panelErrors.additionalMessages.map((message: string, key: number) => {
                                        return (
                                            <li key={key} className={cls.item}>
                                                <Typography variant="body2">{message}</Typography>
                                            </li>
                                        );
                                    })}
                                </ul>
                            </div>
                        </Align>
                    </div>
                    <Align direction="flex-end">
                        <div className={cls.close} onClick={() => setPanelErrors(null)}>
                            <CloseIcon fontSize="large" />
                            {/* <Typography display="block" variant="body2">
                                Click to close
                            </Typography> */}
                        </div>
                    </Align>
                </div>
            </>
        </Fade>
    ) : (
        <></>
    );
};
