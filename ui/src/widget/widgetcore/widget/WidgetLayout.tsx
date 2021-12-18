import * as React from "react";
import { useEffect } from "react";
import { useLocation } from "react-router-dom";
import { renderNextComponent } from "@widgetcore/BotResponse/utils/renderNextComponent";
import { PalavyrWidgetRepository } from "@common/client/PalavyrWidgetRepository";
import { getRootNode } from "@widgetcore/BotResponse/utils/utils";
import { ConvoHeader } from "@widgetcore/components/ConvoHeader/ConvoHeader";
import { Messages } from "@widgetcore/components/Messages/Messages";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";
import { WidgetNodeResource } from "@Palavyr-Types";
import { designerData } from "@frontend/dashboard/content/designer/data/designerData";
import { dropMessages } from "@store-dispatcher";

export interface WidgetLayoutProps {
    titleAvatar?: string;
    profileAvatar?: string;
    designMode?: boolean;
}
export const WidgetLayout = ({ titleAvatar = "", profileAvatar = "", designMode = false }: WidgetLayoutProps) => {
    const location = useLocation();
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
        <>
            <ConvoHeader titleAvatar={titleAvatar} />
            <Messages profileAvatar={profileAvatar} showTimeStamp={true} />
            <BrandingStrip />
        </>
    );
};
