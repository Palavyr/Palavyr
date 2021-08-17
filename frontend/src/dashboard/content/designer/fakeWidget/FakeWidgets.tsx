import React from "react";
import { makeStyles, TextField } from "@material-ui/core";
import { Autocomplete, AutocompleteRenderInputParams } from "@material-ui/lab";
import classNames from "classnames";
import { SpaceEvenly } from "dashboard/layouts/positioning/SpaceEvenly";
import SettingsIcon from "@material-ui/icons/Settings";
import { StandardComponents } from "../standardComponentDuplicate";
import { WidgetPreferences } from "@Palavyr-Types";
import { FakeMessage, fakeMessages } from "./fakeMessages";
import { FakeWidgetFrame } from "./FakeWidgetFrame";
import { FakeMessageComponent } from "./FakeMessage";
import { CHAT_DEMO_LISTBOX_zINDEX } from "@constants";
import { LineSpacer } from "@common/components/typography/LineSpacer";

const useStyles = makeStyles((theme) => ({
    selectListBgColor: (prefs: WidgetPreferences) => ({
        backgroundColor: prefs.selectListColor,
    }),
    selectListFontColor: (prefs: WidgetPreferences) => ({
        color: prefs.listFontColor,
    }),
    selectbox: {
        paddingLeft: "1rem",
        paddingRight: "1rem",
    },
    mainList: {
        height: "100%",
    },
    inputLabel: (prefs: WidgetPreferences) => ({
        "& .MuiFormLabel-root": {
            color: prefs.listFontColor,
            fontSize: "10pt",
        },
    }),
    headerBehavior: {
        wordWrap: "break-word",
        padding: "1rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "18%",
    },
    settingsIcon: {
        position: "fixed",
        right: "5px",
        top: "5px",
        height: "2rem",
        width: "2rem",
    },
    listbox: {
        padding: "0rem",
    },
    popper: {
        zIndex: 1200
    }
}));

export const FakeWidgets = ({ ...prefs }: WidgetPreferences) => {
    const cls = useStyles(prefs);
    const reg = new StandardComponents(prefs);

    return (<>
    <LineSpacer numLines={2} />
                <FakeWidgetFrame title="(Landing Screen)" prefs={prefs} header={<div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: prefs.landingHeader ?? "" }} />}>
                    <Autocomplete
                        size="small"
                        classes={{ popper: cls.popper, root: cls.selectbox, paper: classNames(cls.selectListBgColor, cls.selectListFontColor) }}
                        disableClearable
                        clearOnEscape
                        open={true}
                        className={classNames(cls.mainList, cls.selectListBgColor, cls.selectListFontColor)}
                        onChange={(x) => null}
                        options={[{ x: "First Option" }, { x: "Second Option" }, { x: "Third Option" }]}
                        getOptionLabel={(option) => option.x}
                        ListboxProps={{ className: cls.listbox }}
                        renderInput={(params: AutocompleteRenderInputParams) => (
                            <TextField
                                {...params}
                                className={cls.inputLabel}
                                label="Select an area or start typing..."
                                inputProps={{
                                    ...params.inputProps,
                                    autoComplete: "new-password",
                                }}
                            />
                        )}
                    />
                </FakeWidgetFrame>
                <FakeWidgetFrame
                    title="(Chat Screen)"
                    prefs={prefs}
                    header={
                        <>
                            <SettingsIcon className={cls.settingsIcon} onClick={() => null} />
                            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: prefs.chatHeader ?? "" }} />
                        </>
                    }
                >
                    {fakeMessages(reg).map((message: FakeMessage, index: number) => (
                        <FakeMessageComponent key={index} message={message} prefs={prefs} index={index} />
                    ))}
                </FakeWidgetFrame>

                </>
    );
};
