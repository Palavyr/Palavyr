import { makeStyles, TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";
import { QuantUnitDefinition } from "@Palavyr-Types";
import React, { ChangeEvent } from "react";

export interface PricingStrategySelectorProps {
    pricingStrategySelection: string;
    tableOptions: string[];
    handleChange: (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: string) => void;
    getOptionLabel: (option: string) => string;
    disabled?: boolean;
    toolTipTitle?: string;
    helperText?: string;
}

const useStyles = makeStyles(theme => ({
    selector: {
        marginLeft: "1rem",
        marginRight: "1rem",
        marginBottom: "0.3rem",
    },
}));

export const PricingStrategySelector = ({ toolTipTitle, disabled, pricingStrategySelection, getOptionLabel, handleChange, tableOptions, helperText }: PricingStrategySelectorProps) => {
    const cls = useStyles();
    return (
        <div className={cls.selector}>
            {pricingStrategySelection && (
                <Autocomplete
                    disableClearable
                    disabled={disabled}
                    value={pricingStrategySelection}
                    options={tableOptions}
                    getOptionLabel={getOptionLabel}
                    onChange={handleChange}
                    style={{ width: 300, marginTop: "1rem" }}
                    renderInput={params => <TextField {...params} label={helperText} variant="standard" />}
                />
            )}
        </div>
    );
};

export interface PricingStrategyAutoProps {
    selection: QuantUnitDefinition;
    options: Array<QuantUnitDefinition>;
    handleChange: (event: ChangeEvent<{ name?: string | undefined; value: unknown }>, value: QuantUnitDefinition) => void;
    disabled?: boolean;
    toolTipTitle?: string;
    helperText?: string;
    groupBy: (option: QuantUnitDefinition) => string;
    getOptionLabel: (option: QuantUnitDefinition) => string;
}

export const UnitSelector = ({ disabled, selection, handleChange, options, getOptionLabel, groupBy, helperText }: PricingStrategyAutoProps) => {
    const cls = useStyles();

    return (
        <div className={cls.selector}>
            {selection && (
                <Autocomplete
                    disableClearable
                    value={selection}
                    options={options}
                    getOptionLabel={getOptionLabel}
                    groupBy={groupBy}
                    onChange={handleChange}
                    style={{ width: 300, marginTop: "1rem" }}
                    renderInput={params => <TextField {...params} label="Unit selector" variant="standard" />}
                />
            )}
        </div>
    );
};
