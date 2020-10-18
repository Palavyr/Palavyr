import { ApiClient } from "@api-client/Client";
import React, { useState, useEffect } from "react";
import { FileLink } from "@Palavyr-Types";
import { makeStyles, Paper } from "@material-ui/core";
import { Statement } from "@common/components/Statement";
import { PreviewHelp } from "dashboard/content/help/PreviewHelp";


interface IConfigurationPreview {
    areaIdentifier: string;
}

const useStyles = makeStyles(theme => ({
    paper: (preview: boolean) => ({
        backgroundColor: "#C7ECEE",
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        borderRadius: "0px"
    })
}))

export const ConfigurationPreview = ({ areaIdentifier }: IConfigurationPreview) => {
    var client = new ApiClient();

    const [preview, setPreview] = useState<FileLink>();
    const [loaded, setLoaded] = useState<boolean>(false);

    const classes = useStyles(preview ? true : false);

    const loadPreview = React.useCallback(async () => {
        var res = await client.Configuration.Preview.fetchPreview(areaIdentifier);
        setPreview(res.data);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        loadPreview();
        setLoaded(true);
        return () => {
            setLoaded(false);
        }
    }, [areaIdentifier, loadPreview])

    return (
        <>
            <PreviewHelp />
            <Paper id="dashpaper" className={classes.paper} >
                {
                    loaded && preview &&
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
                {
                    // !loaded && <Spinner />
                }
            </Paper>
        </>
    )

}