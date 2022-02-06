import { DashboardContext } from "@frontend/dashboard/layouts/DashboardContext";
import React from "react";
import { NumberFormatValues } from "react-number-format";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { UnitGroups, UnitPrettyNames } from "@Palavyr-Types";
import { FormControl, FormHelperText, Input, InputAdornment, makeStyles } from "@material-ui/core";
import classNames from "classnames";
import { uuid } from "uuidv4";

const useStyles = makeStyles(theme => ({
    root: {
        display: "flex",
        flexWrap: "wrap",
    },
    margin: {
        margin: theme.spacing(1),
    },
    withoutLabel: {
        marginTop: theme.spacing(3),
    },
    textField: {
        width: "25ch",
    },
}));

export interface UnitInputProps {
    label: string;
    disabled: boolean;
    value: number;
    onBlur: (event?: React.FocusEvent<HTMLInputElement>) => void;
    onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
    onCurrencyChange?: (values: NumberFormatValues) => void;
    currencySymbol?: string;
    unitGroup: UnitGroups;
    unitPrettyName: UnitPrettyNames;
    unitHelperText?: string;
}

export const UnitInput = ({ label, value, disabled, onBlur, onChange, onCurrencyChange, unitGroup, unitPrettyName, unitHelperText }: UnitInputProps) => {
    const { currencySymbol } = React.useContext(DashboardContext);
    const cls = useStyles();

    if (unitGroup === UnitGroups.Currency && onCurrencyChange === undefined) {
        throw new Error("UnitInput onCurrencyChange is undefined");
    }

    if (unitGroup !== UnitGroups.Currency && onChange === undefined) {
        throw new Error("UnitInput onChange is undefined");
    }

    switch (unitGroup) {
        case UnitGroups.Currency:
            return (
                <CurrencyTextField
                    disabled={disabled}
                    label={label}
                    value={value}
                    currencySymbol={currencySymbol}
                    minimumValue="0"
                    decimalCharacter="."
                    digitGroupSeparator=","
                    onBlur={onBlur}
                    onValueChange={onCurrencyChange}
                />
            );
        default:
            return (
                <FormControl className={classNames(cls.margin, cls.withoutLabel, cls.textField)}>
                    <Input disabled={disabled} value={value} onChange={onChange} endAdornment={<InputAdornment position="end">{unitPrettyName}</InputAdornment>} />
                    <FormHelperText>{unitHelperText}</FormHelperText>
                </FormControl>
            );
    }
};
