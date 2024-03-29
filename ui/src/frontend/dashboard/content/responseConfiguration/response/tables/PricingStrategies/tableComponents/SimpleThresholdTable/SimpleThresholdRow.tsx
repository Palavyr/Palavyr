import React, { useContext } from "react";
import { Button, Checkbox, FormControlLabel, makeStyles, TableCell, TableRow } from "@material-ui/core";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { BasicThresholdModifier } from "./SimpleThresholdModifier";
import DeleteIcon from "@material-ui/icons/Delete";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { UnitInput } from "../../components/UnitInput";
import { TableButton } from "../CategorySelectTable/TableButton";
import { TextInput } from "@common/components/TextField/TextInput";
import { UnitGroups, UnitPrettyNames } from "@common/types/api/Enums";
import { BasicThresholdResource } from "@common/types/api/EntityResources";

type StyleProps = {
    isTrue: boolean;
};
import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
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
        width: "30ch",
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
    inputPropsCls: {
        paddingLeft: "0.4rem",
    },
}));

interface IBasicThresholdRow {
    rowIndex: number;
    tableData: BasicThresholdResource[];
    row: BasicThresholdResource;
    modifier: BasicThresholdModifier;
    unitGroup: UnitGroups;
    unitPrettyName: UnitPrettyNames;
}

const cellAlignment = "center";

export const BasicThresholdRow = ({ rowIndex, tableData, row, modifier, unitGroup, unitPrettyName }: IBasicThresholdRow) => {
    const cls = useStyles({ isTrue: !row.range });
    const onTriggerFallbackChange = event => {
        modifier.checkTriggerFallbackChange(tableData, row, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);
    const key = rowIndex.toString() + row.tableId.toString();

    return (
        <TableRow key={key}>
            <TableCell width="25" align={cellAlignment}>
                {rowIndex > 0 ? (
                    <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
                        Delete
                    </Button>
                ) : (
                    <TextInput
                        className={cls.input}
                        variant="standard"
                        label="Name to use in PDF fee table"
                        type="text"
                        value={tableData[0].itemName}
                        InputLabelProps={{ className: cls.inputPropsCls }}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setItemName(tableData, event.target.value);
                        }}
                    />
                )}
            </TableCell>
            <TableCell align={cellAlignment}>
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
                    <TableCell width="200px" align={cellAlignment}>
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
                            <TableButton
                                state={row.range}
                                onClick={() => {
                                    modifier.setRangeOrValue(tableData, row.rowId);
                                }}
                            />
                        )}
                    </TableCell>
                </>
            ) : (
                <>
                    <TableCell>
                        If this threshold value is exceeded in the chat, then a <strong>'Too Complicated'</strong> response will be executed.
                    </TableCell>
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
