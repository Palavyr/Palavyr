import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { PalavyrLinkedList } from "dashboard/content/responseConfiguration/conversation/PalavyrDataStructure/PalavyrLinkedList";
import React, { useCallback, useEffect, useState } from "react";

export interface SelectFromExistingImagesProps {
    node?: ConvoNode;
    container: PalavyrLinkedList;
    nodeId: string;
    setImageName: SetState<string>;
    setImageLink: SetState<string>;
    currentImageId: string;
}

export const SelectFromExistingImages = ({ container, node, nodeId, setImageName, setImageLink, currentImageId }: SelectFromExistingImagesProps) => {
    const repository = new PalavyrRepository();

    const [options, setOptions] = useState<FileLink[] | null>(null);
    const [label, setLabel] = useState<string>("");

    const onChange = async (_: any, option: FileLink) => {
        if (node === null || option === null) {
            return;
        }

        const convoNode = await repository.Configuration.Images.savePreExistingImage(option.fileId, nodeId);
        setLabel(option.fileName);
    };

    const groupGetter = (val: FileLink) => val.fileName;

    const loadOptions = useCallback(async () => {
        const fileLinks = await repository.Configuration.Images.getImages();
        const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
        const filteredOptions = sortedOptions.filter((link: FileLink) => {
            return link.fileId !== currentImageId;
        });
        setOptions(filteredOptions);
    }, [currentImageId]);

    useEffect(() => {
        loadOptions();
    }, [currentImageId]);

    return (
        <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
            {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
        </PalavyrAccordian>
    );
};
