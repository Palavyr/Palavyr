import * as React from "react";
import { Paper, Grid, TextField, makeStyles, Typography, Divider } from "@material-ui/core";
import { useState } from "react";
import { SinglePurposeButton } from "./SinglePurposeButton";
import NumberFormat from "react-number-format";
import { PalavyrSnackbar } from "./PalavyrSnackbar";

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
}

const useStyles = makeStyles(theme => ({
    row: {
        paddingTop: "1rem",
        paddingBottom: "1rem",
        margin: "0rem",
    },
    paper: {
        backgroundColor: "rgb(0, 0, 0 ,0)", //theme.palette.secondary.light,
        border :"0px",
        boxShadow: "none",
        // backgroundColor: theme.palette.primary.light,
        // color: theme.palette.common.white,
        padding: "2rem",
        margin: "1rem",
        width: "100%",
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
    fullWidth,
    inputType,
    alertNode,
    placeholder,
    onClick,
    currentValue,
    clearVal = false,
    buttonText = "Update",
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
            <Paper className={cls.paper}>
                {alertNode}
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
                                <NumberFormat
                                    format={locale === "en-AU" ? "+61 (##) ####-####" : "+1 (###) ###-####"}
                                    mask="_"
                                    type="tel"
                                    onValueChange={values => setInputVal(values.formattedValue)}
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
                {/* <Divider /> */}
                <div style={{ display: "flex", justifyContent: "flex-end" }}>
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
