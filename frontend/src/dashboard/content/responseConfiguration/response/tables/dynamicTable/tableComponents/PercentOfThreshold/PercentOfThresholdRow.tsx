import React from "react";
import { TableRow, TableCell, Button, makeStyles, FormControlLabel, Checkbox, Typography } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { PercentOfThresholdData, TableData } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";

export interface IPercentOfThresholdRow {
    tableData: TableData;
    itemData: PercentOfThresholdData[];
    itemLength: number;
    row: PercentOfThresholdData;
    modifier: PercentOfThresholdModifier;
    baseValue: boolean;
}

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

const cellAlignment = "center";

export const PercentOfThresholdRow = ({ tableData, itemData, itemLength, row, modifier, baseValue }: IPercentOfThresholdRow) => {
    const classes = useStyles({ isTrue: !row.range });

    const onTriggerFallbackChange = event => {
        modifier.checkTriggerFallbackChange(tableData, itemData, row, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow>
            <TableCell align={cellAlignment}>
                <Button size="small" className={classes.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
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
                    onBlur={() => {
                        const reordered = modifier.reorderThresholdData(tableData);
                        modifier.setTables(reordered);
                    }}
                    onChange={(event: any, value: number) => {
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
                        )}
                    </TableCell>
                    <TableCell align={cellAlignment}>
                        {!row.triggerFallback && (
                            <CurrencyTextField
                                label="(5% is 0.05)"
                                variant="standard"
                                value={row.modifier}
                                currencySymbol="%"
                                minimumValue="0"
                                outputFormat="number"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onChange={(event: any, value: number) => {
                                    if (value !== undefined) {
                                        modifier.setPercentToModify(tableData, row.rowId, value);
                                    }
                                }}
                            />
                        )}
                    </TableCell>
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
                                className={classes.maxValInput}
                                label="Amount"
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
                    <TableCell>
                        <Typography align="center" style={{ paddingTop: "10px" }}>
                            If this threshold value is exceeded in the chat,
                        </Typography>
                        <Typography align="center">then a 'Too Complicated' response will be executed.</Typography>
                    </TableCell>
                    <TableCell></TableCell>
                    <TableCell></TableCell>
                </>
            )}
            <TableCell>
                {itemLength > 1 && row.rowOrder === itemLength - 1 && (
                    <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />
                )}
            </TableCell>
        </TableRow>
    );
};
