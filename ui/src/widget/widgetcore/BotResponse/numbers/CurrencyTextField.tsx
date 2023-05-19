import { makeStyles, TextField } from "@material-ui/core";
import classNames from "classnames";
import React from "react";
import { NumericFormat, NumericFormatProps, NumberFormatValues } from "react-number-format";

const useStyles = makeStyles<{}>((theme: any) => ({
    styles: {
        padding: "40px",
    },
}));

export interface CurrencyTextFieldProps extends NumericFormatProps {
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
    ...rest
}: CurrencyTextFieldProps) => {
    const cls = useStyles();
    return (
        <NumericFormat
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
            // allowedDecimalSeparators={true}
            // allowEmptyFormatting={false}
            // customInput={TextField}
            decimalScale={2}
            // isNumericString={true}
            onValueChange={onValueChange}
            {...rest}
        />
    );
};
