import { CircularProgress, makeStyles } from "@material-ui/core";
import { Align } from "@common/positioning/Align";
import React, { useEffect, useState } from "react";

export interface CustomImageProps {
    imageLink: string;
}

const useStyles = makeStyles(theme => ({
    image: {
        height: "100%",
        width: "100%",
        borderRadius: "5px",
        "&:hover": {
            cursor: "pointer",
        },
    },
}));

export const CustomImage = ({ imageLink }: CustomImageProps) => {
    const cls = useStyles();
    const [isLoading, setLoading] = useState<boolean>(true);

    useEffect(() => {}, [imageLink]);

    const onImageClick = e => {
        e.preventDefault();
        window.open(imageLink, "_blank");
    };

    return (
        <>
            {isLoading && (
                <Align>
                    <CircularProgress style={{ padding: ".8rem", margin: "1rem" }} />
                </Align>
            )}
            <Align>
                <div style={{ visibility: isLoading ? "hidden" : "visible", maxWidth: "100px", margin: "0.3rem" }}>
                    <img
                        onClick={onImageClick}
                        className={cls.image}
                        key={Date.now()}
                        src={imageLink}
                        onChange={() => setLoading(true)}
                        onLoadStart={() => setLoading(true)}
                        onLoad={() => setLoading(false)}
                    />
                </div>
            </Align>
        </>
    );
};
