import * as React from "react";
import { useLocation } from "react-router-dom";
import { useState, useCallback, useEffect } from "react";
import { DropdownListOptions } from "./optionFormats/DropdownOptionsList";
import { SelectedOption, WidgetPreferences, AreaTable } from "@Palavyr-Types";
import { WidgetClient } from "client/Client";
import { makeStyles } from "@material-ui/core";
import { BrandingStrip } from "common/BrandingStrip";

interface IOptionSelector {
    setSelectedOption: (option: SelectedOption) => void;
}

const useStyles = makeStyles(theme => ({
    optionsContainer: {
        position: "fixed",
        display: "flex",
        flexDirection: "column",
        width: "100%",
        height: "97%",
    },
}));

export const OptionSelector = ({ setSelectedOption }: IOptionSelector) => {
    var secretKey = new URLSearchParams(useLocation().search).get("key");
    const Client = new WidgetClient(secretKey);
    const cls = useStyles();

    const [, setUseGroups] = useState<boolean>();
    const [options, setOptions] = useState<Array<SelectedOption>>();

    const loadAreas = useCallback(async () => {
        setUseGroups(false);

        var areas = await Client.Widget.Get.Areas();
        var options = areas.data.map((area: AreaTable) => {
            return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier };
        });

        setOptions(options);
    }, []);

    useEffect(() => {
        loadAreas();
    }, [loadAreas]);

    return (
        <>
            <div className={cls.optionsContainer}>{options && <DropdownListOptions options={options} setSelectedOption={setSelectedOption} />}</div>
        </>
    );
};
