import React from "react";
import { TableRow, TableCell, Button, makeStyles, TextField } from "@material-ui/core";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import DeleteIcon from "@material-ui/icons/Delete";
import { CategoryNestedThresholdData, TableData } from "../../DynamicTableTypes";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SetState } from "@Palavyr-Types";
import { CategoryNestedThresholdModifier } from "./CategoryNestedThresholdModifier";

export interface CategoryNestedThresholdProps {
    index: number;
    shouldDisableInnerCategory: boolean;
    categoryId: string;
    categoryName: string;
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

export const CategoryNestedThresholdRow = ({ index, shouldDisableInnerCategory, categoryId, categoryName, setCategoryName, tableData, row, modifier }: CategoryNestedThresholdProps) => {
    const cls = useStyles(!row.range);

    const { currencySymbol } = React.useContext(DashboardContext);

    const outerCategoryColumn =
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
            {outerCategoryColumn}

            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    disabled={shouldDisableInnerCategory}
                    className={cls.input}
                    variant="standard"
                    label="Inner Category Name"
                    type="text"
                    value={row.threshold}
                    color="primary"
                    onChange={(event: { preventDefault: () => void; target: { value: any } }) => {
                        event.preventDefault();
                        modifier.setThreshold(tableData, row.rowId, event.target.value);
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
                    onChange={(_: any, value: number) => {
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
                    onChange={(_: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setValueMax(tableData, row.rowId, value);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                {!shouldDisableInnerCategory ? (
                    <Button
                        disabled={shouldDisableInnerCategory}
                        variant="contained"
                        style={{ width: "18ch" }}
                        color={row.range ? "primary" : "secondary"}
                        onClick={() => {
                            modifier.setRangeOrValue(tableData, row.rowId);
                        }}
                    >
                        {row.range ? "Range" : "Single Value"}
                    </Button>
                ) : (
                    <></>
                )}
            </TableCell>
            <TableCell align={cellAlignment}>
                {shouldDisableInnerCategory ? (
                    <></>
                ) : (
                    <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeThreshold(tableData, row.rowId)}>
                        Delete Inner Category
                    </Button>
                )}
            </TableCell>
            <TableCell></TableCell>
        </TableRow>
    );
};
