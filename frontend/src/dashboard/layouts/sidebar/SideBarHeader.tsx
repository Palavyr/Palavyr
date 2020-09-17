import React from "react";
import ChevronLeftIcon from "@material-ui/icons/ChevronLeft";
import ChevronRightIcon from "@material-ui/icons/ChevronRight";
import IconButton from "@material-ui/core/IconButton";

interface SideBarHeaderProps {
    handleDrawerClose: () => void;
    classes: any;
    theme: any;
}

const SideBarHeader = ({ handleDrawerClose, classes, theme }: SideBarHeaderProps) => {
    return (
        <div className={classes.drawerHeader}>
            <IconButton onClick={() => handleDrawerClose()}>
                <div>
                    <span>Menu</span>
                    {theme.direction === "ltr" ? <ChevronLeftIcon /> : <ChevronRightIcon />}
                </div>
            </IconButton>
        </div>
    );
};

export default SideBarHeader;
