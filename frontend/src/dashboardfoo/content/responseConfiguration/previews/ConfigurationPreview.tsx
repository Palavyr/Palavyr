import { ApiClient } from "@api-client/Client";
import React, { useState, useEffect } from "react";
import { FileLink } from "@Palavyr-Types";
import { Paper } from "@material-ui/core";


interface IConfigurationPreview {
    areaIdentifier: string;
}

export const ConfigurationPreview = ({ areaIdentifier }: IConfigurationPreview) => {
    var client = new ApiClient();

    const [preview, setPreview] = useState<FileLink>();

    const paperStyle = {
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        borderRadius: "0px"
    }
    const loadPreview = React.useCallback(async () => {
        var res = await client.Configuration.Preview.fetchPreview(areaIdentifier);
        setPreview(res.data);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        loadPreview();
    }, [areaIdentifier, loadPreview])

    console.log(preview?.link);

    return (

        <Paper id="dashpaper" style={paperStyle}>
            {
                preview &&
                <object
                    id="output-fram-id"
                    data={preview.link}
                    type="application/pdf"
                    width="100%"
                    height="100%"
                    aria-label={"preview"}
                >
                </object>
            }
        </Paper>

    )

}