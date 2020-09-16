import { GroupRow, Groups } from "@Palavyr-Types";
import React, { useState } from "react";
import { SelectGroup } from "./SelectGroup";

interface INodeTypeSelector {
    areaIdentifier: string;
    groups: Array<GroupRow>;
    setModalState: (val: boolean) => void;
    setNodes: (val: Groups) => void;
}

export const GroupSelector = ({ areaIdentifier, groups, setModalState, setNodes}: INodeTypeSelector) => {
    const [option, setSelectedOption] = useState<string>('');
    const [selectedGroupId, setSelectedGroupId] = useState<string>('')

    const handleChange = async (event: React.ChangeEvent<{ name?: string | undefined; value: unknown }>) => {

        var groupId = event.target.value as string;
        var option = groups.filter(x => x.groupId === groupId).pop()?.groupName as string;
        setSelectedOption(option); // change option in curent node
        setSelectedGroupId(groupId);
    };

    return <SelectGroup
        areaIdentifier={areaIdentifier}
        onChange={handleChange}
        option={option}
        groups={groups}
        selectedGroupId={selectedGroupId}
        setModalState={setModalState}
        setNodes={setNodes}
    />;
};
