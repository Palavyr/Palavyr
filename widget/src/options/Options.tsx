import * as React from 'react';
import { SelectedOption, AreaTable, WidgetPreferences } from '../types';
import { useLocation } from 'react-router-dom';
import CreateClient from '../client/Client';
import { CaroselOptions } from './optionFormats/CaroselOptions';
import { useState, useCallback, useEffect } from 'react';
import GroupedOptions from './optionFormats/GroupedOptions';

interface IOptionSelector {
    setSelectedOption: (option: SelectedOption) => void;
    preferences: WidgetPreferences;
}

export const OptionSelector = ({ setSelectedOption, preferences }: IOptionSelector) => {

    var secretKey = (new URLSearchParams(useLocation().search)).get("key")
    const Client = CreateClient(secretKey);

    const [useGroups, setUseGroups] = useState<boolean>();
    const [options, setOptions] = useState<Array<SelectedOption>>();

    const loadPreference = useCallback(async () => {
        // var prefs = (await Client.Widget.Access.fetchPreferences()).data as WidgetPreferences;
        // setUseGroups(Use.data.shouldGroup); // TODO: check
        setUseGroups(false);

        var areas = await Client.Widget.Access.fetchAreas();
        var options = areas.data.map((area: AreaTable) => {
            return { areaDisplay: area.areaDisplayTitle, areaId: area.areaIdentifier }
        })

        setOptions(options);

    }, [])

    useEffect(() => {
        loadPreference();
    }, [loadPreference])

    return useGroups ? <GroupedOptions /> : <CaroselOptions options={options} setSelectedOption={setSelectedOption} preferences={preferences} />;
}