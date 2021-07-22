import { makeStyles, Typography } from "@material-ui/core";
import React, { useContext, useEffect } from "react";
import { DashboardContext } from "../DashboardContext";
import { Align } from "../positioning/Align";

export type Error = {
    message: string;
};

export type Errors = Error[];

const useStyles = makeStyles((theme) => ({
    container: {
        width: "100%",
        paddingTop: "2rem",
        paddingBottom: "2rem",
        backgroundColor: theme.palette.error.light,
    },
    panelArea: {
        width: "50%",
        backgroundOpacity: 0.6,
    },
}));

export const ErrorPanel = () => {
    const { errors } = useContext(DashboardContext);
    // const errors: Errors = [{ message: "This is an error" }];
    const cls = useStyles();

    useEffect(() => {}, []);
    return errors.length > 0 ? (
        <div className={cls.container}>
            <Align>
                <div className={cls.panelArea}>
                    <ul>
                        {errors.map((error: Error, key: number) => {
                            return (
                                <li key={key}>
                                    <Typography>{error.message}</Typography>
                                </li>
                            );
                        })}
                    </ul>
                </div>
            </Align>
        </div>
    ) : (
        <></>
    );
};
