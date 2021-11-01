import { PalavyrRepository } from "@common/client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { SetState, FileLink } from "@Palavyr-Types";
import React, { useCallback, useEffect, useState } from "react";

export interface SelectFromExistingImagesProps {
    repository: PalavyrRepository;
    nodeId: string;
    imageId?: string | null;
    currentImageId: string;
    setImageLink: SetState<string>;
    setImageName: SetState<string>;
    setImageId: (imageId: string) => void;
}

export const SelectFromExistingImages = ({ setImageId, repository, nodeId, imageId, currentImageId, setImageLink, setImageName }: SelectFromExistingImagesProps) => {
    const [options, setOptions] = useState<FileLink[] | null>(null);
    const [label, setLabel] = useState<string>("");

    const onChange = async (_: any, option: FileLink) => {
        await repository.Configuration.Images.savePreExistingImage(option.fileId, nodeId);

        if (!option.isUrl) {
            const presignedUrl = await repository.Configuration.Images.getSignedUrl(option.s3Key, option.fileId); // need ot add an s3 key property here and use it to check the cache.
            setImageLink(presignedUrl);
            setImageName(option.fileName);
        }
        setLabel(option.fileName);
        setImageId(option.fileId);
    };

    const groupGetter = (val: FileLink) => val.fileName;

    const setfilteredFileLinkOptions = (fileLinks: FileLink[]) => {
        const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
        const filteredOptions = sortedOptions.filter((link: FileLink) => {
            return link.fileId !== currentImageId;
        });
        setOptions(filteredOptions);
    };

    const loadOptions = useCallback(async () => {
        const fileLinks = await repository.Configuration.Images.getImages();
        setfilteredFileLinkOptions(fileLinks);
    }, [currentImageId]);

    useEffect(() => {
        loadOptions();
    }, []);

    return (
        <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
            {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
        </PalavyrAccordian>
    );
};
