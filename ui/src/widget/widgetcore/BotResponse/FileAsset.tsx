import { CircularProgress, makeStyles } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { Align } from "@common/positioning/WidgetAlign";
import { FileDetails } from "@frontend/dashboard/content/fileAssetReview/FileAssetReview";
import { FileAssetResource } from "@Palavyr-Types";
import { ZoomImage } from "@common/components/borrowed/ZoomImage";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface FileAssetProps {
    fileAsset: FileAssetResource;
}

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

export const FileAsset = ({ fileAsset }: FileAssetProps) => {
    const cls = useStyles();
    const [isLoading, setLoading] = useState<boolean>(true);

    useEffect(() => {}, [fileAsset]);

    const onClick = e => {
        e.preventDefault();
        window.open(fileAsset.link, "_blank");
    };

    const extension = fileAsset.fileName.split(".").pop() ?? "";
    const fileDetails = { link: fileAsset.link, extension };

    const renderSwitch = (current: FileDetails) => {
        const { extension, link } = current;

        switch (extension) {
            case "pdf":
                return (
                    <>
                        <div
                            style={{
                                display: "flex",
                                flexDirection: "column",
                                justifyContent: "center",
                                visibility: isLoading ? "hidden" : "visible",
                                width: "100%",
                                height: "100%",
                                maxWidth: "85%",
                                maxHeight: "100vh",
                                border: "none",
                            }}
                        >
                            <ColoredButton styles={{ marginBottom: "0.3rem" }} variant="outlined" color="primary" onClick={onClick}>
                                <PalavyrText variant="body2">View your PDF in a new window</PalavyrText>
                            </ColoredButton>
                            <object onLoad={() => setLoading(false)} data={link} id="upload-preview" type="application/pdf" width="100%" height="100%" aria-label="preview"></object>
                        </div>
                    </>
                );
            default:
                return (
                    <div style={{ visibility: isLoading ? "hidden" : "visible", margin: "0.3rem" }}>
                        <ZoomImage imgSrc={link} onLoad={() => setLoading(false)} alt="preview" />
                    </div>
                );
        }
    };

    return (
        <>
            {isLoading && (
                <Align>
                    <CircularProgress style={{ padding: ".8rem", margin: "1rem" }} />
                </Align>
            )}
            <Align>{renderSwitch(fileDetails)}</Align>
        </>
    );
};
