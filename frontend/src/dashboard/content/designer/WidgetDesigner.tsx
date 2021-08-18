import { SaveOrCancel } from "@common/components/SaveOrCancel";
import { PalavyrText } from "@common/components/typography/PalavyrTypography";
import { Drawer, Grid, makeStyles } from "@material-ui/core";
import { WidgetPreferences } from "@Palavyr-Types";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { Align } from "dashboard/layouts/positioning/Align";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { HeaderEditor } from "../demo/HeaderEditor";
import { CustomSelect } from "../responseConfiguration/response/tables/dynamicTable/CustomSelect";
import { WidgetColorOptions } from "./ColorOptions";
import { ColorSelectors } from "./colors/ColorSelectors";
import { DesignerWidgetDrawer } from "./DesignerWidgetDrawer";
import { FakeWidgets } from "./fakeWidget/FakeWidgets";
import { FontSelector } from "./fonts/FontSelector";
import { DesignHeaders } from "./headers/DesignHeaders";

const drawerWidth = 440;

const useStyles = makeStyles(theme => ({
    paper: {
        padding: theme.spacing(5),
        marginTop: theme.spacing(3),
        height: "100%",
    },
    drawerRoot: {},
    root: {
        display: "flex",
        height: "100%",
    },
    appBar: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginRight: drawerWidth,
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
    },
    drawerPaper: {
        width: drawerWidth,
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    content: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.default,
        padding: theme.spacing(3),
    },
}));

export const WidgetDesignerPage = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Widget Designer");

    const cls = useStyles();
    const [widgetPreferences, setWidgetPreferences] = useState<WidgetPreferences>();

    const saveWidgetPreferences = async () => {
        if (widgetPreferences) {
            const updatedPreferences = await repository.WidgetDemo.SaveWidgetPreferences(widgetPreferences);
            setWidgetPreferences(updatedPreferences);
            return true;
        } else {
            return false;
        }
    };

    const loadDemoWidget = useCallback(async () => {
        const currentWidgetPreferences = await repository.WidgetDemo.GetWidetPreferences();
        setWidgetPreferences(currentWidgetPreferences);
    }, []);

    useEffect(() => {
        loadDemoWidget();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <>
            {widgetPreferences && (
                <div className={cls.root}>
                    <div className={cls.content}>
                        <div style={{ position: "fixed" }}>
                            <Align direction={"flex-end"}>
                                <SaveOrCancel size="large" onSave={saveWidgetPreferences} />
                            </Align>
                        </div>
                        <ColorSelectors widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                        <FontSelector widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                        <DesignHeaders widgetPreferences={widgetPreferences} setWidgetPreferences={setWidgetPreferences} />
                    </div>
                    <DesignerWidgetDrawer widgetPreferences={widgetPreferences} />
                </div>
            )}
        </>
    );
};
