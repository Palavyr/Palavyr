import React from "react";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";

export interface TextNodeFaceProps {
    openEditor: () => void;
    userText: string;
}

export const TextNodeFace = ({ openEditor, userText }: TextNodeFaceProps) => {
    const cls = useNodeInterfaceStyles({
        nodeText: userText,
        isFileAssetNode: false,
    });
    return (
        <PalavyrNodeBody openEditor={openEditor} textCheck={userText} isFileAssetNode={false}>
            <div dangerouslySetInnerHTML={{ __html: userText }}></div>
        </PalavyrNodeBody>
    );
};
