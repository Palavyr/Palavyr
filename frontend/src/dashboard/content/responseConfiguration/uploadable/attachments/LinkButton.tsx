import { Button } from "@material-ui/core";
import React from "react";
import { Link } from "react-scroll";


const composedClickAction = (fileName: string, link: string, fileId: string, clickAction: (fileName: string, link: string, fileId: string) => void) => {
    clickAction(fileName, link, fileId);
}

interface ILinkButton {
    color: any;
    link: string;
    fileId: string;
    clickAction: (fileName: string, link: string, fileId: string) => void;
    childText: string;
    fileName: string;
}

export const LinkButton = ({color, fileName, link, fileId, clickAction, childText}: ILinkButton) => {
    return (
        <Link to="dashpaper" activeClass="active" spy={true} smooth={true} duration={3500}>
            <Button color={color} variant="contained" onClick={() => composedClickAction(fileName, link, fileId, clickAction)}>{childText}</Button>
        </Link>
    )
}