// import { Card, Fade, makeStyles, Tooltip } from "@material-ui/core";
// import React, { RefObject, useContext, useEffect, useRef, useState } from "react";
// import FaceIcon from "@material-ui/icons/Face";
// import ReplayIcon from "@material-ui/icons/Replay";
// import "./style.scss";
// import { WidgetPreferences } from "@Palavyr-Types";
// import { WidgetContext } from "widget/context/WidgetContext";

// export interface ConvoHeaderProps {
//     titleAvatar?: string;
// }

// const useStyles = makeStyles(theme => ({
//     header: (props: WidgetPreferences) => ({
//         backgroundColor: props.headerColor,
//         color: props.headerFontColor,
//         textAlign: "left",
//         minWidth: 275,
//         zIndex: 99999,
//         wordWrap: "break-word",
//         borderRadius: "0px",
//     }),
//     flexProperty: {
//         flexDirection: "column",
//         textAlign: "center",
//         borderRadius: "0px",
//         display: "flex",
//     },
//     settingsIcon: (props: WidgetPreferences) => ({
//         color: theme.palette.getContrastText(props.headerColor),
//         position: "relative",
//         float: "right",
//         top: "0px",
//         margin: "0.2rem",
//         height: "2rem",
//         width: "2rem",

//         "&:hover": {
//             cursor: "pointer",
//         },
//     }),

//     headerBehavior: {
//         textAlign: "left",
//         wordWrap: "break-word",
//         padding: "0rem",
//         paddingBottom: "0rem",
//         width: "100%",
//         wordBreak: "normal",
//         minHeight: "60px",
//     },
//     paper: {
//         boxShadow: "none",
//     },
//     // headerHTML: {
//     //     textAlign: "left",
//     //     wordWrap: "break-word",

//     //     width: "100%",
//     //     wordBreak: "normal",
//     //     minHeight: "60px",
//     // },
//     // header: (props: WidgetPreferences) => ({
//     //     backgroundColor: props.headerColor,
//     //     color: props.headerFontColor,
//     //     textAlign: "left",
//     //     minWidth: 275,
//     //     wordWrap: "break-word",
//     //     borderRadius: "0px",
//     // }),
//     // flexProperty: {
//     //     flexDirection: "column",
//     //     textAlign: "center",
//     //     borderRadius: "0px",
//     //     display: "flex",
//     //     // padding: "15px 0 25px",
//     // },
//     // settingsIcon: (props: WidgetPreferences) => ({
//     //     color: theme.palette.getContrastText(props.headerColor),
//     //     position: "fixed",
//     //     right: "5px",
//     //     top: "5px",
//     //     height: "2rem",
//     //     width: "2rem",
//     //     "&:hover": {
//     //         cursor: "pointer",
//     //     },
//     // }),
//     // replayIcon: {
//     //     color: theme.palette.common.white,
//     //     position: "fixed",
//     //     right: "5px",
//     //     bottom: "8px",
//     //     height: "1.2rem",
//     //     width: "1.2rem",
//     //     "&:hover": {
//     //         cursor: "pointer",
//     //     },
//     // },
// }));

// export const ConvoHeader = ({ titleAvatar }: ConvoHeaderProps) => {
//     const [tipOpen, setTipOpen] = useState<boolean>(false);
//     const ref = useRef<HTMLDivElement>(null);

//     const { preferences, chatStarted } = useContext(WidgetContext);
//     useEffect(() => {
//         if (chatStarted) {
//             setTipOpen(true);
//             setTimeout(() => {
//                 setTipOpen(false);
//             }, 3000);
//         }

//         if (ref && ref.current) {
//             ref.current.addEventListener("mouseover", () => {
//                 setTipOpen(true);
//             });
//             ref.current.addEventListener("mouseout", () => {
//                 setTipOpen(false);
//             });
//         }
//         return () => {
//             if (ref && ref.current) {
//                 ref.current.removeEventListener("mouseover", () => setTipOpen(false));
//                 ref.current.removeEventListener("mouseout", () => setTipOpen(false));
//             }
//         };
//     }, [chatStarted]);

//     const cls = useStyles(preferences);
//     return (
//         <Card className={cls.header}>
//             {chatStarted && (
//                 <Fade in>
//                     <Tooltip open={tipOpen} title="Update your contact details" placement="left">
//                         <FaceIcon ref={ref as any} className={cls.settingsIcon} onClick={openUserDetails} />
//                     </Tooltip>
//                 </Fade>
//             )}
//             {/* <Tooltip title="Restart this chat" placement="left">
//                 <ReplayIcon className={cls.replayIcon} onClick={() => window.location.reload()} />
//             </Tooltip> */}
//             {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
//             <div className={cls.headerHTML} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
//         </Card>
//     );
// };

import { Card, Fade, makeStyles, Tooltip } from "@material-ui/core";
import React, { useContext, useEffect, useRef, useState } from "react";
import FaceIcon from "@material-ui/icons/Face";
import "./style.scss";
import { WidgetPreferences } from "@Palavyr-Types";
import { WidgetContext } from "../../context/WidgetContext";
import { openUserDetails } from "@store-dispatcher";

export interface ConvoHeaderProps {
    titleAvatar?: string;
}

const useStyles = makeStyles(theme => ({
    header: (props: WidgetPreferences) => ({
        backgroundColor: props.headerColor,
        color: props.headerFontColor,
        textAlign: "left",
        minWidth: 275,
        padding: "0rem",
        wordWrap: "break-word",
        borderRadius: "0px",
    }),
    flexProperty: {
        flexDirection: "column",
        textAlign: "center",
        borderRadius: "0px",
        display: "flex",
    },
    settingsIcon: (props: WidgetPreferences) => ({
        color: theme.palette.getContrastText(props.headerColor),
        position: "relative",
        float: "right",
        top: "0px",
        margin: "0.2rem",
        height: "2rem",
        width: "2rem",

        "&:hover": {
            cursor: "pointer",
        },
    }),

    headerBehavior: {
        boxShadow: "none",
        textAlign: "left",
        wordWrap: "break-word",
        padding: "0rem",
        width: "100%",
        wordBreak: "normal",
        minHeight: "60px",
    },
    paper: {
        boxShadow: "none",
    },
}));

export const ConvoHeader = ({ titleAvatar }: ConvoHeaderProps) => {
    const [tipOpen, setTipOpen] = useState<boolean>(false);
    const ref = useRef<HTMLDivElement>(null);

    const { preferences, chatStarted } = useContext(WidgetContext);
    useEffect(() => {
        if (chatStarted) {
            setTipOpen(true);
            setTimeout(() => {
                setTipOpen(false);
            }, 3000);
        }

        if (ref && ref.current) {
            ref.current.addEventListener("mouseover", () => {
                setTipOpen(true);
            });
            ref.current.addEventListener("mouseout", () => {
                setTipOpen(false);
            });
        }
        return () => {
            if (ref && ref.current) {
                ref.current.removeEventListener("mouseover", () => setTipOpen(false));
                ref.current.removeEventListener("mouseout", () => setTipOpen(false));
            }
        };
    }, [chatStarted]);

    const cls = useStyles(preferences);
    return (
        <Card className={cls.header} classes={{ root: cls.paper }}>
            {chatStarted && (
                <Fade in>
                    <Tooltip open={tipOpen} title="Update your contact details" placement="left">
                        <FaceIcon ref={ref as any} className={cls.settingsIcon} onClick={openUserDetails} />
                    </Tooltip>
                </Fade>
            )}
            {titleAvatar && <img src={titleAvatar} className="avatar" alt="profile" />}
            <div className={cls.headerBehavior} dangerouslySetInnerHTML={{ __html: preferences.chatHeader }} />
        </Card>
    );
};
