import { makeStyles, Typography } from "@material-ui/core";
import { PanelErrors } from "@Palavyr-Types";
import React, { useContext, useEffect } from "react";
import { DashboardContext } from "../DashboardContext";
import { Align } from "../positioning/Align";
import Fade from "react-reveal/Fade";

const useStyles = makeStyles((theme) => ({
    container: {
        width: "100%",
        paddingTop: "2rem",
        paddingBottom: "2rem",
        backgroundColor: theme.palette.error.light,
        display: "flex",
    },
    panelArea: {
        width: "50%",
        backgroundOpacity: 0.6,
    },
    innerContainer: {
        marginLeft: "20%",
        justifyContent: "space-between",
    },
    close: {
        float: "right",
        marginRight: "2rem",
    },
}));

export const ErrorPanel = () => {
    const { setPanelErrors } = useContext(DashboardContext);
    const cls = useStyles();
    const panelErrors = ["wow Okay this is a long sentence okay"];
    useEffect(() => {}, []);
    return panelErrors && panelErrors.length > 0 ? (
        <Fade out>
            <div className={cls.container}>
                <div className={cls.innerContainer}>
                    <Align direction="flex-start">
                        <div className={cls.panelArea}>
                            <Typography variant="h5">Errors</Typography>
                            <ul>
                                {panelErrors.map((message: string, key: number) => {
                                    return (
                                        <li key={key}>
                                            <Typography>{message}</Typography>
                                        </li>
                                    );
                                })}
                            </ul>
                        </div>
                    </Align>
                </div>
                <Align direction="flex-end">
                    <div className={cls.close} onClick={() => setPanelErrors([])}>
                        Click to close
                    </div>
                </Align>
            </div>
        </Fade>
    ) : (
        <></>
    );
};
