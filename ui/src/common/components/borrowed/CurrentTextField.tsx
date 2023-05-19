import { makeStyles, TextField } from "@material-ui/core";
import classNames from "classnames";
import React, { useEffect } from "react";
// import CurrencyFormat, { NumberFormatProps, NumberFormatValues } from "react-number-format";
import { NumericFormat, NumericFormatProps, NumberFormatValues } from "react-number-format";

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    styles: {
        marginTop: "15px",
        width: "12ch"
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
        <NumericFormat
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
            decimalScale={2}
            onValueChange={onValueChange}
            {...rest}
        />
    );
};
