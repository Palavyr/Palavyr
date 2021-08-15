import React, { useEffect, useRef, useState } from "react";
import CurrencyFormat from "react-number-format";

export interface CurrencyTextFieldProps {
    currencySymbol: string;
    value?: number | string;
    type?: "text" | "number";
    variant: "standard";
    textAlign?: "right" | "left" | "center";
    maximumValue?: string | number;
    minimumValue: string | number;
    className?: string;
    label: string;
    disabled?: boolean;
    decimalCharacter: string;
    digitGroupSeparator: string;
    onChange: any;
    outputFormat: string;
    onBlur?(): void;

}

export const CurrencyTextField = ({ className, label, variant, disabled, value, currencySymbol, minimumValue, outputFormat, decimalCharacter, digitGroupSeparator, onChange }: CurrencyTextFieldProps) => {
    // const [currentValue, setCurrentValue] = useState<string | number>();
    // useEffect(() => {
    //     if (value) {
    //         setCurrentValue(value);
    //     }
    // }, []);

    return (
        <CurrencyFormat
            onChange={onChange}
            // className={className}
            // min={minimumValue}
            // disabled={disabled}
            value={value}
            displayType="text"
            decimalSeparator={decimalCharacter}
            thousandSeparator={digitGroupSeparator}
            prefix={currencySymbol}
        />
    );
};
