import React from "react";
import { TableRow, makeStyles, TextField, FormControlLabel, Checkbox } from "@material-ui/core";
import { CategoryNestedThresholdData, UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { UnitInput } from "../../components/UnitInput";
import { Cell } from "../../components/Cell";
import { TableButton } from "../SelectOneFlat/TableButton";
import { TableDeleteButton } from "../PercentOfThreshold/TableDeleteButton";

export interface CategoryNestedThresholdProps {
    rowIndex: number;
    categoryId: string;
    categoryName: string;
    categorySize: number;
    tableData: CategoryNestedThresholdData[];
    row: CategoryNestedThresholdData;
    modifier: CategoryNestedThresholdModifier;
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
    categoryInput: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
}));

const cellAlignment = "center";

export const CategoryNestedThresholdRow = ({ rowIndex, categoryId, categoryName, categorySize, tableData, row, modifier, unitGroup, unitPrettyName }: CategoryNestedThresholdProps) => {
    const cls = useStyles({ isTrue: !row.range });

    const onTriggerFallbackChange = event => {
        modifier.checkTriggerFallbackChange(tableData, row, categoryId, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow>
            <Cell>
                <TableDeleteButton onClick={() => modifier.removeThreshold(tableData, row.rowId)} />
            </Cell>
            <Cell>
                {rowIndex === 0 && (
                    <TextField
                        className={cls.categoryInput}
                        variant="standard"
                        label="Category name"
                        type="text"
                        value={categoryName}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setCategoryName(tableData, categoryId, event.target.value);
                        }}
                    />
                )}
            </Cell>
            <Cell>
                <UnitInput
                    unitGroup={unitGroup}
                    unitPrettyName={unitPrettyName}
                    unitHelperText={unitGroup}
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
                            modifier.setThreshold(tableData, row.rowId, values.floatValue);
                        }
                    }}
                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                        const val = event.target.value;
                        if (val !== "") {
                            const result = parseFloat(val);
                            if (result) {
                                modifier.setThreshold(tableData, row.rowId, result);
                            }
                        }
                    }}
                />
            </Cell>
            {!row.triggerFallback ? (
                <>
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
                                state={row.range}
                                onClick={() => {
                                    modifier.setRangeOrValue(tableData, row.rowId);
                                }}
                            />
                        )}
                    </Cell>
                </>
            ) : (
                <>
                    <>
                        If this threshold value is exceeded in the chat, then a <strong>'Too Complicated'</strong> response will be executed.
                        <Cell></Cell>
                        <Cell></Cell>
                        <Cell></Cell>
                    </>
                </>
            )}
            <Cell>
                {row.rowOrder === categorySize - 1 && categorySize > 1 && (
                    <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />
                )}
            </Cell>
        </TableRow>
    );
};
