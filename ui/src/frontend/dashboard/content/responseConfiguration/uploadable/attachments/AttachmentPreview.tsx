import React from "react";
import { Paper, Divider, makeStyles, Theme } from "@material-ui/core";
import { FileAssetResource } from "@common/types/api/EntityResources";

interface IAttachmentPreview {
    preview: FileAssetResource;
}

type StyleProps = {
    preview: boolean;
};

const useStyles = makeStyles((theme: Theme) => ({
    divStyle: (props: StyleProps) => ({
        alignContent: "center",
        padding: "2.5rem",
        height: props.preview ? "1200px" : "0px",
        backgroundColor: "#C7ECEE",
        background: "#C7ECEE",
        borderRadius: "0px",
    }),

    paper: {
        backgroundColor: "#C7ECEE",
        marginTop: "3rem",
        alignContent: "center",
        paddingTop: "2.5rem",
        paddingBottom: ".5rem",
        borderRadius: "0px",
        paddingLeft: "2.5rem",
    },
}));

export const AttachmentPreview = ({ preview }: IAttachmentPreview) => {
    const classes = useStyles({ preview: preview ? true : false });

    return (
        <>
            <Paper className={classes.paper}>
                <h2>Preview</h2>
                {preview.fileName}
            </Paper>
            <Divider />
            <Paper id="dashpaper" className={classes.divStyle}>
                <object data={preview.link} type="application/pdf" width="100%" height="100%" aria-label="preview"></object>
            </Paper>
        </>
    );
};
