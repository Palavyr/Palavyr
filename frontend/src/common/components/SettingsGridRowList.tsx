import * as React from 'react';
import { Paper, Grid, TextField, Button, makeStyles, FormControl, InputLabel, MenuItem, Select } from '@material-ui/core';
import { useEffect, useState } from 'react';
import { AnyVoidFunction } from '@Palavyr-Types';
import { Statement } from './Statement';


export interface ISettingsGridRow {
    name: string;
    details: string;
    children?: React.ReactNode;
    onClick: AnyVoidFunction;
    currentValue?: string;
    menuName: string;
    menu: React.ReactNode;
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
    },
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}))


export const SettingsGridRowList: React.FC<ISettingsGridRow> = ({ name, details, children, onClick, currentValue, menuName, menu }: ISettingsGridRow) => {

    const classes = useStyles();

    return (
        <Paper className={classes.paper}>
            <Statement title={name} details={details} >
                {children}
            </Statement>
            <Grid className={classes.row} container>
                <FormControl className={classes.formControl}>
                    <InputLabel id="select-list-label">{menuName}</InputLabel>
                    {menu}
                </FormControl>
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