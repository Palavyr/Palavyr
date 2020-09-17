import React from "react";
import { makeStyles, TextField, Switch, Button, TableRow, TableCell } from "@material-ui/core";
import { StaticTableMetas } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import DeleteIcon from '@material-ui/icons/Delete';
import ArrowDropDownIcon from '@material-ui/icons/ArrowDropDown';
import ArrowDropUpIcon from '@material-ui/icons/ArrowDropUp';
import CurrencyTextField from '@unicef/material-ui-currency-textfield'


type styleProp = {
    index: number;
}

const useStyles = makeStyles(theme => ({
    tableInputs: {
        margin: "0.6rem",
    },
    largeicon: {
        fontSize: "6rem",
        alignContent: "center",
        paddingBottom: "2.5rem",
    },
    deleteIcon: {
        borderRadius: "5px",
    },
    row: (props: styleProp) => ({
        background: (props.index % 2 == 0) ? "#F5F5F5" : `${theme.palette.background.paper}`,
        borderRadius: "5px"
    })
}));

export interface IStaticRow {
    staticTableMetas: StaticTableMetas;
    tableOrder: number;
    modifier: StaticTablesModifier;
    index: number;
    rowOrder: number;
    minFee: number;
    maxFee?: number;
    rangeState: boolean;
    perState: boolean;
    description: string;

}

export const StaticRow = ({ index, staticTableMetas, tableOrder, rowOrder, modifier, minFee, maxFee, rangeState, perState, description }: IStaticRow) => {

    const classes = useStyles({ index });
    const cellAlignment = "center";

    let inputs: {} | null | undefined;

    inputs = (
        <>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    variant="standard"
                    value={minFee}
                    currencySymbol="$"
                    minimumValue="0"
                    outputFormat="string"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(value: { floatValue: number | undefined; }) => {
                        if (value.floatValue !== undefined) { modifier.setFeeMin(staticTableMetas, tableOrder, rowOrder, value.floatValue) }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    variant="standard"
                    disabled={!rangeState}
                    value={maxFee}
                    currencySymbol="$"
                    minimumValue="0"
                    outputFormat="string"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(value: { floatValue: number | undefined; }) => {
                        if (value.floatValue !== undefined) { modifier.setFeeMax(staticTableMetas, tableOrder, rowOrder, value.floatValue) }
                    }}
                />
            </TableCell>
        </>
    );
    // }

    return (
        <TableRow>
            <TableCell align={cellAlignment}>
                <Button
                    size="small"
                    className={classes.deleteIcon}
                    startIcon={<DeleteIcon />}
                    onClick={() => modifier.delRow(staticTableMetas, tableOrder, rowOrder)}
                >
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    fullWidth
                    value={description}
                    label="Description"
                    color="primary"
                    onChange={(event) => {
                        modifier.setRowDescription(staticTableMetas, tableOrder, rowOrder, event.target.value);
                    }}
                />
            </TableCell>
            {inputs}
            <TableCell align={cellAlignment}>
                {rangeState && "Range"}
                {!rangeState && "Single"}
                <br />
                <Switch
                    checked={rangeState}
                    onChange={() => {
                        modifier.changeRange(staticTableMetas, tableOrder, rowOrder);
                    }}
                    name="perPerson"
                    inputProps={{ "aria-label": "primary checkbox" }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                {perState && "Per Person"}
                {!perState && "Flat Fee"}
                <br />
                <Switch
                    checked={perState}
                    onChange={() => {
                        modifier.changePer(staticTableMetas, tableOrder, rowOrder);
                    }}
                    name="perPerson"
                    inputProps={{ "aria-label": "primary checkbox" }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                {!modifier.isRowFirstPosition(rowOrder) && <ArrowDropUpIcon className={classes.largeicon} onClick={() => modifier.shiftRowUp(staticTableMetas, tableOrder, rowOrder)} />}
                {!modifier.isRowLastPosition(staticTableMetas, tableOrder, rowOrder) && <ArrowDropDownIcon className={classes.largeicon} onClick={() => modifier.shiftRowDown(staticTableMetas, tableOrder, rowOrder)} />}
            </TableCell>
        </TableRow>
    );
};
