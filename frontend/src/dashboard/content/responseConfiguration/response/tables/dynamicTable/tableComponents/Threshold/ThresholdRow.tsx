import React from "react";
import { Button, makeStyles, TableCell, TableRow } from "@material-ui/core";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { TableData, ThresholdData } from "../../DynamicTableTypes";
import { ThresholdModifier } from "./ThresholdModifier";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import DeleteIcon from "@material-ui/icons/Delete";

const useStyles = makeStyles((theme) => ({
    number: {
        border: "1px solid lightgray",
        padding: "1.2rem",
        fontSize: `${theme.typography.fontSize}`,
        font: `${theme.typography.fontFamily}`,
        background: `${theme.palette.background.paper}`,
        outline: "none",
    },
    deleteIcon: {
        borderRadius: "5px",
    },
    input: {
        margin: "0.6rem",
        width: "55ch",
    },
    maxValInput: (prop: boolean) => {
        if (prop === true) {
            return {
                display: "none",
            };
        } else {
            return {};
        }
    },
}));

interface IThresholdRow {
    dataIndex: number;
    tableData: TableData;
    row: ThresholdData;
    modifier: ThresholdModifier;
}

const cellAlignment = "center";

export const ThresholdRow = ({ dataIndex, tableData, row, modifier }: IThresholdRow) => {
    const cls = useStyles();

    const { currencySymbol } = React.useContext(DashboardContext);
    const key = dataIndex.toString() + row.tableId.toString();

    return (
        <TableRow key={key}>
            <TableCell align={cellAlignment}>
                <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    disabled={dataIndex === 0 ? true : false}
                    label="Threshold"
                    variant="standard"
                    value={row.threshold}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setThresholdValue(tableData, row.rowId, value);
                        }
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
                    onChange={(event: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setValueMin(tableData, row.rowId, value);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    className={cls.maxValInput}
                    label="Amount"
                    variant="standard"
                    disabled={!row.range}
                    value={row.range ? row.valueMax : 0.0}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setValueMax(tableData, row.rowId, value);
                        }
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
    );
};
