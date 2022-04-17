import React from "react";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";

export interface TextNodeFaceProps {
    openEditor: () => void;
    userText: string;
}

export const TextNodeFace = ({ openEditor, userText }: TextNodeFaceProps) => {
    return (
        <PalavyrNodeBody openEditor={openEditor} textCheck={userText} isFileAssetNode={false}>
            <div dangerouslySetInnerHTML={{ __html: userText }}></div>
        </PalavyrNodeBody>
    );
};
