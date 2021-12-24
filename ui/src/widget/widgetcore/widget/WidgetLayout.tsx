import * as React from "react";
import { useContext, useEffect } from "react";
import { ConvoHeader } from "@widgetcore/components/ConvoHeader/ConvoHeader";
import { Messages } from "@widgetcore/components/Messages/Messages";
import { BrandingStrip } from "@widgetcore/components/Footer/BrandingStrip";
import { IAppContext } from "widget/hook";
import { WidgetContext } from "@widgetcore/context/WidgetContext";
import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { DashboardContext } from "@frontend/dashboard/layouts/DashboardContext";

export interface WidgetLayoutProps {
    titleAvatar?: string;
    profileAvatar?: string;
    designMode?: boolean;
    initializer(context: IAppContext, repository: PalavyrRepository): void;
}
export const WidgetLayout = ({ initializer, titleAvatar = "", profileAvatar = "", designMode = false }: WidgetLayoutProps) => {
    const { context } = useContext(WidgetContext);
    const { repository } = useContext(DashboardContext);

    const initialize = React.useCallback(async (context: IAppContext) => {
        context.dropMessages();
        initializer(context, repository);
    }, []);

    useEffect(() => {
        initialize(context);
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
