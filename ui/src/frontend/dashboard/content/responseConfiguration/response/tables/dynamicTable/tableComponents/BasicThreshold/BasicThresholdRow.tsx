import React from "react";
import { Button, Checkbox, FormControlLabel, makeStyles, TableCell, TableRow, Typography } from "@material-ui/core";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { TableData, BasicThresholdData } from "@Palavyr-Types";
import { BasicThresholdModifier } from "./BasicThresholdModifier";
import DeleteIcon from "@material-ui/icons/Delete";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { UnitInput } from "../../components/UnitInput";

type StyleProps = {
    isTrue: boolean;
};
const useStyles = makeStyles(theme => ({
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
    maxValInput: (props: StyleProps) => {
        if (props.isTrue === true) {
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
    const cls = useStyles({ isTrue: !row.range });

    const onTriggerFallbackChange = event => {
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
                <UnitInput
                    unitType={}
                    unitId={}
                    unitHelperText={}
                    disabled={rowIndex === 0}
                    label="Threshold"
                    value={row.threshold}
                    currencySymbol={currencySymbol}
                    onBlur={() => {
                        modifier.reorderThresholdData(tableData);
                        modifier.setTables(tableData);
                    }}
                    onCurrencyChange={(values: NumberFormatValues) => {
                        if (values.floatValue !== undefined) {
                            modifier.setThresholdValue(tableData, row.rowId, values.floatValue);
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
                                value={row.valueMin}
                                currencySymbol={currencySymbol}
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onValueChange={(values: NumberFormatValues) => {
                                    if (values.floatValue !== undefined) {
                                        modifier.setValueMin(tableData, row.rowId, values.floatValue);
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
                                disabled={!row.range}
                                value={row.range ? row.valueMax : 0.0}
                                currencySymbol={currencySymbol}
                                minimumValue="0"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onValueChange={(values: NumberFormatValues) => {
                                    if (values.floatValue !== undefined) {
                                        modifier.setValueMax(tableData, row.rowId, values.floatValue);
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
            <TableCell>
                {tableData.length > 1 && row.rowOrder === tableData.length - 1 && (
                    <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />
                )}
            </TableCell>
        </TableRow>
    );
};
