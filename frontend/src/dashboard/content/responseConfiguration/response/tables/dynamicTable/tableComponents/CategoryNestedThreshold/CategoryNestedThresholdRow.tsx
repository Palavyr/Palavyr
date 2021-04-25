import React from "react";
import { TableRow, TableCell, Button, makeStyles, TextField, FormControlLabel, Checkbox, Typography } from "@material-ui/core";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import DeleteIcon from "@material-ui/icons/Delete";
import { CategoryNestedThresholdData, TableData } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SetState } from "@Palavyr-Types";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";

export interface CategoryNestedThresholdProps {
    index: number;
    categoryId: string;
    categoryName: string;
    categorySize: number;
    setCategoryName: SetState<string>;
    tableData: TableData;
    row: CategoryNestedThresholdData;
    modifier: CategoryNestedThresholdModifier;
}

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
    categoryInput: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
}));

const cellAlignment = "center";

export const CategoryNestedThresholdRow = ({ index, categoryId, categoryName, categorySize, setCategoryName, tableData, row, modifier }: CategoryNestedThresholdProps) => {
    const cls = useStyles(!row.range);

    const onTriggerFallbackChange = (event) => {
        modifier.checkTriggerFallbackChange(tableData, row, categoryId, event.target.checked);
    };

    const { currencySymbol } = React.useContext(DashboardContext);
    const categoryColumn =
        index === 0 ? (
            <TableCell align={cellAlignment}>
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
                        setCategoryName(event.target.value);
                    }}
                />
            </TableCell>
        ) : (
            <TableCell></TableCell>
        );

    return (
        <TableRow>
            {categoryColumn}
            <TableCell align={cellAlignment}>
                <CurrencyTextField
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
                            modifier.setThreshold(tableData, row.rowId, value);
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
                    <TableCell align={cellAlignment}>
                        <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeThreshold(tableData, row.rowId)}>
                            Delete Threshold
                        </Button>
                    </TableCell>
                </>
            ) : (
                <>
                    <>
                        <Typography align="center" style={{ paddingTop: "10px" }}>
                            If this threshold value is exceeded in the chat,
                        </Typography>
                        <Typography align="center">then a 'Too Complicated' response will be executed.</Typography>
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                        <TableCell></TableCell>
                    </>
                </>
            )}
            <TableCell>{row.rowOrder === categorySize - 1 && categorySize > 1 && <FormControlLabel label="Trigger Too Complicated" control={<Checkbox checked={row.triggerFallback} onChange={onTriggerFallbackChange} />} />}</TableCell>
        </TableRow>
    );
};
