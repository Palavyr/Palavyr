import { Card, makeStyles, Typography } from "@material-ui/core";
import { SetState } from "@Palavyr-Types";
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

interface INodeBody {
    children: React.ReactNode;
    setModalState: SetState<boolean>;
}

export const NodeBody = ({ setModalState, children }: INodeBody) => {
    const cls = useStyles();
    return (
        <Card elevation={0} className={classNames(cls.interfaceElement, cls.textCard)} onClick={() => setModalState(true)}>
            {/* <CustomImage imageName={imageName} imageLink={imageLink} titleVariant="body1" /> */}
            {children}
            <Typography align="center" className={cls.editorStyle} onClick={() => setModalState(true)}>
                Click to Edit
            </Typography>
        </Card>
    );
};
