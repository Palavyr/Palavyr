import React, { useState, useEffect, useContext } from "react";
import { FileLink } from "@Palavyr-Types";
import { makeStyles, Paper } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Align } from "dashboard/layouts/positioning/Align";

const useStyles = makeStyles((theme) => ({
    paper: (preview: boolean) => ({
        backgroundColor: theme.palette.secondary.light,
        alignContent: "center",
        padding: "2.5rem",
        height: preview ? "1200px" : "0px",
        borderRadius: "0px",
    }),
}));

export const ConfigurationPreview = () => {
    const { repository, setIsLoading } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [preview, setPreview] = useState<FileLink>();
    const classes = useStyles(preview ? true : false);
    const [update, setUpdate] = useState<boolean>(false);

    const loadPreview = React.useCallback(async () => {
        const fileLink = await repository.Configuration.Preview.fetchPreview(areaIdentifier);
        setPreview(fileLink);
        setIsLoading(true);
    }, [areaIdentifier]);

    const reload = () => {
        loadPreview();
    };

    useEffect(() => {
        loadPreview();
        setIsLoading(true);
    }, [areaIdentifier, loadPreview]);

    return (
        <>
            <AreaConfigurationHeader title="Response PDF Preview" subtitle="Preview the response PDF that will be produced for this area." />
            <Align>
                <SinglePurposeButton size="small" buttonText="Reload" variant="contained" color="primary" onClick={() => reload()} />
            </Align>
            <Paper id="dashpaper" className={classes.paper}>
                {preview && <object onLoad={() => setIsLoading(false)} id="output-fram-id" data={preview.link} type="application/pdf" width="100%" height="100%" aria-label="preview"></object>}
            </Paper>
        </>
    );
};
