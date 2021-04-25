import React from "react";
import { Button, Checkbox, FormControlLabel, makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { TableData, BasicThresholdData } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
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

interface IBasicThresholdRow {
    rowIndex: number;
    tableData: TableData;
    row: BasicThresholdData;
    modifier: BasicThresholdModifier;
}

const cellAlignment = "center";

export const BasicThresholdRow = ({ rowIndex, tableData, row, modifier }: IBasicThresholdRow) => {
    const cls = useStyles();

    const onTriggerFallbackChange = (event) => {
        modifier.checkTriggerFallbackChange(tableData, row, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);
    const key = rowIndex.toString() + row.tableId.toString();

    return (
        <TableRow key={key}>
            <TableCell align={cellAlignment}>
                {rowIndex > 0 && (
                    <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
                        Delete
                    </Button>
                )}
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    disabled={rowIndex === 0 ? true : false}
                    label="Threshold"
                    variant="standard"
                    value={row.threshold}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(_: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setThresholdValue(tableData, row.rowId, value);
                        }
                    }}
                />
            </TableCell>
            {!row.triggerFallback ? (
                <>
                    <TableCell align={cellAlignment}>
                        {!row.triggerFallback && (
                            <CurrencyTextField
                                label="Amount"
                                variant="standard"
                                value={row.valueMin}
                                currencySymbol={currencySymbol}
                                minimumValue="0"
                                outputFormat="number"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onChange={(_: any, value: number) => {
                                    if (value !== undefined) {
                                        modifier.setValueMin(tableData, row.rowId, value);
                                    }
                                }}
                            />
                        )}
                    </TableCell>
                    <TableCell align={cellAlignment}>
                        {!row.triggerFallback && (
                            <CurrencyTextField
                                className={cls.maxValInput}
                                label={row.range ? "Amount" : "Not used"}
                                variant="standard"
                                disabled={!row.range}
                                value={row.range ? row.valueMax : 0.0}
                                currencySymbol={currencySymbol}
                                minimumValue="0"
                                outputFormat="number"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onChange={(_: any, value: number) => {
                                    if (value !== undefined) {
                                        modifier.setValueMax(tableData, row.rowId, value);
                                    }
                                }}
                            />
                        )}
                    </TableCell>
                    <TableCell align={cellAlignment}>
                        {!row.triggerFallback && (
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
                        )}
                    </TableCell>
                </>
            ) : (
                <>
                    <Typography align="center" style={{ paddingTop: "10px" }}>
                        If this threshold value is exceeded in the chat,
                    </Typography>
                    <Typography align="center">then a 'Too Complicated' response will be executed.</Typography>
                    <TableCell></TableCell>
                    <TableCell></TableCell>
                    <TableCell></TableCell>
                </>
            )}
            <TableCell>{tableData.length > 1 && row.rowOrder === tableData.length - 1 && <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />}</TableCell>
        </TableRow>
    );
};
