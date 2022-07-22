import React from "react";
import { makeStyles, TextField, TableRow, TableCell } from "@material-ui/core";
import { StaticTableMetaResources } from "@Palavyr-Types";
import { StaticTablesModifier } from "./staticTableModifier";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import ArrowDropUpIcon from "@material-ui/icons/ArrowDropUp";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { NumberFormatValues } from "react-number-format";
import { TableDeleteButton } from "../PricingStrategies/tableComponents/PercentOfThresholdTable/TableDeleteButton";
import { TableButton } from "../PricingStrategies/tableComponents/CategorySelectTable/TableButton";

type styleProp = {
    index: number;
    rangeState: boolean;
};

const useStyles = makeStyles(theme => ({
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
    staticTableMetas: StaticTableMetaResources;
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
                <TableDeleteButton onClick={() => modifier.delRow(staticTableMetas, tableOrder, rowOrder)} />
            </TableCell>
            <TableCell align={cellAlignment}>
                <TextField
                    fullWidth
                    rows={2}
                    value={description}
                    label="Description"
                    color="primary"
                    onChange={event => {
                        modifier.setRowDescription(staticTableMetas, tableOrder, rowOrder, event.target.value);
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    label="Amount"
                    value={minFee}
                    currencySymbol={currencySymbol}
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onValueChange={(values: NumberFormatValues) => {
                        if (values.floatValue !== undefined) {
                            modifier.setFeeMin(staticTableMetas, tableOrder, rowOrder, values.floatValue);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <CurrencyTextField
                    className={cls.maxValInput}
                    label="Amount"
                    disabled={!rangeState}
                    value={maxFee}
                    currencySymbol={currencySymbol}
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onValueChange={(values: NumberFormatValues) => {
                        if (values.floatValue !== undefined) {
                            modifier.setFeeMax(staticTableMetas, tableOrder, rowOrder, values.floatValue);
                        }
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <TableButton
                    state={rangeState}
                    onClick={() => {
                        modifier.changeRange(staticTableMetas, tableOrder, rowOrder);
                    }}
                />
            </TableCell>
            <TableCell align={cellAlignment}>
                <TableButton
                    offMessage="Single Value"
                    onMessage="Per Person"
                    onClick={() => {
                        modifier.changePer(staticTableMetas, tableOrder, rowOrder);
                    }}
                    state={perState}
                />
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
