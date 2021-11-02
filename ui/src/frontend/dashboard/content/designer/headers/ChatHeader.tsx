import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { SetState, WidgetPreferences } from "@Palavyr-Types";
import { HeaderEditor } from "frontend/dashboard/content/demo/HeaderEditor";
import React from "react";

export interface DesignChatHeaderProps {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}
export const DesignChatHeader = ({ widgetPreferences, setWidgetPreferences }: DesignChatHeaderProps) => {
    return (
        <div style={{ display: "flex", flexDirection: "column" }}>
            <PalavyrText align="center" gutterBottom variant="h4">
                Chat Header
            </PalavyrText>
            {widgetPreferences && <HeaderEditor setEditorState={(chatHeader: string) => setWidgetPreferences({ ...widgetPreferences, chatHeader })} initialData={widgetPreferences.chatHeader} />}
        </div>
    );
};
