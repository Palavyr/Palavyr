import { CircularProgress, Grid, makeStyles, Paper, Typography } from "@material-ui/core";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { FileAssetUpload } from "./FileAssetUpload";
import { Align } from "@common/positioning/Align";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { FileAssetResource } from "@Palavyr-Types";
import { FileAssetTables } from "./FileAssetTables";

const useStyles = makeStyles(theme => ({
    display: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export type FileDetails = {
    extension: string;
    link: string;
};

export const FileAssetReview = () => {
    const cls = useStyles();

    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Uploads");

    const [fileAssetResources, setFileAssetRecords] = useState<FileAssetResource[] | null>(null);
    const [showSpinner, setShowSpinner] = useState<boolean>(false);

    const [currentPreview, setCurrentPreview] = useState<FileDetails>(null!);

    const loadFileAssetRecords = useCallback(async () => {
        const fileAssets = await repository.Configuration.FileAssets.GetFileAssets();
        setFileAssetRecords(fileAssets);
    }, []);

    useEffect(() => {
        loadFileAssetRecords();
    }, [loadFileAssetRecords]);

    const onFileAssetClick = e => {
        e.preventDefault();
        window.open(currentPreview.link, "_blank");
    };

    const renderSwitch = (current: FileDetails) => {
        const { extension, link } = current;

        switch (extension) {
            case "pdf":
                return (
                    <div style={{ width: "100%", height: "100vh" }}>
                        <object data={link} id="upload-preview" type="application/pdf" width="100%" height="100%" aria-label="preview"></object>;
                    </div>
                );
            default:
                return (
                    <img
                        onClick={onFileAssetClick}
                        className={cls.display}
                        key={Date.now()}
                        src={current.link}
                        onChange={() => setShowSpinner(true)}
                        onLoadStart={() => setShowSpinner(true)}
                        onLoad={() => setShowSpinner(false)}
                    />
                );
        }
    };

    return (
        <div style={{ marginBottom: "5rem" }}>
            <div>
                <HeaderStrip title="Review the files you've uploaded" subtitle="Add or remove stored files. These are accessible within the Palavy designer." />
            </div>
            <FileAssetUpload setFileAssets={setFileAssetRecords} numImages={fileAssetResources === undefined || fileAssetResources === null ? 1 : fileAssetResources.length} />
            <Grid container style={{ width: "100%" }}>
                <Grid item xs={6}>
                    <FileAssetTables fileAssetResources={fileAssetResources ?? []} setFileAssetResourceRecords={setFileAssetRecords} setCurrentPreview={setCurrentPreview} setShowSpinner={setShowSpinner} />
                </Grid>
                <Grid item xs={6}>
                    {!currentPreview && <HeaderStrip title="No preview selected" />}
                    {showSpinner && (
                        <Align>
                            <CircularProgress style={{ padding: ".5rem", margin: "1rem" }} />
                        </Align>
                    )}
                    <div style={{ visibility: showSpinner ? "hidden" : "visible", maxWidth: "100%", minHeight: "100%", margin: "1rem", display: "flex", justifyContent: "center" }}>
                        {currentPreview && renderSwitch(currentPreview)}
                    </div>
                </Grid>
            </Grid>
        </div>
    );
};
