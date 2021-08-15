import { TextField } from "@material-ui/core";
import React from "react";
import CurrencyFormat, { NumberFormatValues } from "react-number-format";

export interface CurrencyTextFieldProps {
    label: string;
    currencySymbol: string;
    value?: number | string;
    maximumValue?: string | number;
    minimumValue?: string | number;
    className?: string;
    disabled?: boolean;
    decimalCharacter: string;
    digitGroupSeparator: string;
    onBlur?(): void;
    onValueChange?: (values: NumberFormatValues) => void;
}

export const CurrencyTextField = ({
    className,
    label,
    disabled,
    value,
    currencySymbol,
    minimumValue = 0,
    maximumValue = 99999999999,
    decimalCharacter,
    digitGroupSeparator,
    onValueChange,
}: CurrencyTextFieldProps) => {
    return (
        <CurrencyFormat
            fixedDecimalScale={true}
            className={className}
            min={minimumValue}
            max={maximumValue}
            disabled={disabled}
            value={value}
            decimalSeparator={decimalCharacter}
            thousandSeparator={digitGroupSeparator}
            prefix={currencySymbol}
            allowLeadingZeros={false}
            allowNegative={false}
            allowEmptyFormatting={false}
            customInput={TextField}
            decimalScale={2}
            isNumericString={true}
            onValueChange={onValueChange}
        />
    );
};
