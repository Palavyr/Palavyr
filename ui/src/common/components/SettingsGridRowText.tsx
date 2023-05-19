import * as React from "react";
import { Paper, Grid, TextField, makeStyles, Typography, CircularProgress } from "@material-ui/core";
import { useState } from "react";
import { SinglePurposeButton } from "./SinglePurposeButton";
import { PatternFormat } from "react-number-format";
import { PalavyrSnackbar } from "./PalavyrSnackbar";
import classNames from "classnames";

const cn = classNames;
export interface ISettingsGridRow {
    onClick: (data: any) => Promise<boolean | null | undefined | void>;
    placeholder?: string;
    clearVal?: boolean;
    currentValue?: string;
    buttonText?: string;
    alertNode?: React.ReactNode;
    inputType?: "email" | "text" | "number" | "phone";
    fullWidth?: boolean;
    locale?: string;
    successText?: string;
    classNames?: string;
    loading?: boolean;
}

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    row: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        margin: "0rem",
    },
    paper: {
        backgroundColor: "rgb(0, 0, 0 ,0)", //theme.palette.secondary.light,
        border: "0px",
        boxShadow: "none",
        padding: "2rem",
        margin: "1rem",
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
    },
    phone: {
        padding: ".5rem",
        border: "none",
        borderBottom: "1px solid black",
        background: "none",
    },
}));

export const SettingsGridRowText: React.FC<ISettingsGridRow> = ({
    successText,
    locale,
    loading,
    fullWidth,
    inputType,
    alertNode,
    placeholder,
    onClick,
    currentValue,
    clearVal = false,
    buttonText = "Update",
    classNames = "",
}: ISettingsGridRow) => {
    const [inputVal, setInputVal] = useState<string>();
    const [inputValStatus, setInputValStatus] = useState<string | null>(null);
    const [alertState, setAlertState] = useState<boolean>(false);

    const cls = useStyles();
    if (inputType === undefined) {
        inputType = "text";
    }
    return (
        <>
            <Paper className={cn(cls.paper, classNames)}>
                <div style={{ display: "flex", justifyContent: "center", margin: "1rem" }}>{loading ? <CircularProgress /> : alertNode}</div>
                <Grid className={cls.row} container>
                    {placeholder && (
                        <Grid item xs={12}>
                            {inputType === "text" && (
                                <TextField
                                    variant="standard"
                                    fullWidth={fullWidth}
                                    label={placeholder}
                                    onChange={e => {
                                        setInputVal(e.target.value);
                                    }}
                                    value={inputVal}
                                />
                            )}
                            {inputType === "email" && (
                                <TextField
                                    variant="standard"
                                    error={inputValStatus === "invalidEmail"}
                                    fullWidth={fullWidth}
                                    label="New Email Address"
                                    value={inputVal}
                                    autoComplete="off"
                                    type="email"
                                    onChange={e => {
                                        setInputVal(e.target.value);
                                        if (inputValStatus === "invalidEmail") {
                                            setInputValStatus(null);
                                        }
                                    }}
                                    helperText={inputValStatus === "invalidEmail" && "This email address isn't associated with an account."}
                                    FormHelperTextProps={{ error: true }}
                                />
                            )}
                            {inputType === "number" && null}
                            {inputType === "phone" && (
                                <PatternFormat
                                    format={locale === "en-AU" ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                                    mask="_"
                                    type="tel"
                                    onValueChange={(values: { formattedValue: React.SetStateAction<string | undefined>; }) => setInputVal(values.formattedValue)}
                                    className={cls.phone}
                                />
                            )}
                        </Grid>
                    )}
                </Grid>
                <Grid>
                    {currentValue && (
                        <>
                            <Typography display="inline" style={{ paddingTop: "1rem" }} variant="body1">
                                Current:{" "}
                            </Typography>
                            <Typography display="inline" style={{ paddingTop: "1rem", fontWeight: "bold" }}>
                                {currentValue}
                            </Typography>
                        </>
                    )}
                </Grid>
                <div style={{ display: "flex", justifyContent: "center" }}>
                    <SinglePurposeButton
                        variant="outlined"
                        color="primary"
                        buttonText={buttonText}
                        onClick={async () => {
                            const res = await onClick(inputVal);
                            if (clearVal === true) {
                                setInputVal("");
                            }
                            if (res) {
                                setAlertState(true);
                            }
                        }}
                    />
                </div>
            </Paper>
            <PalavyrSnackbar successText={successText} successOpen={alertState} setSuccessOpen={setAlertState} />
        </>
    );
};
