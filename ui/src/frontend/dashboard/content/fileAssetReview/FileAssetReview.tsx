import { CircularProgress, Grid, makeStyles } from "@material-ui/core";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { FileAssetUpload } from "./FileAssetUpload";
import { Align } from "@common/positioning/Align";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { FileAssetResource } from "@common/types/api/EntityResources";
import { FileAssetTables } from "./FileAssetTables";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";

type StyleProps = { showSpinner: boolean };

import { Theme } from "@material-ui/core";
const useStyles = makeStyles<{}>((theme: any) => ({
    display: {
        display: "flex",
        justifyContent: "center",
        borderRadius: "5px",
        "&:hover": {
            cursor: "pointer",
        },
    },
    image: (props: StyleProps) => ({
        textAlign: "center",
        visibility: props.showSpinner ? "hidden" : "visible",
        margin: "1rem",
        display: "flex",
        justifyContent: "center",
    }),
}));

export type FileDetails = {
    extension: string;
    link: string;
};

export const FileAssetReview = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Uploads");

    const [fileAssetResources, setFileAssetRecords] = useState<FileAssetResource[] | null>(null);
    const [showSpinner, setShowSpinner] = useState<boolean>(false);

    const cls = useStyles({ showSpinner });
    const [currentPreview, setCurrentPreview] = useState<FileDetails>(null!);

    const loadFileAssetRecords = useCallback(async () => {
        const fileAssets = await repository.Configuration.FileAssets.GetFileAssets();
        setFileAssetRecords(fileAssets);
    }, []);

    useEffect(() => {
        loadFileAssetRecords();
    }, [loadFileAssetRecords]);

    const renderSwitch = (current: FileDetails) => {
        const { extension, link } = current;

        switch (extension) {
            case "pdf":
                return (
                    <div style={{ width: "100%", height: "76vh" }}>
                        <object data={link} id="upload-preview" type="application/pdf" width="100%" height="100%" aria-label="preview"></object>;
                    </div>
                );
            default:
                return (
                    <ZoomImage
                        alt="Image"
                        className={cls.display}
                        key={Date.now()}
                        imgSrc={current.link}
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
                    <div className={cls.image}>{currentPreview && renderSwitch(currentPreview)}</div>
                </Grid>
            </Grid>
        </div>
    );
};
