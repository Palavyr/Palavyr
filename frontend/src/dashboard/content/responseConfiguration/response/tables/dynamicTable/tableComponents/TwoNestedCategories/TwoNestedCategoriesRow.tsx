import React from "react";
import { TableRow, TableCell, Button, makeStyles, TextField } from "@material-ui/core";
import CurrencyTextField from "@unicef/material-ui-currency-textfield";
import DeleteIcon from "@material-ui/icons/Delete";
import { TableData, TwoNestedCategoryData } from "../../DynamicTableTypes";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

export interface ITwoNestedCategoriesRow {
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
}));

const cellAlignment = "center";

export const TwoNestedCategoriesRow = ({ tableData, row, modifier }: ITwoNestedCategoriesRow) => {
    const classes = useStyles(!row.range);

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow>
            <TableCell align={cellAlignment}>
                <Button size="small" className={classes.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeRow(tableData, row.rowId)}>
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    className={classes.input}
                    variant="standard"
                    label="Option"
                    type="text"
                    value={row.subCategory}
                    color="primary"
                    onChange={(event: { preventDefault: () => void; target: { value: any; }; }) => {
                        event.preventDefault();
                        modifier.setSubCategoryName(tableData, row.rowId, event.target.value);
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
            </TableCell>
            <TableCell align={cellAlignment}>
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
            </TableCell>
        </TableRow>
    );
};
