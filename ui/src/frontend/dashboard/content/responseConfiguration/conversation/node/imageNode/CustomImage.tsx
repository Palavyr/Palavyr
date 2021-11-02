import { isNullOrUndefinedOrWhitespace } from "@common/utils";
import { CircularProgress, makeStyles, Typography } from "@material-ui/core";
import { Variant } from "@material-ui/core/styles/createTypography";
import { Align } from "@common/positioning/Align";
import { cloneDeep } from "lodash";
import React, { memo, useEffect, useState } from "react";

export interface CustomImageProps {
    imageLink: string;
    imageName: string;
    titleVariant?: Variant;
    imageId: string;
}

const useStyles = makeStyles((theme) => ({
    image: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
    },
}));

export const CustomImage = memo(({ imageId, imageName, imageLink, titleVariant = "h6" }: CustomImageProps) => {
    const cls = useStyles();
    const [isLoading, setLoading] = useState<boolean>(true);
    const [bounce, setBounce] = useState<boolean>(false);
    const onImageClick = (e) => {
        e.preventDefault();
        window.open(imageLink, "_blank");
    };

    useEffect(() => {
        setBounce(!bounce);
    }, [imageId]);

    return (
        <>
            <Typography variant={titleVariant} align="center">
                {!isLoading && isNullOrUndefinedOrWhitespace(imageLink) ? "No Image" : `${imageName}`}
            </Typography>
            {isLoading && imageLink && (
                <Align>
                    <CircularProgress style={{ padding: ".5rem", margin: "1rem" }} />
                </Align>
            )}
            <Align>
                <div style={{ visibility: isLoading ? "hidden" : "visible", maxWidth: "100px", margin: "1rem" }}>
                    <img onClick={onImageClick} className={cls.image} key={Date.now()} src={imageLink} onChange={() => setLoading(true)} onLoadStart={() => setLoading(true)} onLoad={() => setLoading(false)} />
                </div>
            </Align>
        </>
    );
});
