import React, { useState, useEffect, useContext } from "react";
import { FileAssetResource } from "@Palavyr-Types";
import { CircularProgress, makeStyles, Paper } from "@material-ui/core";
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
        height: props.preview ? "100vh" : "auto",
        borderRadius: "0px",
    }),
    buttonWrap: {
        margin: "2rem",
    },
}));

export const ConfigurationPreview = () => {
    const { repository, setIsLoading } = useContext(DashboardContext);
    const { areaIdentifier } = useParams<{ areaIdentifier: string }>();
    const [localLoading, setLocalLoading] = useState<boolean>(true);

    const [preview, setPreview] = useState<FileAssetResource>();
    const cls = useStyles({ preview: preview ? true : false });

    const loadPreview = React.useCallback(async () => {
        const fileAssetResource = await repository.Configuration.Preview.FetchPreview(areaIdentifier);
        setPreview(fileAssetResource);
        setIsLoading(true);
    }, [areaIdentifier]);

    const reload = () => {
        loadPreview();
    };

    useEffect(() => {
        loadPreview();
        setIsLoading(true);
    }, [areaIdentifier, loadPreview]);

    const onLoad = () => {
        setIsLoading(false);
        setLocalLoading(false);
    };

    return (
        <>
            <HeaderStrip title="Response PDF Preview" subtitle="Preview the response PDF that will be produced for this intent." />
            <Align>
                <div className={cls.buttonWrap}>
                    <SinglePurposeButton size="large" buttonText="Reload" variant="contained" color="primary" onClick={() => reload()} />
                </div>
            </Align>
            <Paper id="dashpaper" className={cls.paper}>
                {preview && <object onLoad={onLoad} id="output-fram-id" data={preview.link} type="application/pdf" width="100%" height="100%" aria-label="preview"></object>}
                {localLoading && (
                    <div style={{ width: "100%", display: "flex", justifyContent: "center", margin: "1rem" }}>
                        <CircularProgress />
                    </div>
                )}
            </Paper>
        </>
    );
};
