import React, { memo } from "react";
import { SelectOneFlatModifier } from "./SelectOneFlatModifier";
import { TableRow, TableCell, Button, TextField, makeStyles } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { SelectOneFlatData, TableData } from "@Palavyr-Types";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import RemoveIcon from "@material-ui/icons/Remove";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { uuid } from "uuidv4";

export interface ISelectOneFlatRow {
    dataIndex: number;
    tableData: TableData;
    row: SelectOneFlatData;
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
                    value={row.option}
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
                <Button
                    startIcon={
                        row.range ? (
                            <div style={{ display: "flex", flexDirection: "row", justifyContent: "flex-start", alignItems: "center" }}>
                                <ChevronLeftIcon />
                                {/* <RemoveIcon />  */}
                                <ChevronRightIcon />
                            </div>
                        ) : (
                            <RemoveIcon />
                        )
                    }
                    // endIcon={row.range ? <ChevronRightIcon /> : null}
                    variant="contained"
                    style={{ width: "18ch" }}
                    color={row.range ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.setRangeOrValue(tableData, dataIndex);
                    }}
                >
                    {row.range ? "Range" : "Single Value"}
                </Button>
            </TableCell>
        </TableRow>
    );
};
