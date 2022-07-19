import React from "react";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { WidgetPreferencesResource, SetState } from "@Palavyr-Types";
import { HeaderEditor } from "frontend/dashboard/content/demo/HeaderEditor";

export interface DesignLandingHeaderProps {
    widgetPreferences: WidgetPreferencesResource;
    setWidgetPreferences: SetState<WidgetPreferencesResource>;
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
