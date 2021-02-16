import * as React from "react";
import { SelectedOption, AreaTable, WidgetPreferences } from "../types";
import { useLocation } from "react-router-dom";
import CreateClient from "../client/Client";
import { useState, useCallback, useEffect } from "react";
import { DropdownListOptions } from "./optionFormats/DropdownOptionsList";
import { Dispatch } from "react";
import { SetStateAction } from "react";

interface IOptionSelector {
    setSelectedOption: (option: SelectedOption) => void;
    preferences: WidgetPreferences;
}

export const OptionSelector = ({ setSelectedOption, preferences}: IOptionSelector) => {
    var secretKey = new URLSearchParams(useLocation().search).get("key");
    const Client = CreateClient(secretKey);

    const [, setUseGroups] = useState<boolean>();
    const [options, setOptions] = useState<Array<SelectedOption>>();

    const loadAreas = useCallback(async () => {
        setUseGroups(false);

        var areas = await Client.Widget.Access.fetchAreas();
        var options = areas.data.map((area: AreaTable) => {
            return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier };
        });

        setOptions(options);
    }, []);

    useEffect(() => {
        loadAreas();
    }, [loadAreas]);

    return (
        <div style={{position: "fixed", display: "flex", flexDirection: "column", width: "100%", height: "100%"}}>
            <DropdownListOptions options={options} setSelectedOption={setSelectedOption} preferences={preferences} />
        </div>
    );
};
