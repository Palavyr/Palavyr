import React from "react";
import { TableRow, TableCell, Button, makeStyles } from "@material-ui/core";
import CurrencyTextField from '@unicef/material-ui-currency-textfield'
import DeleteIcon from '@material-ui/icons/Delete';
import { PercentOfThresholdData, TableData } from "../../DynamicTableTypes";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { DashboardContext } from "dashboard/layouts/DashboardContext";


export interface IPercentOfThresholdRow {
    tableData: TableData;
    row: PercentOfThresholdData;
    modifier: PercentOfThresholdModifier;
    baseValue: boolean;
    dataIndex: number;
}

const useStyles = makeStyles(theme => ({
    number: {
        border: "1px solid lightgray",
        padding: "1.2rem",
        fontSize: `${theme.typography.fontSize}`,
        font: `${theme.typography.fontFamily}`,
        background: `${theme.palette.background.paper}`,
        outline: "none"
    },
    deleteIcon: {
        borderRadius: "5px",
    },
    input: {
        margin: "0.6rem",
        width: "55ch"
    },
    maxValInput: (prop: boolean) => {
        if (prop === true) {
            return {
                display: "none",
            }
        } else {
            return {}
        }
    }
}))

const cellAlignment = "center";

export const PercentOfThresholdRow = ({ dataIndex, tableData, row, modifier, baseValue }: IPercentOfThresholdRow) => {

    const classes = useStyles(!row.range);
    const key = dataIndex.toString() + row.tableId.toString();

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow key={key}>
            <TableCell align={cellAlignment}>
                <Button
                    size="small"
                    className={classes.deleteIcon}
                    startIcon={<DeleteIcon />}
                    onClick={() => modifier.removeRow(tableData, row.rowId)}
                >
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    disabled={baseValue}
                    label="Threshold"
                    variant="standard"
                    value={row.threshold}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number ) => {
                        if (value !== undefined) { modifier.setThresholdValue(tableData, row.rowId, value) }
                    }}
                />

            </TableCell>
            <TableCell align={cellAlignment}>
                <Button
                    variant="contained"
                    style={{ width: "18ch" }}
                    color={row.posNeg ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.setAddOrSubtract(tableData, row.rowId);
                    }}
                >
                    {row.posNeg === true ? "Add" : "Subtract"}
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="(5% is 0.05)"
                    variant="standard"
                    value={row.modifier}
                    currencySymbol="%"
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number ) => {
                        if (value !== undefined) { modifier.setPercentToModify(tableData, row.rowId, value) }
                    }}
                />

            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    variant="standard"
                    value={row.valueMin}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number ) => {
                        if (value !== undefined) { modifier.setValueMin(tableData, row.rowId, value) }
                    }}
                />

            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    className={classes.maxValInput}
                    label="Amount"
                    variant="standard"
                    disabled={!row.range}
                    value={row.range ? row.valueMax : 0.00}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number ) => {
                        if (value !== undefined) { modifier.setValueMax(tableData, row.rowId, value) }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <Button
                    variant="contained"
                    style={{ width: "18ch" }}
                    color={row.range ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.setRangeOrValue(tableData, row.rowId);
                    }}
                >
                    {row.range ? "Range" : "Single Value"}
                </Button>
            </TableCell>
        </TableRow>
    )
}
