import { makeStyles, TextField } from "@material-ui/core";
import classNames from "classnames";
import React, { useEffect } from "react";
import CurrencyFormat, { NumberFormatProps, NumberFormatValues } from "react-number-format";

const useStyles = makeStyles(theme => ({
    styles: {
        // padding: "40px",
    },
}));
export interface CurrencyTextFieldProps extends NumberFormatProps {
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
    id,
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
    ...rest
}: CurrencyTextFieldProps) => {
    const cls = useStyles();

    return (
        <CurrencyFormat
            id={id}
            fixedDecimalScale={true}
            className={classNames(className, cls.styles)}
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
            {...rest}
        />
    );
};
