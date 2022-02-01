import { DashboardContext } from "@frontend/dashboard/layouts/DashboardContext";
import React from "react";
import { NumberFormatValues } from "react-number-format";
import { CurrencyTextField } from "@common/components/borrowed/CurrentTextField";
import { UnitIds, UnitTypes } from "@Palavyr-Types";
import { FormControl, FormHelperText, Input, InputAdornment, makeStyles } from "@material-ui/core";
import classNames from "classnames";

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
    unitType: UnitTypes;
    unitId: UnitIds;
    unitHelperText?: string;
}

export const UnitInput = ({ label, value, disabled, onBlur, onChange, onCurrencyChange, unitType, unitId, unitHelperText }: UnitInputProps) => {
    const { currencySymbol } = React.useContext(DashboardContext);
    const cls = useStyles();

    if (unitType === UnitTypes.Currency && onCurrencyChange === undefined) {
        throw new Error("UnitInput onCurrencyChange is undefined");
    }

    if (unitType !== UnitTypes.Currency && onChange === undefined) {
        throw new Error("UnitInput onChange is undefined");
    }

    switch (unitType) {
        case UnitTypes.Currency:
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
                    <Input value={value} onChange={onChange} endAdornment={<InputAdornment position="end">Kg</InputAdornment>} />
                    <FormHelperText>{unitHelperText}</FormHelperText>
                </FormControl>
            );
    }
};
