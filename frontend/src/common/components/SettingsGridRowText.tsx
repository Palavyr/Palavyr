import * as React from 'react';
import { Paper, Grid, TextField, Button, makeStyles } from '@material-ui/core';
import { useState } from 'react';
import { Statement } from './Statement';
import { AnyVoidFunction } from '@Palavyr-Types';


export interface ISettingsGridRow {
    name: string;
    details: string;
    children?: React.ReactNode;
    onClick: AnyVoidFunction;
    placeholder?: string;
    clearVal?: boolean;
    currentValue?: string;
    buttonText?: string;
}

const useStyles = makeStyles(theme => ({
    row: {
        padding: "1rem",
        margin: "1rem"
    },
    paper: {
        padding: "2rem",
        margin: "2rem",
        width: "100%"
    }
}))


export const SettingsGridRowText: React.FC<ISettingsGridRow> = ({ name, details, children, placeholder, onClick, currentValue, clearVal = false, buttonText = "Update" }: ISettingsGridRow) => {

    const [inputVal, setInputVal] = useState<string>();
    const classes = useStyles();

    return (
        <Paper className={classes.paper}>
            <Statement title={name} details={details} >
                {children}
            </Statement>
            <Grid className={classes.row} container>
                {
                    placeholder &&
                    <Grid item xs={4}>
                        <TextField placeholder={placeholder} onChange={(e) => setInputVal(e.target.value)} value={inputVal} />
                    </Grid>
                }

                <Grid item xs={4}>
                    <Button variant="contained" color="primary" onClick={() => {
                        onClick(inputVal);
                        if (clearVal === true) {
                            setInputVal("");
                        }
                    }}
                    >
                        {buttonText}
                    </Button>
                </Grid>
                {
                    currentValue &&
                    <Grid item xs={4}>
                        <strong>Current: {currentValue} </strong>
                    </Grid>
                }

            </Grid>
        </Paper>
    )
}