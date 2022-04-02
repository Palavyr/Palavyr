import React from "react";
import { IconButton, makeStyles, Tooltip } from "@material-ui/core";
import classNames from "classnames";
import InfoIcon from "@material-ui/icons/Info";
import { routesToExclude } from "./DashboardHeader";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

const useStyle = makeStyles(theme => ({
    icon: {
        "&:hover": {
            backgroundColor: theme.palette.primary.light,
        },
        fontSize: "22pt",
        color: theme.palette.info.light,
    },
    hide: {
        display: "none",
    },
    toolTipInternal: {
        backgroundColor: theme.palette.primary.light,
        maxWidgth: "none",
        zIndex: 9999,
    },
}));

export interface InfoIconButtonProps {
    handleHelpDrawerOpen: () => void;
    helpOpen: boolean;
}

export const InfoIconButton = ({ handleHelpDrawerOpen, helpOpen }: InfoIconButtonProps) => {
    const cls = useStyle();
    return (
        <>
            {!routesToExclude.includes(location.pathname) ? (
                <Tooltip title={<PalavyrText>About This Page</PalavyrText>} classes={{ tooltip: cls.toolTipInternal }} interactive>
                    <IconButton className={classNames(cls.icon, { [cls.hide]: helpOpen })} onClick={handleHelpDrawerOpen}>
                        <InfoIcon />
                    </IconButton>
                </Tooltip>
            ) : (
                <div></div>
            )}
        </>
    );
};
