import { makeStyles } from "@material-ui/core";
import React from "react";
import { BookLoader } from "react-awesome-loaders";
import { BoxesLoader } from "react-awesome-loaders";

const useStyles = makeStyles(theme => ({
    box: {},
}));
export const BoxesLoaderComponent = () => {
    return (
        <>
            <BoxesLoader boxColor={"#6366F1"} style={{ marginBottom: "20px" }} desktopSize={"128px"} mobileSize={"80px"} />
        </>
    );
};

export const BookLoaderComponent = () => {
    const cls = useStyles();
    return (
        <>
            <BookLoader className={cls.box} background={"linear-gradient(135deg, #6066FA, #4645F6)"} desktopSize={"129px"} mobileSize={"80px"} textColor={"#000000"} />
        </>
    );
};
