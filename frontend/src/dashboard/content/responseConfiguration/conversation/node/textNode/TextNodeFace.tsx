import { Typography } from "@material-ui/core";
import React from "react";
import { useNodeInterfaceStyles } from "../../nodeInterfaceStyles";
import { PalavyrNodeBody } from "../baseNode/PalavyrNodeBody";

export interface TextNodeFaceProps {
    openEditor: () => void;
    userText: string;
}

export const TextNodeFace = ({ openEditor, userText }: TextNodeFaceProps) => {
    const cls = useNodeInterfaceStyles();
    return (
        <PalavyrNodeBody openEditor={openEditor}>
            <Typography className={cls.text} variant="body2" component="span" noWrap={false}>
                {userText}
            </Typography>
        </PalavyrNodeBody>
    );
};
