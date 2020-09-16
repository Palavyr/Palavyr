
/// Example working pdf url
/// http://localhost:5000/user-1/abc-123/attachments/PaulGradie_medicare_exemption_base_ms015-1807en-f-Partial.pdf
///

import { FileLink } from "@Palavyr-Types"
import React from "react"
import { Paper, Divider } from "@material-ui/core"

interface IAttachmentPreview {
    preview: FileLink;
}

export const AttachmentPreview = ({ preview }: IAttachmentPreview) => {

    const divStyle = {
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        borderRadius: "0px"
    }

    return (
        <>
            <Paper style={{
                marginTop: "3rem",
                alignContent: "center",
                paddingTop: "2.5rem",
                paddingBottom: ".5rem",
                borderRadius: "0px",
                paddingLeft: "2.5rem"

            }}>
                <h2>Preview</h2>
                {preview.fileName}
            </Paper>
            <Divider/>
            <Paper id="dashpaper" style={divStyle}>
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