import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { CircularProgress, makeStyles, Typography } from "@material-ui/core";
import { Variant } from "@material-ui/core/styles/createTypography";
import { Align } from "@common/positioning/Align";
import React, { memo, useEffect, useState } from "react";

export interface FileAssetDisplayProps {
    fileAssetLink: string;
    fileAssetName: string;
    titleVariant?: Variant;
    fileAssetId: string;
}

const useStyles = makeStyles(theme => ({
    display: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
    },
}));

export const FileAssetDisplay = memo(({ fileAssetId, fileAssetName, fileAssetLink, titleVariant = "h6" }: FileAssetDisplayProps) => {
    const cls = useStyles();
    const [isLoading, setLoading] = useState<boolean>(true);
    const [bounce, setBounce] = useState<boolean>(false);
    const onImageClick = e => {
        e.preventDefault();
        window.open(fileAssetLink, "_blank");
    };

    useEffect(() => {
        setBounce(!bounce);
    }, [fileAssetId]);

    const renderSwitch = () => {
        const extension = fileAssetName.split(".")[0];

        switch (extension) {
            case "pdf":
                return (
                    <div style={{ width: "100%", height: "100vh" }}>
                        <object data={fileAssetLink} id="upload-preview" type="application/pdf" width="100%" height="100%" aria-label="preview"></object>
                    </div>
                );
            default:
                return (
                    <img
                        onClick={onImageClick}
                        className={cls.display}
                        key={Date.now()}
                        src={fileAssetLink}
                        onChange={() => setLoading(true)}
                        onLoadStart={() => setLoading(true)}
                        onLoad={() => setLoading(false)}
                    />
                );
        }
    };

    return (
        <>
            <Typography variant={titleVariant} align="center">
                {!isLoading && isNullOrUndefinedOrWhitespace(fileAssetLink) ? "No Image" : `${fileAssetName}`}
            </Typography>
            {isLoading && fileAssetLink && (
                <Align>
                    <CircularProgress style={{ padding: ".5rem", margin: "1rem" }} />
                </Align>
            )}
            <Align>
                <div style={{ visibility: isLoading ? "hidden" : "visible", maxWidth: "100px", margin: "1rem" }}>{renderSwitch()}</div>
            </Align>
        </>
    );
});
