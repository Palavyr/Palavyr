import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { CircularProgress, Grid, makeStyles, TableContainer } from "@material-ui/core";
import { FileLink } from "@Palavyr-Types";
import React, { useCallback, useEffect, useState } from "react";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ImageRecordTableHeader } from "./ImageRecordTableHeader";
import { ImageRecordTableBody } from "./ImageRecordTableBody";
import { ImageReviewUpload } from "./ImageReviewUpload";
import { Align } from "dashboard/layouts/positioning/Align";

const useStyles = makeStyles((theme) => ({
    image: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export const ImageReview = () => {
    const cls = useStyles();

    const repository = new PalavyrRepository();
    const [imageRecords, setImageRecords] = useState<FileLink[] | null>(null);
    const [showSpinner, setShowSpinner] = useState<boolean>(false);

    const [currentPreview, setCurrentPreview] = useState<string>("");

    const loadImageRecords = useCallback(async () => {
        const fileLinks = await repository.Configuration.Images.getImages();
        setImageRecords(fileLinks);
    }, []);

    useEffect(() => {
        loadImageRecords();
    }, []);

    const onImageClick = (e) => {
        e.preventDefault();
        window.open(currentPreview, "_blank");
    };

    return (
        <div style={{ marginBottom: "5rem" }}>
            <div>
                <AreaConfigurationHeader title="Review the images you've uploaded" subtitle="Add or remove stored images. These are accessible within the Palavy designer." />
            </div>
            <ImageReviewUpload setImageRecords={setImageRecords} />
            <Grid container style={{ width: "100%" }}>
                <Grid item xs={6}>
                    <TableContainer style={{ width: "100%", paddingLeft: "1rem", paddingRight: "1rem" }}>
                        <ImageRecordTableHeader />
                        {imageRecords && <ImageRecordTableBody imageRecords={imageRecords} setImageRecords={setImageRecords} setCurrentPreview={setCurrentPreview} setShowSpinner={setShowSpinner} />}
                    </TableContainer>
                </Grid>
                <Grid item xs={6}>
                    {!currentPreview && <AreaConfigurationHeader title="No preview selected" />}
                    {showSpinner && (
                        <Align>
                            <CircularProgress style={{ padding: ".5rem", margin: "1rem" }} />
                        </Align>
                    )}
                    <div style={{ visibility: showSpinner ? "hidden" : "visible", maxWidth: "100%", margin: "1rem", display: "flex", justifyContent: "center" }}>
                        {currentPreview && <img onClick={onImageClick} className={cls.image} key={Date.now()} src={currentPreview} onChange={() => setShowSpinner(true)} onLoadStart={() => setShowSpinner(true)} onLoad={() => setShowSpinner(false)} />}
                    </div>
                </Grid>
            </Grid>
        </div>
    );
};
