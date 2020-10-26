import * as React from 'react';
import { Paper, Grid, TextField, Button, makeStyles, Typography, Divider } from '@material-ui/core';
import { useState } from 'react';
import { AnyVoidFunction } from '@Palavyr-Types';
import { SinglePurposeButton } from './SinglePurposeButton';


export interface ISettingsGridRow {
    name?: string;
    details?: string;
    title?: string;
    children?: React.ReactNode;
    onClick: AnyVoidFunction;
    placeholder?: string;
    clearVal?: boolean;
    currentValue?: string;
    buttonText?: string;
    alertNode?: React.ReactNode;
    inputType?: "email" | "text" | "number";
    fullWidth?: boolean;
}

const useStyles = makeStyles(theme => ({
    row: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        margin: "0rem"
    },
    paper: {
        backgroundColor: "#C7ECEE",
        padding: "2rem",
        margin: "1rem",
        width: "100%"
    },
    singlePurposeButton: {
        marginTop: "1rem",
        color: "black",
        background: "white"
    }
}))

export const SettingsGridRowText: React.FC<ISettingsGridRow> = ({ name, details, title, children, fullWidth, inputType, alertNode, placeholder, onClick, currentValue, clearVal = false, buttonText = "Update" }: ISettingsGridRow) => {

    const [inputVal, setInputVal] = useState<string>();
    const [inputValStatus, setInputValStatus] = useState<string | null>(null);

    const classes = useStyles();
    if (inputType === undefined) {
        inputType = "text"
    }
    return (
        <Paper className={classes.paper}>
            {alertNode}
            <Grid className={classes.row} container>
                {
                    placeholder &&
                    <Grid item xs={12}>
                        {
                            inputType === "text" &&
                            <TextField
                                fullWidth={fullWidth}
                                variant="standard"
                                placeholder={placeholder}
                                onChange={
                                    (e) => {
                                        setInputVal(e.target.value)
                                    }}
                                value={inputVal}
                            />
                        }
                        {
                            inputType === "email" &&
                            <TextField
                                variant="standard"
                                error={inputValStatus === "invalidEmail"}
                                fullWidth={fullWidth}
                                label="New Email Address"
                                value={inputVal}
                                autoComplete="off"
                                type="email"
                                onChange={(e) => {
                                    setInputVal(e.target.value)
                                    if (inputValStatus === "invalidEmail") {
                                        setInputValStatus(null);
                                    }
                                }}
                                helperText={
                                    inputValStatus === "invalidEmail" &&
                                    "This email address isn't associated with an account."
                                }
                                FormHelperTextProps={{ error: true }}
                            />

                        }
                        {inputType === "number" && null}
                    </Grid>
                }
            </Grid>
            <Grid>
                {
                    currentValue &&
                    <>
                        <Typography display="inline" style={{ paddingTop: "1rem" }} variant="body1">Current value: </Typography>
                        <Typography display="inline" style={{ paddingTop: "1rem", fontWeight: "bold" }}>{currentValue}</Typography>
                    </>
                }
            </Grid>
            <Divider />
            <div style={{ display: "flex", justifyContent: "flex-end" }}>
                <SinglePurposeButton
                    classes={classes.singlePurposeButton}
                    variant="contained"
                    color="primary"
                    buttonText={buttonText}
                    onClick={
                        () => {
                            onClick(inputVal);
                            if (clearVal === true) {
                                setInputVal("");
                            }
                        }
                    }

                />
            </div>
        </Paper >
    )
}