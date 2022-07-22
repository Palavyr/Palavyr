import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { CircularProgress, makeStyles, Typography } from "@material-ui/core";
import { Variant } from "@material-ui/core/styles/createTypography";
import { Align } from "@common/positioning/Align";
import React, { memo, useEffect, useState } from "react";
import { FileAssetResource } from "@common/types/api/EntityResources";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

export interface FileAssetDisplayProps {
    fileAsset: FileAssetResource;
    titleVariant?: Variant;
}

const useStyles = makeStyles(theme => ({
    display: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
    },
    width: {
        width: "100%",
    },
}));

export const FileAssetDisplay = memo(({ fileAsset, titleVariant = "h6" }: FileAssetDisplayProps) => {
    const cls = useStyles();
    const [isLoading, setLoading] = useState<boolean>(false);
    const [bounce, setBounce] = useState<boolean>(false);

    const onClick = e => {
        e.preventDefault();
        window.open(fileAsset.link, "_blank");
    };

    useEffect(() => {
        setBounce(!bounce);
    }, [fileAsset]);

    const renderSwitch = () => {
        const extension = fileAsset.fileName.split(".").pop() ?? "";
        switch (extension) {
            case "pdf":
                return (
                    <Align extraClassNames={cls.width}>
                        <ColoredButton styles={{ marginBottom: "0.3rem", width: "100%" }} variant="outlined" color="primary" onClick={onClick}>
                            PDF
                        </ColoredButton>
                    </Align>
                );
            default:
                return (
                    <img
                        onClick={onClick}
                        className={cls.display}
                        key={Date.now()}
                        src={fileAsset.link}
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
                {!isLoading && isNullOrUndefinedOrWhitespace(fileAsset.link) ? "No Image" : `${fileAsset.fileName}`}
            </Typography>
            {isLoading && fileAsset.link && (
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
