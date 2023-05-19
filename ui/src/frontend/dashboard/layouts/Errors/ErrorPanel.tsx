import { makeStyles, Typography } from "@material-ui/core";
import React, { useContext, useEffect } from "react";
import { DashboardContext } from "../DashboardContext";
import { Align } from "../../../../common/positioning/Align";
import CloseIcon from "@material-ui/icons/Close";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    container: {
        width: "100%",
        paddingTop: "0.8rem",
        paddingBottom: "0.8rem",
        backgroundColor: theme.palette.error.light,
    },
    panelIntent: {
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
    ul: {
        margin: "0",
        padding: "0",
    },
    errorTitle: {
        marginBottom: "1.2rem",
    },
}));

export const ErrorPanel = () => {
    const { panelErrors, setPanelErrors } = useContext(DashboardContext);
    const cls = useStyles();
    useEffect(() => { }, []);
    return panelErrors !== null ? (
        <>
            <div className={cls.container}>
                <Typography gutterBottom align="center" variant="h5">
                    Error
                </Typography>
                <div className={cls.innerContainer}>
                    <Align direction="flex-start">
                        <div className={cls.panelIntent}>
                            {panelErrors.message && (
                                <Typography variant="body1" align="center" gutterBottom className={cls.errorTitle}>
                                    {panelErrors.message}
                                </Typography>
                            )}
                            {panelErrors && panelErrors.additionalMessages && panelErrors.additionalMessages.length > 0 && (
                                <>
                                    <Typography align="left">{panelErrors.additionalMessages.length === 1 ? "Reason" : "Reasons"}</Typography>
                                    <ul className={cls.ul}>
                                        {panelErrors.additionalMessages.map((message: string, key: number) => {
                                            return (
                                                <li key={key} className={cls.item}>
                                                    <Typography variant="body2">{message}</Typography>
                                                </li>
                                            );
                                        })}
                                    </ul>
                                </>
                            )}
                        </div>
                    </Align>
                </div>
                <Align direction="flex-end">
                    <div className={cls.close} onClick={() => setPanelErrors(null)}>
                        <CloseIcon fontSize="large" />
                    </div>
                </Align>
            </div>
        </>
    ) : (
        <></>
    );
};
