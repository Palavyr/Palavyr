import * as React from "react";
import { useEffect } from "react";
import { ConvoHeader } from "@widgetcore/components/ConvoHeader/ConvoHeader";
import { Messages } from "@widgetcore/components/Messages/Messages";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";
import { dropMessages } from "@store-dispatcher";

export interface WidgetLayoutProps {
    titleAvatar?: string;
    profileAvatar?: string;
    designMode?: boolean;
    initializer(): void;
}
export const WidgetLayout = ({ initializer, titleAvatar = "", profileAvatar = "", designMode = false }: WidgetLayoutProps) => {
    const initialize = React.useCallback(async () => {
        dropMessages();
        initializer();
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
