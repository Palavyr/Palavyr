import React, { useState, useEffect, useContext } from "react";
import { FileLink } from "@Palavyr-Types";
import { makeStyles, Paper } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { Align } from "@common/positioning/Align";

type StyleProps = {
    preview: boolean;
};
const useStyles = makeStyles(theme => ({
    paper: (props: StyleProps) => ({
        backgroundColor: theme.palette.secondary.light,
        alignContent: "center",
        padding: "2.5rem",
        height: props.preview ? "1200px" : "0px",
        borderRadius: "0px",
    }),
    buttonWrap: {
        margin: "2rem",
    },
}));

export const ConfigurationPreview = () => {
    const { repository, setIsLoading } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();

    const [preview, setPreview] = useState<FileLink>();
    const cls = useStyles({ preview: preview ? true : false });

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
            <HeaderStrip title="Response PDF Preview" subtitle="Preview the response PDF that will be produced for this intent." />
            <Align>
                <div className={cls.buttonWrap}>
                    <SinglePurposeButton size="large" buttonText="Reload" variant="contained" color="primary" onClick={() => reload()} />
                </div>
            </Align>
            <Paper id="dashpaper" className={cls.paper}>
                {preview && <object onLoad={() => setIsLoading(false)} id="output-fram-id" data={preview.link} type="application/pdf" width="100%" height="100%" aria-label="preview"></object>}
            </Paper>
        </>
    );
};
