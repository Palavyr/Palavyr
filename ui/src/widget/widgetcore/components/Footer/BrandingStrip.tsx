import { makeStyles, useTheme } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import ReplayIcon from "@material-ui/icons/Replay";
import classNames from "classnames";
import { useWidgetStyles } from "@widgetcore/widget/Widget";
import { IAppContext } from "widget/hook";
import { getSelectorNode } from "@widgetcore/BotResponse/utils/utils";
import { useLocation } from "react-router-dom";
import { PalavyrWidgetRepository } from "@api-client/PalavyrWidgetRepository";
import { renderNextBotMessage } from "@widgetcore/BotResponse/utils/renderBotMessage";
import { Tooltip } from "@material-ui/core";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { TextSpeedSwitch } from "./TextSpeedSwitch";
import "@widgetcore/widget/widget.module.scss";

type StyleProps = {
    resetEnabled: boolean;
};


const useStyles = makeStyles<{}>((theme: any) => ({
    leadingText: {},
    wrapper: {
        fontFamily: theme.typography.fontFamily,
        justifyItems: "c enter",
        paddingLeft: "1rem",
    },
    brand: {
        "&:hover": {
            cursor: "pointer",
        },
    },
    replayIcon: {
        fontSize: "1rem",
        "&:hover": {
            cursor: "pointer",
        },
        color: theme.palette.common.white,
    },
    replayIconDisabled: {
        fontSize: "1rem",
        "&:hover": {
            cursor: "default",
        },
        color: theme.palette.error.main,
    },
    iconRoot: {
        marginRight: ".3rem",
        marginLeft: ".3rem",
        alignItems: "center",
    },
    iconRootDisabled: {
        marginRight: ".3rem",
        marginLeft: ".3rem",
    },
    visible: (props: StyleProps) => ({
        visibility: props.resetEnabled ? "visible" : "hidden",
    }),

    container: {
        minHeight: "30px",
        width: "100%",
        display: "flex",
        justifyContent: "space-between",
        backgroundColor: "#264B94",
        color: "white",
    },
    switchroot: {
        color: "white",
    },
}));
export const BrandingStrip = ({ context }: { context: IAppContext }) => {
    const [resetEnabled, setResetEnabled] = useState<boolean>(false);

    const location = useLocation();
    const cls = useStyles({ resetEnabled: context.resetEnabled });
    const wcls = useWidgetStyles();
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

    useEffect(() => {
        if (context.resetEnabled === true) {
            setResetEnabled(true);
        } else {
            setResetEnabled(false);
        }
    }, [context.resetEnabled]);
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
                {resetEnabled ? (
                    <Tooltip key="replay" title="Restart">
                        <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIcon} onClick={resetOnClick} />
                    </Tooltip>
                ) : (
                    <Tooltip key="replay-disabled" title="Restart Disabled">
                        <ReplayIcon classes={{ root: cls.iconRoot }} className={cls.replayIconDisabled} onClick={() => null} />
                    </Tooltip>
                )}
                <Tooltip key="speed-check" title="Chat Speed">
                    <TextSpeedSwitch
                        style={{ opacity: 1 }}
                        classes={{ root: cls.switchroot }}
                        onChange={(_: any, checked: boolean) => {
                            if (checked) {
                                context.setReadingSpeed(10);
                            } else {
                                context.setReadingSpeed(2);
                            }
                        }}
                    />
                </Tooltip>
            </div>
        </div>
    );
};
