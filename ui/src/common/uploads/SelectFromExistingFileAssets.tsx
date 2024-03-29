import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { FileAssetResource } from "@common/types/api/EntityResources";
import React, { useCallback, useEffect, useState } from "react";

export interface SelectFromExistingImagesProps {
    repository: PalavyrRepository;
    onSelectChange: (_: any, option: FileAssetResource) => void;
    currentFileAssetId?: string | null;
    excludableFileAssets?: FileAssetResource[];
    disable?: boolean;
}

export const SelectFromExistingFileAssets = ({ repository, disable, onSelectChange, currentFileAssetId, excludableFileAssets }: SelectFromExistingImagesProps) => {
    const [options, setOptions] = useState<FileAssetResource[] | null>(null);

    const groupGetter = (val: FileAssetResource) => {
        return val.fileName.split(".").pop()!;
    };

    const setfilteredFileAssetOptions = (fileAssets: FileAssetResource[]) => {
        const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileAssets);
        const filteredOptions = sortedOptions.filter((asset: FileAssetResource) => {
            if (excludableFileAssets) {
                const alreadyPresent = excludableFileAssets.map(x => x.fileId);
                if (alreadyPresent.includes(asset.fileId)) {
                    return true;
                }
            }
            if (currentFileAssetId) {
                return asset.fileId !== currentFileAssetId;
            }
            return true;
        });

        return filteredOptions;
    };

    const loadOptions = useCallback(async () => {
        const fileAssets = await repository.Configuration.FileAssets.GetFileAssets();
        const filteredOptions = setfilteredFileAssetOptions(fileAssets);
        setOptions(filteredOptions);
    }, [currentFileAssetId]);

    useEffect(() => {
        loadOptions();
    }, [loadOptions, currentFileAssetId]);

    const groupby = (option: FileAssetResource) => {
        const extension = option.fileName.split(".").pop() ?? "";
        return extension;
    };

    const getOptionSelected = (option: FileAssetResource, value: FileAssetResource) => option.fileId === value.fileId;

    return (
        <>
            <PalavyrAccordian title="Select from your files" disable={disable} initialState={false}>
                {options && (
                    <PalavyrAutoComplete<FileAssetResource>
                        groupBy={groupby}
                        options={options}
                        label=""
                        disabled={false}
                        onChange={onSelectChange}
                        getOptionLabel={(option: FileAssetResource) => option.fileName}
                        getOptionSelected={getOptionSelected}
                        renderInput={params => <PalavyrText {...params} variant="standard" />}
                    />
                )}
            </PalavyrAccordian>
            {disable && (
                <PalavyrText display="block">
                    <strong>Upgrade your subscription to enable file selection</strong>
                </PalavyrText>
            )}
        </>
    );
};
