import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { PalavyrAccordian } from "@common/components/PalavyrAccordian";
import { PalavyrAutoComplete } from "@common/components/PalavyrAutoComplete";
import { sortByPropertyAlphabetical } from "@common/utils/sorting";
import { ConvoNode, FileLink, SetState } from "@Palavyr-Types";
import { ConversationTreeContext } from "dashboard/layouts/DashboardContext";
import { findIndex } from "lodash";
import React, { useCallback, useContext, useEffect, useState } from "react";

export interface SelectFromExistingImagesProps {
    node: ConvoNode;
    setImageName: SetState<string>;
    setImageLink: SetState<string>;
    currentImageId: string;
}
export const SelectFromExistingImages = ({ node, setImageName, setImageLink, currentImageId }: SelectFromExistingImagesProps) => {
    const repository = new PalavyrRepository();
    const { nodeList, setNodes } = useContext(ConversationTreeContext);

    const [options, setOptions] = useState<FileLink[] | null>(null);
    const [label, setLabel] = useState<string>("");

    const onChange = async (_: any, option: FileLink) => {
        // set the imageId to the node
        if (option === null) {
            console.log("OPTIONS WERE NULL WHAT");
        }
        if (node === null) {
            console.log("NODE WAS NULL");
        }

        if (node === null || option === null) {
            return;
        }

        const convoNode = await repository.Configuration.Images.savePreExistingImage(option.fileId, node.nodeId);
        setLabel(option.fileName);
        const nodeIndex = findIndex(nodeList, (n: ConvoNode) => n.nodeId == convoNode.nodeId);
        nodeList[nodeIndex].imageId = option.fileId;

        if (!option.isUrl) {
            const presignedUrl = await repository.Configuration.Images.getSignedUrl(option.link);
            setImageLink(presignedUrl);
            setImageName(option.fileName);
        }
        setNodes(nodeList);
    };

    const groupGetter = (val: FileLink) => val.fileName;

    const loadOptions = useCallback(async () => {
        const fileLinks = await repository.Configuration.Images.getImages();
        const sortedOptions = sortByPropertyAlphabetical(groupGetter, fileLinks);
        const filteredOptions = sortedOptions.filter((l: FileLink) => {
            return l.fileId !== currentImageId;
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
