import React from "react";
import { makeStyles, TextField, Button, TableRow, TableCell } from "@material-ui/core";
import { StaticTableMetas } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import DeleteIcon from "@material-ui/icons/Delete";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import ArrowDropUpIcon from "@material-ui/icons/ArrowDropUp";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import RemoveIcon from "@material-ui/icons/Remove";
import PeopleAltIcon from "@material-ui/icons/PeopleAlt";
import GroupAddIcon from "@material-ui/icons/GroupAdd";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";

type styleProp = {
    index: number;
    rangeState: boolean;
};

const useStyles = makeStyles((theme) => ({
    tableInputs: {
        margin: "0.6rem",
    },
    largeicon: {
        fontSize: "6rem",
        alignContent: "center",
        paddingBottom: "2.5rem",
    },
    deleteIcon: {
        borderRadius: "5px",
    },
    row: (props: styleProp) => ({
        background: theme.palette.secondary.light,
        borderRadius: "5px",
    }),
    maxValInput: (props: styleProp) => {
        if (props.rangeState) {
            return {};
        } else {
            return {
                display: "none",
            };
        }
    },
}));

export interface IStaticRow {
    staticTableMetas: StaticTableMetas;
    tableOrder: number;
    modifier: StaticTablesModifier;
    index: number;
    rowOrder: number;
    minFee: number;
    maxFee?: number;
    rangeState: boolean;
    perState: boolean;
    description: string;
}

export const StaticRow = ({ index, staticTableMetas, tableOrder, rowOrder, modifier, minFee, maxFee, rangeState, perState, description }: IStaticRow) => {
    const cls = useStyles({ index, rangeState });
    const cellAlignment = "center";
    const { currencySymbol } = React.useContext(DashboardContext);

    return (
        <TableRow className={cls.row}>
            <TableCell align={cellAlignment}>
                <Button size="small" className={cls.deleteIcon} startIcon={<DeleteIcon />} onClick={() => modifier.delRow(staticTableMetas, tableOrder, rowOrder)}>
                    Delete
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    fullWidth
                    rows={2}
                    value={description}
                    label="Description"
                    color="primary"
                    onChange={(event) => {
                        modifier.setRowDescription(staticTableMetas, tableOrder, rowOrder, event.target.value);
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    variant="standard"
                    value={minFee}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setFeeMin(staticTableMetas, tableOrder, rowOrder, value);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    className={cls.maxValInput}
                    label="Amount"
                    variant="standard"
                    disabled={!rangeState}
                    value={maxFee}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    outputFormat="number"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onChange={(event: any, value: number) => {
                        if (value !== undefined) {
                            modifier.setFeeMax(staticTableMetas, tableOrder, rowOrder, value);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <Button
                    startIcon={rangeState ? <ChevronLeftIcon /> : <RemoveIcon />}
                    endIcon={rangeState ? <ChevronRightIcon /> : null}
                    variant="contained"
                    color={rangeState ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.changeRange(staticTableMetas, tableOrder, rowOrder);
                    }}
                >
                    {rangeState ? "Range" : "Single"}
                </Button>
            </TableCell>
            <TableCell align={cellAlignment}>
                <Button
                    startIcon={perState ? <GroupAddIcon /> : <PeopleAltIcon />}
                    variant="contained"
                    color={perState ? "primary" : "secondary"}
                    onClick={() => {
                        modifier.changePer(staticTableMetas, tableOrder, rowOrder);
                    }}
                >
                    {perState ? "Per Individual" : "Static Fee"}
                </Button>
            </TableCell>

            <TableCell align={cellAlignment}>
                {!modifier.isRowFirstPosition(rowOrder) && <ArrowDropUpIcon className={cls.largeicon} onClick={() => modifier.shiftRowUp(staticTableMetas, tableOrder, rowOrder)} />}
                {!modifier.isRowLastPosition(staticTableMetas, tableOrder, rowOrder) && (
                    <ArrowDropDownIcon className={cls.largeicon} onClick={() => modifier.shiftRowDown(staticTableMetas, tableOrder, rowOrder)} />
                )}
            </TableCell>
        </TableRow>
    );
};
