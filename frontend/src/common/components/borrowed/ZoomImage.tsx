import React, { Fragment, useState, useCallback, useEffect } from "react";
import { Portal, Backdrop, makeStyles } from "@material-ui/core";
import ScrollbarSize from "@material-ui/core/Tabs/ScrollbarSize";
import classNames from "classnames";

const useStyles = makeStyles((theme) => ({
    backdrop: {
        zIndex: theme.zIndex.modal,
        backgroundColor: "rgba(0, 0, 0, 0.8)",
    },
    portalImgWrapper: {
        position: "fixed",
        top: "0",
        left: "0",
        width: "100%",
        height: "100%",
        zIndex: theme.zIndex.modal,
        cursor: "pointer",
    },
    portalImgInnerWrapper: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        paddingLeft: theme.spacing(1),
        paddingRight: theme.spacing(1),
        paddingTop: theme.spacing(1),
        paddingBottom: theme.spacing(1),
    },
    portalImg: {
        objectFit: "contain",
        maxWidth: "100%",
        maxHeight: "100%",
    },
    zoomedOutImage: {
        cursor: "pointer",
        maxWidth: "100%",
        maxHeight: "100%",
    },
}));


export interface IZoomImage {
    alt: string;
    src: string;
    className: string;
}

export const ZoomImage = ({ alt, src, className }: IZoomImage) => {

    const classes = useStyles();

    const [zoomedIn, setZoomedIn] = useState(false);
    const [scrollbarSize, setScrollbarSize] = useState(null);

    const zoomIn = useCallback(() => {
        setZoomedIn(true);
    }, [setZoomedIn]);

    const zoomOut = useCallback(() => {
        setZoomedIn(false);
    }, [setZoomedIn]);

    useEffect(() => {
        var header = document.querySelector("header");
        if (header == null) return;

        if (zoomedIn) {
            document.body.style.overflow = "hidden";
            document.body.style.paddingRight = `${scrollbarSize}px`;
            header.style.paddingRight = `${scrollbarSize}px`;
        } else {
            document.body.style.overflow = "auto";
            document.body.style.paddingRight = "0px";
            header.style.paddingRight = "0px";
        }
    }, [zoomedIn, scrollbarSize]);

    return (
        <Fragment>
            <ScrollbarSize onChange={setScrollbarSize}></ScrollbarSize>
            {zoomedIn && (
                <Portal>
                    <Backdrop
                        open={zoomedIn}
                        className={classes.backdrop}
                        onClick={zoomOut}
                    ></Backdrop>
                    <div onClick={zoomOut} className={classes.portalImgWrapper}>
                        <div className={classes.portalImgInnerWrapper}>
                            <img
                                alt={alt}
                                src={src}
                                className={classes.portalImg}
                            ></img>
                        </div>
                    </div>
                </Portal>
            )}
            <img
                alt={alt}
                src={src}
                onClick={zoomIn}
                className={classNames(className, classes.zoomedOutImage)}
            ></img>
        </Fragment>
    );
}
