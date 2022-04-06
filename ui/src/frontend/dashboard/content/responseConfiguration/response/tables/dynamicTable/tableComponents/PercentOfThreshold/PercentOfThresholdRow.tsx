import React from "react";
import { TableRow, makeStyles, FormControlLabel, Checkbox, Typography } from "@material-ui/core";
import { PercentOfThresholdData, UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { PercentOfThresholdModifier } from "./PercentOfThresholdModifier";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { UnitInput } from "../../components/UnitInput";
import { Cell } from "../../components/Cell";
import { TableButton } from "../SelectOneFlat/TableButton";
import { TableDeleteButton } from "./TableDeleteButton";

export interface IPercentOfThresholdRow {
    tableData: PercentOfThresholdData[];
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
export const useStyles = makeStyles(theme => ({
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

export const PercentOfThresholdRow = ({ tableData, itemData, itemLength, row, modifier, baseValue, unitGroup, unitPrettyName }: IPercentOfThresholdRow) => {
    const cls = useStyles({ isTrue: !row.range });

    const onTriggerFallbackChange = event => {
        modifier.checkTriggerFallbackChange(tableData, itemData, row, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow className={cls.tableRow}>
            <Cell>
                <TableDeleteButton onClick={() => modifier.removeRow(tableData, row.rowId)} />
            </Cell>
            <Cell>
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
            </Cell>
            {!row.triggerFallback ? (
                <>
                    <Cell>
                        {!row.triggerFallback && (
                            <TableButton
                                onClick={() => {
                                    modifier.setAddOrSubtract(tableData, row.rowId);
                                }}
                                onMessage="Add"
                                offMessage="Subtract"
                                state={row.posNeg}
                            />
                        )}
                    </Cell>
                    <Cell>
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
                    </Cell>
                    <Cell>
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
                    </Cell>
                    <Cell>
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
                    </Cell>
                    <Cell>
                        {!row.triggerFallback && (
                            <TableButton
                                onClick={() => {
                                    modifier.setRangeOrValue(tableData, row.rowId);
                                }}
                                onMessage="Range"
                                offMessage="Value"
                                state={row.range}
                            />
                        )}
                    </Cell>
                </>
            ) : (
                <>
                    <Cell>
                        <Typography align="center" style={{ paddingTop: "10px" }}>
                            If this threshold value is exceeded in the chat,
                        </Typography>
                        <Typography align="center">then a 'Too Complicated' response will be executed.</Typography>
                    </Cell>
                    <Cell></Cell>
                    <Cell></Cell>
                </>
            )}
            <Cell>
                {itemLength > 1 && row.rowOrder === itemLength - 1 && (
                    <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />
                )}
            </Cell>
        </TableRow>
    );
};
