import React from "react";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableRow, TableCell, Button, TextField, makeStyles } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { CategorySelectTableRowResource } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { TableButton } from "./TableButton";

export interface ISelectOneFlatRow {
    dataIndex: number;
    tableData: CategorySelectTableRowResource[];
    row: CategorySelectTableRowResource;
    modifier: SelectOneFlatModifier;
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
}));

export const SelectOneFlatRow = ({ dataIndex, tableData, row, modifier }: ISelectOneFlatRow) => {
    const cls = useStyles({ isTrue: !row.range });
    const cellAlignment = "center";
    const key = dataIndex.toString() + row.tableId.toString();

    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow key={key}>
            <TableCell align={cellAlignment}>
                <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.removeOption(tableData, dataIndex)}>
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    className={cls.input}
                    variant="standard"
                    label="Option"
                    type="text"
                    value={row.category}
                    color="primary"
                    onChange={event => {
                        event.preventDefault();
                        modifier.setOptionText(tableData, dataIndex, event.target.value);
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    value={row.valueMin}
                    currencySymbol={currencySymbol}
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onValueChange={(values: NumberFormatValues) => {
                        if (values.floatValue !== undefined) {
                            modifier.setOptionValue(tableData, dataIndex, values.floatValue);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
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
                            modifier.setOptionMaxValue(tableData, dataIndex, values.floatValue);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <TableButton
                    state={row.range}
                    onMessage="Range"
                    offMessage="Value"
                    onClick={() => {
                        modifier.setRangeOrValue(tableData, dataIndex);
                    }}
                />
            </TableCell>
        </TableRow>
    );
};
