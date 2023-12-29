import * as React from "react";
import { Paper, Grid, makeStyles, FormControl, InputLabel, Typography, Divider, Select, CircularProgress } from "@material-ui/core";
import { useState } from "react";
import { PalavyrSnackbar } from "./PalavyrSnackbar";

export interface ISettingsGridRow {
    currentValue?: string;
    menuName: string;
    menu: React.ReactNode;
    alertNode?: React.ReactNode;
    useModal?: boolean;
    modalMessage: string;
    onChange(event: any): void;
    loading?: boolean;
}


const useStyles = makeStyles<{}>((theme: any) => ({
    row: {
        padding: "1rem",
        margin: "1rem",
        display: "flex",
        justifyContent: "center",
    },
    paper: {
        backgroundColor: "rgb(0, 0, 0 ,0)",
        border: "0px",
        boxShadow: "none",

        padding: "2rem",
        margin: "1rem",
        width: "100%",
    },
    formControl: {
        margin: theme.spacing(1),
        width: "50%",
        alignSelf: "center",
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}));

export const SettingsGridRowList: React.FC<ISettingsGridRow> = ({ onChange, modalMessage, useModal, alertNode, currentValue, menuName, menu, loading }: ISettingsGridRow) => {
    const cls = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);

    return (
        <>
            <Paper className={cls.paper}>
                <div style={{ display: "flex", justifyContent: "center", margin: "1rem" }}>{loading ? <CircularProgress /> : alertNode}</div>
                <Grid alignContent="center" className={cls.row} container>
                    <FormControl className={cls.formControl}>
                        <InputLabel id="select-list-label">{menuName}</InputLabel>
                        <Select
                            fullWidth
                            labelId="select-list-locale"
                            id="select-text-locale"
                            value={currentValue}
                            onChange={event => {
                                onChange(event);
                                setAlertState(true);
                            }}
                        >
                            {menu}
                        </Select>
                    </FormControl>
                </Grid>
                <Grid className={cls.row}>
                    {currentValue && (
                        <>
                            <Typography display="inline" style={{ paddingTop: "1rem" }} variant="body1">
                                Current locale:{" "}
                            </Typography>
                            <Typography display="inline" style={{ paddingTop: "1rem", fontWeight: "bold" }}>
                                {currentValue}
                            </Typography>
                        </>
                    )}
                </Grid>
            </Paper>
            <PalavyrSnackbar successOpen={alertState} setSuccessOpen={setAlertState} successText={modalMessage ?? "Save Successful"} />
        </>
    );
};
