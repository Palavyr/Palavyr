import React from "react";
import { TableRow, Button, makeStyles, TextField } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { TwoNestedCategoryResource } from "@Palavyr-Types";
import { TwoNestedCategoriesModifier } from "./TwoNestedCategoriesModifier";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { Cell } from "../../components/Cell";
import { TableButton } from "../SelectOneFlat/TableButton";

export interface TwoNestedCategoriesRowProps {
    index: number;
    shouldDisableInnerCategory: boolean;
    outerCategoryId: string;
    outerCategoryName: string;
    tableData: TwoNestedCategoryResource[];
    row: TwoNestedCategoryResource;
    modifier: TwoNestedCategoriesModifier;
}

type StyleProps = {
    isTrue: boolean;
};
const useStyles = makeStyles(theme => ({
    number: {
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
    outerCategoryInput: {
        margin: "0.6rem",
        width: "30ch",
    },
}));

export const TwoNestedCategoriesRow = ({ index, shouldDisableInnerCategory, outerCategoryId, outerCategoryName, tableData, row, modifier }: TwoNestedCategoriesRowProps) => {
    const cls = useStyles({ isTrue: !row.range });
    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow>
            <Cell>
                {shouldDisableInnerCategory ? (
                    <div style={{ width: "22ch" }}></div>
                ) : (
                    <Button style={{ width: "22ch" }} size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeInnerCategory(tableData, row.rowOrder)}>
                        Delete {row.innerItemName}
                    </Button>
                )}
            </Cell>
            <Cell>
                {index === 0 && (
                    <TextField
                        className={cls.outerCategoryInput}
                        variant="standard"
                        label="Category name"
                        type="text"
                        value={outerCategoryName || ""}
                        color="primary"
                        onChange={(event: { preventDefault: () => void; target: { value: string } }) => {
                            event.preventDefault();
                            modifier.setOuterCategoryName(tableData, outerCategoryId, event.target.value);
                        }}
                    />
                )}
            </Cell>
            <Cell>
                <TextField
                    disabled={shouldDisableInnerCategory}
                    className={cls.input}
                    variant="standard"
                    label="Inner Category Name"
                    type="text"
                    value={row.innerItemName || ""}
                    color="primary"
                    onChange={(event: { preventDefault: () => void; target: { value: any } }) => {
                        event.preventDefault();
                        modifier.setInnerCategoryName(tableData, row.rowOrder, event.target.value);
                    }}
                />
            </Cell>
            <Cell>
                <CurrencyTextField
                    label="Amount"
                    value={row.valueMin || 0}
                    currencySymbol={currencySymbol}
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onValueChange={(values: NumberFormatValues) => {
                        if (values.floatValue !== undefined) {
                            modifier.setValueMin(tableData, row.rowId, values.floatValue);
                        }
                    }}
                />
            </Cell>
            <Cell>
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
            </Cell>
            <Cell>
                {!shouldDisableInnerCategory ? (
                    <TableButton
                        state={row.range}
                        disabled={shouldDisableInnerCategory}
                        onClick={() => {
                            modifier.setRangeOrValue(tableData, row.rowOrder);
                        }}
                    />
                ) : (
                    <div style={{ width: "18ch" }}></div>
                )}
            </Cell>

            <Cell></Cell>
        </TableRow>
    );
};
