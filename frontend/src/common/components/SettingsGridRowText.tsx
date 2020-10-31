import * as React from 'react';
import { Paper, Grid, TextField, Button, makeStyles, Typography, Divider } from '@material-ui/core';
import { useState } from 'react';
import { AnyVoidFunction } from '@Palavyr-Types';
import { SinglePurposeButton } from './SinglePurposeButton';
import { AlertMessage } from './SaveOrCancel';
import { CustomAlert } from './customAlert/CutomAlert';
import NumberFormat from 'react-number-format';
import { PalavyrAlert } from './PalavyrAlert';


export interface ISettingsGridRow {
    name?: string;
    details?: string;
    title?: string;
    children?: React.ReactNode;
    onClick: (data: any) => Promise<boolean | null | undefined | void>;
    placeholder?: string;
    clearVal?: boolean;
    currentValue?: string;
    buttonText?: string;
    alertNode?: React.ReactNode;
    inputType?: "email" | "text" | "number" | "phone";
    fullWidth?: boolean;
    useAlert?: boolean;
    alertMessage?: AlertMessage;
    CustomInput?: React.ReactNode;
    locale?: string;
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
}))

export const SettingsGridRowText: React.FC<ISettingsGridRow> = ({ name, details, title, children, CustomInput, locale, useAlert, alertMessage, fullWidth, inputType, alertNode, placeholder, onClick, currentValue, clearVal = false, buttonText = "Update" }: ISettingsGridRow) => {

    const [inputVal, setInputVal] = useState<string>();
    const [inputValStatus, setInputValStatus] = useState<string | null>(null);
    const [alertState, setAlertState] = useState<boolean>(false);

    const classes = useStyles();
    if (inputType === undefined) {
        inputType = "text"
    }
    return (
        <>
            <Paper className={classes.paper}>
                {alertNode}
                <Grid className={classes.row} container>
                    {
                        placeholder &&
                        <Grid item xs={12}>
                            {
                                inputType === "text" &&
                                <TextField
                                    variant="standard"

                                    fullWidth={fullWidth}
                                    label={placeholder}
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
                            {
                                inputType === "phone" &&
                                <NumberFormat
                                    format={locale === "en-AU" ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                                    mask="_"
                                    type="tel"
                                    onValueChange={(values) => setInputVal(values.formattedValue)}
                                />
                            }
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
                        variant="outlined"
                        color="primary"
                        buttonText={buttonText}
                        onClick={
                            () => {
                                const res = onClick(inputVal);
                                if (clearVal === true) {
                                    setInputVal("");
                                }
                                if (res) {
                                    setAlertState(true)
                                }
                            }
                        }

                    />
                </div>
            </Paper >
            {
                useAlert && <PalavyrAlert
                    alertMessage={alertMessage}
                    alertState={alertState}
                    setAlertState={setAlertState}
                />
            }
        </>
    )
}
