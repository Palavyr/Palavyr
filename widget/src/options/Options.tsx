import * as React from "react";
import { useLocation } from "react-router-dom";
import { useState, useCallback, useEffect } from "react";
import { DropdownListOptions } from "./optionFormats/DropdownOptionsList";
import { SelectedOption, WidgetPreferences, AreaTable } from "@Palavyr-Types";
import { PalavyrWidgetRepository } from "client/PalavyrWidgetRepository";
import { makeStyles } from "@material-ui/core";
import { BrandingStrip } from "common/BrandingStrip";

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
