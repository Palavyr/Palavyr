import React from "react";
import { TableRow, TableCell, Button, makeStyles, FormControlLabel, Checkbox, Typography } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { PercentOfThresholdData, TableData, UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { UnitInput } from "../../components/UnitInput";

export interface IPercentOfThresholdRow {
    tableData: TableData;
    itemData: PercentOfThresholdData[];
    itemLength: number;
    row: PercentOfThresholdData;
    modifier: PercentOfThresholdModifier;
    baseValue: boolean;
    unitGroup: UnitGroups;
    unitPrettyName: UnitPrettyNames;
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
    tableRow: {
        boxShadow: "none",
        border: "0px solid black",
    },
}));

const cellAlignment = "center";

export const PercentOfThresholdRow = ({ tableData, itemData, itemLength, row, modifier, baseValue, unitGroup, unitPrettyName }: IPercentOfThresholdRow) => {
    const cls = useStyles({ isTrue: !row.range });

    const onTriggerFallbackChange = event => {
        modifier.checkTriggerFallbackChange(tableData, itemData, row, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow className={cls.tableRow}>
            <TableCell align={cellAlignment}>
                <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <UnitInput
                    unitGroup={unitGroup}
                    unitPrettyName={unitPrettyName}
                    unitHelperText={unitGroup}
                    disabled={baseValue}
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
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                        const val = event.target.value;
                        if (val !== "") {
                            const result = parseFloat(val);
                            if (result) {
                                modifier.setThresholdValue(tableData, row.rowId, result);
                            }
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
                                value={row.modifier}
                                currencySymbol="%"
                                decimalCharacter="."
                                digitGroupSeparator=","
                                onValueChange={(values: NumberFormatValues) => {
                                    if (values.floatValue !== undefined) {
                                        modifier.setPercentToModify(tableData, row.rowId, values.floatValue);
                                    }
                                }}
                            />
                        )}
                    </TableCell>
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
                                label="Amount"
                                disabled={!row.range}
                                value={row.range ? row.valueMax : 0.0}
                                currencySymbol={currencySymbol}
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
