import { Card, makeStyles, Typography } from "@material-ui/core";
import classNames from "classnames";
import React from "react";

type StyleProps = {
    nodeText: string;
    isImageNode: boolean;
};

const useStyles = makeStyles((theme) => ({
    interfaceElement: {
        paddingBottom: "1rem",
    },
    textCard: (props: StyleProps) => ({
        border: "1px solid gray",
        padding: "10px",
        textAlign: "center",
        color: props.nodeText === "Ask your question!" && !props.isImageNode ? "white" : "black",
        background: props.nodeText === "Ask your question!" && !props.isImageNode ? "red" : "white",
        "&:hover": {
            background: "lightgray",
            color: "black",
        },
    }),

    editorStyle: {
        fontSize: "12px",
        color: "lightgray",
    },
}));

interface PalvyrNodeBodyProps {
    children: React.ReactNode;
    openEditor(): void;
}
export const PalavyrNodeBody = ({ openEditor, children }: PalvyrNodeBodyProps) => {
    const cls = useStyles();
    return (
        <Card elevation={0} className={classNames(cls.interfaceElement, cls.textCard)} onClick={openEditor}>
            {children}
            <Typography align="center" className={cls.editorStyle} onClick={openEditor}>
                Click to Edit
            </Typography>
        </Card>
    );
};
