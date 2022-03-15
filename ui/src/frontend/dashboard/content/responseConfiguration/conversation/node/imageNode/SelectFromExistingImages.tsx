import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { SetState, FileAssetResource } from "@Palavyr-Types";
import React, { useCallback, useEffect, useState } from "react";

export interface SelectFromExistingImagesProps {
    repository: PalavyrRepository;
    nodeId: string;
    currentFileAssetId: string;
    setFileAssetLink: SetState<string>;
    setFileAssetName: SetState<string>;
    setFileAssetId: (fileId: string) => void;
}

export const SelectFromExistingImages = ({ setFileAssetId, repository, nodeId, currentFileAssetId, setFileAssetLink, setFileAssetName }: SelectFromExistingImagesProps) => {
    const [options, setOptions] = useState<FileAssetResource[] | null>(null);
    const [label, setLabel] = useState<string>("");

    const onChange = async (_: any, option: FileAssetResource) => {
        await repository.Configuration.FileAssets.LinkFileAssetToNode(option.fileId, nodeId);

        setFileAssetLink(option.link);
        setFileAssetName(option.fileName);
        setLabel(option.fileName);
        setFileAssetId(option.fileId);
    };

    const groupGetter = (val: FileAssetResource) => val.fileName;

    const setfilteredFileAssetOptions = (fileAssets: FileAssetResource[]) => {
        const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileAssets);
        const filteredOptions = sortedOptions.filter((link: FileAssetResource) => {
            return link.fileId !== currentFileAssetId;
        });
        setOptions(filteredOptions);
    };

    const loadOptions = useCallback(async () => {
        const fileAssets = await repository.Configuration.FileAssets.GetFileAssets();
        setfilteredFileAssetOptions(fileAssets);
    }, [currentFileAssetId]);

    useEffect(() => {
        loadOptions();
    }, []);

    return (
        <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
            {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={option => option.fileName} />}
        </PalavyrAccordian>
    );
};
