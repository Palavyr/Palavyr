import React from "react";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableRow, TableCell, Button, TextField, makeStyles } from "@material-ui/core";
import CurrencyTextField from '@unicef/material-ui-currency-textfield'
import DeleteIcon from '@material-ui/icons/Delete';
import { SelectOneFlatData, TableData } from "../../DynamicTableTypes";
import { uuid } from "uuidv4";
import { DashboardContext } from "dashboard/layouts/DashboardContext";


export interface ISelectOneFlatRow {
    dataIndex: number;
    tableData: TableData;
    row: SelectOneFlatData;
    modifier: SelectOneFlatModifier;
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
        width: "30ch"
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


export const SelectOneFlatRow = ({ dataIndex, tableData, row, modifier }: ISelectOneFlatRow) => {

    const classes = useStyles(!row.range);
    const cellAlignment = "center";
    const key = dataIndex.toString() + row.tableId.toString();

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow key={key}>
            <TableCell align={cellAlignment}>
                <Button
                    size="small"
                    className={classes.deleteIcon}
                    startIcon={<DeleteIcon />}
                    onClick={() => modifier.removeOption(tableData, dataIndex)}
                >
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    className={classes.input}
                    variant="standard"
                    label="Option"
                    type="text"
                    value={row.option}
                    color="primary"
                    onChange={(event) => {
                        event.preventDefault();
                        modifier.setOptionText(tableData, dataIndex, event.target.value)
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
                        if (value !== undefined) { modifier.setOptionValue(tableData, dataIndex, value) }
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
                        if (value !== undefined) { modifier.setOptionMaxValue(tableData, dataIndex, value) }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <Button
                    variant="contained"
                    style={{ width: "18ch" }}
                    color={row.range ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.setRangeOrValue(tableData, dataIndex);
                    }}
                >
                    {row.range ? "Range" : "Single Value"}
                </Button>
            </TableCell>
        </TableRow>
    )
}
