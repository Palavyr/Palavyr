import React, { Fragment, useState, useCallback, useEffect } from "react";
import PropTypes from "prop-types";
import { Portal, Backdrop, withStyles, makeStyles } from "@material-ui/core";
import ScrollbarSize from "@material-ui/core/Tabs/ScrollbarSize";
import classNames from "classnames";

const useStyles = makeStyles(theme => ({
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
    },
}));

export interface ZoomImageProps {
    classes: any;
    alt: string;
    src: string;
    theme: object;
    zoomedImgProps: object;
    className: string;
}

export const ZoomImage = ({ alt, src, zoomedImgProps, classes, className }: ZoomImageProps) => {
    const [zoomedIn, setZoomedIn] = useState(false);
    const [scrollbarSize, setScrollbarSize] = useState(null);
    classes = { ...useStyles(), ...classes };
    const zoomIn = useCallback(() => {
        setZoomedIn(true);
    }, [setZoomedIn]);

    const zoomOut = useCallback(() => {
        setZoomedIn(false);
    }, [setZoomedIn]);

    useEffect(() => {
        if (zoomedIn) {
            document.body.style.overflow = "hidden";
            document.body.style.paddingRight = `${scrollbarSize}px`;
            document.querySelector("header")!.style.paddingRight = `${scrollbarSize}px`;
        } else {
            document.body.style.overflow = "auto";
            document.body.style.paddingRight = "0px";
            document.querySelector("header")!.style.paddingRight = "0px";
        }
    }, [zoomedIn, scrollbarSize]);

    return (
        <Fragment>
            <ScrollbarSize onChange={setScrollbarSize}></ScrollbarSize>
            {zoomedIn && (
                <Portal>
                    <Backdrop open={zoomedIn} className={classes.backdrop} onClick={zoomOut}></Backdrop>
                    <div onClick={zoomOut} className={classes.portalImgWrapper}>
                        <div className={classes.portalImgInnerWrapper}>
                            <img alt={alt} src={src} className={classes.portalImg} {...zoomedImgProps}></img>
                        </div>
                    </div>
                </Portal>
            )}
            <img alt={alt} src={src} onClick={zoomIn} className={classNames(className, classes.zoomedOutImage)}></img>
        </Fragment>
    );
};
