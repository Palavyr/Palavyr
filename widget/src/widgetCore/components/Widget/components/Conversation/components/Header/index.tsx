import { makeStyles } from "@material-ui/core";
import classNames from "classnames";
import React from "react";
import { WidgetPreferences } from "src/types";
import SettingsIcon from '@material-ui/icons/Settings';
// const close = require("assets/clear-button.svg") as string;

import "./style.scss";

type Props = {
    title: string;
    subtitle: string;
    toggleChat: () => void;
    showCloseButton: boolean;
    titleAvatar?: string;
    customPreferences: WidgetPreferences;
    openUserDetails: any;
};

type StyleProps = {
    headerColor: string;
    headerFontColor: string;
};

const setHeaderStyles = (props: StyleProps) => {
    let headerObj = {
        backgroundColor: "gray",
        color: "white"
    };

    if (props.headerColor) {
        headerObj = { ...headerObj, backgroundColor: props.headerColor };
    }
    if (props.headerFontColor) {
        headerObj = { ...headerObj, color: props.headerFontColor };
    }

    return headerObj;
};

const useStyles = makeStyles({
    header: (props: StyleProps) => setHeaderStyles(props),
    flexProperty: {
        flexDirection: "column",
        textAlign: "center",
        borderRadius: "0px",
        display: "flex",
        padding: "15px 0 25px",
    },
    settingsIcon: {
        position: "static",
        right: "15px",
        top: "-15px"
    }
});

function Header({ title, subtitle, toggleChat, showCloseButton, titleAvatar, customPreferences, openUserDetails }: Props) {
    const cls = useStyles({ headerColor: customPreferences.headerColor, headerFontColor: customPreferences.headerFontColor });
    return (
        <div className={classNames(cls.header, cls.flexProperty)}>
            {/* {showCloseButton && (
                <button className="rcw-close-button" onClick={toggleChat}>
                    <img src={close} className="rcw-close" alt="close" />
                </button>
            )} */}
            <SettingsIcon className={cls.settingsIcon} onClick={openUserDetails} />
            <h4 className={"rcw-title"}>
                {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
                {title}
            </h4>
            <span>{subtitle}</span>
        </div>
    );
}

export default Header;
