import { ApiClient } from "@api-client/Client";
import React, { useState, useEffect } from "react";
import { FileLink } from "@Palavyr-Types";
import { makeStyles, Paper } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";

const MediaType = "application/pdf";

const useStyles = makeStyles(theme => ({
    paper: (preview: boolean) => ({
        backgroundColor: theme.palette.secondary.light,//"#C7ECEE",
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        borderRadius: "0px"
    })
}))

export const ConfigurationPreview = () => {
    var client = new ApiClient();
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const { setIsLoading } = React.useContext(DashboardContext);

    const [preview, setPreview] = useState<FileLink>();
    const [loaded, setLoaded] = useState<boolean>(false);

    const classes = useStyles(preview ? true : false);

    const loadPreview = React.useCallback(async () => {
        var { data: fileLink } = await client.Configuration.Preview.fetchPreview(areaIdentifier);
        setPreview(fileLink);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [areaIdentifier])

    useEffect(() => {
        setIsLoading(true);
        loadPreview();
        setLoaded(true);
        return () => {
            setLoaded(false);
        }
    }, [areaIdentifier, loadPreview])

    return (
        <>
            <AreaConfigurationHeader title="Response PDF Preview" subtitle="Preview the response PDF that will be produced for this area." />
            <Paper id="dashpaper" className={classes.paper} >
                {
                    loaded && preview &&
                    <object
                        id="output-fram-id"
                        data={preview.link}
                        type={MediaType}
                        width="100%"
                        height="100%"
                        aria-label="preview"
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