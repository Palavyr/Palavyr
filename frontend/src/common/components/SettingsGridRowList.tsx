import * as React from "react";
import { Paper, Grid, makeStyles, FormControl, InputLabel, Typography, Divider, Select } from "@material-ui/core";
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
}

const useStyles = makeStyles((theme) => ({
    row: {
        padding: "1rem",
        margin: "1rem",
    },
    paper: {
        backgroundColor: theme.palette.secondary.light,
        padding: "2rem",
        margin: "1rem",
        width: "100%",
    },
    formControl: {
        margin: theme.spacing(1),
        width: "50%",
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}));

export const SettingsGridRowList: React.FC<ISettingsGridRow> = ({ onChange, modalMessage, useModal, alertNode, currentValue, menuName, menu }: ISettingsGridRow) => {
    const classes = useStyles();
    const [alertState, setAlertState] = useState<boolean>(false);

    return (
        <>
            <Paper className={classes.paper}>
                {alertNode}
                <Grid className={classes.row} container>
                    <FormControl className={classes.formControl}>
                        <InputLabel id="select-list-label">{menuName}</InputLabel>
                        <Select
                            fullWidth
                            labelId="select-list-locale"
                            id="select-text-locale"
                            value={currentValue}
                            onChange={(event) => {
                                onChange(event);
                                setAlertState(true);
                            }}
                        >
                            {menu}
                        </Select>
                    </FormControl>
                </Grid>
                <Divider />
                <Grid className={classes.row}>
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
