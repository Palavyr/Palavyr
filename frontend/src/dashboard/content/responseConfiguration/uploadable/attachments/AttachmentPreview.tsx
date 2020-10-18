
/// Example working pdf url
/// http://localhost:5000/user-1/abc-123/attachments/PaulGradie_medicare_exemption_base_ms015-1807en-f-Partial.pdf
///

import { FileLink } from "@Palavyr-Types"
import React from "react"
import { Paper, Divider, makeStyles } from "@material-ui/core"
import { Statement } from "@common/components/Statement"
import { AttachmentsHelp } from "dashboard/content/help/AttachmentsHelp"

interface IAttachmentPreview {
    preview: FileLink;
}

const useStyles = makeStyles(theme => ({
    divStyle: (preview: boolean) => ({
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        backgroundColor: "#C7ECEE",
        background: "#C7ECEE",
        borderRadius: "0px"
    }),

    paper: {
        backgroundColor: "#C7ECEE",
        marginTop: "3rem",
        alignContent: "center",
        paddingTop: "2.5rem",
        paddingBottom: ".5rem",
        borderRadius: "0px",
        paddingLeft: "2.5rem"
    }
}))

export const AttachmentPreview = ({ preview }: IAttachmentPreview) => {

    const classes = useStyles(preview ? true : false);

    return (
        <>
            <AttachmentsHelp />
            <Paper className={classes.paper}>
                <h2>Preview</h2>
                {preview.fileName}
            </Paper>
            <Divider />
            <Paper id="dashpaper" className={classes.divStyle}>
                <object
                    data={preview.link}
                    type="application/pdf"
                    width="100%"
                    height="100%"
                    aria-label={"preview"}
                >
                </object>
            </Paper>
        </>
    )
}