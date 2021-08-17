import React from "react";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { WidgetPreferences, SetState } from "@Palavyr-Types";
import { HeaderEditor } from "dashboard/content/demo/HeaderEditor";

export interface DesignLandingHeaderProps {
    widgetPreferences: WidgetPreferences;
    setWidgetPreferences: SetState<WidgetPreferences>;
}

export const DesignLandingHeader = ({ widgetPreferences, setWidgetPreferences }: DesignLandingHeaderProps) => {
    return (
        <div style={{ display: "flex", flexDirection: "column" }}>
            <PalavyrText align="center" gutterBottom variant="h4">
                Landing Header
            </PalavyrText>
            {widgetPreferences && <HeaderEditor setEditorState={(landingHeader: string) => setWidgetPreferences({ ...widgetPreferences, landingHeader })} initialData={widgetPreferences.landingHeader} />}
        </div>
    );
};
