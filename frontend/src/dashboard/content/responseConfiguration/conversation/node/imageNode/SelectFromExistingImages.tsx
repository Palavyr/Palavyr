import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { SetState, FileLink } from "@Palavyr-Types";
import { SessionStorage } from "localStorage/sessionStorage";
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

export const SelectFromExistingImages = ({  setImageId, repository, nodeId, imageId, currentImageId, setImageLink, setImageName }: SelectFromExistingImagesProps) => {
    const [options, setOptions] = useState<FileLink[] | null>(null);
    const [label, setLabel] = useState<string>("");

    const onChange = async (_: any, option: FileLink) => {
        await repository.Configuration.Images.savePreExistingImage(option.fileId, nodeId);

        if (!option.isUrl) {
            const imageData = SessionStorage.getImageData(option.fileId);
            if (imageData !== null) {
                setImageLink(imageData.presignedUrl);
                setImageName(imageData.fileName);
            } else {
                const presignedUrl = await repository.Configuration.Images.getSignedUrl(option.link);
                setImageLink(presignedUrl);
                setImageName(option.fileName);
                SessionStorage.setImageData(option.fileId, presignedUrl, option.fileName, "");
            }
        }
        console.log(`Does this match current ID: ${option.fileId}`);
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
        SessionStorage.setFileLinks(fileLinks);
    }, [currentImageId]);

    useEffect(() => {
        const fileLinks = SessionStorage.getFileLinks();
        if (fileLinks === null) {
            loadOptions();
        } else {
            setfilteredFileLinkOptions(fileLinks);
        }
    }, []);

    return (
        <PalavyrAccordian title="Select a file you've already uploaded" initialState={false}>
            {options && <PalavyrAutoComplete label={label} options={options} shouldDisableSelect={false} onChange={onChange} getOptionLabel={(option) => option.fileName} />}
        </PalavyrAccordian>
    );
};
