import * as React from "react";
import { DropdownListOptions } from "../widgetcore/BotResponse/optionFormats/DropdownOptionsList";
import { SelectedOption } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";

interface IOptionSelector {
    options: SelectedOption[];
    onChange(event: any, newOption: SelectedOption): void;
    disabled: boolean;
}

const useStyles = makeStyles(theme => ({
    optionsContainer: {
        position: "fixed",
        display: "flex",
        flexDirection: "column",
        width: "100%",
        height: "100%",
    },
}));

export const OptionSelector = ({ disabled, options, onChange }: IOptionSelector) => {
    const cls = useStyles();

    return (
        <>
            <div className={cls.optionsContainer}>
                {options && <DropdownListOptions disabled={disabled} onChange={onChange} options={options} />}
                <BrandingStrip />
            </div>
        </>
    );
};
