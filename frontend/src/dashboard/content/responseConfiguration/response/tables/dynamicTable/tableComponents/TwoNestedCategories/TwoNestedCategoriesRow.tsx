import React from "react";
import { TableRow, TableCell, Button, makeStyles, TextField } from "@material-ui/core";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import DeleteIcon from "@material-ui/icons/Delete";
import { TableData, TwoNestedCategoryData } from "@Palavyr-Types";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SetState } from "@Palavyr-Types";


export interface ITwoNestedCategoriesRow {
    index: number;
    shouldDisableInnerCategory: boolean;
    outerCategoryId: string;
    outerCategoryName: string;
    setOuterCategoryName: SetState<string>;
    tableData: TableData;
    row: TwoNestedCategoryData;
    modifier: TwoNestedCategoriesModifier;
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
    outerCategoryInput: {
        margin: "0.6rem",
        width: "30ch",
        paddingLeft: "0.4rem",
    },
}));

const cellAlignment = "center";

export const TwoNestedCategoriesRow = ({ index, shouldDisableInnerCategory, outerCategoryId, outerCategoryName, setOuterCategoryName, tableData, row, modifier }: ITwoNestedCategoriesRow) => {
    const cls = useStyles(!row.range);

    const { currencySymbol } = React.useContext(DashboardContext);

    const outerCategoryColumn =
        index === 0 ? (
            <TableCell align={cellAlignment}>
                <TextField
                    className={cls.outerCategoryInput}
                    variant="standard"
                    label="Category name"
                    type="text"
                    value={outerCategoryName}
                    color="primary"
                    onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                        event.preventDefault();
                        modifier.setOuterCategoryName(tableData, outerCategoryId, event.target.value);
                        setOuterCategoryName(event.target.value);
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
                <TextField
                    disabled={shouldDisableInnerCategory}
                    className={cls.input}
                    variant="standard"
                    label="Inner Category Name"
                    type="text"
                    value={row.innerItemName}
                    color="primary"
                    onChange={(event: { preventDefault: () => void; target: { value: any } }) => {
                        event.preventDefault();
                        modifier.setInnerCategoryName(tableData, row.rowOrder, event.target.value);
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    // disabled={shouldDisableInnerCategory}
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
                            modifier.setRangeOrValue(tableData, row.rowOrder);
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
                    <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeInnerCategory(tableData, row.rowOrder)}>
                        Delete Inner Category
                    </Button>
                )}
            </TableCell>
            <TableCell></TableCell>
        </TableRow>
    );
};
