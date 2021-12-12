import * as React from "react";

import { useEffect } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "@widgetcore/BotResponse/utils/renderNextComponent";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { _openFullscreenPreview } from "@store-actions";
import { getRootNode } from "@widgetcore/BotResponse/utils/utils";
import { ConvoHeader } from "@widgetcore/components/ConvoHeader/ConvoHeader";
import { Messages } from "@widgetcore/components/Messages/Messages";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";

import { WidgetNodeResource } from "@Palavyr-Types";
import { makeStyles } from "@material-ui/core";
import { designerData } from "@frontend/dashboard/content/designer/data/designerData";
import { dropMessages } from "@store-dispatcher";

const useStyles = makeStyles(theme => ({
    root: {
        display: "flex",
        flexFlow: "column",
        height: "100%",
    },
}));

export interface WidgetProps {
    designMode?: boolean;
}

export const Widget = ({ designMode }: WidgetProps) => {
    const location = useLocation();
    const cls = useStyles();
    let secretKey = new URLSearchParams(location.search).get("key");
    if (!secretKey) {
        secretKey = "123";
    }
    const client = new PalavyrWidgetRepository(secretKey);

    const initializeIntroduction = async () => {
        const intro = await client.Widget.Get.IntroSequence();
        const rootNode = getRootNode(intro);
        renderNextComponent(rootNode, intro, client, null);
    };

    const initializeDesignMode = async () => {
        const conversation = designerData as WidgetNodeResource[];
        renderNextComponent(conversation[0], conversation, client, null);
    };
    const initialize = React.useCallback(async () => {
        dropMessages();
        if (designMode) {
            initializeDesignMode();
        } else {
            await initializeIntroduction();
        }
    }, []);

    useEffect(() => {
        initialize();
        return () => {};
    }, []);

    return (
        <div className={cls.root}>
            <ConvoHeader titleAvatar={""} />
            <Messages profileAvatar={""} showTimeStamp={true} />
            <BrandingStrip />
        </div>
    );
};

// a react component where the inner row is a scrolling area, and the outer rows are fixed size
// the inner row is the main chat area, and the outer rows are the header and footer
// the header and footer are fixed size, and are always visible
// the inner row is scrollable, and is always visible
