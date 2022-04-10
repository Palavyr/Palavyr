import { makeStyles } from "@material-ui/core";
import React from "react";
import ReplayIcon from "@material-ui/icons/Replay";
import "@widgetcore/widget/widget.module.scss";
import classNames from "classnames";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import { IAppContext } from "widget/hook";
import { TextSpeedToggle } from "./TextSpeedToggle";
import { getSelectorNode } from "@widgetcore/BotResponse/utils/utils";
import { useLocation } from "react-router-dom";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { renderNextBotMessage } from "@widgetcore/BotResponse/utils/renderBotMessage";
import { Tooltip } from "@material-ui/core";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";

type StyleProps = {
    resetEnabled: boolean;
};

const useStyles = makeStyles(theme => ({
    leadingText: {},
    wrapper: {
        fontFamily: theme.typography.fontFamily,
        justifyItems: "center",
        paddingLeft: "1rem",
    },
    brand: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    replayIcon: {
        fontSize: "1rem",
        // paddingRight: "1rem",
        "&:hover": {
            cursor: "pointer",
        },
        color: theme.palette.common.white,
    },
    iconRoot: {
        marginRight: ".3rem",
        marginLeft: ".3rem",
        // textAlign: "right",
    },
    visible: (props: StyleProps) => ({
        visibility: props.resetEnabled ? "visible" : "hidden",
    }),

    container: {
        minHeight: "30px",
        width: "100%",
        display: "flex",
        justifyContent: "space-between",
        backgroundColor: theme.palette.primary.main, // "#264B94",
        color: "white",
    },
}));
export const BrandingStrip = ({ context }: { context: IAppContext }) => {
    const cls = useStyles({ resetEnabled: context.resetEnabled });
    const wcls = useWidgetStyles();
    // we don't need to enable this. We just allow users to click the restart button and if they do, then we treat
    // the convo as if someone just left.
    // When they hit it, we DO NOT reset set the user info form details (name, email, locale, phone), but we do
    // 1. load the selector
    // 2. clear all other context (responses, keyvalues, etc)

    const location = useLocation();
    let secretKey = new URLSearchParams(location.search).get("key");
    if (!secretKey) {
        secretKey = "123";
    }
    const client = new PalavyrWidgetRepository(secretKey);

    const resetOnClick = async () => {
        if (context.resetEnabled) {
            context.resetToSelector();
            const intro = await client.Widget.Get.IntroSequence();
            const selectorNode = getSelectorNode(intro);
            renderNextBotMessage(context, selectorNode, intro, client, null);
        }
    };

    return (
        <div className={classNames(wcls.pwrow, wcls.pfooter, cls.container)}>
            <div style={{ alignItems: "center", display: "flex" }}>
                <PalavyrText className={cls.wrapper} variant="caption">
                    Powered by{" "}
                    <strong className={cls.brand} onClick={() => window.open("https://www.palavyr.com")}>
                        Palavyr
                    </strong>
                </PalavyrText>
            </div>
            <div style={{ display: "flex", flexGrow: 1 }} />
            <div style={{ alignItems: "center", display: "flex", flexDirection: "row", justifyContent: "space-evenly" }}>
                {context.resetEnabled && (
                    <Tooltip key="replay" title="Restart">
                        <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIcon} onClick={resetOnClick} />
                    </Tooltip>
                )}
                {/* <TextSpeedToggle disabled={false} name="Active" style={{ zIndex: 10000 }} /> */}
            </div>
        </div>
    );
};
