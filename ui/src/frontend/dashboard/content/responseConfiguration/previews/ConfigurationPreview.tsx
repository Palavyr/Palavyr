import React, { useState, useEffect, useContext } from "react";
import { FileAssetResource } from "@common/types/api/EntityResources";
import { CircularProgress, makeStyles, Paper } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";


type StyleProps = {
    preview: boolean;
};


const useStyles = makeStyles<{}>((theme: any) => ({
    paper: (props: StyleProps) => ({
        backgroundColor: theme.palette.secondary.light,
        alignContent: "center",
        padding: "2.5rem",
        marginLeft: "4rem",
        marginRight: "4rem",
        height: props.preview ? "120vh" : "auto",
        borderRadius: "0px",
    }),
    buttonWrap: {
        margin: "2rem",
    },
}));

export const ConfigurationPreview = () => {
    const { repository, setIsLoading } = useContext(DashboardContext);
    const { intentId } = useParams<{ intentId: string }>();
    const [localLoading, setLocalLoading] = useState<boolean>(true);

    const [preview, setPreview] = useState<FileAssetResource>();
    const cls = useStyles({ preview: preview ? true : false });

    const loadPreview = React.useCallback(async () => {
        const fileAssetResource = await repository.Configuration.Preview.FetchPreview(intentId);
        setPreview(fileAssetResource);
        setIsLoading(true);
    }, [intentId]);

    const reload = () => {
        loadPreview();
    };

    useEffect(() => {
        loadPreview();
        setIsLoading(true);
    }, [intentId, loadPreview]);

    const onLoad = () => {
        setIsLoading(false);
        setLocalLoading(false);
    };

    return (
        <>
            <HeaderStrip title="Response PDF Preview" subtitle="Preview the response PDF that will be produced for this intent." />
            {/* <Align>
                <div className={cls.buttonWrap}>
                    <SinglePurposeButton size="large" buttonText="Reload" variant="contained" color="primary" onClick={() => reload()} />
                </div>
            </Align> */}
            <Paper id="dashpaper" className={cls.paper}>
                {preview && <object onLoad={onLoad} id="output-fram-id" data={`${preview.link}#zoom=50`} type="application/pdf" width="100%" height="100%" aria-label="preview"></object>}
                {localLoading && (
                    <div style={{ width: "100%", display: "flex", justifyContent: "center", margin: "1rem" }}>
                        <CircularProgress />
                    </div>
                )}
            </Paper>
        </>
    );
};
