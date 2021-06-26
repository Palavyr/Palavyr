import { makeStyles } from "@material-ui/core";

type StyleProps = {
    nodeText: string;
    nodeType: string;
    checked: boolean;
    isDecendentOfSplitMerge: boolean;
    splitMergeRootSiblingIndex: number;
    debugOn: boolean;
    isImageNode: boolean;
};

export const useNodeInterfaceStyles = makeStyles((theme) => ({
    root: (props: StyleProps) => ({
        minWidth: "275px",
        maxWidth: props.debugOn ? "600px" : "250px",
        minHeight: "350px",
        borderColor: props.nodeType === "" ? "red" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "purple" : "#54585A",
        borderWidth: props.nodeType === "" ? "5px" : props.isDecendentOfSplitMerge && props.splitMergeRootSiblingIndex > 0 ? "8px" : "2px",
        borderRadius: "3px",
        backgroundColor: theme.palette.success.light,
    }),
    card: {
        display: "flex",
        flexDirection: "column",
        justifyContent: "area",
    },
    bullet: {
        display: "inline-block",
        margin: "0 2px",
        transform: "scale(0.8)",
    },
    title: {
        fontSize: 14,
    },
    pos: {
        marginBottom: 12,
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
    text: {
        margin: ".1rem",
    },
    formstyle: {
        fontSize: "12px",
        alignSelf: "bottom",
    },
    editorStyle: {
        fontSize: "12px",
        color: "lightgray",
    },
    formLabelStyle: (props: StyleProps) => ({
        fontSize: "12px",
        color: props.checked ? "black" : "gray",
    }),
    interfaceElement: {
        paddingBottom: "1rem",
    },
    imageBlock: {
        padding: "1rem",
        marginBottom: "0.5rem",
        marginTop: "0.5rem",
    },
}));
