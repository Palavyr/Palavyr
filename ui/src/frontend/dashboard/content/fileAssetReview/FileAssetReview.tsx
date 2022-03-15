import { CircularProgress, Grid, makeStyles, Table, TableContainer } from "@material-ui/core";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { FileAssetRecordTableHeader } from "./FileAssetRecordTableHeader";
import { FileAssetRecordTableBody } from "./FileAssetRecordTableBody";
import { FileAssetUpload } from "./FileAssetUpload";
import { Align } from "@common/positioning/Align";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { FileAssetResource } from "@Palavyr-Types";

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

export const FileAssetReview = () => {
    const cls = useStyles();

    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Images");

    const [fileAssetRecords, setFileAssetRecords] = useState<FileAssetResource[] | null>(null);
    const [showSpinner, setShowSpinner] = useState<boolean>(false);

    const [currentPreview, setCurrentPreview] = useState<string>("");

    const loadFileAssetRecords = useCallback(async () => {
        const fileAssets = await repository.Configuration.FileAssets.GetFileAssets();
        setFileAssetRecords(fileAssets);
    }, []);

    useEffect(() => {
        loadFileAssetRecords();
    }, []);

    const onFileAssetClick = e => {
        e.preventDefault();
        window.open(currentPreview, "_blank");
    };

    return (
        <div style={{ marginBottom: "5rem" }}>
            <div>
                <HeaderStrip title="Review the files you've uploaded" subtitle="Add or remove stored files. These are accessible within the Palavy designer." />
            </div>
            <FileAssetUpload setFileAssets={setFileAssetRecords} numImages={fileAssetRecords === undefined || fileAssetRecords === null ? 1 : fileAssetRecords.length} />
            <Grid container style={{ width: "100%" }}>
                <Grid item xs={6}>
                    <TableContainer style={{ width: "100%", paddingLeft: "1rem", paddingRight: "1rem" }}>
                        <Table>
                            <FileAssetRecordTableHeader />
                            {fileAssetRecords && (
                                <FileAssetRecordTableBody
                                    fileAssetResources={fileAssetRecords}
                                    setFileAssetResourceRecord={setFileAssetRecords}
                                    setCurrentPreview={setCurrentPreview}
                                    setShowSpinner={setShowSpinner}
                                />
                            )}
                        </Table>
                    </TableContainer>
                </Grid>
                <Grid item xs={6}>
                    {!currentPreview && <HeaderStrip title="No preview selected" />}
                    {showSpinner && (
                        <Align>
                            <CircularProgress style={{ padding: ".5rem", margin: "1rem" }} />
                        </Align>
                    )}
                    <div style={{ visibility: showSpinner ? "hidden" : "visible", maxWidth: "100%", margin: "1rem", display: "flex", justifyContent: "center" }}>
                        {currentPreview && (
                            <img
                                onClick={onFileAssetClick}
                                className={cls.display}
                                key={Date.now()}
                                src={currentPreview}
                                onChange={() => setShowSpinner(true)}
                                onLoadStart={() => setShowSpinner(true)}
                                onLoad={() => setShowSpinner(false)}
                            />
                        )}
                    </div>
                </Grid>
            </Grid>
        </div>
    );
};
