import { DEFAULT_NODE_TEXT_LIST } from "@constants";
import { Card, makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

type StyleProps = {
    nodeText?: string;
    isFileAssetNode?: boolean;
};

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    interfaceElement: {
        paddingBottom: "1rem",
    },
    textCard: (props: StyleProps) => ({
        border: "1px solid gray",
        padding: "10px",
        textAlign: "center",
        color: DEFAULT_NODE_TEXT_LIST.includes(props.nodeText ?? "") && !props.isFileAssetNode ? "black" : "black",
        background: DEFAULT_NODE_TEXT_LIST.includes(props.nodeText ?? "") && !props.isFileAssetNode ? theme.palette.warning.main : "white",
        "&:hover": {
            background: "lightgray",
            color: "black",
            cursor: "pointer",
        },
    }),

    editorStyle: {
        fontSize: "12px",
        color: "darkgray",
    },
}));

interface PalvyrNodeBodyProps {
    children: React.ReactNode;
    textCheck?: string;
    isFileAssetNode?: boolean;
    openEditor(): void;
}
export const PalavyrNodeBody = ({ openEditor, children, textCheck, isFileAssetNode }: PalvyrNodeBodyProps) => {
    const cls = useStyles({
        nodeText: textCheck,
        isFileAssetNode,
    });
    return (
        <Card elevation={0} className={classNames(cls.interfaceElement, cls.textCard)} onClick={openEditor}>
            {children}
            <Typography align="center" className={cls.editorStyle} onClick={openEditor}>
                Click to Edit
            </Typography>
        </Card>
    );
};
